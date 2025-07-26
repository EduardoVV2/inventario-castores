document.addEventListener("DOMContentLoaded", function () {

    //Llenar datos en el modal para editar cantidad
    const botonesEditar = document.querySelectorAll("button[name='editar']");

    botonesEditar.forEach(boton => {
        boton.addEventListener("click", function (event) {

            const productoId = boton.value;

            document.getElementById("idRestarCantProducto").value = productoId;

        });
    });

});