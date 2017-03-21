$(document).ready(function () {
    PageLoad();
    javascript: window.history.forward(1);
});

function ImprimirOrden(Server,strOrden) {
    var nro_op = strOrden.split(",");
    for (i = 0 ; i < nro_op.length; i++) {
        window.open(Server.replace('@nro_op',nro_op[i]));
    }
}

var bPreguntar = false;

//window.onbeforeunload = function () {
//    $('.CerrarSesion').click();
//    return null;
//}

window.onbeforeunload = preguntarAntesDeSalir;

function preguntarAntesDeSalir() {
    if (bPreguntar) {
        return "¿Seguro que quieres salir?";
    }
    else {
        $('.CerrarSesion').click();
    }
        
}

$("body").on("click", ".CerrarSesion", function () {
    __doPostBack(this.name, '');
});

$("body").on("click", ".modalExhibiciones", function () {
    $('#MensajeModal').modal('hide');
});

//Funciones de Consulta--------------------------------------------------------------------------------------------------------------------------------
function load_Data(Consulta, Tipo) {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: 'OrdenPago.aspx/ObtieneDatos',
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
    });70
    return false;
};


function Consulta(id_pv) {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: 'OrdenPago.aspx/ObtieneDatosRTF',
        data: "{ 'id_pv': '" + id_pv + "'}",
        dataType: 'JSON',
        success: function (response) {
            if (response.d.length > 0) {
                $('#AclaracionesModal').modal('show');
                //ftb = FTB_API['ctl00_ContentMaster_FTB_Aclaraciones']
                //ftb.SetHtml(response.d);

                $(".Info")[0].innerHTML = response.d;
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


function CargaCatalogo(Tipo, IndexCia, IndexCta) {
    $("input[id$='hid_Catalogo']")[0].value = "Cto";
    $("input[id$='hid_Control']")[0].value = Tipo + '|' + (IndexCia - 1) + "," + (IndexCta - 1);
    $("#lblCatalogo")[0].innerText = 'CONCEPTOS';
    load_Data("spS_CatalogosOP ==Cto==", "Unica");
}


$("body").on("click", "#btn_AddBroker", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Bro"
    $("#lblCatalogo")[0].innerText = 'BROKERS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Broker]"), $('[id*=lbl_ClaveBro]'), $('[id*=chk_SelBro]'),false);
    load_Data("spS_CatalogosOP ==Bro==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddCia", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Cia"
    $("#lblCatalogo")[0].innerText = 'COMPAÑIAS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Compañia]"), $('[id*=lbl_ClaveCia]'), $('[id*=chk_SelCia]'),false);
    load_Data("spS_CatalogosOP ==Cia==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddUsuario", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Usu"
    $("#lblCatalogo")[0].innerText = 'USUARIOS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Usuario]"), $('[id*=lbl_ClaveUsu]'), $('[id*=chk_SelUsu]'),true);
    load_Data("spS_CatalogosOP ==Usu==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_AddEstatus", function () {
    $("input[id$='hid_Catalogo']")[0].value = "Est"
    $("#lblCatalogo")[0].innerText = 'ESTATUS';
    var strSel = ElementosSeleccionados($("[id*=gvd_Estatus]"), $('[id*=lbl_ClaveEst]'), $('[id*=chk_SelEst]'),false);
    load_Data("spS_CatalogosOP ==Est==,====" + strSel, "Multiple");
});

$("body").on("click", "#btn_SelCia", function () {
    $("input[id$='hid_Catalogo']")[0].value = "CiaU"
    $("#lblCatalogo")[0].innerText = 'COMPAÑIAS';
    load_Data("spS_CatalogosOP ==CiaU==", "Unica");
});

$("body").on("click", "#btn_SelRam", function () {
    $("input[id$='hid_Catalogo']")[0].value = "RamU"
    $("#lblCatalogo")[0].innerText = 'RAMOS';
    load_Data("spS_CatalogosOP ==RamU==", "Unica")
});

//$("body").on("click", "#btn_SelRamCont", function () {
//    $("input[id$='hid_Catalogo']")[0].value = "RamC"
//    $("#lblCatalogo")[0].innerText = 'RAMOS CONTABLES';
//    load_Data("spS_CatalogosOP ==RamC==", "Unica")
//});

//$("body").on("click", "#btn_SelRamTec", function () {
//    $("input[id$='hid_Catalogo']")[0].value = "RamT"
//    $("#lblCatalogo")[0].innerText = 'RAMOS TÉCNICOS';
//    load_Data("spS_CatalogosOP ==RamU==", "Unica")
//});

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


$("body").on("click", "#btn_AddCtr", function () {
    var Condicion = "";
    $("input[id$='hid_Catalogo']")[0].value = "Fac"
    $("#lblCatalogo")[0].innerText = 'FACULTATIVOS';

    //Fechas de Vigencia
    if ($("input[id$='txt_FechaDe']")[0].value.length == 10) {
        var Fecha1 = $("input[id$='txt_FechaDe']")[0].value;
        FechaDe = Fecha1.substring(6, 10) + Fecha1.substring(3, 5) + Fecha1.substring(0, 2);
        Condicion = " AND fecha_fac >= ==" + FechaDe + "==";
    }

    if ($("input[id$='txt_FechaA']")[0].value.length == 10) {
        var Fecha2 = $("input[id$='txt_FechaA']")[0].value;
        FechaA = Fecha2.substring(6, 10) + Fecha2.substring(3, 5) + Fecha2.substring(0, 2);
        Condicion = Condicion + " AND fecha_fac <= ==" + FechaA + "==";
    }


    var Brks = '-1';
    if ($("[id*=gvd_Broker]")[0] != undefined) {
        var Rows = $("[id*=gvd_Broker]")[0].rows;
        for (i = 0; i <= Rows.length - 2; i++) {
            if ($('[id*=chk_SelBro]')[i].value != "true") {
                Brks = Brks + ',' + $('[id*=lbl_ClaveBro]')[i].innerText;
            }
        }
    }

    if (Brks != '-1') {
        Condicion = Condicion + ' AND mr.cod_broker IN (' + Brks + ')'
    }

    var Cias = '-1';
    if ($("[id*=gvd_Compañia]")[0] != undefined) {
        var Rows = $("[id*=gvd_Compañia]")[0].rows;
        for (i = 0; i <= Rows.length - 2; i++) {
            if ($('[id*=chk_SelCia]')[i].value != "true") {
                Cias = Cias + ',' + $('[id*=lbl_ClaveCia]')[i].innerText;
            }
        }
    }

    if (Cias != '-1') {
        Condicion = Condicion + ' AND mr.cod_cia IN (' + Cias + ')'
    }

    if (Condicion.length == 0) {
        Condicion = "";
    }
    else{
        Condicion = ",==" + Condicion + "==";
    }

    var strSel = ElementosSeleccionados($("[id*=gvd_Contrato]"), $('[id*=lbl_ClaveFac]'), $('[id*=chk_SelFac]'), true);
    load_Data("spS_CatalogosOP ==Fac==" + Condicion + strSel, "Multiple");
});

$("body").on("click", "[id*=gvd_OrdenPago] .btnCuenta", function (e) {
    e.preventDefault();
    var row = $(this).closest("tr");
    var Moneda = row.find('.Moneda');
    var Persona = row.find('.Persona');

    $("input[id$='hid_Control']")[0].value = row[0].rowIndex;
    $("input[id$='hid_Catalogo']")[0].value = 'Cta';
    $("#lblCatalogo")[0].innerText = 'CUENTAS BANCARIAS';
    var Condicion = ' WHERE id_persona = ' + Persona[0].value + ' AND cod_moneda = ' + Moneda[0].value
    load_Data("spS_CatalogosOP ==Cta==" + ",==" + Condicion + "==", "Unica");
    
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

$("body").on("click", "[id*=btn_CerrarExhibiciones]", function () {
    $('#ExhibicionesModal').modal('hide');
    __doPostBack(this.name, '');
});

//Botón Cuentas Bancarias
$("body").on("click", "[id*=gvd_OrdenPago] .Cuentas", function () {
    event.preventDefault();
    $('[data-toggle="popover"]').popover();
});

$("body").on("click", "[id*=btn_ConfirmarOP]", function () {
    $('#OrdenesModal').modal('hide');
    __doPostBack(this.name, '');
});

$("body").on("click", "[id*=gvd_Cuotas] .Link", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    var Control = row.find('.Link');
    var nro_cuota = row.find('.nro_cuota_ori');

    if (Control[0].text != '0' && Control[0].text != 'Recuperado') {
        var id_pv = $("input[id$='hid_idPv']")[0].value;
        var Capa = $('[id*=txt_Capa]')[0].value;
        var Contrato = $('[id*=txt_Contrato]')[0].value;
        var nro_tramo = $("input[id$='hid_nrotramo']")[0].value;
        var Ramo = $('[id*=txt_Ramo]')[0].value.split(" .- ");
        
        var rowP = row.parents('tr');
        var IdCia = rowP.find('.IdCia');

        var url = $("input[id$='hid_Url']")[0].value + "&nro_op=" + Control[0].text + "&id_pv=" + id_pv + "&nro_layer=" + Capa + "&id_contrato=" + Contrato + "&nro_tramo=" + nro_tramo + "&cod_ramo_contable=" + Ramo[0] + "&cod_cia=" + IdCia[0].innerText + "&nro_cuota=" + nro_cuota[0].value;
        window.open(url, "OP" + Control[0].text, "directories=no, resizable=yes,  menubar=no, statusbar=no, tittlebar=no, width=1366, height=700, top=0, left=0");
    }
});

$("body").on("click", "[id*=gvd_LstOrdenPago] .Link", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    var Control = row.find('.Link');
    if (Control[0].text != '0') {
        var url = $("input[id$='hid_Url']")[0].value + "&nro_op=" + Control[0].text
        window.open(url, "OP" + Control[0].text, "directories=no, resizable=yes, menubar=no,  statusbar=no, tittlebar=no, width=1366, height=700, top=0, left=0");
    }
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

//$("body").on("click", "#coVentana4", function () {
//    event.preventDefault();
//    CambiaEstado("4", "1");
//});

//$("body").on("click", "#exVentana4", function () {
//    event.preventDefault();
//    CambiaEstado("4", "0");
//});

//$("body").on("click", "#coVentana5", function () {
//    event.preventDefault();
//    CambiaEstado("5", "1");
//});

//$("body").on("click", "#exVentana5", function () {
//    event.preventDefault();
//    CambiaEstado("5", "0");
//});

$("body").on("click", "[id*=gvd_CiasXBroker] .BotonCierra", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    var MarcoExhibiciones = row.find('.MarcoExhibiciones')
    $(MarcoExhibiciones).hide();

    var MarcoCompañias = row.find('.MarcoCompañias')
    $(MarcoCompañias).show();

    var EdoOculta = row.find('.EdoOculta')
    $(EdoOculta)[0].value = '1';

    var BotonCierra = row.find('.BotonCierra')
    $(BotonCierra).hide();

    var BotonAbre = row.find('.BotonAbre')
    $(BotonAbre).show();

});

$("body").on("click", "[id*=gvd_CiasXBroker] .BotonAbre", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    var MarcoExhibiciones = row.find('.MarcoExhibiciones')
    $(MarcoExhibiciones).show();

    var MarcoCompañias = row.find('.MarcoCompañias')
    $(MarcoCompañias).hide();

    var EdoOculta = row.find('.EdoOculta')
    $(EdoOculta)[0].value = '0';

    var BotonCierra = row.find('.BotonCierra')
    $(BotonCierra).show();

    var BotonAbre = row.find('.BotonAbre')
    $(BotonAbre).hide();
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
$("body").on("click", "[id*=gvd_Contrato] .Delete", function () {
    var row = $(this).closest("tr");
    $('[id*=chk_SelFac]')[row[0].rowIndex - 1].value = "true";
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

//$("body").on("click", "#btn_CnlCatalogo", function () {
//    $("[id*=gvd_Catalogo] tr").not($("[id*=gvd_Catalogo] tr:first")).remove();
//});

//Selecciona Cuota
$("body").on("click", "[id*=gvd_Cuotas] .SelectCuota", function () {
    var row = $(this).closest("tr");

    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        SelectCuota = row.find('.SelectCuota');
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        SeleccionaCuota(SelectCuota, nro_cuota_ori, nro_cuota_subori)
    }
    else {
        SumasTotales(row.parents('tr'));
    }
});

function SeleccionaCuota(chk_Sel, Cuota,SubCuota) {
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var gvd_Cuotas = row.find('.gvd_Cuotas tr');
        $(gvd_Cuotas).each(function (e) {
            var rowCuota = $(this).closest("tr");

            var chk_SelCuota = rowCuota.find('.SelectCuota');
            var hid_Cuota = rowCuota.find('.nro_cuota_ori');
            var hidSubcuota = rowCuota.find('.nro_cuota_subori');
           
            if (chk_SelCuota != undefined) {
                if (hid_Cuota.length > 0 && hidSubcuota.length > 0 && chk_SelCuota.length > 0) {
                    if (hid_Cuota[0].value == Cuota && hidSubcuota[0].value == SubCuota) {
                        chk_SelCuota[0].childNodes[0].checked = chk_Sel[0].childNodes[0].checked;
                        SumasTotales(rowCuota.parents('tr'));
                    }
                }   
            }
        });
    });
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------





//Eventos Doble Click----------------------------------------------------------------------------------------------------------------------------------
$("body").on("dblclick", "[id*=gvd_Cuotas] .PagoPrima", function () {
    var row = $(this).closest("tr");
    var nro_op = row.find('.Link');
    if ($(nro_op)[0].text == 0) {
        CargaCatalogo('P', row.parents('tr').index(), row.index());
    }
});

$("body").on("dblclick", "[id*=gvd_Cuotas] .PagoComision", function () {
    var row = $(this).closest("tr");
    var nro_op = row.find('.Link');
    if ($(nro_op)[0].text == 0) {
        CargaCatalogo('C', row.parents('tr').index(), row.index());
    }
});

$("body").on("dblclick", "[id*=gvd_Cuotas] .PrcPrima", function () {
    var row = $(this).closest("tr");
    var nro_op = row.find('.Link');
    if ($(nro_op)[0].text == 0) {
        CargaCatalogo('P', row.parents('tr').index(), row.index());
    }
});

$("body").on("dblclick", "[id*=gvd_Cuotas] .PrcCom", function () {
    var row = $(this).closest("tr");
    var nro_op = row.find('.Link');
    if ($(nro_op)[0].text == 0) {
        CargaCatalogo('C', row.parents('tr').index(), row.index());
    }
});

$("body").on("dblclick", "[id*=gvd_OrdenPago] .Cuenta", function () {
    var row = $(this).closest("tr");
    var Moneda = row.find('.Moneda');
    var Persona = row.find('.Persona');

    $("input[id$='hid_Control']")[0].value = row[0].rowIndex;
    $("input[id$='hid_Catalogo']")[0].value = 'Cta';
    $("#lblCatalogo")[0].innerText = 'CUENTAS BANCARIAS';
    var Condicion = ' WHERE id_persona = ' + Persona[0].value + ' AND cod_moneda = ' + Moneda[0].value
    load_Data("spS_CatalogosOP ==Cta==" + ",==" + Condicion + "==", "Unica");
});

$("body").on("click", "[id*=gvd_GrupoPolizas] .MuestraAclaracion", function () {
    event.preventDefault();
    var row = $(this).closest("tr");
    
    var id_pv = row.find('.id_pv');

    Consulta($(id_pv)[0].value);

});
//-----------------------------------------------------------------------------------------------------------------------------------------------------




//Eventos Focus----------------------------------------------------------------------------------------------------------------------------------------
$("body").on("focus", "[id*=gvd_Cuotas] .PrcPrima", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PrcPrima');
    Seleccion($(Control)[0]);
});

$("body").on("focus", "[id*=gvd_Cuotas] .PrcCom", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PrcCom');
    Seleccion($(Control)[0]);
});

$("body").on("focus", "[id*=gvd_Cuotas] .PagoPrima", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PagoPrima');
    Seleccion($(Control)[0]);
});


$("body").on("focus", "[id*=gvd_Cuotas] .PagoComision", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PagoComision');
    Seleccion($(Control)[0]);
});

$("body").on("focus", "[id*=gvd_Cuotas] .Cuota", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Cuota');
    Seleccion($(Control)[0]);
});

$("body").on("focus", "[id*=gvd_Reaseguro] .MontoGenerar", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.MontoGenerar');
    Seleccion($(Control)[0]);
});

//-----------------------------------------------------------------------------------------------------------------------------------------------------




//Eventos FocusOut-------------------------------------------------------------------------------------------------------------------------------------

$("body").on("focusout", ".Fraccionado", function () {
    var Valor = parseFloat($('[id*=txt_MontoFraccionado]')[0].value.replace(",","").replace(",",""))
    $('[id*=txt_MontoFraccionado]')[0].value = Valor.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
});

$("body").on("focusout", "[id*=gvd_Cuotas] .PagoPrima", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PagoPrima');
    var PagoPrima = parseFloat($(Control)[0].value.replace(",", "").replace(",", ""))
    if (isNaN(PagoPrima) == true) {
        $(Control)[0].value = '0.00';
    }
    else {
        $(Control)[0].value = PagoPrima.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    }
});

$("body").on("focusout", "[id*=gvd_Cuotas] .PagoComision", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PagoComision');
    var PagoComision = parseFloat($(Control)[0].value.replace(",", "").replace(",", ""))
    if (isNaN(PagoComision) == true) {
        $(Control)[0].value = '0.00';
    }
    else {
        $(Control)[0].value = PagoComision.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    }
});

