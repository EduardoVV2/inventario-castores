// Función para cargar los estados y seleccionar el estado inicial
function cargarEstados(paisId, estadoId, municipioId, coloniaId) {
    fetch(`/InformacionComplementaria?handler=Estados&paisId=${paisId}`)
        .then(response => response.json())
        .then(data => {
            const estadoSelect = document.getElementById('estado');
            const municipioSelect = document.getElementById('municipio');
            const coloniaSelect = document.getElementById('colonia');
            estadoSelect.innerHTML = '<option value="">Seleccione una opción</option>';
            data.forEach(estado => {
                estadoSelect.innerHTML += `<option value="${estado.id}">${estado.nombre}</option>`;
            });

            if (estadoId !== "") {
                estadoSelect.value = estadoId;
                cargarMunicipios(estadoId, municipioId, coloniaId);
            } 

            estadoSelect.disabled = false;
            municipioSelect.disabled = true;
            coloniaSelect.disabled = true;
            
            requestsCompleted++;
            comparadorRequestCounter();
        });
}

// Función para cargar los municipios y seleccionar el municipio inicial
function cargarMunicipios(estadoId, municipioId, coloniaId) {
    fetch(`/InformacionComplementaria?handler=Municipios&estadoId=${estadoId}`)
        .then(response => response.json())
        .then(data => {
            const municipioSelect = document.getElementById('municipio');
            const coloniaSelect = document.getElementById('colonia');
            municipioSelect.innerHTML = '<option value="">Seleccione una opción</option>';
            data.forEach(municipio => {
                municipioSelect.innerHTML += `<option value="${municipio.id}">${municipio.nombre}</option>`;
            });

            if (municipioId !== "") {
                municipioSelect.value = municipioId;
                cargarColonias(municipioId, coloniaId);
            }

            municipioSelect.disabled = false;
            coloniaSelect.disabled = true;

            requestsCompleted++;
            comparadorRequestCounter();
        });
}

// Función para cargar las colonias y seleccionar la colonia inicial
function cargarColonias(municipioId, coloniaId) {
    fetch(`/InformacionComplementaria?handler=Colonias&municipioId=${municipioId}`)
        .then(response => response.json())
        .then(data => {
            const coloniaSelect = document.getElementById('colonia');
            coloniaSelect.innerHTML = '<option value="">Seleccione una opción</option>';
            data.forEach(colonia => {
                coloniaSelect.innerHTML += `<option value="${colonia.id}">${colonia.nombre}</option>`;
            });

            if (coloniaId !== "") {
                coloniaSelect.value = coloniaId;
            }

            coloniaSelect.disabled = false;

            requestsCompleted++;
            comparadorRequestCounter();
        });
}

// Función para detectar cambios en los selects y actualizar los campos ocultos
document.getElementById('pais').addEventListener('change', function () {
    var selectedOption = this.options[this.selectedIndex];
    var paisNombre = selectedOption.text;
    document.getElementById('paisNombre').value = paisNombre;

    let paisId = this.value;
    let estadoSelect = document.getElementById('estado');
    let municipioSelect = document.getElementById('municipio');
    let coloniaSelect = document.getElementById('colonia');

    // Limpiar y deshabilitar los selects dependientes
    estadoSelect.innerHTML = '<option value="">Seleccione una opción</option>';
    municipioSelect.innerHTML = '<option value="">Seleccione una opción</option>';
    coloniaSelect.innerHTML = '<option value="">Seleccione una opción</option>';

    estadoSelect.disabled = true;
    municipioSelect.disabled = true;
    coloniaSelect.disabled = true;

    if (paisId) {
        // Llamar a cargar los estados
        cargarEstados(paisId, "", "", "");
    }
});

document.getElementById('estado').addEventListener('change', function () {
    var selectedOption = this.options[this.selectedIndex];
    var estadoNombre = selectedOption.text;
    document.getElementById('estadoNombre').value = estadoNombre;

    let estadoId = this.value;
    let municipioSelect = document.getElementById('municipio');
    let coloniaSelect = document.getElementById('colonia');

    // Limpiar y deshabilitar los selects dependientes
    municipioSelect.innerHTML = '<option value="">Seleccione una opción</option>';
    coloniaSelect.innerHTML = '<option value="">Seleccione una opción</option>';

    municipioSelect.disabled = true;
    coloniaSelect.disabled = true;

    if (estadoId) {
        // Llamar a cargar los municipios
        cargarMunicipios(estadoId, "", "");
    }
});

