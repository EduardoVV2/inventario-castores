document.addEventListener("DOMContentLoaded", function(){

    // Selecciona todos los botones de eliminación
    const botonesEliminar = document.querySelectorAll(".btn-eliminar");

    botonesEliminar.forEach((boton) => {
        boton.addEventListener("click", function (event) {
            // Evita que el formulario se envíe automáticamente
            event.preventDefault();

            // Obtén el formulario relacionado con este botón
            const form = boton.closest("form");

            // Muestra la alerta de confirmación
            Swal.fire({
                title: "¿Estás seguro de eliminarla?",
                text: "Esta acción eliminará tu solicitud de forma permanente.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar",
            }).then((result) => {
                if (result.isConfirmed) {
                    // Si el usuario confirma, envía el formulario
                    form.submit();
                }
            });
        });
    });



    // Selecciona todos los botones de enviar
    const botonesEnviar = document.querySelectorAll(".btn-enviar");

    botonesEnviar.forEach((boton) => {
        boton.addEventListener("click", function (event) {
            // Evita que el formulario se envíe automáticamente
            event.preventDefault();

            // Obtén el formulario relacionado con este botón
            const form = boton.closest("form");

            // Muestra la alerta de confirmación
            Swal.fire({
                title: "¿Estás seguro de enviarla?",
                text: "Despues de esto tu soliciud no se podra anular.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, enviar",
                cancelButtonText: "Cancelar",
            }).then((result) => {
                if (result.isConfirmed) {
                    // Si el usuario confirma, envía el formulario
                    form.submit();
                }
            });
        });
    });
});


function descargarPdfVacacion(button) {

    var idVacacion = document.getElementById("vacacionIdSeleccionada").value;
    
    if (!idVacacion) {
        alert("No se ha seleccionado una vacación.");
        return;
    }

    
    // Redirigir a la pantalla VerSolicitudPdf.cshtml con el ID en la URL
    window.location.href = `/VerSolicitudPdf?tipo=Vacaciones&id=${idVacacion}`;
}


function descargarPdfLicencia(button) {

    var idLicencia = document.getElementById("licenciaIdSeleccionada").value;
    
    if (!idLicencia) {
        alert("No se ha seleccionado una licencia.");
        return;
    }

    
    window.location.href = `/VerSolicitudPdf?tipo=Licencias&id=${idLicencia}`;
}



function cargarDetalleVacacion(button) {
    
    const idVacacion = button.getAttribute("data-id"); // Obtén el ID del botón
    

    const inputVacacion = document.getElementById("IdVacacionConfirmar");

    if (!inputVacacion) {
        console.error("El input oculto 'vacacionIdSeleccionada' no está en el DOM.");
        return;
    }

    inputVacacion.value = idVacacion; // Guardarlo en el input oculto


    $.ajax({
        url: '/PermisoLicencias?handler=DetalleVacacion', // Ruta al método del backend
        type: 'GET',
        data: { id: idVacacion }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#diasUsadosVacaciones').val(data.diasSolicitados).prop('readonly', true);
                $('#diasUsadosVacaiones').show();
                $('#diasDisponiblesVacaciones').hide();
                $('#estatusVacacion').val(data.estatus).prop('readonly', true);
                $('#VacacionEstatus').show();
                $('#fechaDeVacacion').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaVacacion').val(data.fechaFin).prop('readonly', true);
                $('#fechaReincorporacionVacacion').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudVacacion').val(data.fechaSolicitud).prop('readonly', true);
                $('#DetallesVacaciones').show();
                $('#ReincorporacionVacaciones').show();
                $('#comentarioVacaciones').val(data.comentarios).prop('readonly', true);
                /* $('#jefeInmediatoVacaciones').val(data.idJefeDirecto).change().prop('disabled', true); */
                $('#botonCrearVacacion').hide();
                $('#SeccionJefeDirecto').hide();
                $('#SeccionJefeDirectoParaVcaciones').hide();
                $('#jefeInmediatoParaVacacion').val(data.jefe).prop('readonly', true);

                $('#jefeInmediatoVacaciones').val(data.NombreCompletoJefe).prop('disabled', true);
                $('#nombreUsuarioSolicitante').val(data.NombreCompletoUsuario).prop('readonly', true);

                if(data.estatus == "Aceptada por jefe directo" || data.estatus == "Rechazada por jefe directo"){
                    $('#UsuarioComentariosJefeVacaciones').show();
                    $('#comentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);

                    $('#UsuarioComentariosRHVacaciones').hide();
                    $('#UsuarioComentariosGerenteVacaciones').hide();
                }else if(data.estatus == "Aceptada por RH" || data.estatus == "Rechazada por RH" ){
                    if(data.comentariosJefe){
                        $('#UsuarioComentariosJefeVacaciones').show();
                        $('#comentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosJefeVacaciones').hide();
                    }
                    
                    $('#UsuarioComentariosRHVacaciones').show();
                    $('#comentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);

                    $('#UsuarioComentariosGerenteVacaciones').hide();
                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.comentariosJefe){
                        $('#UsuarioComentariosJefeVacaciones').show();
                        $('#comentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosJefeVacaciones').hide();
                    }
                    
                    if(data.comentariosRH){
                        $('#UsuarioComentariosRHVacaciones').show();
                        $('#comentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosRHVacaciones').hide();
                    }
                    
                    $('#UsuarioComentariosGerenteVacaciones').show();
                    $('#comentarioVacacionesGerente').val(data.comentariosGerente).prop('readonly', true);
                }else{
                    $('#UsuarioComentariosJefeVacaciones').hide();
                    $('#UsuarioComentariosRHVacaciones').hide();
                    $('#UsuarioComentariosGerenteVacaciones').hide();
                }
                
                $('.btn-solicitud').show();

                $('#tituloModalVacacionesDetalle').show();
                $('#tituloModalVacaciones').hide();

                $('#textoDiasSabadosDetalle').show();
                $('#textoDiasSabados').hide();
                
               
                $('#diasUsados').val(data.diasUsados).prop('readonly', true);
                // Muestra el modal
                $('#modalVacaciones').show();
            } else {
                alert("No se encontró información para la vacación seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la vacación.");
        }
    });
}