$("body").on("focusout", "[id*=gvd_Cuotas] .PrcPrima", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PrcPrima');
    var PrcPrima = parseFloat($(Control)[0].value.replace(",", "").replace(",", ""))
    if (isNaN(PrcPrima) == true) {
        $(Control)[0].value = '0.00';replace
    }
    else {
        $(Control)[0].value = PrcPrima.toFixed(4);
    }
});

$("body").on("focusout", "[id*=gvd_Cuotas] .PrcCom", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.PrcCom');
    var PrcCom = parseFloat($(Control)[0].value.replace(",", "").replace(",", ""))
    if (isNaN(PrcCom) == true) {
        $(Control)[0].value = '0.00';
    }
    else {
        $(Control)[0].value = PrcCom.toFixed(4);
    }
});


$("body").on("focusout", ".cod_ramo", function (e) {
    $(".NroPol").select();
});

$("body").on("focusout", ".desc_ramo", function (e) {
    $(".NroPol").select();
});

$("body").on("focusout", "[id*=gvd_Reaseguro] .MontoGenerar", function () {
    var row = $(this).closest("tr");

    var Control = row.find('.MontoGenerar');

    var MontoGenerar = parseFloat(row.find('.MontoGenerar').val());
    var MontoLimite = row.find('.MontoLimite').text();

    var MontoLimite = parseFloat(MontoLimite.replace(",", "").replace(",", ""));

    if (MontoGenerar > MontoLimite + 10) {
        EvaluaMensaje('Órden de Pago', 'El Monto a generar sobrepasa el Monto Restante por pagar')
        $(Control)[0].value = '0.00';
        return false;
    }
    
    if (isNaN(MontoGenerar) == true) {
        $(Control)[0].value = '0.00';
    }
    else {
        $(Control)[0].value = MontoGenerar.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    }
});

