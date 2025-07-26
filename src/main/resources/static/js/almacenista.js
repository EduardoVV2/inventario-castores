document.addEventListener("DOMContentLoaded", function () {

    //Llenar datos en el modal para editar cantidad
    const botonesEditar = document.querySelectorAll("button[name='editar']");

    botonesEditar.forEach(boton => {
        boton.addEventListener("click", function (event) {

            const productoId = boton.value;

            document.getElementById("idRestarCantProducto").value = productoId;

        });
    });

    //Alerta
    const alertDiv = document.getElementById("alert-data");
    const message = alertDiv.dataset.message;
    const messageType = alertDiv.dataset.messagetype;

    if (message != null) {
        Swal.fire({
            title: messageType === "success" ? "¡Éxito!" :
                messageType === "error" ? "¡Error!" : "¡Aviso!",
            text: message,
            icon: messageType,
            confirmButtonText: "Aceptar"
        });
    }

});