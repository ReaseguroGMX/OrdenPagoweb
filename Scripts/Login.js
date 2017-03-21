$(window).bind("pageshow", function () {
    var form = $('#frmMaster');
    //form[0].reset();
});

//Usuario
//$("body").on("keyup", ".Usuario", function () {
//    $("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
//});

//$("body").on("keyup", ".Contraseña", function () {
//    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
//});


$(document).keypress(function (e) {
    if (e.which == 13) {
        e.preventDefault();
        //$("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
        //$("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
        //$('.submit').click();
        $("input[id$='hid_Mensaje']")[0].value = 'Debe presionar el botón Ingresar';
        EvaluaMensaje();
    }
});

$("body").on("click", ".submit", function () {
    $("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
});

//Usuario
$("body").on("change", ".Usuario", function () {
    $("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
});

$("body").on("change", ".Contraseña", function () {
    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
});

$("body").on("keypress", ".Contraseña", function () {
    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
});

$("body").on("focusout", ".Usuario", function () {
    $("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
});

$("body").on("focusout", ".Contraseña", function () {
    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;
});


//Estado si existe Mensaje a Desplegar
function EvaluaMensaje() {
    if ($("input[id$='hid_Mensaje']")[0].value != '') {
        $('#Mensaje')[0].innerText = $("input[id$='hid_Mensaje']")[0].value;
        $('#MensajeModal').modal('show');
        $('#MensajeModal').draggable();
        $("input[id$='hid_Mensaje']")[0].value = '';
    }
   
}

$(document).ready(function () {

    EvaluaMensaje();

    $(".input").val("");

    $(".Usuario")[0].value = $("input[id$='hid_usuario']")[0].value;

    //$("input[id$='hid_usuario']")[0].value = "";

    $(".Contraseña")[0].value = "";


    $("input[id$='hid_usuario']")[0].value = $(".Usuario")[0].value;
    $("input[id$='hid_contraseña']")[0].value = $(".Contraseña")[0].value;

    $('#frmMaster').bootstrapValidator({
        container: '#messages',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
        },
        fields: {
            Usuario: {
                validators: {
                    notEmpty: {
                        message: 'Debe especificar su Usuario'
                    }
                }
            },
            Contraseña: {
                validators: {
                    notEmpty: {
                        message: 'Debe especificar su Contraseña'
                    },
                }
            },
        },
    });
});