function ReplicaFechas(Valor, Cuota, SubCuota) {
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var gvd_Cuotas = row.find('.gvd_Cuotas tr');
        $(gvd_Cuotas).each(function (e) {
            var rowCuota = $(this).closest("tr");

            var hid_Cuota = rowCuota.find('.nro_cuota_ori');
            var hidSubcuota = rowCuota.find('.nro_cuota_subori');

            var lbl_Fecha = rowCuota.find('.CambioFecha');
            var hid_Cambio = rowCuota.find('.Cambio');

            if (lbl_Fecha != undefined) {
                if (hid_Cuota.length > 0 && hidSubcuota.length > 0 && lbl_Fecha.length > 0) {
                    if (hid_Cuota[0].value == Cuota && hidSubcuota[0].value == SubCuota) {
                        lbl_Fecha[0].value = Valor;
                        $(hid_Cambio)[0].value = 1;
                    }
                }
            }
        });
    });
}

function ReplicaCuotas(Valor, Cuota, SubCuota, Version) {
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var gvd_Cuotas = row.find('.gvd_Cuotas tr');
        $(gvd_Cuotas).each(function (e) {
            var rowCuota = $(this).closest("tr");

            var hid_Cuota = rowCuota.find('.nro_cuota_ori');
            var hidSubcuota = rowCuota.find('.nro_cuota_subori');
            var hid_Version = rowCuota.find('.Version');

            var txt_Cuota = rowCuota.find('.CambioCuota');

            if (txt_Cuota != undefined) {
                if (hid_Cuota.length > 0 && hidSubcuota.length > 0 && hid_Version.length > 0 && txt_Cuota.length > 0) {
                    if (hid_Cuota[0].value == Cuota && hidSubcuota[0].value == SubCuota && hid_Version[0].value == Version) {
                        txt_Cuota[0].value = Valor;
                    }
                }
            }
        });
    });
}