document.getElementById('municipio').addEventListener('change', function () {
    var selectedOption = this.options[this.selectedIndex];
    var municipioNombre = selectedOption.text;
    document.getElementById('municipioNombre').value = municipioNombre;

    let municipioId = this.value;
    let coloniaSelect = document.getElementById('colonia');

    // Limpiar y deshabilitar el select de colonias
    coloniaSelect.innerHTML = '<option value="">Seleccione una opción</option>';
    coloniaSelect.disabled = true;

    if (municipioId) {
        // Llamar a cargar las colonias
        cargarColonias(municipioId, "");
    }
});

document.getElementById('colonia').addEventListener('change', function () {
    var selectedOption = this.options[this.selectedIndex];
    var coloniaNombre = selectedOption.text;
    document.getElementById('coloniaNombre').value = coloniaNombre;
});


/*----------------------------------------------------Funciones para ocultar los select de datos medicos segun si tiene o no la enfermedad*/


//Funcion para mostrar los selects secundarios según el valor seleccionado en el select principal
function handleSelectChangeAlergias() {

    const alergiaSelect = document.getElementById("alergias").value;

    const hayAlergiasContainer = document.getElementById("hayAlergiasContainer");

    const inputDescripcionAlergias = document.getElementById("alergiasCuales");

    // Reiniciar visibilidad
    hayAlergiasContainer.style.display = "none";

    if (alergiaSelect == "true") {
        hayAlergiasContainer.style.display = "block";
        inputDescripcionAlergias.setAttribute("required", "");
    } else {
        inputDescripcionAlergias.removeAttribute("required");
    }
}

function handleSelectChangeEnfermedades() {
    
    const enfermedadSelect = document.getElementById("enfermedadCronica").value;

    const hayEnfermedadesContainer = document.getElementById("enfermedadCronicaContainer");

    const inputDescripcionEnfermedades = document.getElementById("enfermedadCronicaCuales");

    // Reiniciar visibilidad
    hayEnfermedadesContainer.style.display = "none";

    if (enfermedadSelect == "true") {
        hayEnfermedadesContainer.style.display = "block";
        inputDescripcionEnfermedades.setAttribute("required", "");
    } else {
        inputDescripcionEnfermedades.removeAttribute("required");
    }
}

function handleSelectChangeMedicamentos() {

    const medicamentoSelect = document.getElementById("consumeMedicamentos").value;

    const hayMedicamentosContainer = document.getElementById("consumirMedicamentosContainer");

    const inputDescripcionMedicamentos = document.getElementById("medicamentosCuales");

    // Reiniciar visibilidad
    hayMedicamentosContainer.style.display = "none";

    if (medicamentoSelect == "true") {
        hayMedicamentosContainer.style.display = "block";
        inputDescripcionMedicamentos.setAttribute("required", "");
    } else {
        inputDescripcionMedicamentos.removeAttribute("required");
    }
}


/*----------------------------------Codigo para que el color del boton cambie dependiendo de la seccion en la que estemos */ 

document.addEventListener("DOMContentLoaded", function () {
    const buttons = document.querySelectorAll("#buttonMenu .button");
    const sections = document.querySelectorAll(".accordion-collapse");

    // Agregar evento de clic a cada botón
    buttons.forEach((button, index) => {
        button.addEventListener("click", () => {
            buttons.forEach(btn => btn.classList.remove("active"));

            // Agregar la clase 'active' al botón actual
            button.classList.add("active");

            // Cerrar todas las secciones colapsables y expandir la seleccionada
            sections.forEach((section, secIndex) => {
                section.classList.remove("show"); // Cierrtodo
                if (index === secIndex) {
                    section.classList.add("show"); // Abre la correspondiente
                }
            });
        });
    });
});


// Función para verificar si el contenedor está vacío
function verificarHistorial(contenedorId, mensajeId) {
    const container = document.getElementById(`${contenedorId}`);
    const mensaje = document.getElementById(`${mensajeId}`);
    if (!container || !mensaje) return;

    // Filtrar hijos visibles
    const hijosVisibles = Array.from(container.children).filter((hijo) => {
        const estilo = window.getComputedStyle(hijo);
        return estilo.display !== "none";
    });

    if (hijosVisibles.length <= 0) {
        mensaje.style.display = "block"; // Muestra el mensaje
    } else {
        mensaje.style.display = "none"; // Oculta el mensaje
    }
}