function cargarDetalleLicencia(button) {

    const idLicencia = button.getAttribute("data-id"); // Obtén el ID del botón
    
    const inputLicencia = document.getElementById("licenciaIdSeleccionada");

    if (!inputLicencia) {
        console.error("El input oculto 'licenciaIdSeleccionada' no está en el DOM.");
        return;
    }

    inputLicencia.value = idLicencia; // Guardarlo en el input oculto

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleLicencia', // Ruta al método del backend
        type: 'GET',
        data: { id: idLicencia }, // Enviar el ID como parámetro
        success: function (data) {
            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#jefeInmediatoLicencias').val(data.idJefe).prop('disabled', true);
                $('#tipoLicencia').val(data.idTipoLicencia).prop('disabled', true);
                $('#fechaDeLicencia').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaLicencia').val(data.fechaFin).prop('readonly', true);
                $('#comentarioLicencias').val(data.comentarios).prop('readonly', true);
                $('#fechaReincorporacionLicencia').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudLicencia').val(data.fechaSolicitud).prop('readonly', true);
                $('#estatuslicencia').val(data.estatus).prop('readonly', true);

                if(data.estatus == "Aceptada por jefe directo" || data.estatus == "Rechazada por jefe directo"){
                    $('#UsuarioComentariosJefeLicencias').show();
                    $('#comentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                
                    $('#UsuarioComentariosRHLicencias').hide();
                    $('#UsuarioComentariosGerenteLicencias').hide();
                }else if(data.estatus == "Aceptada por RH" || data.estatus == "Rechazada por RH" ){
                    if(data.comentariosJefe){
                        $('#UsuarioComentariosJefeLicencias').show();
                        $('#comentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosJefeLicencias').hide();
                    }

                    $('#UsuarioComentariosRHLicencias').show();
                    $('#comentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);
                
                    $('#UsuarioComentariosGerenteLicencias').hide();
                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.comentariosJefe){
                        $('#UsuarioComentariosJefeLicencias').show();
                        $('#comentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosJefeLicencias').hide();
                    }
                    
                    if(data.comentariosRH){
                        $('#UsuarioComentariosRHLicencias').show();
                        $('#comentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#UsuarioComentariosRHLicencias').hide();
                    }
                   
                    $('#UsuarioComentariosGerenteLicencias').show();
                    $('#comentarioLicenciasGerente').val(data.comentariosGerente).prop('readonly', true);
                }else{
                    $('#UsuarioComentariosJefeLicencias').hide();
                    $('#UsuarioComentariosRHLicencias').hide();
                    $('#UsuarioComentariosGerenteLicencias').hide();
                }

                $('#FechasReincorporacionSolicitud').show();
                $('#botonCrearLicencias').hide();
                $('.btn-solicitud').show();

                $('#tituloModalLicenciaDetalle').show(); 
                $('#tituloModalLicencia').hide(); 

                $('#textoDiasSabadosDetalle').show();
                $('#textoDiasSabados').hide();
                $('#textoDiasSabadosJefes').hide();

                // Muestra el modal
                $('#modalLicencia').show();
                
            } else {
                alert("No se encontró información para la licencia seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la licencia.");
        }
    });
}

function cargarDetalleLicenciaJefe(button) {
    const idLicencia = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleLicenciaConfirmarJefe', // Ruta al método del backend
        type: 'GET',
        data: { id: idLicencia }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#JefeYTipoLicencia').hide()
                $('#JefeYTipoLicenciaParaConfirmar').show()
                $('#JefeYTipoLicenciaSecciondeConfirmacion').hide()
                $('#SeccionjefeInmediatoConfirmar').hide();
                $('#UsuarioLicencia').val(data.usuario).prop('readonly', true);
                $('#tipoLicenciaConfirmar').val(data.tipoLicencia).prop('readonly', true);;
                $('#fechaDeLicencia').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaLicencia').val(data.fechaFin).prop('readonly', true);
                $('#comentarioLicencias').val(data.comentarios).prop('readonly', true);
                $('#fechaReincorporacionLicencia').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudLicencia').val(data.fechaSolicitud).prop('readonly', true);
                $('#estatuslicencia').val(data.estatus).prop('readonly', true);
                $('#idLicenciaConfirmar').val(data.id);

                $('#FechasReincorporacionSolicitud').show();
                $('#botonCrearLicencias').hide();

                $('#tituloModalLicenciaDetalle').show(); 
                $('#tituloModalLicencia').hide(); 

                $('#textoDiasSabadosDetalle').hide();
                $('#textoDiasSabados').hide();
                $('#textoDiasSabadosJefes').show();

                // Muestra el modal
                $('#modalLicencia').show();
            } else {
                alert("No se encontró información para la licencia seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la licencia.");
        }
    });
}




