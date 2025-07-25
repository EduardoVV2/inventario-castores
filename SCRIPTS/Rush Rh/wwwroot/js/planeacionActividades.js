function handleAnioChange(añoIndicadoYa = null, seccionIndicadaYa = null) {
    const tituloAnio = document.getElementById('anioTitulo');
    const spinner = document.getElementById('contenedorSpinner');
    const anioInput = document.getElementById('anioInput');
    const anioInputEliminar = document.getElementById('anioInputEliminar');

    // Mostrar el spinner
    spinner.style.display = 'flex';

    // Obtener el valor del select
    let anioSeleccionado = document.getElementById('anio').value;

    if (añoIndicadoYa != null && añoIndicadoYa != '') {
        anioSeleccionado = añoIndicadoYa;
        document.getElementById('anio').value = anioSeleccionado;
    } 

    // Actualizar el título con el año seleccionado
    tituloAnio.textContent = anioSeleccionado;

    // Actualizar el valor del input con el año seleccionado
    anioInput.value = anioSeleccionado;

    // Actualizar el valor del inputEliminar con el año seleccionado
    anioInputEliminar.value = anioSeleccionado;

    // Actualizar la fecha mínima y maxima de cada input fecha-inicio al año seleccionado
    const fechaInicio = document.getElementsByClassName('fecha-inicio');
    for (let i = 0; i < fechaInicio.length; i++) {
        fechaInicio[i].min = `${anioSeleccionado}-01-01T00:00`; // Mínimo: Inicio del año
        fechaInicio[i].max = `${anioSeleccionado}-12-31T23:59`; // Máximo: Fin del año

        // Limpiar el valor del input
        fechaInicio[i].value = '';
    } 

    // Limpiar el valor del input fecha-fin
    const fechaFin = document.getElementsByClassName('fecha-termino');
    for (let i = 0; i < fechaFin.length; i++) {
        fechaFin[i].min = `${anioSeleccionado}-01-01T00:00`; // Mínimo: Inicio del año
        fechaFin[i].max = `${anioSeleccionado}-12-31T23:59`; // Máximo: Fin del año

        // Limpiar el valor del input
        fechaFin[i].value = '';
    }


    // Realizar la petición al servidor
    if (anioSeleccionado) {
        fetch(`/PlaneacionActividades?handler=PlaneacionAnual&anio=${anioSeleccionado}`)
        .then(response => response.json())
        .then(data => {

            // Renderizar las planeaciones y actividades
            renderizarPlaneaciones(data);

            //Para que el modal reciba el atributo del boton que dice que id de planeacion es
            // Seleccionar todos los botones que abren el modal
            const botonesAgregarActividad = document.querySelectorAll('.agregarActividadButton');

            // Seleccionar el campo oculto del modal
            const idPlaneacionInput = document.getElementById('IdPlaneacion');

            // Agregar evento de clic a cada botón
            botonesAgregarActividad.forEach(boton => {
                boton.addEventListener('click', () => {
                    const idPlaneacion = boton.getAttribute('data-id-planeacion');
                    idPlaneacionInput.value = idPlaneacion; // Asignar el ID al campo oculto

                    // Obtener las fechas de inicio y fin de los atributos data
                    const fechaInicio = boton.getAttribute('data-fecha-inicio');
                    const fechaFin = boton.getAttribute('data-fecha-fin');

                    console.log("fechas", fechaInicio, fechaFin);

                    // Configurar los campos de fecha en el modal para que sea entre las fechas de la planeación
                    const inputFechaInicio = document.getElementById('fechaInicioActividad');
                    const inputFechaFin = document.getElementById('fechaFinActividad');

                    console.log("inputs", inputFechaInicio, inputFechaFin);

                    if (inputFechaInicio && inputFechaFin) {
                        inputFechaInicio.min = fechaInicio;
                        inputFechaInicio.max = fechaFin;
                        inputFechaFin.min = fechaInicio;
                        inputFechaFin.max = fechaFin;

                        // Limpiar valores previos del modal
                        inputFechaInicio.value = '';
                        inputFechaFin.value = '';
                    }
                });
            });


            // Evento para manejar clics en el botón "Eliminar planeacion"
            const botonesEliminarSubplanes = document.querySelectorAll('.botonEliminarCosas');
            botonesEliminarSubplanes.forEach(boton => {
                boton.addEventListener('click', (e) => {
                    const IdPlaneacion = boton.getAttribute('data-id');
                    const numeroActividades = boton.getAttribute('data-numero-actividades');
                    Swal.fire({
                        title: '¿Estás seguro?',
                        text: "Esta acción eliminará al la subplaneacion actual.",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'Cancelar'
                    }).then((result) => {
                        if (result.isConfirmed) {

                            //MOstrar alerta de error si hya actividades
                            if (numeroActividades > 0) {
                                Swal.fire({
                                    title: 'Error',
                                    text: 'No se puede eliminar la subplaneación porque tiene actividades asignadas.',
                                    icon: 'error',
                                    confirmButtonText: 'Aceptar'
                                });
                                return;
                            }

                            //Poner el id del integrante en el fomrulario de eliminar integrante
                            document.getElementById('idPlaneacion').value = IdPlaneacion;

                            // Aquí puedes realizar la acción de eliminar el estudiante
                            document.getElementById('formEliminarSubplan').submit();
                        }
                    });
                });
            });


            //Evento para manejar clics en el botón "Editar planeacion"
            const botonesEditarActividad = document.querySelectorAll('.editarActividadButton');

            // Seleccionar los campos del modal
            const idPlaneacionAnualInput = document.getElementById('IdPlaneacionAnual');
            const nombrePlaneacionInput = document.getElementById('nombrePlaneacion');
            const fechaInicioPlaneacionInput = document.getElementById('fechaInicio');
            const fechaFinPlaneacionInput = document.getElementById('fechaFin');

            // Agregar evento de clic a cada botón
            botonesEditarActividad.forEach(boton => {
                boton.addEventListener('click', () => {
                    //cambiar titulo del modal 
                    document.getElementById('miModalLabelSubplane').textContent = 'Editar subplan';

                    //cambiar manejador del formulario
                    document.getElementById('formSubplane').action = '/PlaneacionActividades?handler=EditarPlaneacionAnual';

                    const idPlaneacion = boton.getAttribute('data-id-planeacion');
                    idPlaneacionAnualInput.value = idPlaneacion; // Asignar el ID al campo oculto
                    const nombrePlaneacion = boton.getAttribute('data-nombre');
                    nombrePlaneacionInput.value = nombrePlaneacion; // Asignar el nombre al campo de texto
                    const fechaInicioPlaneacion = boton.getAttribute('data-fecha-inicio');
                    fechaInicioPlaneacionInput.value = fechaInicioPlaneacion; // Asignar la fecha de inicio al campo de fecha
                    const fechaFinPlaneacion = boton.getAttribute('data-fecha-fin');
                    fechaFinPlaneacionInput.value = fechaFinPlaneacion; // Asignar la fecha de fin al campo de fecha 

                    let alerta = document.getElementById('alertaFechas');
                    alerta.style.display = 'block';

                    //Sacar la fecha mas chica y la mas grande de las actividades para maandar alerta por si se edita la fecha de la planeacion no pueda ser menor o mayor que las actividades
                    const fechaInicioActividadMasChica = boton.getAttribute('data-fecha-inicio-actividadMasChica');
                    const fechaFinActividadMasGrande = boton.getAttribute('data-fecha-fin-actividadMasGrande');

                    // Configurar los campos de fecha en el modal para que sea entre las fechas de la planeación
                    const inputFechaInicio = document.getElementById('fechaInicio');
                    const inputFechaFin = document.getElementById('fechaFin');

                    if (inputFechaInicio && inputFechaFin) {
                        inputFechaInicio.max = fechaInicioActividadMasChica;
                        inputFechaFin.min = fechaFinActividadMasGrande;

                        // Limpiar valores previos del modal
                        inputFechaInicio.value = fechaInicioPlaneacion;
                        inputFechaFin.value = fechaFinPlaneacion;
                    }   
                    

                });
            });

            // Reset modal values when it is closed
            const modalSubplane = document.getElementById('modalSubplane');
            modalSubplane.addEventListener('hidden.bs.modal', () => {
                document.getElementById('miModalLabelSubplane').textContent = 'Guardar subplan';
                document.getElementById('formSubplane').action = '/PlaneacionActividades?handler=GuardarPlaneacionAnual';
                idPlaneacionAnualInput.value = '';
                nombrePlaneacionInput.value = '';
                fechaInicioPlaneacionInput.value = '';
                fechaFinPlaneacionInput.value = '';
                let alerta = document.getElementById('alertaFechas');
                alerta.style.display = 'none';

                // Actualizar la fecha mínima y maxima de cada input fecha-inicio al año seleccionado
                for (let i = 0; i < fechaInicio.length; i++) {
                    fechaInicio[i].min = `${anioSeleccionado}-01-01T00:00`; // Mínimo: Inicio del año
                    fechaInicio[i].max = `${anioSeleccionado}-12-31T23:59`; // Máximo: Fin del año

                    // Limpiar el valor del input
                    fechaInicio[i].value = '';
                } 

                // Limpiar el valor del input fecha-fin
                for (let i = 0; i < fechaFin.length; i++) {
                    fechaFin[i].min = `${anioSeleccionado}-01-01T00:00`; // Mínimo: Inicio del año
                    fechaFin[i].max = `${anioSeleccionado}-12-31T23:59`; // Máximo: Fin del año

                    // Limpiar el valor del input
                    fechaFin[i].value = '';
                }
        

            });


            //Si viene una seccion ya indicada porque viene del controlador la insersion de actividad, la abrimos
            if (seccionIndicadaYa != null && seccionIndicadaYa != '') {
                activarSeccion(seccionIndicadaYa);
            }


            // Ocultar el spinner
            spinner.style.display = 'none';

    
        });
    }
}