/*-------------------------------------------------Codigo para agregar idiomas */
document.addEventListener("DOMContentLoaded", function () {
    

    // Agregar nuevo idioma
    document.getElementById("addIdiomaBtn").addEventListener("click", function () {
        const idiomasContainer = document.getElementById("idiomasContainer");
        const idiomasIndex = document.getElementById("numeroIndice").value
        const newRow = document.createElement("div");
        newRow.className = "form-group row mb-3 align-items-center idioma-row gap-3 idiomasContainerPreguntas";

        newRow.innerHTML = `
            <input type="hidden" name="idiomas[${idiomasIndex}].Id" value="0" />
            <div class="col-md-12">
                <label for="idiomas_${idiomasIndex}_Nombre" class="form-label">Idioma:</label>
                <input type="text" class="form-control" name="idiomas[${idiomasIndex}].Nombre" id="idiomas_${idiomasIndex}_Nombre" placeholder="Ejemplo: Inglés" required>
            </div>
            <div class="col-md-12">
                <label for="idiomas_${idiomasIndex}_NivelEscrito" class="form-label">Nivel escrito (%):</label>
                <input type="number" class="form-control" name="idiomas[${idiomasIndex}].NivelEscrito" id="idiomas_${idiomasIndex}_NivelEscrito" min="0" max="100" placeholder="Ejemplo: 80" required>
            </div>
            <div class="col-md-12">
                <label for="idiomas_${idiomasIndex}_NivelHablado" class="form-label">Nivel hablado (%):</label>
                <input type="number" class="form-control" name="idiomas[${idiomasIndex}].NivelHablado" id="idiomas_${idiomasIndex}_NivelHablado" min="0" max="100" placeholder="Ejemplo: 70" required>
            </div>
            <div class="col-md-2 contenedorBoton">
                <button type="button" class="btn btn-danger remove-btn mt-4 botonEliminar"><span class="material-symbols-outlined">remove</span></button>
            </div>
        `;

        idiomasContainer.appendChild(newRow);

        //Sumarle uno al indice
        document.getElementById("numeroIndice").value = parseInt(idiomasIndex) + 1;

        verificarHistorial("idiomasContainer", "sinIdiomas");
    });

    // Eliminar o restaurar idioma
    document.getElementById("idiomasContainer").addEventListener("click", function (event) {
        // Verificar si se hizo clic en el botón o dentro de él
        const removeButton = event.target.closest(".remove-btn");
        if (removeButton) {
            // Obtener la fila del idioma
            const idiomaRow = removeButton.closest(".idioma-row");

            if (idiomaRow) {
                // Obtener el input del ID
                const idiomaIdInput = idiomaRow.querySelector("input[name$='.Id']");
                const allInputs = idiomaRow.querySelectorAll("input, select, textarea");
                const etiquetaA = idiomaRow.querySelector("a");
                const buttonUndo = idiomaRow.querySelector(".botonEliminarDocumento");

                let tieneContenido = Array.from(allInputs).some(input => {
                    // Verificar si el input es visible
                    if (input.offsetParent === null || getComputedStyle(input).display === "none" || getComputedStyle(input).visibility === "hidden") {
                        return false; // Ignorar inputs no visibles
                    }
                                
                    if (input.tagName === "SELECT") {
                        // Para selects, verificar que tengan un valor seleccionado
                        return input.value.trim() !== "";
                    } else if (input.tagName === "TEXTAREA" || input.tagName === "INPUT") {
                        // Para inputs o textareas, ignorar valores como 0 si son numéricos
                        if (input.type === "number") {
                            return input.value.trim() !== "" && input.value.trim() !== "0";
                        }
                        return input.value.trim() !== "";
                    } else if (input.type === "file") {
                        // Para inputs de tipo file, verificar si hay archivos seleccionados
                        return input.files.length > 0;
                    } else if (input.type === "checkbox" || input.type === "radio") {
                        // Para checkboxes y radios, verificar si están seleccionados
                        return input.checked;
                    }
                    return false;
                });
            
                if (idiomaIdInput) {
                    const idiomaIdValue = Number(idiomaIdInput.value);

                    if (idiomaRow.classList.contains("eliminado")) {
                        // Restaurar la fila
                        idiomaIdInput.value = Math.abs(idiomaIdValue); // Cambiar a positivo
                        idiomaRow.classList.remove("eliminado"); // Quitar sombreado
                        removeButton.querySelector("span").textContent = "remove"; // Cambiar ícono
                        allInputs.forEach(input => input.removeAttribute("readonly")); // Habilitar inputs
                        etiquetaA.style.pointerEvents = "auto"; // Habilitar enlace
                        buttonUndo.style.pointerEvents = "auto"; // Habilitar botón
                    } else {
                        if (idiomaIdValue !== 0) { 
                            idiomaIdInput.value = idiomaIdValue * -1; // Cambiar a negativo

                            idiomaRow.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar botón
                        } 
                        if(idiomaIdValue === 0 && !tieneContenido){
                            idiomaRow.remove(); // Eliminar completamente si no tiene contenido
                        }
                        else{
                            idiomaRow.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar botón
                        }
                    }
                }
            }

            verificarHistorial("idiomasContainer", "sinIdiomas");
        }
    });


    
    verificarHistorial("idiomasContainer", "sinIdiomas");

});