function cargarDetalleLicenciaRH(button) {
    const idLicencia = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleLicenciaConfirmarRHGerente', // Ruta al método del backend
        type: 'GET',
        data: { id: idLicencia }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                
                $('#JefeYTipoLicencia').hide()
                $('#JefeYTipoLicenciaParaConfirmar').show()
                $('#JefeYTipoLicenciaSecciondeConfirmacion').hide()
                $('#jefeInmediatoConfirmar').val(data.jefe).prop('readonly', true);
                $('#UsuarioLicencia').val(data.usuario).prop('readonly', true);
                $('#tipoLicenciaConfirmar').val(data.tipoLicencia).prop('readonly', true);;
                $('#fechaDeLicencia').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaLicencia').val(data.fechaFin).prop('readonly', true);
                $('#comentarioLicencias').val(data.comentarios).prop('readonly', true);
                $('#fechaReincorporacionLicencia').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudLicencia').val(data.fechaSolicitud).prop('readonly', true);
                $('#estatuslicencia').val(data.estatus).prop('readonly', true);
                $('#idLicenciaConfirmar').val(data.id);

                if(data.estatus == "Enviada"){
                    if(data.quienAprueba == "RH es Jefe Directo"){
                        $('#RHComentariosRHLicencias').show();

                        $('#SeccionBotonesConfirmarLicenciaRH').show();
                        $('#SeccionBotonesCerrarLicenciaRH').hide();
                    }else{
                        $('#RHComentariosJefeLicencias').hide();
                        $('#RHComentariosRHLicencias').hide();
                        $('#RHComentariosGerenteLicencias').hide();

                        $('#SeccionBotonesConfirmarLicenciaRH').hide();
                        $('#SeccionBotonesCerrarLicenciaRH').show();
                    }
                }
                else if(data.estatus == "Vencida"){
                    if(data.comentariosJefe){
                        $('#RHComentariosJefeLicencias').show();
                        $('#RHcomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeLicencias').hide();
                    }

                    if(data.comentariosRH){
                        $('#RHComentariosRHLicencias').show();
                        $('#RHcomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);           
                    }else{
                        $('#RHComentariosRHLicencias').hide();
                    }

                    if(data.comentariosGerente){
                        $('#RHComentariosGerenteLicencias').show();
                        $('#RHcomentarioLicenciasGerente').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#RHComentariosGerenteLicencias').hide();
                    }

                    $('#SeccionBotonesConfirmarLicenciaRH').hide();
                    $('#SeccionBotonesCerrarLicenciaRH').show();
                }else if(data.estatus == "Aceptada por jefe directo"){
                    $('#RHComentariosJefeLicencias').show();
                    $('#RHcomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);

                    $('#RHComentariosRHLicencias').show();

                    $('#RHComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaRH').show();
                    $('#SeccionBotonesCerrarLicenciaRH').hide();
                }else if(data.estatus == "Rechazada por jefe directo"){
                    $('#RHComentariosJefeLicencias').show();
                    $('#RHcomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    
                    $('#RHComentariosRHLicencias').hide();
                    $('#RHComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaRH').hide();
                    $('#SeccionBotonesCerrarLicenciaRH').show();
                }else if(data.estatus == "Aceptada por RH" || data.estatus == "Rechazada por RH" ){
                    if(data.quienAprueba == "flujo normal"){
                        $('#RHComentariosJefeLicencias').show();
                        $('#RHcomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeLicencias').hide();
                    }

                    $('#RHComentariosRHLicencias').show();
                    $('#RHcomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);
                    
                    $('#RHComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaRH').hide();
                    $('#SeccionBotonesCerrarLicenciaRH').show();
                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.comentariosJefe){
                        $('#RHComentariosJefeLicencias').show();
                        $('#RHcomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeLicencias').hide();
                    }

                    if(data.comentariosRH){
                        $('#RHComentariosRHLicencias').show();
                        $('#RHcomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#RHComentariosRHLicencias').hide();
                    }
                    
                    $('#RHComentariosGerenteLicencias').show();
                    $('#RHcomentarioLicenciasGerente').val(data.comentariosGerente).prop('readonly', true);
                
                    $('#SeccionBotonesConfirmarLicenciaRH').hide();
                    $('#SeccionBotonesCerrarLicenciaRH').show();
                }else{
                    $('#RHComentariosJefeLicencias').hide();
                    $('#RHComentariosRHLicencias').hide();
                    $('#RHComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaRH').hide();
                    $('#SeccionBotonesCerrarLicenciaRH').show();
                }

                $('#FechasReincorporacionSolicitud').show();
                $('#botonCrearLicencias').hide();

                $('#tituloModalLicenciaDetalle').show(); 
                $('#tituloModalLicencia').hide(); 

                $('#textoDiasSabadosDetalle').hide();
                $('#textoDiasSabados').hide();
                $('#textoDiasSabadosJefes').show();

                // Muestra el modal
                $('#modalLicencia').show();
            } else {
                alert("No se encontró información para la licencia seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la licencia.");
        }
    });
}

