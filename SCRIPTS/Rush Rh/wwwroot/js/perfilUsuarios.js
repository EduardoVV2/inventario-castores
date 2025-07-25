//Mostrar una alerta antes de guardar cambios 
document.querySelectorAll('.darBajaBoton').forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault(); // Evita que el formulario se envíe inmediatamente

        const form = button.closest('form'); // Encuentra el formulario más cercano al botón
        if (!form) return; // Si no hay formulario, no hace nada

        // Muestra la alerta de SweetAlert
        Swal.fire({
            title: '¿Estás seguro?',
            text: "Se dará de baja al usuario seleccionado",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, enviar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                //ENVIAR EL FORMULARIO SI ES VALIDO
                if (form.checkValidity()) {
                    form.submit(); // Envía el formulario si es válido
                } else {
                    Swal.fire({
                        position: "center",
                        icon: "error",
                        title: "Por favor, soluciona los problemas para poder guardar",
                        showConfirmButton: true,
                      });
                    // Si el formulario no es válido, mostrar errores
                    form.querySelectorAll(':invalid').forEach(field => {
                        field.classList.add('is-invalid');
                        field.insertAdjacentHTML('afterend', `<div class="invalid-feedback mb-4">${field.validationMessage}</div>`);
                    });
                }
            }
        });
    });
});

function scrollToSectionDown() {
    const section = document.getElementById('sectionDown');
    section.scrollIntoView({ behavior: 'smooth' });
}
function scrollToSectionUp() {
    const section = document.getElementById('sectionUp');
    section.scrollIntoView({ behavior: 'smooth' });
}
function editarDatos() {
    const btnGuardar = document.getElementById('btnGuardar');
    const inputs = document.querySelectorAll('.inputE');
    btnGuardar.classList.remove("hidden");
    inputs.forEach(input => input.removeAttribute("disabled"));
}



document.addEventListener("DOMContentLoaded", function () {

    const departamentoSelect = document.getElementById("departamentoSelect");
    //El 1 es para el select de la pagina inicial, mientras que el anterior es el del modal
    const departamentoSelect1 = document.getElementById("departamentoSelect1");
    const cargoSelects = document.querySelectorAll(".cargo-select");
    const cargoInputs = document.querySelectorAll(".cargoInput");

    // Manejar el evento change del select de departamento
    if (departamentoSelect){
        departamentoSelect.addEventListener("change", function () {
            // Ocultar todos los selects de cargos y eliminar el atributo required
            cargoSelects.forEach(cargo => cargo.classList.add("hidden"));
            cargoInputs.forEach(input => {
                input.removeAttribute("required");
                input.setAttribute("disabled", "disabled");
                input.removeAttribute("name"); // Elimina el atributo name para evitar que se envíe
            });
    
            // Mostrar el select correspondiente al departamento seleccionado en el modal
            const selectedDepartamento = departamentoSelect.value;
            if (selectedDepartamento) {
                const relatedCargo = document.getElementById(`cargo-${selectedDepartamento}`);
                if (relatedCargo) {
                    relatedCargo.classList.remove("hidden");
                    // Agregar el atributo required al select visible
                    relatedCargo.querySelector(".cargoInput").setAttribute("required", "required");
                    relatedCargo.querySelector(".cargoInput").setAttribute("name", "cardex.IdPuesto");
                    relatedCargo.querySelector(".cargoInput").removeAttribute("disabled");
                }
            }
        });
    }

    if (departamentoSelect1){
        const selectedDepartamento1 = departamentoSelect1.value;
        // Ocultar todos los selects de cargos y eliminar el atributo required
        cargoSelects.forEach(cargo => cargo.classList.add("hidden"));
        cargoInputs.forEach(input => {
            input.removeAttribute("required");
            input.setAttribute("disabled", "disabled");
            input.removeAttribute("name"); // Elimina el atributo name para evitar que se envíe
        });
        if (selectedDepartamento1) {
            const relatedCargo = document.getElementById(`cargo-${selectedDepartamento1}`);
            if (relatedCargo) {
                relatedCargo.classList.remove("hidden");
                // Agregar el atributo required al select visible
                relatedCargo.querySelector(".cargoInput").setAttribute("required", "required");
                relatedCargo.querySelector(".cargoInput").setAttribute("name", "cardex.IdPuesto");
            }
        }
    
        // Manejar el evento change del select de departamento
        departamentoSelect1.addEventListener("change", function () {
            // Ocultar todos los selects de cargos y eliminar el atributo required
            cargoSelects.forEach(cargo => cargo.classList.add("hidden"));
            cargoInputs.forEach(input => {
                input.removeAttribute("required");
                input.setAttribute("disabled", "disabled");
                input.removeAttribute("required");
                input.removeAttribute("name"); // Elimina el atributo name para evitar que se envíe
            });
    
            // Mostrar el select correspondiente al departamento seleccionado
            var selectDepartamento1 = departamentoSelect1.value;
            if (selectDepartamento1) {
                const relatedCargo = document.getElementById(`cargo-${selectDepartamento1}`);
                if (relatedCargo) {
                    relatedCargo.classList.remove("hidden");
                    // Agregar el atributo required al select visible
                    relatedCargo.querySelector(".cargoInput").setAttribute("required", "required");
                    relatedCargo.querySelector(".cargoInput").setAttribute("name", "cardex.IdPuesto");
                    relatedCargo.querySelector(".cargoInput").removeAttribute("disabled");
                }
            }
        });

    }
    
});
//Parametros para cambio de contraseña
function validarContrasena(password) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/;
    return regex.test(password);
}

function validarTodo() {
    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;
    const mensaje = document.getElementById("mensaje");
    const mensajeConfirmacion = document.getElementById("mensajeConfirmacion");
    const boton = document.getElementById("btnConfirmarContrasenia");

    let passwordValida = validarContrasena(password);
    let contrasenasCoinciden = password === confirmPassword && confirmPassword !== "";

    if (passwordValida) {
        mensaje.textContent = "✅ La contraseña es válida.";
        mensaje.className = "success";
    } else {
        mensaje.textContent = "❌ La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas y un número.";
        mensaje.className = "error";
    }

    if (contrasenasCoinciden) {
        mensajeConfirmacion.textContent = "✅ Las contraseñas coinciden.";
        mensajeConfirmacion.className = "success";
    } else {
        mensajeConfirmacion.textContent = "❌ Las contraseñas no coinciden.";
        mensajeConfirmacion.className = "error";
    }

    // Activar botón solo si todo está bien
    boton.disabled = !(passwordValida && contrasenasCoinciden);
}