//-------------------------------------------------Codigo para agregar datos academicos
document.addEventListener("DOMContentLoaded", function () {
    let estudiosIndex = 1;

    // Almacenar las opciones de los selects en un array para usarlas más tarde
    const opcionesNivelEstudios = [];
    const opcionesEstatusEstudio = [];
    const originalSelectNivelEstudios = document.getElementById('nivelEstudios_0'); // El primer select de "Nivel de Estudios"
    const originalSelectEstatusEstudio = document.getElementById('estatusEstudio_0'); // El primer select de "Estatus Estudio"

    // Rellenar el array con las opciones de los select
    Array.from(originalSelectNivelEstudios.options).forEach(option => {
        opcionesNivelEstudios.push({ value: option.value, text: option.text });
    });

    Array.from(originalSelectEstatusEstudio.options).forEach(option => {
        opcionesEstatusEstudio.push({ value: option.value, text: option.text });
    });

    // Función para cargar opciones dinámicamente en un select
    function cargarOpcionesSelect(selectElement, opciones) {
        selectElement.innerHTML = ''; // Limpiar cualquier opción existente
        opciones.forEach(opcion => {
            const newOption = document.createElement('option');
            newOption.value = opcion.value;
            newOption.text = opcion.text;
            selectElement.appendChild(newOption);
        });
    }

    // Agregar nuevo estudio
    document.getElementById("addEstudioBtn").addEventListener("click", function () {
        const estudiosContainer = document.getElementById("estudiosContainer");
        const estudiosIndex = document.getElementById("numeroIndiceDatosAcademicos").value;
        const newRow = document.createElement("div");
        newRow.className = "form-group row mb-3 align-items-center estudio-row estudioContainerPreguntas";

        newRow.innerHTML = `
            <input type="hidden" name="estudios[${estudiosIndex}].Id" value="0" />
            <div class="col-md-12">
                <label for="nivelEstudios_${estudiosIndex}" class="form-label">Nivel de Estudios:</label>
                <select id="nivelEstudios_${estudiosIndex}" name="estudios[${estudiosIndex}].IdNivelAcademico" class="form-control" required>
                    <option value="" selected>Seleccione una opción</option>
                </select>
            </div>
            <div class="col-md-12">
                <label for="tituloObtenido_${estudiosIndex}" class="form-label">Título Obtenido:</label>
                <input type="text" class="form-control" name="estudios[${estudiosIndex}].NombreTitulo" id="tituloObtenido_${estudiosIndex}" required>
            </div>
            <div class="col-md-12">
                <label for="celula_${estudiosIndex}" class="form-label">Cédula(opcional):</label>
                <input type="number" class="form-control" name="estudios[${estudiosIndex}].Cedula" id="celula_${estudiosIndex}">
            </div>
            <div class="col-md-12">
                <label for="estatusEstudio_${estudiosIndex}" class="form-label">Estatus Estudio:</label>
                <select id="estatusEstudio_${estudiosIndex}" name="estudios[${estudiosIndex}].IdEstatusEstudio" class="form-control" required>
                    <option value="" selected>Seleccione una opción</option>
                    @foreach (var estatus in Model.estatusEstudio)
                    {
                        <option value="@estatus.Id">@estatus.Nombre</option>
                    }
                </select>
            </div>
            <div class="col-md-12">
                <label for="documento_${estudiosIndex}" class="form-label">Documento(opcional):</label>
                <input type="file" class="form-control" name="estudios[${estudiosIndex}].IdDocumento.Archivo" id="documento_${estudiosIndex}">
            </div>
            <div class="col-md-2 contenedorBoton">
                <button type="button" class="btn btn-danger remove-btn mt-4 botonEliminar"><span class="material-symbols-outlined remove-span">remove</span></button>
            </div>
        `;

        estudiosContainer.appendChild(newRow);

        // Llenar los selects con las opciones desde el array
        cargarOpcionesSelect(newRow.querySelector(`#nivelEstudios_${estudiosIndex}`), opcionesNivelEstudios);
        cargarOpcionesSelect(newRow.querySelector(`#estatusEstudio_${estudiosIndex}`), opcionesEstatusEstudio);

        //Sumarle uno al indice
        document.getElementById("numeroIndiceDatosAcademicos").value = parseInt(estudiosIndex) + 1;
        
        verificarHistorial("estudiosContainer", "sinDatosAcademicos");
    });

    // Eliminar o restaurar estudio
    document.getElementById("estudiosContainer").addEventListener("click", function (event) {
        const removeButton = event.target.closest(".remove-btn");
        if (removeButton) {
            const estudioRow = removeButton.closest(".estudio-row");
            
            if (estudioRow) {
                const estudioIdInput = estudioRow.querySelector("input[name$='.Id']");
                const allInputs = estudioRow.querySelectorAll("input, select, textarea");
                const etiquetaA = estudioRow.querySelector("a");
                const buttonUndo = estudioRow.querySelector(".botonEliminarDocumento");

                let tieneContenido = Array.from(allInputs).some(input => {
                    // Verificar si el input es visible
                    if (input.offsetParent === null || getComputedStyle(input).display === "none" || getComputedStyle(input).visibility === "hidden") {
                        return false; // Ignorar inputs no visibles
                    }
                                
                    if (input.tagName === "SELECT") {
                        // Para selects, verificar que tengan un valor seleccionado
                        return input.value.trim() !== "";
                    } else if (input.tagName === "TEXTAREA" || input.tagName === "INPUT") {
                        // Para inputs o textareas, ignorar valores como 0 si son numéricos
                        if (input.type === "number") {
                            return input.value.trim() !== "" && input.value.trim() !== "0";
                        }
                        return input.value.trim() !== "";
                    } else if (input.type === "file") {
                        // Para inputs de tipo file, verificar si hay archivos seleccionados
                        return input.files.length > 0;
                    } else if (input.type === "checkbox" || input.type === "radio") {
                        // Para checkboxes y radios, verificar si están seleccionados
                        return input.checked;
                    }
                    return false;
                });
            
                
                if (estudioIdInput) {
                    const estudioIdValue = Number(estudioIdInput.value);

                    if (estudioRow.classList.contains("eliminado")) {
                        // Restaurar la fila
                        estudioIdInput.value = Math.abs(estudioIdValue); // Cambiar a positivo
                        estudioRow.classList.remove("eliminado"); // Quitar sombreado
                        removeButton.querySelector("span").textContent = "remove"; // Cambiar ícono
                        allInputs.forEach(input => input.removeAttribute("readonly")); // Habilitar inputs
                        etiquetaA.style.pointerEvents = "auto"; // Habilitar enlace
                        buttonUndo.style.pointerEvents = "auto"; // Habilitar enlace
                    } else {
                        if (estudioIdValue !== 0) {
                            estudioIdInput.value = estudioIdValue * -1; // Cambiar a negativo

                            estudioRow.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar enlace
                        }
                        if(estudioIdValue === 0 && !tieneContenido){
                            estudioRow.remove(); // Eliminar completamente si no tiene contenido
                        } else {
                            estudioRow.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar enlace
                        }
                    }
                }
            }

            verificarHistorial("estudiosContainer", "sinDatosAcademicos");
        }
    });


    // Inicializar el mensaje de "Sin historial" si está vacío al cargar
    verificarHistorial("estudiosContainer", "sinDatosAcademicos");
});