function cargarDetalleLicenciaGerente(button) {
    const idLicencia = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleLicenciaConfirmarRHGerente', // Ruta al método del backend
        type: 'GET',
        data: { id: idLicencia }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                
                $('#JefeYTipoLicencia').hide()
                $('#JefeYTipoLicenciaParaConfirmar').show()
                $('#JefeYTipoLicenciaSecciondeConfirmacion').hide()
                $('#jefeInmediatoConfirmar').val(data.jefe).prop('readonly', true);
                $('#UsuarioLicencia').val(data.usuario).prop('readonly', true);
                $('#tipoLicenciaConfirmar').val(data.tipoLicencia).prop('readonly', true);;
                $('#fechaDeLicencia').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaLicencia').val(data.fechaFin).prop('readonly', true);
                $('#comentarioLicencias').val(data.comentarios).prop('readonly', true);
                $('#fechaReincorporacionLicencia').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudLicencia').val(data.fechaSolicitud).prop('readonly', true);
                $('#estatuslicencia').val(data.estatus).prop('readonly', true);
                $('#idLicenciaConfirmar').val(data.id);

                if(data.estatus == "Enviada"){
                    if(data.quienAprueba == "Gerente es Jefe Directo"){
                        $('#GerenteComentariosGerenteLicencias').show();
                
                        $('#SeccionBotonesConfirmarLicenciaGerente').show();
                        $('#SeccionBotonesCerrarLicenciaGerente').hide();
                    }else{
                        $('#GerenteComentariosJefeLicencias').hide();
                        $('#GerenteComentariosRHLicencias').hide();
                        $('#GerenteComentariosGerenteLicencias').hide();

                        $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                        $('#SeccionBotonesCerrarLicenciaGerente').show();
                    }
                }else if(data.estatus == "Vencida"){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeLicencias').show();
                        $('#GerentecomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeLicencias').hide();
                    }

                    if(data.comentariosRH){
                        $('#GerenteComentariosRHLicencias').show();
                        $('#GerentecomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true);           
                    }else{
                        $('#GerenteComentariosRHLicencias').hide();
                    }

                    if(data.comentariosGerente){
                        $('#GerenteComentariosGerenteLicencias').show();
                        $('#GerentecomentarioLicenciasGerente').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosGerenteLicencias').hide();
                    }

                    $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                    $('#SeccionBotonesCerrarLicenciaGerente').show();
                }
                else if(data.estatus == "Aceptada por jefe directo" || data.estatus == "Rechazada por jefe directo"){
                    $('#GerenteComentariosJefeLicencias').show();
                    $('#GerentecomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);

                    $('#GerenteComentariosRHLicencias').hide();
                    $('#GerenteComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                    $('#SeccionBotonesCerrarLicenciaGerente').show();    
                }else if(data.estatus == "Aceptada por RH"){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeLicencias').show();
                        $('#GerentecomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeLicencias').hide();
                    }

                    $('#GerenteComentariosRHLicencias').show();
                    $('#GerentecomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true); 
                        
                    $('#GerenteComentariosGerenteLicencias').show();
                
                    $('#SeccionBotonesConfirmarLicenciaGerente').show();
                    $('#SeccionBotonesCerrarLicenciaGerente').hide();
                }else if(data.estatus == "Rechazada por RH" ){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeLicencias').show();
                        $('#GerentecomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeLicencias').hide();
                    }

                    $('#GerenteComentariosRHLicencias').show();
                    $('#GerentecomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true); 

                    $('#GerenteComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                    $('#SeccionBotonesCerrarLicenciaGerente').show();
                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.quienAprueba == "flujo normal"){
                        if(data.comentariosJefe){
                            $('#GerenteComentariosJefeLicencias').show();
                            $('#GerentecomentarioLicenciasJefe').val(data.comentariosJefe).prop('readonly', true);
                        }
                       
                        $('#GerenteComentariosRHLicencias').show();
                        $('#GerentecomentarioLicenciasRH').val(data.comentariosRH).prop('readonly', true); 
                    }else if(data.quienAprueba == "Gerente es Jefe Directo"){
                        $('#GerenteComentariosJefeLicencias').hide();
                        $('#GerenteComentariosRHLicencias').hide();
                    }

                    
              
                    $('#GerenteComentariosGerenteLicencias').show();
                    $('#GerentecomentarioLicenciasGerente').val(data.comentariosGerente).prop('readonly', true);
                
                    $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                    $('#SeccionBotonesCerrarLicenciaGerente').show();
                }else{
                    $('#RHComentariosJefeLicencias').hide();
                    $('#RHComentariosRHLicencias').hide();
                    $('#RHComentariosGerenteLicencias').hide();

                    $('#SeccionBotonesConfirmarLicenciaGerente').hide();
                    $('#SeccionBotonesCerrarLicenciaGerente').show();
                }

                $('#FechasReincorporacionSolicitud').show();
                $('#botonCrearLicencias').hide();

                $('#tituloModalLicenciaDetalle').show(); 
                $('#tituloModalLicencia').hide(); 

                $('#textoDiasSabadosDetalle').hide();
                $('#textoDiasSabados').hide();
                $('#textoDiasSabadosJefes').show();

                // Muestra el modal
                $('#modalLicencia').show();
            } else {
                alert("No se encontró información para la licencia seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la licencia.");
        }
    });
}


