// Función para mostrar los selects secundarios según el valor seleccionado en el select principal

document.addEventListener("DOMContentLoaded", function () {
    const tipoIncidente = document.getElementById("TipoIncidente");
    const horaRetardo = document.getElementById("horaRetardo");
    const horaSalidaTemprana = document.getElementById("horaSalidaTemprana");
    const horaAusenciaTemporalInicio = document.getElementById("horaAusenciaInicio");
    const horaAusenciaTemporalFin = document.getElementById("horaAusenciaFin");

    if (tipoIncidente) {
        tipoIncidente.addEventListener("change", function () {
            const selectedValue = tipoIncidente.value; // Obtener el valor seleccionado

            horaRetardo.removeAttribute("required");
            horaSalidaTemprana.removeAttribute("required");
            horaAusenciaTemporalInicio.removeAttribute("required");
            horaAusenciaTemporalFin.removeAttribute("required");
            horaRetardo.removeAttribute("name");
            horaSalidaTemprana.removeAttribute("name");
            horaAusenciaTemporalInicio.removeAttribute("name");
            horaAusenciaTemporalFin.removeAttribute("name");

            const containers = {
                2: document.getElementById("Retardo"),
                3: document.getElementById("Salida_temprana"),
                4: document.getElementById("Ausencia_temporal"),
            };

            // Reiniciar visibilidad
            Object.values(containers).forEach(container => {
                if (container) container.style.display = "none";
            });

            // Mostrar el contenedor correspondiente si existe
            if (containers[selectedValue]) {
                containers[selectedValue].style.display = "block";
                if (selectedValue === "2") {
                    horaRetardo.setAttribute("required", "required");
                    horaRetardo.setAttribute("name", "incidencia.HoraEntrada");
                }
                else if (selectedValue === "3") {
                    horaSalidaTemprana.setAttribute("required", "required");
                    horaSalidaTemprana.setAttribute("name", "incidencia.HoraSalida");
                }
                else if (selectedValue === "4") {
                    horaAusenciaTemporalInicio.setAttribute("required", "required");
                    horaAusenciaTemporalFin.setAttribute("required", "required");
                    horaAusenciaTemporalInicio.setAttribute("name", "incidencia.HoraSalida");
                    horaAusenciaTemporalFin.setAttribute("name", "incidencia.HoraEntrada");
                }
            }
        });
    }

    const botonesComentarios = document.querySelectorAll("div[data-name='comentario']");
    botonesComentarios.forEach(boton => {
        boton.addEventListener("click", function () {
            const idIncidente = parseInt(boton.dataset.value);
            const incidenteComentario = incidentes.find(incidente => incidente.Id === parseInt(idIncidente));
            if(incidenteComentario!=null){
                if(incidenteComentario.Comentarios!=""){
                    document.getElementById("contenidoComentario").textContent = incidenteComentario.Comentarios;
                }
                else{
                    document.getElementById("contenidoComentario").textContent = "Sin comentarios de la incidencia";
                }
                document.getElementById("cEstado").textContent = incidenteComentario.Estatus;
            }
        });
    });
});