//-------------------------------------------------Codigo para agregar historial profesioanl

document.addEventListener("DOMContentLoaded", function () {
    let experienciaIndex = 1;

    // Almacenar las opciones del select de tipo de experiencia
    const opcionesTipoExperiencia = [];
    const originalSelectTipoExperiencia = document.getElementById('IdEstatusTipoExperiencia0'); // El primer select de "Tipo de Experiencia"

    // Rellenar el array con las opciones del select
    Array.from(originalSelectTipoExperiencia.options).forEach(option => {
        opcionesTipoExperiencia.push({ value: option.value, text: option.text });
    });

    // Función para cargar opciones dinámicamente en un select
    function cargarOpcionesSelect(selectElement, opciones) {
        selectElement.innerHTML = ''; // Limpiar cualquier opción existente
        opciones.forEach(opcion => {
            const newOption = document.createElement('option');
            newOption.value = opcion.value;
            newOption.text = opcion.text;
            selectElement.appendChild(newOption);
        });
    }

    // Agregar nueva experiencia profesional
    document.getElementById("addHistorialAcademicoBtn").addEventListener("click", function () {
        const container = document.querySelector(".containerDatosProfesionales");
        const newCard = document.createElement("div");
        const experienciaIndex = document.getElementById("numeroIndiceHistorialProfesional").value;
        newCard.className = "form-group containerDatosProfesionales containerDatosProfesionalesCard";

        newCard.innerHTML = `
            <input type="hidden" name="experiencias[${experienciaIndex}].Id" value="0" />
            <div class="row mb-3 containerDatosProfesionales__headerCar">
                <div class="col-md-12">
                    <h3>Información de la experiencia</h3>
                </div>
                <div class="col-md-2 contenedorBoton">
                    <button type="button" class="btn btn-danger remove-btn botonEliminar"><span class="material-symbols-outlined remove-span">remove</span></button>
                </div>
            </div>
            <div>
                <div class="row mb-3">
                    <div class="col-md-12 mb-3">
                        <label for="nombreEmpleador${experienciaIndex}" class="form-label">Nombre del Empleador(opcional):</label>
                        <input type="text" class="form-control" name="experiencias[${experienciaIndex}].NombreEmpleador" id="nombreEmpleador${experienciaIndex}">
                    </div>
                    <div class="col-md-12">
                        <label for="cargoOcupado${experienciaIndex}" class="form-label">Cargo Ocupado:</label>
                        <input type="text" class="form-control" name="experiencias[${experienciaIndex}].CargoOcupado" id="cargoOcupado${experienciaIndex}" required>
                    </div>
                </div> 
                <div class="row mb-3">
                    <div class="col-md-12 mb-3">
                        <label for="IdEstatusTipoExperiencia${experienciaIndex}" class="form-label">Tipo de Experiencia:</label>
                        <select name="experiencias[${experienciaIndex}].IdEstatusTipoExperiencia" class="form-control" id="IdEstatusTipoExperiencia${experienciaIndex}" required>
                            <option value="" selected>Seleccione una opción</option>
                        </select>
                    </div>
                    <div class="col-md-12">
                        <label for="descripcion${experienciaIndex}" class="form-label">Descripción:</label>
                        <input type="text" class="form-control" name="experiencias[${experienciaIndex}].Descripcion" id="descripcion${experienciaIndex}" required>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-12 mb-3">
                        <label for="fechaInicio${experienciaIndex}" class="form-label">Fecha de Inicio:</label>
                        <input type="date" class="form-control" name="experiencias[${experienciaIndex}].FechaInicio" id="fechaInicio${experienciaIndex}" required>
                    </div>
                    <div class="col-md-12">
                        <label for="FechaTermino${experienciaIndex}" class="form-label">Fecha de Fin(opcional):</label>
                        <input type="date" class="form-control" name="experiencias[${experienciaIndex}].FechaTermino" id="FechaTermino${experienciaIndex}">
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-12">
                        <label for="archivo${experienciaIndex}" class="form-label">Carta de recomendación(opcional):</label>
                        <input type="file" class="form-control" name="experiencias[${experienciaIndex}].IdDocumento.Archivo" id="archivo${experienciaIndex}">
                    </div>
                </div>
            </div>
        `;

        container.appendChild(newCard);

        // Llenar el select de "Tipo de Experiencia" con las opciones
        cargarOpcionesSelect(newCard.querySelector(`#IdEstatusTipoExperiencia${experienciaIndex}`), opcionesTipoExperiencia);

        //Sumarle uno al indice
        document.getElementById("numeroIndiceHistorialProfesional").value = parseInt(experienciaIndex) + 1;

        verificarHistorial("containerDatosProfesionales", "sinHistorialProfesional");
    });

    // Eliminar experiencia profesional
    document.addEventListener("click", function (e) {
        const removeButton = e.target.closest(".remove-btn");
        if (removeButton) {
            const headercard = removeButton.closest(".containerDatosProfesionales__headerCar");
            const card = headercard.closest(".containerDatosProfesionalesCard");
            if (card) {
                const experienciaIdInput = card.querySelector("input[name$='.Id']");
                const allInputs = card.querySelectorAll("input, select, textarea");
                const etiquetaA = card.querySelector("a");
                const buttonUndo = card.querySelector(".botonEliminarDocumento");

                let tieneContenido = Array.from(allInputs).some(input => {
                    // Verificar si el input es visible
                    if (input.offsetParent === null || getComputedStyle(input).display === "none" || getComputedStyle(input).visibility === "hidden") {
                        return false; // Ignorar inputs no visibles
                    }
                                
                    if (input.tagName === "SELECT") {
                        // Para selects, verificar que tengan un valor seleccionado
                        return input.value.trim() !== "";
                    } else if (input.tagName === "TEXTAREA" || input.tagName === "INPUT") {
                        // Para inputs o textareas, ignorar valores como 0 si son numéricos
                        if (input.type === "number") {
                            return input.value.trim() !== "" && input.value.trim() !== "0";
                        }
                        return input.value.trim() !== "";
                    } else if (input.type === "file") {
                        // Para inputs de tipo file, verificar si hay archivos seleccionados
                        return input.files.length > 0;
                    } else if (input.type === "checkbox" || input.type === "radio") {
                        // Para checkboxes y radios, verificar si están seleccionados
                        return input.checked;
                    }
                    return false;
                });

                if (experienciaIdInput) {
                    const experienciaIdValue = Number(experienciaIdInput.value);

                    if (card.classList.contains("eliminado")) {
                        // Restaurar la fila
                        experienciaIdInput.value = Math.abs(experienciaIdValue); // Cambiar a positivo
                        card.classList.remove("eliminado"); // Quitar sombreado
                        removeButton.querySelector("span").textContent = "remove"; // Cambiar ícono
                        allInputs.forEach(input => input.removeAttribute("readonly")); // Habilitar inputs
                        etiquetaA.style.pointerEvents = "auto"; // Habilitar enlace
                        buttonUndo.style.pointerEvents = "auto"; // Habilitar enlace
                    } else {
                        if (experienciaIdValue !== 0) {
                            experienciaIdInput.value = experienciaIdValue * -1; // Cambiar a negativo

                            card.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar enlace
                        }
                        if(experienciaIdValue === 0 && !tieneContenido){
                            card.remove(); // Eliminar completamente si no tiene contenido
                        } else {
                            card.classList.add("eliminado"); // Marcar como eliminado
                            removeButton.querySelector("span").textContent = "undo"; // Cambiar ícono
                            allInputs.forEach(input => input.setAttribute("readonly", true)); // Deshabilitar visualmente
                            etiquetaA.style.pointerEvents = "none"; // Deshabilitar enlace
                            buttonUndo.style.pointerEvents = "none"; // Deshabilitar enlace
                        }
                    }
                }
            }

            verificarHistorial("containerDatosProfesionales", "sinHistorialProfesional");
        }
    });

    // Inicializar el mensaje de "Sin historial" si está vacío al cargar
    verificarHistorial("containerDatosProfesionales", "sinHistorialProfesional");
});