function cargarDetalleVacacionJefe(button) {
    const idVacacion = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleVacacionConfirmarJefe', // Ruta al método del backend
        type: 'GET',
        data: { id: idVacacion }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#SeccionJefeDirecto').hide();
                $('#SeccionJefeDirectoConfirmar').show();
                $('#diasUsadosVacaciones').val(data.diasUsados).prop('readonly', true);
                $('#diasUsadosVacaiones').show();
                $('#diasDisponiblesVacaciones').hide();
                $('#estatusVacacion').val(data.estatus).prop('readonly', true);
                $('#VacacionEstatus').show();
                $('#fechaDeVacacion').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaVacacion').val(data.fechaFin).prop('readonly', true);
                $('#fechaReincorporacionVacacion').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudVacacion').val(data.fechaSolicitud).prop('readonly', true);
                $('#DetallesVacaciones').show();
                $('#ReincorporacionVacaciones').show();
                $('#comentarioVacaciones').val(data.comentarios).prop('readonly', true); 
                $('#seccionBotonesSolicitarVacaciones').hide();
               /*  $('#idVacacionConfirmarJefe').val(data.id); */

                $('#diasUsados').val(data.diasUsados).prop('readonly', true);

                $('#seccionJefeDirectoConfirmarVacacion').hide();
                $('#usuarioVacacion').val(data.usuario).prop('readonly', true);
                $('#IdVacacionConfirmar').val(data.id).prop('readonly', true);
                $('#botonCrearVacacion').hide();

                $('.btn-solicitud').show();       

                $('#tituloModalVacacionesDetalle').show();
                $('#tituloModalVacaciones').hide();

                $('#textoDiasSabadosDetalle').show();
                $('#textoDiasSabados').hide();

                // Muestra el modal
                $('#modalVacaciones').show();
            } else {
                alert("No se encontró información para la vacación seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la vacación.");
        }
    });
}




function cargarDetalleVacacionRH(button) {
    const idVacacion = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleVacacionConfirmarRHGerente', // Ruta al método del backend
        type: 'GET',
        data: { id: idVacacion }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#SeccionJefeDirecto').hide();
                $('#SeccionJefeDirectoConfirmar').show();
                $('#diasUsadosVacaciones').val(data.diasUsados).prop('readonly', true);
                $('#diasUsadosVacaiones').show();
                $('#diasDisponiblesVacaciones').hide();
                $('#estatusVacacion').val(data.estatus).prop('readonly', true);
                $('#VacacionEstatus').show();
                $('#fechaDeVacacion').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaVacacion').val(data.fechaFin).prop('readonly', true);
                $('#fechaReincorporacionVacacion').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudVacacion').val(data.fechaSolicitud).prop('readonly', true);
                $('#DetallesVacaciones').show();
                $('#ReincorporacionVacaciones').show();
                $('#comentarioVacaciones').val(data.comentarios).prop('readonly', true);
                $('#IdVacacionConfirmar').val(data.id).prop('readonly', true);

                $('#diasUsados').val(data.diasUsados).prop('readonly', true);
                
                $('#jefeInmediatoConfirmarVacacion').val(data.jefe).prop('readonly', true);
                $('#usuarioVacacion').val(data.usuario).prop('readonly', true);
                

                if(data.estatus == "Enviada"){
                    if(data.quienAprueba == "RH es Jefe Directo"){
                        $('#RHComentariosRHVacaciones').show();

                        $('#SeccionBotonesConfirmarVacacionRH').show();
                        $('#SeccionBotonesCerrarVacacionRH').hide();
                    }else{
                        $('#RHComentariosJefeVacaciones').hide();
                        $('#RHComentariosRHVacaciones').hide();
                        $('#RHComentariosGerenteVacaciones').hide();

                        $('#SeccionBotonesConfirmarVacacionRH').hide();
                        $('#SeccionBotonesCerrarVacacionRH').show();
                    }
                }
                else if(data.estatus == "Vencida"){
                    if(data.comentariosJefe){
                        $('#RHComentariosJefeVacaciones').show();
                        $('#RHcomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeVacaciones').hide();
                    }

                    if(data.comentariosRH){
                        $('#RHComentariosRHVacaciones').show();
                        $('#RHcomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);           
                    }else{
                        $('#RHComentariosRHVacaciones').hide();
                    }

                    if(data.comentariosGerente){
                        $('#RHComentariosGerenteVacaciones').show();
                        $('#RHcomentarioVacacionesGerente').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#RHComentariosGerenteVacaciones').hide();
                    }

                    $('#SeccionBotonesConfirmarVacacionRH').hide();
                    $('#SeccionBotonesCerrarVacacionRH').show();
                }else if(data.estatus == "Aceptada por jefe directo"){
                    $('#RHComentariosJefeVacaciones').show();
                    $('#RHcomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);

                    $('#RHComentariosRHVacaciones').show();

                    $('#RHComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionRH').show();
                    $('#SeccionBotonesCerrarVacacionRH').hide();
                }else if(data.estatus == "Rechazada por jefe directo"){
                    $('#RHComentariosJefeVacaciones').show();
                    $('#RHcomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    
                    $('#RHComentariosRHVacaciones').hide();
                    $('#RHComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionRH').hide();
                    $('#SeccionBotonesCerrarVacacionRH').show();
                }else if(data.estatus == "Aceptada por RH" || data.estatus == "Rechazada por RH" ){
                    if(data.quienAprueba == "flujo normal"){
                        $('#RHComentariosJefeVacaciones').show();
                        $('#RHcomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeVacaciones').hide();
                    }

                    $('#RHComentariosRHVacaciones').show();
                    $('#RHcomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                    
                    $('#RHComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionRH').hide();
                    $('#SeccionBotonesCerrarVacacionRH').show();
                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.comentariosJefe){
                        $('#RHComentariosJefeVacaciones').show();
                        $('#RHcomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#RHComentariosJefeVacaciones').hide();
                    }
                    
                    if(data.comentariosRH){
                        $('#RHComentariosRHVacaciones').show();
                        $('#RHcomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#RHComentariosRHVacaciones').hide();
                    }
                    
                    
                    $('#RHComentariosGerenteVacaciones').show();
                    $('#RHcomentarioVacacionesGerente').val(data.comentariosGerente).prop('readonly', true);
                
                    $('#SeccionBotonesConfirmarVacacionRH').hide();
                    $('#SeccionBotonesCerrarVacacionRH').show();
                }else{
                    $('#RHComentariosJefeVacaciones').hide();
                    $('#RHComentariosRHVacaciones').hide();
                    $('#RHComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionRH').hide();
                    $('#SeccionBotonesCerrarVacacionRH').show();
                }


                $('#botonCrearVacacion').hide();

                $('.btn-solicitud').show();       

                $('#tituloModalVacacionesDetalle').show();
                $('#tituloModalVacaciones').hide();

                $('#textoDiasSabadosDetalle').show();
                $('#textoDiasSabados').hide();

                // Muestra el modal
                $('#modalVacaciones').show();
            } else {
                alert("No se encontró información para la vacación seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la vacación.");
        }
    });
}