function ReplicaValores(Valor, Clase, Cuota, SubCuota) {
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var gvd_Cuotas = row.find('.gvd_Cuotas tr');
        $(gvd_Cuotas).each(function (e) {
            var rowCuota = $(this).closest("tr");

            var Control = rowCuota.find(Clase);
            var hid_Cuota = rowCuota.find('.nro_cuota_ori');
            var hidSubcuota = rowCuota.find('.nro_cuota_subori');

            if (Control != undefined) {
                if (hid_Cuota.length > 0 && hidSubcuota.length > 0 && Control.length > 0) {
                    if (hid_Cuota[0].value == Cuota && hidSubcuota[0].value == SubCuota) {
                        Control[0].value = Valor;
                        switch (Clase) {
                            case '.PrcPrima':
                                CambioPrcPrima(rowCuota);
                                break;
                            case '.PagoPrima':
                                CambioPrima(rowCuota);
                                break;
                            case '.PrcCom':
                                CambioPrcCom(rowCuota);
                                break;
                            case '.PagoComision':
                                CambioComision(rowCuota);
                                break;
                        }
                    }
                }
            }
        });
    });
}

//-----------------------------------------------------------------------------------------------------------------------------------------------------




//Eventos KeyPress-------------------------------------------------------------------------------------------------------------------------------------
$("body").on("keypress", "[id*=gvd_Cuotas] .PrcPrima", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", "[id*=gvd_Cuotas] .PrcCom", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", "[id*=gvd_Cuotas] .PagoPrima", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", "[id*=gvd_Cuotas] .PagoComision", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", "[id*=gvd_Cuotas] .Cuota", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", ".NroPol", function (e) {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});

$("body").on("keypress", "[id*=gvd_Reaseguro] .MontoGenerar", function () {
    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8) || (keynum == 46))
        return true;

    return /\d/.test(String.fromCharCode(keynum));
});
//-----------------------------------------------------------------------------------------------------------------------------------------------------


function CambioPrcPrima(row){
    var Prc = row.find('.PrcPrima').val();

    var Prima = row.find('.Prima').val();
    var Pago = row.find('.PagoPrima');

    Prima = Prima.replace(",", "").replace(",", "");
    var resultado = Prima * (Prc / 100);

    $(Pago)[0].value = resultado.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    //Replica el valor en Comisión----------------------------------------------------------
    var PrcCom = row.find('.PrcCom')
    $(PrcCom)[0].value = Prc;

    var Comision = row.find('.Comision').val();
    var PagoCom = row.find('.PagoComision');

    Comision = Comision.replace(",", "").replace(",", "");
    var resultadoCom = Comision * (Prc / 100);

    $(PagoCom)[0].value = resultadoCom.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    //--------------------------------------------------------------------------------------

    EvaluaMontos();

    //SumasTotales(row.parents('tr'))
    //ClasificaMonto(row.parents('tr'))
}

