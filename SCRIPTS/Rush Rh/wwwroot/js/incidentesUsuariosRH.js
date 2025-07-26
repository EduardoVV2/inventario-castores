document.addEventListener("DOMContentLoaded", function () {

    // Botones de rechazar y aprobar para abrir el modal de confirmación y comentarios
    const botonesRechazar = document.querySelectorAll("button[name='rechazada']");
    const botonesAprobar = document.querySelectorAll("button[name='aceptada']");
    const botonesComentarios = document.querySelectorAll("button[name='comentario']");

    botonesRechazar.forEach(boton => {
        boton.addEventListener("click", function () {
            const idIncidente = boton.value;
            const incidenteRechazar = incidentes.find(incidente => incidente.Id === parseInt(idIncidente));
            if(incidenteRechazar!=null){
                if(incidenteRechazar.IdTipoIncidencia==1){
                    document.getElementById("tipoIncidenciaModal").value = "Falta";
                }
                else if(incidenteRechazar.IdTipoIncidencia==2){
                    document.getElementById("tipoIncidenciaModal").value = "Retardo";
                }
                else if(incidenteRechazar.IdTipoIncidencia==3){
                    document.getElementById("tipoIncidenciaModal").value = "Salida Temprana";
                }
                else if(incidenteRechazar.IdTipoIncidencia==4){
                    document.getElementById("tipoIncidenciaModal").value = "Ausencia Temporal";
                }
                document.getElementById("idIncidente").value = incidenteRechazar.Id;
                document.getElementById("descripcionModal").value = incidenteRechazar.Descripcion;
                document.getElementById("usuarioModal").value = incidenteRechazar.NombreUsuario+" "+incidenteRechazar.ApellidoPaternoUsuario+" "+incidenteRechazar.ApellidoMaternoUsuario;
            }

        });
    }
    );
    botonesAprobar.forEach(boton => {
        boton.addEventListener("click", function () {
            const idIncidente = boton.value;
            const incidenteAceptar = incidentes.find(incidente => incidente.Id === parseInt(idIncidente));
            if(incidenteAceptar!=null){
                if(incidenteAceptar.IdTipoIncidencia==1){
                    document.getElementById("aTipoIncidenciaModal").value = "Falta";
                }
                else if(incidenteAceptar.IdTipoIncidencia==2){
                    document.getElementById("aTipoIncidenciaModal").value = "Retardo";
                }
                else if(incidenteAceptar.IdTipoIncidencia==3){
                    document.getElementById("aTipoIncidenciaModal").value = "Salida Temprana";
                }
                else if(incidenteAceptar.IdTipoIncidencia==4){
                    document.getElementById("aTipoIncidenciaModal").value = "Ausencia Temporal";
                }
                document.getElementById("aIdIncidente").value = incidenteAceptar.Id;
                document.getElementById("aDescripcionModal").value = incidenteAceptar.Descripcion;
                document.getElementById("aUsuarioModal").value = incidenteAceptar.NombreUsuario+" "+incidenteAceptar.ApellidoPaternoUsuario+" "+incidenteAceptar.ApellidoMaternoUsuario;
            }
        });
    }
    );
    botonesComentarios.forEach(boton => {
        boton.addEventListener("click", function () {
            const idIncidente = boton.value;
            const incidenteComentario = incidentes.find(incidente => incidente.Id === parseInt(idIncidente));
            if(incidenteComentario!=null){
                if(incidenteComentario.IdTipoIncidencia==1){
                    document.getElementById("cTipoIncidenciaModal").value = "Falta";
                }
                else if(incidenteComentario.IdTipoIncidencia==2){
                    document.getElementById("cTipoIncidenciaModal").value = "Retardo";
                }
                else if(incidenteComentario.IdTipoIncidencia==3){
                    document.getElementById("cTipoIncidenciaModal").value = "Salida Temprana";
                }
                else if(incidenteComentario.IdTipoIncidencia==4){
                    document.getElementById("cTipoIncidenciaModal").value = "Ausencia Temporal";
                }
                if(incidenteComentario.Comentarios!=""){
                    document.getElementById("contenidoComentario").textContent = incidenteComentario.Comentarios;
                }
                else{
                    document.getElementById("contenidoComentario").textContent = "Sin comentarios de la incidencia";
                }
                document.getElementById("cDescripcionModal").value = incidenteComentario.Descripcion;
                document.getElementById("cUsuarioModal").value = incidenteComentario.NombreUsuario+" "+incidenteComentario.ApellidoPaternoUsuario+" "+incidenteComentario.ApellidoMaternoUsuario;
                document.getElementById("cEstado").value = incidenteComentario.Estatus;
            }
        });
    });

});
