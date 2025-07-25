function login(){
    var x = {
        usuario: $("#usuario").val(),
        password: $("#password").val()
    }
    $.ajax({
        type: "POST",
        url: '/Login?handler=DoLogin',
        data: JSON.stringify(x),
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


// Función para mostrar/ocultar la contraseña
const togglePassword = document.querySelector('#toggle-password');
const password = document.querySelector('#Password');
const imagenOjoAbierto = document.querySelector('#imagenOjoAbierto');
const imagenOjoCerrado = document.querySelector('#imagenOjoCerrado');
togglePassword.addEventListener('click', function (e) {
    // Cambiar el tipo de input
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    // Cambiar el icono del ojito
    if (type === 'password') {
        imagenOjoAbierto.classList.remove('visible');
        imagenOjoAbierto.classList.add('no-visible');
        imagenOjoCerrado.classList.remove('no-visible');
        imagenOjoCerrado.classList.add('visible');
    } else {
        imagenOjoAbierto.classList.remove('no-visible');
        imagenOjoAbierto.classList.add('visible');
        imagenOjoCerrado.classList.remove('visible');
        imagenOjoCerrado.classList.add('no-visible');
    }
});