function CambioPrima(row) {
    var Prima = row.find('.Prima').val();
    var Pago = row.find('.PagoPrima').val();
    var PagoPrima = row.find('.PagoPrima');

    var Prc = row.find('.PrcPrima');

    Prima = Prima.replace(",", "").replace(",", "");
    Pago = Pago.replace(",", "").replace(",", "");
    var resultado = (Pago * 100) / Prima;

    $(Prc)[0].value = resultado.toFixed(4);

    EvaluaMontos();

    //SumasTotales(row.parents('tr'))
    //ClasificaMonto(row.parents('tr'))
}

function CambioPrcCom(row){
    var Prc = row.find('.PrcCom').val();
    var Comision = row.find('.Comision').val();
    var Pago = row.find('.PagoComision');

    Comision = Comision.replace(",", "").replace(",", "");
    var resultado = Comision * (Prc / 100);

    $(Pago)[0].value = resultado.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    EvaluaMontos();

    //SumasTotales(row.parents('tr'))
    //ClasificaMonto(row.parents('tr'))
}

function CambioComision(row) {
    var Comision = row.find('.Comision').val();
    var Pago = row.find('.PagoComision').val();

    var Prc = row.find('.PrcCom');

    Comision = Comision.replace(",", "").replace(",", "");
    Pago = Pago.replace(",", "").replace(",", "");
    var resultado = (Pago * 100) / Comision;

    $(Prc)[0].value = resultado.toFixed(4);

    EvaluaMontos();

    //SumasTotales(row.parents('tr'))
    //ClasificaMonto(row.parents('tr'))
}

//Eventos KeyUp----------------------------------------------------------------------------------------------------------------------------------------
//Calculo Prima
$("body").on("keyup", "[id*=gvd_Cuotas] .PrcPrima", function () {
    var row = $(this).closest("tr");
    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        ReplicaValores($(this).val(), '.PrcPrima', nro_cuota_ori, nro_cuota_subori);
    }
    else {
        CambioPrcPrima(row);
    }

    return false;
});

//Calculo de Porcentaje Prima 
$("body").on("keyup", "[id*=gvd_Cuotas] .PagoPrima", function () {
    var row = $(this).closest("tr");
    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        ReplicaValores($(this).val(), '.PagoPrima', nro_cuota_ori, nro_cuota_subori);
    }
    else {
        CambioPrima(row);
    }
    return false;
});


//Calculo Comisión
$("body").on("keyup", "[id*=gvd_Cuotas] .PrcCom", function () {
    var row = $(this).closest("tr");
    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        ReplicaValores($(this).val(), '.PrcCom', nro_cuota_ori, nro_cuota_subori);
    }
    else {
        CambioPrcCom(row);
    }
    return false;
});


//Calculo de Porcentaje Comision 
$("body").on("keyup", "[id*=gvd_Cuotas] .PagoComision", function () {
    var row = $(this).closest("tr");
    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        ReplicaValores($(this).val(), '.PagoComision', nro_cuota_ori, nro_cuota_subori);
    }
    else {
        CambioComision(row);
    }
    return false;
});
//-----------------------------------------------------------------------------------------------------------------------------------------------------





//Eventos KeyDown--------------------------------------------------------------------------------------------------------------------------------------
$("body").on("keydown", "[id$=txtSearchCia]", function (e) {
    if (e.which != 13) {
        $("input[id$='txtClaveCia']")[0].value = "";
    }

    $('[id$=txtSearchCia]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "ConsultaBD.asmx/GetCompañias",
                data: "{ 'prefix': '" + request.term + "', 'filtro': ''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('|')[0],
                            val: item.split('|')[1]
                        }
                    }))
                },
                error: function (response) {
                    EvaluaMensaje('JSON', response.responseText);
                },
            });
        },
        select: function (e, i) {
            $("input[id$='txtClaveCia']")[0].value = i.item.val;
        },
        minLength: 1
    });
});

$("body").on("keydown", "[id$=txtSearchRam]", function (e) {

    if (e.which != 13) {
        $("input[id$='txtClaveRam']")[0].value = "";
    }

    $('[id$=txtSearchRam]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "ConsultaBD.asmx/GetRamos",
                data: "{ 'prefix': '" + request.term + "', 'filtro': ''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('|')[0],
                            val: item.split('|')[1]
                        }
                    }))
                },
                error: function (response) {
                    EvaluaMensaje('JSON', response.responseText);
                },
            });
        },
        select: function (e, i) {
            $("input[id$='txtClaveRam']")[0].value = i.item.val;
        },
        minLength: 1
    });
});

$("body").on("keydown", "[id$=txtSearchRamCont]", function (e) {
    if (e.which != 13) {
        $("input[id$='txtClaveRamCont']")[0].value = "";
    }

    $('[id$=txtSearchRamCont]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "ConsultaBD.asmx/GetRamosContables",
                data: "{ 'prefix': '" + request.term + "', 'filtro': ''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('|')[0],
                            val: item.split('|')[1]
                        }
                    }))
                },
                error: function (response) {
                    EvaluaMensaje('JSON', response.responseText);
                },
            });
        },
        select: function (e, i) {
            $("input[id$='txtClaveRamCont']")[0].value = i.item.val;
        },
        minLength: 1
    });
});

$("body").on("keydown", "[id$=txtSearchRamTec]", function (e) {

    if (e.which != 13) {
        $("input[id$='txtClaveRamTec']")[0].value = "";
    }

    $('[id$=txtSearchRamTec]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "ConsultaBD.asmx/GetRamos",
                data: "{ 'prefix': '" + request.term + "', 'filtro': ''}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('|')[0],
                            val: item.split('|')[1]
                        }
                    }))
                },
                error: function (response) {
                    EvaluaMensaje('JSON', response.responseText);
                },
            });
        },
        select: function (e, i) {
            $("input[id$='txtClaveRamTec']")[0].value = i.item.val;
        },
        minLength: 1
    });
});
//-----------------------------------------------------------------------------------------------------------------------------------------------------




