$(document).ready(function () {
    PageLoad();
    javascript: window.history.forward(1);
});


$("body").on("click", ".modalExhibiciones", function () {
    $('#MensajeModal').modal('hide');
});

//Funciones de Consulta--------------------------------------------------------------------------------------------------------------------------------
function load_Data(Consulta, Tipo) {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: 'RepDistReaseguro.aspx/ObtieneDatos',
        data: "{ 'Consulta': '" + Consulta + "'}",
        dataType: 'JSON',
        success: function (response) {
            if (response.d.length > 0) {
                $('#CatalogoModal').modal('show');
                $("[id*=gvd_Catalogo] tr").not($("[id*=gvd_Catalogo] tr:first")).remove();
                for (var i = 0; i < response.d.length; i++) {
                    $("[id*=gvd_Catalogo]").append('<tr>' +
                                                        '<td><input type="checkbox" id="chk_Cat" class="Select" onclick="CambioSeleccion(this,' + "'" + Tipo + "'" + ')" /></td>' +
                                                        '<td><label id="lbl_ClaveCat" style="Width:80px;">' + response.d[i].Clave + '</label></td>' +
                                                        '<td><label id="lbl_DesCat" style="Width:245px;">' + response.d[i].Descripcion + '</label></td>' +
                                                   '</tr>')
                };
                //Reference the GridView.
                var gridView = $("[id*=gvd_Catalogo]");

                //Reference the first row.
                var row = gridView.find("tr").eq(1);

                if ($.trim(row.find("td").eq(0).html()) == "") {
                    row.remove();
                }
                $('[id$=gvd_Catalogo]').tablePagination({});
                $('[id$=gvd_Catalogo]').each(function () {
                    $('tr:odd', this).addClass('odd').removeClass('even');
                    $('tr:even', this).addClass('even').removeClass('odd');
                })
                $('#EsperaModal').modal('hide');
            }
            else {
                EvaluaMensaje('Catálogo', 'No se encontraron registros');
                $('#EsperaModal').modal('hide');
            }
        },
        error: function (e) {
            EvaluaMensaje('JSON', e.responseText);
            $('#EsperaModal').modal('hide');
        }
    });
    return false;
};