//-------------------------------------------------FUNCIONES PARA DETECTAR CAMBIOS Y MOSTRAR ALERTAS DE CAMBIOS NO GUARDADOS

// Función para capturar datos iniciales de una sección
function captureInitialData(sectionId) {
    const form = document.getElementById(`form${sectionId}`);
    if (form) {
        // Captura los datos del formulario
        const formData = new FormData(form);
        // Crear un objeto para almacenar los valores iniciales, incluyendo los archivos
        const initialData = {};
        
        // Guardar los valores de los campos de texto y otros tipos
        formData.forEach((value, key) => {
            initialData[key] = value;
        });

        // Guardar los archivos seleccionados
        form.querySelectorAll('input[type="file"]').forEach(fileInput => {
            initialData[fileInput.name] = fileInput.files.length;
        });

        // Almacenar el estado inicial
        form.dataset.initialData = JSON.stringify(initialData);
    }
}

// Verifica si hay cambios en una sección
function hasDataChanged(sectionId) {
    const form = document.getElementById(`form${sectionId}`);
    if (form) {
        const initialData = JSON.parse(form.dataset.initialData);
        const currentData = {};
        
        // Comparar los datos actuales del formulario
        new FormData(form).forEach((value, key) => {
            currentData[key] = value;
        });

        // Comparar los archivos seleccionados
        form.querySelectorAll('input[type="file"]').forEach(fileInput => {
            currentData[fileInput.name] = fileInput.files.length;
        });

        // Verificar si hay diferencias
        return JSON.stringify(initialData) !== JSON.stringify(currentData);
    }
    return false;
}