//Eventos change---------------------------------------------------------------------------------------------------------------------------------------
//Detecta cambios
$("body").on("change", "[id*=gvd_Cuotas] .CambioFecha", function () {
    var row = $(this).closest("tr");
    
    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        ReplicaFechas($(this).val(), nro_cuota_ori, nro_cuota_subori)
    }
    else {
        var hid_Cambio = row.find('.Cambio');
        $(hid_Cambio)[0].value = 1;
    }
    return false;
});

$("body").on("change", "[id*=gvd_Cuotas] .CambioCuota", function () {
    var row = $(this).closest("tr");

    if ($('[id*=chk_AplicaTodos]')[0].checked == true) {
        var nro_cuota_ori = row.find('.nro_cuota_ori')[0].value;
        var nro_cuota_subori = row.find('.nro_cuota_subori')[0].value;
        var Version = row.find('.Version')[0].value;
        ReplicaCuotas($(this).val(), nro_cuota_ori, nro_cuota_subori, Version)
    }

    return false;
});

//Detecta cambios
$("body").on("change", "[id*=gvd_Cuotas] .PrcPrima", function () {
    var row = $(this).closest("tr");
    var hid_Cambio = row.find('.Cambio');

    $(hid_Cambio)[0].value = 1;
    return false;
});

//Detecta cambios
$("body").on("change", "[id*=gvd_Cuotas] .PagoPrima", function () {
    var row = $(this).closest("tr");
    var hid_Cambio = row.find('.Cambio');

    $(hid_Cambio)[0].value = 1;
    return false;
});

//Detecta cambios
$("body").on("change", "[id*=gvd_Cuotas] .PrcCom", function () {
    var row = $(this).closest("tr");
    var hid_Cambio = row.find('.Cambio');

    $(hid_Cambio)[0].value = 1;
    return false;
});

//Detecta cambios
$("body").on("change", "[id*=gvd_Cuotas] .PagoComision", function () {
    var row = $(this).closest("tr");
    var hid_Cambio = row.find('.Cambio');

    $(hid_Cambio)[0].value = 1;
    return false;
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

//Estado de Tabs de Navegación segun Operación
function EstadoTabs() {
    $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5, 6, 7] });
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------




//Funciones Varias------------------------------------------------------------------------------------------------------------------------------------
function SumasParticipantes() {
    var SumaGenerar = parseFloat(0);
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var MntGenerar = row.find('.MntGenerar');
        if (MntGenerar.length > 0) {
            var Monto = parseFloat(MntGenerar[0].textContent.replace(",", "").replace(",", "").replace(",", ""));
            if (isNaN(Monto) == false) {
                SumaGenerar = SumaGenerar + Monto;
            }
        }
    });
    $('[id*=txt_MontoTotal]')[0].value = SumaGenerar.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
}

function SumasTotales(row) {
    var PriReaTot = row.find('.PriReaTot');
    var PriReaPag = row.find('.PriReaPag');
    var PriReaSaldo = row.find('.PriReaSaldo');
    var MntGenerar = row.find('.MntGenerar');
    
    var gvd_Cuotas = row.find('.gvd_Cuotas tr');

    var PrcPriGen = row.find('.PrcPriGen');
    var PrimaGen = row.find('.PrimaGen');
    var PrcComGen = row.find('.PrcComGen');
    var ComGen = row.find('.ComGen');

    var PrcPriPend = row.find('.PrcPriPend');
    var PrimaPend = row.find('.PrimaPend');
    var PrcComPend = row.find('.PrcComPend');
    var ComPend = row.find('.ComPend');

    var PrcPriTotal = row.find('.PrcPriTotal');
    var PrimaTotal = row.find('.PrimaTotal');
    var PrcComTotal = row.find('.PrcComTotal');
    var ComTotal = row.find('.ComTotal');

    var SumaPrcPriGen = parseFloat(0);
    var SumaPrimaGen = parseFloat(0);
    var SumaPrcComGen = parseFloat(0);
    var SumaComGen = parseFloat(0);

    var SumaPrcPriPend = parseFloat(0);
    var SumaPrimaPend = parseFloat(0);
    var SumaPrcComPend = parseFloat(0);
    var SumaComPend = parseFloat(0);

    var SumaPriTemp = parseFloat(0);

    $(gvd_Cuotas).each(function (e) {
        var rowCuota = $(this).closest("tr");

        var PrcPrima = rowCuota.find('.PrcPrima').val();
        var PagoPrima = rowCuota.find('.PagoPrima').val();
        var PrcCom = rowCuota.find('.PrcCom').val();
        var PagoComision = rowCuota.find('.PagoComision').val();
        var OP = rowCuota.find('.Link').text();

        if (PrcPrima != undefined && PrcPrima != '') {

            if (OP != 0) {
                SumaPrcPriGen = SumaPrcPriGen + parseFloat(PrcPrima);

                PagoPrima = PagoPrima.replace(",", "").replace(",", "").replace(",", "");
                if (PagoPrima == '') { PagoPrima = 0; }
                SumaPrimaGen = SumaPrimaGen + parseFloat(PagoPrima);

                SumaPrcComGen = SumaPrcComGen + parseFloat(PrcCom);

                PagoComision = PagoComision.replace(",", "").replace(",", "").replace(",", "");
                if (PagoComision == '') { PagoComision = 0; }
                SumaComGen = SumaComGen + parseFloat(PagoComision);
            }
            else {
                SumaPrcPriPend = SumaPrcPriPend + parseFloat(PrcPrima);

                PagoPrima = PagoPrima.replace(",", "").replace(",", "").replace(",", "");
                if (PagoPrima == '') { PagoPrima = 0; }
                SumaPrimaPend = SumaPrimaPend + parseFloat(PagoPrima);

                SumaPrcComPend = SumaPrcComPend + parseFloat(PrcCom);

                PagoComision = PagoComision.replace(",", "").replace(",", "").replace(",", "");
                if (PagoComision == '') { PagoComision = 0; }
                SumaComPend = SumaComPend + parseFloat(PagoComision);

                var SelectCuota = rowCuota.find('.SelectCuota');
                if (SelectCuota[0].childNodes[0].checked == true) {
                    SumaPriTemp = SumaPriTemp + (parseFloat(PagoPrima) - parseFloat(PagoComision));
                }
            }
        }
    });

    $(PriReaPag)[0].textContent = (SumaPrimaGen - SumaComGen).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PriReaSaldo)[0].textContent = (parseFloat($(PriReaTot)[0].textContent.replace(",", "").replace(",", "").replace(",", "")) - (SumaPrimaGen - SumaComGen)).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    $(PrcPriGen)[0].value = SumaPrcPriGen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrimaGen)[0].value = SumaPrimaGen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrcComGen)[0].value = SumaPrcComGen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(ComGen)[0].value = SumaComGen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    $(PrcPriPend)[0].value = SumaPrcPriPend.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrimaPend)[0].value = SumaPrimaPend.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrcComPend)[0].value = SumaPrcComPend.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(ComPend)[0].value = SumaComPend.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    $(PrcPriTotal)[0].value = (SumaPrcPriGen + SumaPrcPriPend).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrimaTotal)[0].value = (SumaPrimaGen + SumaPrimaPend).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(PrcComTotal)[0].value = (SumaPrcComGen + SumaPrcComPend).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    $(ComTotal)[0].value = (SumaComGen + SumaComPend).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    $(MntGenerar)[0].textContent = SumaPriTemp.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    SumasParticipantes();
}