function cargarDetalleVacacionGerente(button) {
    const idVacacion = button.getAttribute("data-id"); // Obtén el ID del botón

    $.ajax({
        url: '/PermisoLicencias?handler=DetalleVacacionConfirmarRHGerente', // Ruta al método del backend
        type: 'GET',
        data: { id: idVacacion }, // Enviar el ID como parámetro
        success: function (data) {

            if (data) {
                // Actualiza los campos del modal con los datos recibidos
                $('#SeccionJefeDirecto').hide();
                $('#SeccionJefeDirectoConfirmar').show();
                $('#diasUsadosVacaciones').val(data.diasUsados).prop('readonly', true);
                $('#diasUsadosVacaiones').show();
                $('#diasDisponiblesVacaciones').hide();
                $('#estatusVacacion').val(data.estatus).prop('readonly', true);
                $('#VacacionEstatus').show();
                $('#fechaDeVacacion').val(data.fechaInicio).prop('readonly', true);
                $('#fechaHastaVacacion').val(data.fechaFin).prop('readonly', true);
                $('#fechaReincorporacionVacacion').val(data.reincorporacion).prop('readonly', true);
                $('#fechaSolicitudVacacion').val(data.fechaSolicitud).prop('readonly', true);
                $('#DetallesVacaciones').show();
                $('#ReincorporacionVacaciones').show();
                $('#comentarioVacaciones').val(data.comentarios).prop('readonly', true);
                $('#IdVacacionConfirmar').val(data.id).prop('readonly', true);

                $('#diasUsados').val(data.diasUsados).prop('readonly', true);
                
                $('#jefeInmediatoConfirmarVacacion').val(data.jefe).prop('readonly', true);
                $('#usuarioVacacion').val(data.usuario).prop('readonly', true);
                

                if(data.estatus == "Enviada"){
                    if(data.quienAprueba == "Gerente es Jefe Directo"){
                        $('#GerenteComentariosGerenteVacaciones').show();

                        $('#SeccionBotonesConfirmarVacacionGerente').show();
                        $('#SeccionBotonesCerrarVacacionGerente').hide();
                    }else{
                        $('#GerenteComentariosJefeVacaciones').hide();
                        $('#GerenteComentariosRHVacaciones').hide();
                        $('#GerenteComentariosGerenteVacaciones').hide();

                        $('#SeccionBotonesConfirmarVacacionGerente').hide();
                        $('#SeccionBotonesCerrarVacacionGerente').show();
                    }
                }
                else if(data.estatus == "Vencida"){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeVacaciones').show();
                        $('#GerentecomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeVacaciones').hide();
                    }

                    if(data.comentariosRH){
                        $('#GerenteComentariosRHVacaciones').show();
                        $('#GerentecomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);           
                    }else{
                        $('#GerenteComentariosRHVacaciones').hide();
                    }

                    if(data.comentariosGerente){
                        $('#GerenteComentariosGerenteVacaciones').show();
                        $('#GerentecomentarioVacacionesGerente').val(data.comentariosRH).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosGerenteVacaciones').hide();
                    }

                    $('#SeccionBotonesConfirmarVacacionGerente').hide();
                    $('#SeccionBotonesCerrarVacacionGerente').show();
                }else if(data.estatus == "Aceptada por jefe directo"  || data.estatus == "Rechazada por jefe directo"){
                    $('#GerenteComentariosJefeVacaciones').show();
                    $('#GerentecomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);

                    $('#GerenteComentariosRHVacaciones').hide();
                    $('#GerenteComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionGerente').hide();
                    $('#SeccionBotonesCerrarVacacionGerente').show();
                }else if(data.estatus == "Aceptada por RH"){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeVacaciones').show();
                        $('#GerentecomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeVacaciones').hide();
                    }

                    $('#GerenteComentariosRHVacaciones').show();
                    $('#GerentecomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                
                    $('#GerenteComentariosGerenteVacaciones').show();

                    $('#SeccionBotonesConfirmarVacacionGerente').show();
                    $('#SeccionBotonesCerrarVacacionGerente').hide();

                }else if(data.estatus == "Rechazada por RH"){
                    if(data.comentariosJefe){
                        $('#GerenteComentariosJefeVacaciones').show();
                        $('#GerentecomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                    }else{
                        $('#GerenteComentariosJefeVacaciones').hide();
                    }

                    $('#GerenteComentariosRHVacaciones').show();
                    $('#GerentecomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                
                    $('#GerenteComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionGerente').hide();
                    $('#SeccionBotonesCerrarVacacionGerente').show();

                }else if(data.estatus == "Aceptada por gerente" || data.estatus == "Rechazada por gerente" ){
                    if(data.quienAprueba == "flujo normal"){
                        if(data.comentariosJefe){
                            $('#GerenteComentariosJefeVacaciones').show();
                            $('#GerentecomentarioVacacionesJefe').val(data.comentariosJefe).prop('readonly', true);
                        }else{
                            $('#GerenteComentariosJefeVacaciones').hide();
                        }

                        $('#GerenteComentariosRHVacaciones').show();
                        $('#GerentecomentarioVacacionesRH').val(data.comentariosRH).prop('readonly', true);
                    }else if(data.quienAprueba == "erente es Jefe Directo"){
                        $('#GerenteComentariosJefeVacaciones').hide();
                        $('#GerenteComentariosRHVacaciones').hide();
                    }
                    
                    $('#GerenteComentariosGerenteVacaciones').show();
                    $('#GerentecomentarioVacacionesGerente').val(data.comentariosGerente).prop('readonly', true);
                
                    $('#SeccionBotonesConfirmarVacacionGerente').hide();
                    $('#SeccionBotonesCerrarVacacionGerente').show();
                }else{
                    $('#GerenteComentariosJefeVacaciones').hide();
                    $('#GerenteComentariosRHVacaciones').hide();
                    $('#GerenteComentariosGerenteVacaciones').hide();

                    $('#SeccionBotonesConfirmarVacacionGerente').hide();
                    $('#SeccionBotonesCerrarVacacionGerente').show();
                }


                $('#botonCrearVacacion').hide();

                $('.btn-solicitud').show();       

                $('#tituloModalVacacionesDetalle').show();
                $('#tituloModalVacaciones').hide();

                $('#textoDiasSabadosDetalle').show();
                $('#textoDiasSabados').hide();

                // Muestra el modal
                $('#modalVacaciones').show();
            } else {
                alert("No se encontró información para la vacación seleccionada.");
            }
        },
        error: function (error) {
            console.error("Error al cargar los detalles:", error);
            alert("Ocurrió un error al cargar los detalles de la vacación.");
        }
    });
}

function confirmarLicencia(tipoConfirmacion){

    let tituloConfirmacion = "";

    if ((tipoConfirmacion == 6) || (tipoConfirmacion == 8)){
        tituloConfirmacion = "¿Estás seguro de confirmar la licencia?";
    }else if((tipoConfirmacion == 7)  || (tipoConfirmacion == 9)){
        tituloConfirmacion = "¿Estás seguro de rechazar la licencia?";
    }else {
        // Por si llega otro tipo por error
        tituloConfirmacion = "¿Estás seguro de realizar esta acción?";
    }

    Swal.fire({
        title: tituloConfirmacion,
        text: "Esta acción no se podrá cambiar.",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            var id = document.getElementById("idLicenciaConfirmar").value;
            if(tipoConfirmacion == 6){
                var comentario = document.getElementById("RHcomentarioLicenciasRH").value;
            }else if(tipoConfirmacion == 8){
                var comentario = document.getElementById("GerentecomentarioLicenciasGerente").value;
            }
            

            $.ajax({
                type: "POST",
                url: '/PermisoLicencias?handler=ConfirmarLicenciaRHGerente',
                data: {
                    id: parseInt(id),
                    tipoConfirmacion : parseInt(tipoConfirmacion),
                    comentario : comentario,
                    
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                dataType: "json"
            }).done(function (data) {
                if (data.success) {
                    Swal.fire({
                        title: "¡Éxito!",
                        text: "Operación realizada con éxito",
                        icon: "success",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalLicencia').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
                } else
                    Swal.fire({
                        title: "¡Error!",
                        text: 'Hubo un problema, intenta nuevamente',
                        icon: "error",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalLicencia').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
            }).fail(function(error) {
                console.error("Error:", error);
            });
        }
    });
}

function confirmarLicenciaJefe(tipoConfirmacion){
    // Definir dinámicamente el título
    let tituloConfirmacion = "";

    if (tipoConfirmacion == 3){
        tituloConfirmacion = "¿Estás seguro de confirmar la licencia?";
    }else if(tipoConfirmacion == 4){
        tituloConfirmacion = "¿Estás seguro de rechazar la licencia?";
    }else {
        // Por si llega otro tipo por error
        tituloConfirmacion = "¿Estás seguro de realizar esta acción?";
    }

    Swal.fire({
        title: tituloConfirmacion,
        text: "Esta acción no se podrá cambiar.",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed) {
            var id = document.getElementById("idLicenciaConfirmar").value;
            var comentario = document.getElementById("comentario-LicenciasJefe").value;

            $.ajax({
                type: "POST",
                url: '/PermisoLicencias?handler=ConfirmarLicenciaJefe',
                data: {
                    id: parseInt(id),
                    tipoConfirmacion : parseInt(tipoConfirmacion),
                    comentario : comentario,
                    
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                dataType: "json"
            }).done(function (data) {
                if (data.success) {
                    Swal.fire({
                        title: "¡Éxito!",
                        text: "Operación realizada con éxito",
                        icon: "success",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        
                        $('#modalLicencia').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
                } else
                    Swal.fire({
                        title: "¡Error!",
                        text: 'Hubo un problema, intenta nuevamente',
                        icon: "error",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalLicencia').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
            }).fail(function(error) {
                console.error("Error:", error);
            });
        }
    });
}

function confirmarVacacion(tipoConfirmacion){

    let tituloConfirmacion = "";

    if ((tipoConfirmacion == 6) || (tipoConfirmacion == 8)){
        tituloConfirmacion = "¿Estás seguro de confirmar?";
    }else if((tipoConfirmacion == 7)  || (tipoConfirmacion == 9)){
        tituloConfirmacion = "¿Estás seguro de rechazar";
    }else {
        // Por si llega otro tipo por error
        tituloConfirmacion = "¿Estás seguro de realizar esta acción?";
    }

    Swal.fire({
        title: tituloConfirmacion,
        text: "Esta acción no se podrá cambiar.",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed){
            var id = document.getElementById("IdVacacionConfirmar").value;
            if(tipoConfirmacion == 6){
                var comentario = document.getElementById("RHcomentarioVacacionesRH").value;
            }else if(tipoConfirmacion == 8){
                var comentario = document.getElementById("GerentecomentarioVacacionesGerente").value;
            }

            $.ajax({
                type: "POST",
                url: '/PermisoLicencias?handler=ConfirmarVacacionRHGerente',
                data: {
                    id: parseInt(id),
                    tipoConfirmacion : parseInt(tipoConfirmacion),
                    comentario : comentario,
                    
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                dataType: "json"
            }).done(function (data) {
                if (data.success) {
                    Swal.fire({
                        title: "¡Éxito!",
                        text: "Operación realizada con éxito",
                        icon: "success",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalVacaciones').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
                } else
                    Swal.fire({
                        title: "¡Error!",
                        text: 'Hubo un problema, intenta nuevamente',
                        icon: "error",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalVacaciones').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
            }).fail(function(error) {
                console.error("Error:", error);
            });
        }
    });
}

function confirmarVacacionJefe(tipoConfirmacion){

    let tituloConfirmacion = "";

    if (tipoConfirmacion == 3){
        tituloConfirmacion = "¿Estás seguro de confirmar?";
    }else if(tipoConfirmacion == 4){
        tituloConfirmacion = "¿Estás seguro de rechazar";
    }else {
        // Por si llega otro tipo por error
        tituloConfirmacion = "¿Estás seguro de realizar esta acción?";
    }

    Swal.fire({
        title: tituloConfirmacion,
        text: "Esta acción no se podrá cambiar.",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Sí, continuar",
        cancelButtonText: "Cancelar",
    }).then((result) => {
        if (result.isConfirmed){
            var id = document.getElementById("IdVacacionConfirmar").value;
            var comentario = document.getElementById("comentarioVacacionesJefe").value;
           
            $.ajax({
                type: "POST",
                url: '/PermisoLicencias?handler=ConfirmarVacacionJefe',
                data: {
                    id: parseInt(id),
                    tipoConfirmacion : parseInt(tipoConfirmacion),
                    comentario : comentario,
                    
                },
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                dataType: "json"
            }).done(function (data) {
                if (data.success) {
                    Swal.fire({
                        title: "¡Éxito!",
                        text: "Operación realizada con éxito",
                        icon: "success",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalVacaciones').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
                } else
                    Swal.fire({
                        title: "¡Error!",
                        text: 'Hubo un problema, intenta nuevamente',
                        icon: "error",
                        confirmButtonText: 'Aceptar'
                    }).then(() => {
                        $('#modalVacaciones').hide();// Oculta el modal con Bootstrap
                        location.reload(); // Recarga la página
                    });
            }).fail(function(error) {
                console.error("Error:", error);
            });
        }
    });
}


