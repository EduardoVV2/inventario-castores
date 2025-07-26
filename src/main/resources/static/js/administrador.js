document.addEventListener("DOMContentLoaded", function () {
    
    //Llenar datos en el modal para editar cantidad
    const botonesEditar = document.querySelectorAll("button[name='editar']");

    botonesEditar.forEach(boton => {
        boton.addEventListener("click", function (event) {

            const productoId = boton.value;

            document.getElementById("idAgregarCantProducto").value = productoId;

        });
    });

    // Dar de Baja
    document.querySelectorAll('.btnBaja').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.getAttribute('data-id');
            document.getElementById('inputBaja').value = id;
            document.getElementById('formBaja').submit();
        });
    });

    // Reactivar
    document.querySelectorAll('.btnReactivar').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.getAttribute('data-id');
            document.getElementById('inputReactivar').value = id;
            document.getElementById('formReactivar').submit();
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