$(document).ready(function () {
    PageLoadOrden();
});

function ImprimirOrden(Server, strOrden) {
    var nro_op = strOrden.split(",");
    for (i = 0 ; i < nro_op.length; i++) {
        window.open(Server.replace('@nro_op', nro_op[i]));
    }
}

function ClosePopup(Popup) {
    $(Popup).modal('hide');
}

function SeleccionGread(Control, Valor) {
    //Get target base & child control.
    var TargetChildControl = "chk_SelCta";

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
function CambioSeleccion(Control) {
    //Get target base & child control.

    var row = $(Control).closest("tr");

    var Gread = document.getElementById($('[id$=gvd_CuentasBancarias]')[0].id)

    //Evalua el tipo de seleccion
    SeleccionGread(Gread, false)
    SeleccionarElemento(row[0].rowIndex)
    return false;
}



//Selecciona solo un elemento en caso de ser seleccion Unica
function SeleccionarElemento(rowIndex) {
    $("[id*=gvd_CuentasBancarias] tr").each(function (e) {
        var row = $(this).closest("tr");
        if (row[0].rowIndex == rowIndex) {
            var Select = row.find('.Select');
            $(Select).attr('checked', true)
        }
    })
    return false;
}

//-----------------------------------------------------------------------------------------------------------------------


function SumasTotales() {
    var Suma = 0;

    $("[id*=gvd_Detalle] tr").each(function (e) {
        var row = $(this).closest("tr");
        var Monto = row.find('.Monto').val();
        var cod_deb_cred = row.find('.cod_deb_cred').val();

        if (Monto != undefined) {
            Monto = Monto.replace(",", "").replace(",", "").replace(",", "");
            if (cod_deb_cred == 'D') {
                Suma = Suma + parseFloat(Monto);
            }
            else {
                Suma = Suma - parseFloat(Monto);
            }
        }
    });
    $("input[id$='txt_MontoTotal']")[0].value = Suma.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');;
}

//Estado si existe Mensaje a Desplegar
function EvaluaMensaje(Titulo, Mensaje) {
    $('[id*=lbl_TitMensaje]')[0].innerText = Titulo;
    $('[id*=txt_Mensaje]')[0].innerText = Mensaje;
    $('#MensajeModal').modal('show');
    $('#MensajeModal').draggable();
}


function Seleccion(Control) {
    $(Control).focus(function () {
        this.select();
    });
}

$("body").on("focus", "[id*=gvd_Detalle] .Descripcion", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Descripcion');
    Seleccion($(Control)[0]);
});

$("body").on("focus", "[id*=gvd_Detalle] .Prc", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Prc');
    Seleccion($(Control)[0]);
});


$("body").on("focus", "[id*=gvd_Detalle] .Monto", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Monto');
    Seleccion($(Control)[0]);
});

//Calculo de Monto  
$("body").on("keyup", "[id*=gvd_Detalle] .Prc", function () {
    var row = $(this).closest("tr");

    var Prc = row.find('.Prc').val();

    var Total = row.find('.prima_cedida').val();


    var Monto = row.find('.Monto');

    Total = Total.replace(",", "").replace(",", "").replace(",", "");
    var resultado = Total * (Prc / 100);

    $(Monto)[0].value = resultado.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    SumasTotales();

    return false;
});

//Calculo de Porcentaje  
$("body").on("keyup", "[id*=gvd_Detalle] .Monto", function () {
    var row = $(this).closest("tr");

    var Total = row.find('.prima_cedida').val();

    var Monto = row.find('.Monto').val();

    var Prc = row.find('.Prc');

    Total = Total.replace(",", "").replace(",", "").replace(",", "");
    Monto = Monto.replace(",", "").replace(",", "").replace(",", "");
    var resultado = (Monto * 100) / Total;

    $(Prc)[0].value = resultado.toFixed(2);

    SumasTotales();

    return false;
});


$("body").on("focusout", "[id*=gvd_Detalle] .Monto", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Monto');
    var Monto = parseFloat($(Control)[0].value)
    $(Control)[0].value = Monto.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
});

$("body").on("focusout", "[id*=gvd_Detalle] .Prc", function () {
    var row = $(this).closest("tr");
    var Control = row.find('.Prc');
    var Prc = parseFloat($(Control)[0].value)
    $(Control)[0].value = Prc.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
});

$("body").on("click", "[id*=btn_Cerrar]", function () {
    event.preventDefault();
    window.close()
    return false;
});


$("body").on("click", "[id*=btn_Texto]", function () {
    event.preventDefault();
});

$("body").on("click", "[id*=btn_CuentasBancarias]", function () {
    event.preventDefault();
});

function PageLoadOrden() {
    //EvaluaMensaje();

    SumasTotales()

    $(".Monto").css('text-align', 'right');

    $("input[id$='txt_Orden']").focus(function () {
        this.select();
    });

    $("input[id$='txt_Orden']").numeric({ decimal: false, negative: false, min: 0, max: 9999 });
    $("input[id$='txt_Orden']").attr({ maxLength: 6 });
    $("input[id$='txt_Orden']").css('text-align', 'right');


    //Control de tres puntos COMPAÑIA----------------------------------------------------------------------------------------------------------------
    $("input[id$='txtClaveCia']").numeric({ decimal: false, negative: false, min: 0, max: 9999 });
    $("input[id$='txtClaveCia']").attr({ maxLength: 4 });
    $("input[id$='txtClaveCia']").css('text-align', 'center');

    //$("input[id$='txtSearchCia']").bind("input", function () {
    //    $("input[id$='txtClaveCia']")[0].value = "";
    //});

    $("input[id$='txtSearchCia']").focus(function () {
        this.select();
    });

    $("input[id$='txtSearchCia']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("input[id$='txtClaveCia']").bind("input", function () {
        $("input[id$='txtSearchCia']")[0].value = "";
    });

    $("input[id$='txtClaveCia']").focus(function () {
        this.select();
    });

    $("input[id$='txtClaveCia']").bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("input[id$='txtClaveCia']").focusout(function () {
        var Id = $("input[id$='txtClaveCia']")[0].value;
        if (Id == "") {
            Id = 10000; //Coloca un número de compañia inexistente
        }
        $.ajax({
            url: "ConsultaBD.asmx/GetCompañia",
            data: "{ 'Id': " + Id + "}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("input[id$='txtSearchCia']")[0].value = data.d;
            },
            error: function (response) {
                alert(response.responseText);
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
}