$(document).ready(function () {
    PageLoad();
});

function ClosePopup(Popup) {
    $(Popup).modal('hide');
}

function OpenPopup(Popup) {
    $(Popup).modal('show');
}

$("body").on("click", ".CerrarSesion", function () {
    __doPostBack(this.name, '');
});



function ImprimirOrden(Server, strOrden) {
    var nro_op = strOrden.split(",");
    for (i = 0 ; i < nro_op.length; i++) {
        window.open(Server.replace('@nro_op', nro_op[i]));
    }
}

//Funciones de Consulta--------------------------------------------------------------------------------------------------------------------------------
function load_Data(Consulta, Tipo) {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: 'FirmasElectronicas.aspx/ObtieneDatos',
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

$("body").on("click", ".CierraAutoriza", function () {
    event.preventDefault();
    ClosePopup('#AutorizaModal');
    $('.User').val('');
    $('.Password').val('');
});

$("body").on("click", ".CierraFirma", function () {
    event.preventDefault();
    ClosePopup('#FirmasModal');
});

//Delete event handler.
$("body").on("click", "[id*=gvd_Usuario] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelUsu]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});

//Delete event handler.
$("body").on("click", "[id*=gvd_Estatus] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelEst]')[row[0].rowIndex - 1].value = "true";
    row.hide();
    return false;
});

function CargaCatalogo(Tipo, IndexCia, IndexCta) {
    $("input[id$='hid_Catalogo']")[0].value = "Cto";
    $("input[id$='hid_Control']")[0].value = Tipo + '|' + (IndexCia - 1) + "," + (IndexCta - 1);
    $("#lblCatalogo")[0].innerText = 'CONCEPTOS';
    load_Data("spS_CatalogosOP ==Cto==", "Unica");
}

$("body").on("click", "#btn_AddUsuario", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Usu"
    $("#lblCatalogo")[0].innerText = 'USUARIOS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Usuario]"), $('[id*=lbl_ClaveUsu]'), $('[id*=chk_SelUsu]'), true);
    load_Data("spS_CatalogosOP ==Usu==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddEstatus", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Est"
    $("#lblCatalogo")[0].innerText = 'ESTATUS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Estatus]"), $('[id*=lbl_ClaveEst]'), $('[id*=chk_SelEst]'), false);
    load_Data("spS_CatalogosOP ==Est==,====" + strSel, "Multiple");
});

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

//Estado si existe Mensaje a Desplegar
function EvaluaMensaje(Titulo, Mensaje) {
    $('[id*=lbl_TitMensaje]')[0].innerText = Titulo;
    $('[id*=txt_Mensaje]')[0].innerText = Mensaje;
    $('#MensajeModal').modal('show');
    $('#MensajeModal').draggable();
}

$("body").on("click", "[id*=gvd_LstOrdenPago] .Link", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    var Control = row.find('.Link');
    if (Control[0].text != '0') {
        var url = $("input[id$='hid_Url']")[0].value + "&nro_op=" + Control[0].text
        window.open(url, "OP" + Control[0].text, "directories=no, resizable=yes, menubar=no,  statusbar=no, tittlebar=no, width=1366, height=700, top=0, left=0");
    }
});

function PageLoad() {
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

        ClosePopup('#CatalogoModal');

        $("input[id$='hid_Seleccion']")[0].value = varSeleccion
        __doPostBack(this.name, '');
    });
}