// Función para abrir la sección y marcar el botón correspondiente
function activarSeccion(target) {
    document.querySelector(`[data-bs-target="${target}"]`).click();

    const botones = document.querySelectorAll('#buttonMenu button');
    botones.forEach(function(button) {
        button.classList.remove('active');
    });

    document.querySelector(`[data-bs-target="${target}"]`).classList.add('active');
}

// Manejador de eventos para botones del menú
document.querySelectorAll('#buttonMenu button').forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault();  // Evita comportamiento predeterminado del botón
        event.stopImmediatePropagation();  // Evita que Bootstrap procese el cambio inmediatamente

        const nextSectionId = this.getAttribute('data-bs-target').substring(1); // Extrae el ID sin #
        
        // Verificar si hay cambios en la sección actual
        if (hasDataChanged(seccionActualId)) {
            Swal.fire({
                title: '¿Tienes cambios sin guardar?',
                text: "¿Deseas guardar los cambios antes de cambiar de sección?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, guardar',
                cancelButtonText: 'No, continuar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Enviar el formulario si es válido
                    const form = document.getElementById(`form${seccionActualId}`);
                    if (form && form.checkValidity()) {
                        form.requestSubmit(); // Respetar validaciones de formulario
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
                } else {
                    // Si el usuario cancela, cambiar de sección sin guardar
                    captureInitialData(seccionActualId);
                    activarSeccion(`#${nextSectionId}`);
                    captureInitialData(nextSectionId);
                    seccionActualId = nextSectionId;
                }
            });
        } else {
            // Si no hay cambios, cambiar de sección
            activarSeccion(`#${nextSectionId}`);
            captureInitialData(nextSectionId);
            seccionActualId = nextSectionId;
        }
    });
});