$("body").on("click", "#btn_AddBroker", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Bro"
    $("#lblCatalogo")[0].innerText = 'BROKERS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Broker]"), $('[id*=lbl_ClaveBro]'), $('[id*=chk_SelBro]'), false);
    load_Data("spS_CatalogosOP ==Bro==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddCia", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Cia"
    $("#lblCatalogo")[0].innerText = 'COMPAÑIAS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Compañia]"), $('[id*=lbl_ClaveCia]'), $('[id*=chk_SelCia]'), false);
    load_Data("spS_CatalogosOP ==Cia==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddRamoContable", function () {
    $("input[id$='hid_Catalogo']")[0].value = "RamC"
    $("#lblCatalogo")[0].innerText = 'RAMOS CONTABLES';
    var strSel = ElementosSeleccionados($("[id*=gvd_RamoContable]"), $('[id*=lbl_ClaveRamC]'), $('[id*=chk_SelRamC]'), true);
    load_Data("spS_CatalogosOP ==RamC==,====" + strSel, "Multiple")
});

$("body").on("click", "#btn_AddProducto", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Pro"
    $("#lblCatalogo")[0].innerText = 'PRODUCTOS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Producto]"), $('[id*=lbl_ClavePro]'), $('[id*=chk_SelPro]'), true);
    load_Data("spS_CatalogosOP ==Pro==,====" + strSel, "Multiple")
});

//-----------------------------------------------------------------------------------------------------------------------------------------------------

//Funciones de Seleccion-------------------------------------------------------------------------------------------------------------------------------
function Seleccion(Control) {
    $(Control).focus(function () {
        this.select();
    });
}

function SeleccionGread(Control, Valor) {
    //Get target base & child control.
    var TargetChildControl = "chk_Cat";

    if (Control == null) {
        alert('No hay elementos')
    }

    if (Control != null) {
        //Get all the control of the type INPUT in the base control.
        var Inputs = Control.getElementsByTagName("input");

        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = Valor;
    }
    return false;
}

//Funcion que recibe el checkBox que se selecciono
function CambioSeleccion(Control, TipoSeleccion) {
    //Get target base & child control.

    var row = $(Control).closest("tr");

    var Gread = document.getElementById($('[id$=gvd_Catalogo]')[0].id)

    //Evalua el tipo de seleccion
    if (TipoSeleccion == "Unica") {
        SeleccionGread(Gread, false)
        SeleccionarElemento(row[0].rowIndex)
    }
    return false;
}

//Selecciona solo un elemento en caso de ser seleccion Unica
function SeleccionarElemento(rowIndex) {
    $("[id*=gvd_Catalogo] tr").each(function (e) {
        var row = $(this).closest("tr");
        if (row[0].rowIndex == rowIndex) {
            var Select = row.find('.Select');
            $(Select)[0].checked = true;
        }
    })
    return false;
}

function ElementosSeleccionados(Gread, Control, Seleccion, blnTexto) {
    var caracter = '';

    if (blnTexto == true) { caracter = '===='; }

    var strSel = caracter + '-1' + caracter;

    if (Gread.length > 0) {
        var Filas = Gread[0].rows;
        for (i = 0; i <= Filas.length - 2; i++) {
            var Clave = Control[i].innerText;
            var chk_Sel = Seleccion[i].value

            //Verifica que no haya sido descartado de la lista
            if (chk_Sel != 'true') { strSel = strSel + ',' + caracter + Clave + caracter; }
        }
    }

    if (strSel == caracter + '-1' + caracter) { strSel = ''; }
    else { strSel = ",==" + strSel + "=="; }

    return strSel;
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------





//Eventos Click----------------------------------------------------------------------------------------------------------------------------------------
function BuscaEndoso() {
    $("input[id$='hid_Polizas']")[0].value = ElementosSeleccionados($("[id*=gvd_Poliza]"), $('[id*=lbl_ClavePol]'), $('[id*=chk_SelPol]'), true);
    __doPostBack($('[id*=btn_BuscaEndoso]')[0].name, '');
}

$("body").on("click", "[id*=btn_BuscaEndoso]", function () {
    $("input[id$='hid_Polizas']")[0].value = ElementosSeleccionados($("[id*=gvd_Poliza]"), $('[id*=lbl_ClavePol]'), $('[id*=chk_SelPol]'), true);
});

$("body").on("click", "#coVentana0", function () {
    event.preventDefault();
    CambiaEstado("0", "1");
});

$("body").on("click", "#exVentana0", function () {
    event.preventDefault();
    CambiaEstado("0", "0");
});

$("body").on("click", "#coVentana1", function () {
    event.preventDefault();
    CambiaEstado("1", "1");
});

$("body").on("click", "#exVentana1", function () {
    event.preventDefault();
    CambiaEstado("1", "0");
});

$("body").on("click", "#coVentana2", function () {
    event.preventDefault();
    CambiaEstado("2", "1");
});

$("body").on("click", "#exVentana2", function () {
    event.preventDefault();
    CambiaEstado("2", "0");
});

$("body").on("click", "#coVentana3", function () {
    event.preventDefault();
    CambiaEstado("3", "1");
});

$("body").on("click", "#exVentana3", function () {
    event.preventDefault();
    CambiaEstado("3", "0");
});



//Delete event handler.
$("body").on("click", "[id*=gvd_Broker] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelBro]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});

//Delete event handler.
$("body").on("click", "[id*=gvd_Compañia] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelCia]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});


//Delete event handler.
$("body").on("click", "[id*=gvd_Poliza] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelPol]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});


//Delete event handler.
$("body").on("click", "[id*=gvd_RamoContable] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelRamC]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});

//Delete event handler.
$("body").on("click", "[id*=gvd_Producto] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelPro]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});
//-----------------------------------------------------------------------------------------------------------------------------------------------------



$("body").on("focusout", ".cod_ramo", function (e) {
    $(".NroPol").select();
});

$("body").on("focusout", ".desc_ramo", function (e) {
    $(".NroPol").select();
});
//-----------------------------------------------------------------------------------------------------------------------------------------------------








//Funciones Estado-------------------------------------------------------------------------------------------------------------------------------------
function EstadoVentanas() {
    var Ventana = $("input[id$='hid_Ventanas']")[0].value;
    var Estado = Ventana.split("|");

    for (i = 0; i < Estado.length - 1; i++) {
        if (Estado[i] == "1") {
            $('.ventana' + i).hide();
            $("#coVentana" + i).hide();
            $("#exVentana" + i).show();
        }
        else {
            $("#coVentana" + i).show();
            $("#exVentana" + i).hide();
        }
    }
}

function CambiaEstado(IdControl, Colapsado) {

    $('.ventana' + IdControl).slideToggle();

    var Ventana = $("input[id$='hid_Ventanas']")[0].value;
    var Estado = Ventana.split("|");

    Estado[IdControl] = Colapsado

    $("input[id$='hid_Ventanas']")[0].value = "";

    for (i = 0; i < Estado.length - 1; i++) {
        $("input[id$='hid_Ventanas']")[0].value = $("input[id$='hid_Ventanas']")[0].value + Estado[i] + "|";
    }

    if (Colapsado == "1") {
        $("#coVentana" + IdControl).hide();
        $("#exVentana" + IdControl).show();
    }
    else {
        $("#coVentana" + IdControl).show();
        $("#exVentana" + IdControl).hide();
    }
}

function EstadoGrid(Grid, Control) {
    if ($("[id*=" + Grid + "]")[0] != undefined) {
        var Rows = $("[id*=" + Grid + "]")[0].rows;
        for (i = 0; i <= Rows.length - 2; i++) {
            if ($('[id*=' + Control + ']')[i].value == "true") {
                var row = $('[id*=' + Control + ']')[i].parentNode.parentNode;
                row.style.display = "none";
            }
        }
    }
}

//-----------------------------------------------------------------------------------------------------------------------------------------------------

//Estado si existe Mensaje a Desplegar
function EvaluaMensaje(Titulo, Mensaje) {
    $('[id*=lbl_TitMensaje]')[0].innerText = Titulo;
    $('[id*=txt_Mensaje]')[0].innerText = Mensaje;
    $('#MensajeModal').modal('show');
    $('#MensajeModal').draggable();
}

function EstiloGrid() {
    //$('[id$=gvd_Reaseguro]').tablePagination({});
    $('[id$=gvd_Catalogo]').tablePagination({});
}

function ClosePopup(Popup) {
    $(Popup).modal('hide');
}

function OpenPopup(Popup) {
    $(Popup).modal('show');
}

function DisableControls() {
    $("#btn_AddBroker").attr('disabled', 'disabled');
    $("#btn_AddCia").attr('disabled', 'disabled');
    $("#btn_AddCtr").attr('disabled', 'disabled');
    $("#btn_AddPol").attr('disabled', 'disabled');
    $("#btn_AddUsuario").attr('disabled', 'disabled');
    $("#btn_AddEstatus").attr('disabled', 'disabled');
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------

function PageLoad() {

    $('#txtFiltrar').keyup(function (event) {
        var searchKey = $(this).val().toLowerCase();
        $("[id$=gvd_Catalogo] tr td").each(function () {
            var cellText = $(this).text().toLowerCase();
            if (cellText.indexOf(searchKey) >= 0) {
                $(this).parent().show();
            }
            else {
                $(this).parent().hide();
            }
        });

        if ($('#txtFiltrar')[0].value == "") {
            $('[id$=gvd_Catalogo]').tablePagination({});
        }
    });

    EstiloGrid();
    EstadoVentanas();
    EstadoGrid("gvd_Broker", "chk_SelBro");
    EstadoGrid("gvd_Compañia", "chk_SelCia");
    EstadoGrid("gvd_Poliza", "chk_SelPol");
    EstadoGrid("gvd_RamoContable", "chk_SelRamC");
    EstadoGrid("gvd_Producto", "chk_SelPro");
    //-------------------------------------------------------------

    //Botón Aceptar en Catalogo
    $("[id*=btn_OkCatalogo]").click(function () {
        var varSeleccion = '';
        var Filas = $("[id*=gvd_Catalogo]")[0].rows;

        for (i = 0; i <= Filas.length - 2; i++) {
            if ($('[id*=chk_Cat]')[i].checked == true) {
                varSeleccion = varSeleccion + $('[id*=lbl_ClaveCat]')[i].innerText + '~' +
                                                $('[id*=lbl_DesCat]')[i].innerText + '|';

            }
        }

        $("input[id$='hid_Seleccion']")[0].value = varSeleccion
        __doPostBack(this.name, '');
    });

    $(".Fecha").datepicker({
        showOn: 'both',
        buttonImageOnly: true,
        buttonImage: '../Images/Calendar.png',
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
                        'Junio', 'Julio', 'Agosto', 'Septiembre',
                        'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
                            'May', 'Jun', 'Jul', 'Ago',
                            'Sep', 'Oct', 'Nov', 'Dic']
    });

    $(".Fecha").mask("99/99/9999");


    $(".FechaSB").datepicker({
        showOn: 'focus',
        buttonImageOnly: false,
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 1,
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
                        'Junio', 'Julio', 'Agosto', 'Septiembre',
                        'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
                            'May', 'Jun', 'Jul', 'Ago',
                            'Sep', 'Oct', 'Nov', 'Dic']
    });

    $(".FechaSB").mask("99/99/9999");

}