function ClasificaMonto(row) {
    var PrimaCedida = parseFloat(row.find('.PrimaCedidaEmitida').val());
    var ComisionCedida = parseFloat(row.find('.ComisionCedidaEmitida').val());
    var gvd_Cuotas = row.find('.gvd_Cuotas tr');

    var PrcPriTotal = row.find('.PrcPriTotal');
    var PrimaTotal = row.find('.PrimaTotal');
    var PrcComTotal = row.find('.PrcComTotal');
    var ComTotal = row.find('.ComTotal');

    var SaldoPrima = PrimaCedida - parseFloat($(PrimaTotal)[0].value.replace(",", "").replace(",", ""));

    if (SaldoPrima <= -10) {
        $(PrimaTotal).css('background-color', 'lightgray');
        $(PrimaTotal).css('color', 'green');
    }
    else if (SaldoPrima >= -9.9 && SaldoPrima <= 9.9) {
        $(PrimaTotal).css('background-color', 'lightgreen');
        $(PrimaTotal).css('color', 'green');
    }
    else {
        $(PrimaTotal).css('background-color', 'white');
        $(PrimaTotal).css('color', '#284775');
    }


    var SaldoComision = ComisionCedida - parseFloat($(ComTotal)[0].value.replace(",", "").replace(",", ""));
    if (SaldoComision <= -10) {
        $(ComTotal).css('background-color', 'lightgray');
        $(ComTotal).css('color', 'green');
    }
    else if (SaldoComision >= -9.9 && SaldoComision <= 9.9) {
        $(ComTotal).css('background-color', 'lightgreen');
        $(ComTotal).css('color', 'green');
    }
    else {
        $(ComTotal).css('background-color', 'white');
        $(ComTotal).css('color', '#284775');
    }


    var PrcPri = parseFloat($(PrcPriTotal)[0].value.replace(",", "").replace(",", ""));

    if ((PrcPri > 100 && PrimaCedida > 0) || (PrcPri > 0 && PrimaCedida == 0)) {
        $(PrcPriTotal).css('background-color', 'lightgray');
        $(PrcPriTotal).css('color', 'green');
    }
    else if ((PrcPri == 100 && PrimaCedida > 0) || (PrcPri == 0 && PrimaCedida == 0)) {
        $(PrcPriTotal).css('background-color', 'lightgreen');
        $(PrcPriTotal).css('color', 'green');
    }
    else {
        $(PrcPriTotal).css('background-color', 'white');
        $(PrcPriTotal).css('color', '#284775');
    }

    var PrcCom = parseFloat($(PrcComTotal)[0].value.replace(",", "").replace(",", ""));

    if ((PrcCom > 100 && ComisionCedida > 0) || (PrcCom > 0 && ComisionCedida == 0)) {
        $(PrcComTotal).css('background-color', 'lightgray');
        $(PrcComTotal).css('color', 'green');
    }
    else if ((PrcCom == 100 && ComisionCedida > 0) || (PrcCom == 0 && ComisionCedida == 0)) {
        $(PrcComTotal).css('background-color', 'lightgreen');
        $(PrcComTotal).css('color', 'green');
    }
    else {
        $(PrcComTotal).css('background-color', 'white');
        $(PrcComTotal).css('color', '#284775');
    }
}

function EvaluaMarco(row) {
    var MarcoExhibiciones = row.find('.MarcoExhibiciones')
    var MarcoCompañias = row.find('.MarcoCompañias')
    var EdoOculta = row.find('.EdoOculta')
    var BotonCierra = row.find('.BotonCierra')
    var BotonAbre = row.find('.BotonAbre')

    if ($(EdoOculta)[0].value == '1') {
        $(MarcoExhibiciones).hide();
        $(MarcoCompañias).show();
        $(BotonCierra).hide();
        $(BotonAbre).show();
    }
    else {
        $(MarcoExhibiciones).show();
        $(MarcoCompañias).hide();
        $(BotonCierra).show();
        $(BotonAbre).hide();
    }
}