// Evita comportamiento predeterminado de Bootstrap en el evento show.bs.collapse
document.querySelectorAll('.collapse').forEach(collapse => {
    collapse.addEventListener('show.bs.collapse', function (event) {
        // Prevenir que el colapso ocurra si hay cambios no guardados
        if (hasDataChanged(seccionActualId)) {
            event.preventDefault(); // Evita que el colapso suceda
        }
    });
});



/*--------------------------------------------------------------------------------------Funciones generales*/


//Funcion para capturar la data inicial dependeindo de si ya se completaron las peticiones

// Incrementar el contador cuando se completa una petición
function comparadorRequestCounter() {
    // Si el contador ha alcanzado el número total de solicitudes pendientes
    if (requestsCompleted === totalRequests && !estatus) {
        captureInitialData(seccionActualId); // Ejecutamos la función cuando todas las peticiones han terminado
        estatus = true;
    }
}



//Actualizar input de documentos si cambio.
function actualizarVistaPrevia(event) {
    const input = event.target;
    const file = input.files[0];

    if (file) {
        const id = input.id; //El input poculto tinee el mismo id actual per agregado _Id
        inputOculto = document.getElementById(`${id}_Id`); // Obtener el input oculto para cambiarlo a negativo si se edito
        if (inputOculto) {
            inputOculto.value *= -1;
        }

    }
}

//CAMBIOAR EL INPUT OCULTO PARA SABER SI UN DOCUMENTO FUE ELIMINADO
function cambiarEstadoEliminado(event) {
    // Busca el botón más cercano con la clase "botonEliminarDocumento"
    const button = event.target.closest(".botonEliminarDocumento");
    if (button) {
        const span = button.querySelector("span");
        // Busca el enlace más cercano en el mismo contenedor que el botón
        const etiqueta = button.parentElement.querySelector("a");
        const input = button.parentElement.querySelector("input");
        if (span && etiqueta) {
            // Alterna el texto dentro del span
            span.textContent = span.textContent === "delete" ? "undo" : "delete";
            // Alterna el estilo de texto tachado en la etiqueta
            etiqueta.style.textDecoration = etiqueta.style.textDecoration === "line-through" ? "none" : "line-through";
            //ELterna el value del input
            input.value = input.value === "true" ? "false" : "true";
        }
    }
}



//Mostrar una alerta antes de guardar cambios 
document.querySelectorAll('.envioSubmirButton').forEach(button => {
    button.addEventListener('click', function (event) {
        event.preventDefault(); // Evita que el formulario se envíe inmediatamente

        const form = button.closest('form'); // Encuentra el formulario más cercano al botón
        if (!form) return; // Si no hay formulario, no hace nada

        // Muestra la alerta de SweetAlert
        Swal.fire({
            title: '¿Estás seguro?',
            text: "La información nueva reemplazará la anterior",
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



//eVITANDO QUE LOS LABEL ABRAN INPUTS DE ARCHIVOS CUANDO YA ESTAN DESHABILITADOS
document.addEventListener("click", function (event) {
    const label = event.target.closest("label[for]");
    if (label) {
        const inputId = label.getAttribute("for");
        const fileInput = document.getElementById(inputId);

        if (fileInput && fileInput.hasAttribute("readonly")) {
            event.preventDefault(); // Evitar que se abra el cuadro de selección de archivos
        }
    }
});