function renderizarPlaneaciones(data) {
    const contenedor = document.getElementById('sortable-rows');
    contenedor.innerHTML = ''; // Limpiar el contenedor

    const { planeaciones, actividades } = data;

    // Si no hay planeaciones, mostrar un mensaje indicando esto
    if (planeaciones.length === 0) {
        contenedor.innerHTML = `<div class="alert alert-info text-center">No hay planeaciones registradas para este año.</div>`;
        return;
    }

    planeaciones.forEach(planeacion => {
        const actividadesPlaneacion = actividades.filter(act => act.idPlaneacion === planeacion.id);

        const acordeonHTML = `
            <div class="row draggable-item">
                <div class="col-12">
                    <div class="accordion accordion" id="accordionFlushExample-${planeacion.id}">
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="heading-${planeacion.id}">
                                <button  class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse-${planeacion.id}" aria-expanded="false" aria-controls="collapse-${planeacion.id}">
                                    <div class="textoButton"><strong>${new Date(planeacion.fechaHoraInicio).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })} ${new Date(planeacion.fechaHoraInicio).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</strong> - <strong>${new Date(planeacion.fechaHoraFin).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })} ${new Date(planeacion.fechaHoraFin).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</strong>${planeacion.nombre}</div>
                                </button>
                            </h2>
                            <div id="collapse-${planeacion.id}" class="accordion-collapse collapse" aria-labelledby="heading-${planeacion.id}" data-bs-parent="#accordionFlushExample-${planeacion.id}">
                                <div class="accordion-body">
                                    <div class="actividades__encabezado">
                                        <div class="planeacion__botones">
                                            <button type="button" data-bs-toggle="modal" data-bs-target="#modalSubplane" class="btn buttonGeneral editarActividadButton" data-id-planeacion="${planeacion.id}" data-nombre="${planeacion.nombre}" data-fecha-inicio="${planeacion.fechaHoraInicio}" data-fecha-fin="${planeacion.fechaHoraFin}" ${actividadesPlaneacion.length > 0 ? `data-fecha-inicio-actividadMasChica="${new Date(Math.min(...actividadesPlaneacion.map(act => new Date(act.fechaHoraInicio).getTime()))).toLocaleString('sv-SE', { timeZone: 'UTC' }).replace(' ', 'T')}" data-fecha-fin-actividadMasGrande="${new Date(Math.max(...actividadesPlaneacion.map(act => new Date(act.fechaHoraFin).getTime()))).toLocaleString('sv-SE', { timeZone: 'UTC' }).replace(' ', 'T')}"` : ''}>
                                                <div class="textoButton"><span class="material-symbols-outlined">edit</span>Editar subplan</div>
                                            </button>
                                            <button class="btn buttonGeneral botonEliminarCosas"  data-id="${planeacion.id}" data-numero-actividades="${actividadesPlaneacion.length}">
                                                <div class="textoButton"><span class="material-symbols-outlined">delete</span>Eliminar subplan</div>
                                            </button>
                                        </div>
                                        <button type="button" data-bs-toggle="modal" data-bs-target="#modalActividad" class="btn btn-primary buttonGeneral agregarActividadButton" data-id-planeacion="${planeacion.id}" data-fecha-inicio="${planeacion.fechaHoraInicio}" data-fecha-fin="${planeacion.fechaHoraFin}">
                                            <div class="textoButton"><span class="material-symbols-outlined">add</span>Agregar actividad</div>
                                        </button>
                                    </div>
                                    <div id="sortable-container-${planeacion.id}" class="actividades__contenedor">
                                        <table class="table">
                                            <thead>
                                                <tr>
                                                    <th style="width: 25%; text-align: center;">Actividad</th>
                                                    <th style="width: 25%; text-align: center;">Fecha inicio</th>
                                                    <th style="width: 25%; text-align: center;">Fecha fin</th>
                                                    <th style="width: 12.5%; text-align: center;">DC3</th>
                                                    <th style="width: 12.5%; text-align: center;">Acciones</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                ${renderizarActividades(actividadesPlaneacion, planeacion.fechaHoraInicio, planeacion.fechaHoraFin)}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        contenedor.insertAdjacentHTML('beforeend', acordeonHTML);
    });
}



function renderizarActividades(actividades, fechaHoraInicioPlaneacion, fechaHoraFinPlaneacion) {
    // Si no hay actividades, mostrar un mensaje indicando esto
    if (actividades.length === 0) {
        return `<tr><td colspan="5"><div class="alert alert-info text-center">No hay actividades registradas para esta planeacion.</div></td></tr>`;
    }

    // Generar HTML para cada actividad
    return actividades.map(actividad => `
            <tr>
                <td class="text-center align-middle">${actividad.nombreActividad}</td>
                <td class="text-center align-middle">${new Date(actividad.fechaHoraInicio).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })} ${new Date(actividad.fechaHoraInicio).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</td>
                <td class="text-center align-middle">${new Date(actividad.fechaHoraFin).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })} ${new Date(actividad.fechaHoraFin).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</td>
                <td class="text-center align-middle">${actividad.dC3 ? 'Sí' : 'No'}</td>
                <td class="text-center align-middle">
                    <button type="button" class="btn btn-primary buttonGeneral detallesButton" onclick='window.location.href="/DetallesActividad?idActividad=${actividad.id}&AnioActual=${document.getElementById('anioInput').value}&FechaMinima=${fechaHoraInicioPlaneacion}&FechaMaxima=${fechaHoraFinPlaneacion}"'>
                        <div class="textoButton"><span class="material-symbols-outlined">page_info</span>Más detalles</div>
                    </button>
                </td>
            </tr>
    `).join(''); // Combinar todas las actividades en una sola cadena de HTML
}



function actualizarFechaMinima(campoFechaInicio) {
    // Encuentra el contenedor padre más cercano
    const contenedor = campoFechaInicio.closest('.modal-body').parentElement;
    
    // Busca el input de fecha de fin dentro del mismo contenedor
    const fechaTermino = contenedor.querySelector('.fecha-termino');
    
    if (fechaTermino) {
        // Configura el atributo min de la fecha de fin
        fechaTermino.min = campoFechaInicio.value;
        if (new Date(fechaTermino.value) < new Date(campoFechaInicio.value)) {
            fechaTermino.value = campoFechaInicio.value;
        }
    }
}


// Función para abrir la sección y marcar el botón correspondiente
function activarSeccion(target) {
    document.querySelector(`[data-bs-target="#collapse-${target}"]`).click();

    const botones = document.querySelectorAll('#buttonMenu button');
    botones.forEach(function(button) {
        button.classList.remove('active');
    });

    document.querySelector(`[data-bs-target="#collapse-${target}"]`).classList.add('active');
}