//Evalua los montos al momento de la captura
function EvaluaMontos() {
    $("[id*=gvd_CiasXBroker] tr").each(function (e) {
        var row = $(this).closest("tr");
        var PrimaCedida = parseFloat(row.find('.PrimaCedidaEmitida').val());
        var ComisionCedida = parseFloat(row.find('.ComisionCedidaEmitida').val());
        if (PrimaCedida != undefined && ComisionCedida != undefined) {
            if (isNaN(PrimaCedida) == false && isNaN(ComisionCedida) == false) {
                SumasTotales(row);
                ClasificaMonto(row);
                EvaluaMarco(row);
            }
        }
    });
}

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



    $('[data-toggle="popover"]').popover();

    $('.masterTooltip').click(function () {
        // Hover over code
        var title = $(this).attr('title');
        $(this).data('tipText', title).removeAttr('title');
        $('<p class="tooltip"></p>')
        .text(title)
        .appendTo('body')
        .fadeIn('slow');
    }, function () {
        // Hover out code
        $(this).attr('title', $(this).data('tipText'));
        $('.tooltip').remove();
    }).mousemove(function (e) {
        var mousex = e.pageX + 20; //Get X coordinates
        var mousey = e.pageY + 10; //Get Y coordinates
        $('.tooltip')
        .css({ top: mousey, left: mousex })
    });



    $('#CatalogoModal').modal('hide');

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


    $(function () {
        $("#tabs").tabs();
        $("#tabs").addClass('ui-tabs-vertical ui-helper-clearfix');
    });

    /*Funciones que deben ejecutarse al termino de Update panel*/
    EstadoTabs();
    //EvaluaMensaje();
    EstiloGrid();
    EstadoVentanas();
    EstadoGrid("gvd_Broker", "chk_SelBro");
    EstadoGrid("gvd_Compañia", "chk_SelCia");
    EstadoGrid("gvd_Contrato", "chk_SelFac"); 
    EstadoGrid("gvd_Poliza", "chk_SelPol");
    EstadoGrid("gvd_Usuario", "chk_SelUsu");
    EstadoGrid("gvd_Estatus", "chk_SelEst");
    EstadoGrid("gvd_RamoContable", "chk_SelRamC");
    EstadoGrid("gvd_Producto", "chk_SelPro");
    //-------------------------------------------------------------


    $(".Monto").css('text-align', 'right');
    $(".PrcPrima").attr({ maxLength: 8 });
    $(".PrcCom").attr({ maxLength: 8 });
    $(".Id").css('text-align', 'Center');


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




    //Control de tres puntos RAMO----------------------------------------------------------------------------------------------------------------
    $("input[id$='txtClaveRam']").numeric({ decimal: false, negative: false, min: 0, max: 9999 });
    $("input[id$='txtClaveRam']").attr({ maxLength: 4 });
    $("input[id$='txtClaveRam']").css('text-align', 'center');

    //$("input[id$='txtSearchRam']").bind("input", function () {
    //    $("input[id$='txtClaveRam']")[0].value = "";
    //});

    $("input[id$='txtSearchRam']").focus(function () {
        this.select();
    });

    $("input[id$='txtSearchRam']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });


    $("input[id$='txtClaveRam']").bind("input", function () {
        $("input[id$='txtSearchRam']")[0].value = "";
    });

    $("input[id$='txtClaveRam']").focus(function () {
        this.select();
    });

    $("input[id$='txtClaveRam']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("input[id$='txtClaveRam']").focusout(function () {
        var Id = $("input[id$='txtClaveRam']")[0].value;
        if (Id == "") {
            Id = 10000; //Coloca un número de compañia inexistente
        }
        $.ajax({
            url: "ConsultaBD.asmx/GetRamo",
            data: "{ 'Id': " + Id + "}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input[id$='txtSearchRam']")[0].value = data.d;
            },
            error: function (response) {
                EvaluaMensaje('JSON', response.responseText);
            },
        });
    });
    //-----------------------------------------------------------------------------------------------------------------------------------------------

    //Control de tres puntos RAMO CONTABLE--------------------------------------------------------------------------------------------------
    $("input[id$='txtClaveRamCont']").numeric({ decimal: false, negative: false, min: 0, max: 999 });
    $("input[id$='txtClaveRamCont']").attr({ maxLength: 3 });
    $("input[id$='txtClaveRamCont']").css('text-align', 'center');

    $("input[id$='txtSearchRamCont']").focus(function () {
        this.select();
    });

    $("input[id$='txtSearchRamCont']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });


    $("input[id$='txtClaveRamCont']").bind("input", function () {
        $("input[id$='txtSearchRamCont']")[0].value = "";
    });

    $("input[id$='txtClaveRamCont']").focus(function () {
        this.select();
    });

    $("input[id$='txtClaveRamCont']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("input[id$='txtClaveRamCont']").focusout(function () {
        var Id = $("input[id$='txtClaveRamCont']")[0].value;
        if (Id == "") {
            Id = 10000; //Coloca un número de compañia inexistente
        }
        $.ajax({
            url: "ConsultaBD.asmx/GetRamoContable",
            data: "{ 'Id': " + Id + "}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input[id$='txtSearchRamCont']")[0].value = data.d;
            },
            error: function (response) {
                EvaluaMensaje('JSON', response.responseText);
            },
        });
    });
    //-----------------------------------------------------------------------------------------------------------------------------------------------
     

    //Control de tres puntos RAMO TECNICO--------------------------------------------------------------------------------------------------
    $("input[id$='txtClaveRamTec']").numeric({ decimal: false, negative: false, min: 0, max: 999 });
    $("input[id$='txtClaveRamTec']").attr({ maxLength: 3 });
    $("input[id$='txtClaveRamTec']").css('text-align', 'center');

    $("input[id$='txtSearchRamTec']").focus(function () {
        this.select();
    });

    $("input[id$='txtSearchRamTec']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });


    $("input[id$='txtClaveRamTec']").bind("input", function () {
        $("input[id$='txtSearchRamTec']")[0].value = "";
    });

    $("input[id$='txtClaveRamTec']").focus(function () {
        this.select();
    });

    $("input[id$='txtClaveRamTec']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("input[id$='txtClaveRamTec']").focusout(function () {
        var Id = $("input[id$='txtClaveRamTec']")[0].value;
        if (Id == "") {
            Id = 10000; //Coloca un número de compañia inexistente
        }
        $.ajax({
            url: "ConsultaBD.asmx/GetRamo",
            data: "{ 'Id': " + Id + "}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input[id$='txtSearchRamTec']")[0].value = data.d;
            },
            error: function (response) {
                EvaluaMensaje('JSON', response.responseText);
            },
        });
    });
    //-----------------------------------------------------------------------------------------------------------------------------------------------

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


    EvaluaMontos();
    SumasParticipantes();
      
}

  

   
