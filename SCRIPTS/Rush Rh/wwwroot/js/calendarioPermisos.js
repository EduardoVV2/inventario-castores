function abrirModalLicencias(registro) {
    $.ajax({
        url: '/CalendarioPermisos?handler=DetalleLicencia',  // Ajusta la ruta según tu configuración
        type: 'GET',
        data: { id: registro.id }, // Enviar ID del registro
        success: function (data) {
            if (data) {
                // Llenar los campos del modal con los datos recibidos
                $('#usuario_Calendario').val(data.usuario);
                $('#jefe_Calendario').val(data.jefe);
                $('#SeccionParaLicencia').show();
                $('#SeccionParaVacacion').hide();
                $('#tipo_licencia').val(data.tipoLicencia);
                $('#fechaSolicitud_Calendario').val(data.fechaSolicitud);
                $('#fechaInicio_Calendario').val(data.fechaInicio);
                $('#fechaFin_Calendario').val(data.fechaFin);
                $('#fechaReincorporacion_Calendario').val(data.reincorporacion);
                $('#comentarios_Calendario').val(data.comentarios);

                // Cambiar el título dinámicamente
                $('.TituloModalCalendario h1').text('Detalle licencia');
                
                // Mostrar el modal SOLO si la solicitud es exitosa
                let modal = new bootstrap.Modal(document.getElementById('ModalCalendario'));
                modal.show();
                //$('#ModalCalendario').show()
            } else {
                Swal.fire("Error", "No se pudo cargar la información", "error");
            }
        },
        error: function () {
            Swal.fire("Error", "Ocurrió un problema con la solicitud", "error");
            return;  // Detiene la ejecución y evita abrir el modal
        }
    });
}

function abrirModalVacaciones(registro) {
    $.ajax({
        url: '/CalendarioPermisos?handler=DetalleVacacion',  // Ajusta la ruta según tu configuración
        type: 'GET',
        data: { id: registro.id }, // Enviar ID del registro
        success: function (data) {
            if (data) {
                // Llenar los campos del modal con los datos recibidos
                $('#usuario_Calendario').val(data.usuario);
                $('#jefe_Calendario').val(data.jefe);
                $('#SeccionParaLicencia').hide();
                $('#SeccionParaVacacion').show();
                $('#dias_Usados').val(data.diasUsados);
                $('#fechaSolicitud_Calendario').val(data.fechaSolicitud);
                $('#fechaInicio_Calendario').val(data.fechaInicio);
                $('#fechaFin_Calendario').val(data.fechaFin);
                $('#fechaReincorporacion_Calendario').val(data.reincorporacion);
                $('#comentarios_Calendario').val(data.comentarios);

                // Cambiar el título dinámicamente
                $('.TituloModalCalendario h1').text('Detalle vacaciones');

                // Mostrar el modal SOLO si la solicitud es exitosa
                let modal = new bootstrap.Modal(document.getElementById('ModalCalendario'));
                modal.show();
                //$('#ModalCalendario').show()
            } else {
                Swal.fire("Error", "No se pudo cargar la información", "error");
            }
        },
        error: function () {
            Swal.fire("Error", "Ocurrió un problema con la solicitud", "error");
            return;  // Detiene la ejecución y evita abrir el modal
        }
    });
}