// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {
    // Capturar el evento submit del formulario
    $("#formRegistro").on("submit", function (event) {
        event.preventDefault(); // Evitar el envío predeterminado del formulario
        registraUsuarios(); // Llama a la función para manejar la lógica de AJAX
    });
});

// Write your JavaScript code.
function registraUsuarios (){
    var Nombre = $("#nombres").val();
    var ApellidoMaterno = $("#ApellidoMaterno").val();
    var ApellidoPaterno = $("#apellidoPaterno").val();
    var FechaNacimiento = $("#fechanacimiento").val();
    var IdGenero = $("#genero").val();
    var IdSexo = $("#sexo").val();
    var RFC = $("#rfc").val();
    var CURP = $("#curp").val();
    var Nacionalidad = $("#nacionalidad").val();
    var IdEstadoCivil = $("#estadocivil").val();
    var Contraseña = $("#contraseña").val();
    var Nick = $("#nick").val();

    // Validar que todos los campos estén llenos
    if (
        !Nombre || !ApellidoMaterno || !ApellidoPaterno ||
        !FechaNacimiento || !IdGenero || !IdSexo ||
        !RFC || !CURP || !Nacionalidad ||
        !IdEstadoCivil 
    ) {
        alert("Por favor, completa todos los campos antes de enviar el formulario.");
        return false; // Detener la ejecución si falta algún dato
    }
    
    var Info = { 
        Nombre : Nombre,
        ApellidoMaterno : ApellidoMaterno,
        ApellidoPaterno: ApellidoPaterno,
        FechaNacimiento: FechaNacimiento,
        IdGenero: IdGenero,
        IdSexo: IdSexo,
        RFC: RFC,
        CURP: CURP,
        Nacionalidad: Nacionalidad,
        IdEstadoCivil: IdEstadoCivil,
        Contraseña: Contraseña,
        Nick: Nick,
    }
    $.ajax({
        type: "POST",
        url: '/Registro?handler=RegistrarUsuario',
        data: JSON.stringify(Info),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        console.log("funciona")
    })
    return false;
}

