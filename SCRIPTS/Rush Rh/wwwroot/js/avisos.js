// Obtener los elementos necesarios
const filtroInput = document.getElementById("filtroInput");
const tabla = document.getElementById("tablaAvisos").getElementsByTagName("tbody")[0];

// Agregar un evento al input para detectar cambios
filtroInput.addEventListener("input", function () {
    const filtro = filtroInput.value.toLowerCase(); // Convertir texto a minúsculas
    const filas = tabla.getElementsByTagName("tr"); // Obtener todas las filas del tbody

    // Recorrer todas las filas y ocultar las que no coincidan con el filtro
    for (let i = 0; i < filas.length; i++) {
        const celdas = filas[i].getElementsByTagName("td"); // Obtener las celdas de la fila
        let coincide = false;

        // Recorrer las celdas y verificar si alguna contiene el texto filtrado
        for (let j = 0; j < celdas.length; j++) {
            if (celdas[j].innerText.toLowerCase().includes(filtro)) {
                coincide = true;
                break;
            }
        }

        // Mostrar u ocultar la fila según si coincide o no
        filas[i].style.display = coincide ? "" : "none";
    }
});




// Selecciona todos los botones de radio por el atributo name
const filtrosFechas = document.querySelectorAll('input[name="fechaFiltro"]');
const claseFecha = document.getElementById('claseFecha');

// Agrega un event listener a cada botón de radio
filtrosFechas.forEach(radio => {
    radio.addEventListener("change", function () {
        if (radio.checked) {
            if (radio.value == "1") {
                claseFecha.textContent = 'Filtro de periodo de fechas por fecha de edicion:';
            } else if (radio.value == "2") {
                claseFecha.textContent = 'Filtro de periodo de fechas por fecha de evento:';
            } else {
                claseFecha.textContent = 'Filtro de periodo de fechas por fecha de envío:';
            }
        }
    });
});



//Cambiar de mostrar los avisos activos a inactivos
const activos = this.document.getElementById('Activos');
activos.addEventListener("change", function(){
    const filtro = activos.value;
    if (filtro=="activos"){
        const avisosActivos = document.getElementsByClassName('avisos_activos');
        for (let j = 0; j < avisosActivos.length; j++) {
            avisosActivos[j].style.display = "";
        }
        const avisosInactivos = document.getElementsByClassName('avisos_inactivos');
        for (let j = 0; j < avisosInactivos.length; j++) {
            avisosInactivos[j].style.display = "none";        
        }
    }
    else if (filtro=="inactivos"){
        const avisosActivos = document.getElementsByClassName('avisos_activos');
        for (let j = 0; j < avisosActivos.length; j++) {
            avisosActivos[j].style.display = "none";
        }
        const avisosInactivos = document.getElementsByClassName('avisos_inactivos');
        for (let j = 0; j < avisosInactivos.length; j++) {
            avisosInactivos[j].style.display = "";        
        }
    }
    else{
        const avisosActivos = document.getElementsByClassName('avisos_activos');
        for (let j = 0; j < avisosActivos.length; j++) {
            avisosActivos[j].style.display = "";
        }
        const avisosInactivos = document.getElementsByClassName('avisos_inactivos');
        for (let j = 0; j < avisosInactivos.length; j++) {
            avisosInactivos[j].style.display = "";        
        }
    }
});


//Funcion para mostrar el editor de texto en el modal
document.addEventListener("DOMContentLoaded", function () {
    var textarea = document.getElementById("myTextArea");
    sceditor.create(textarea, {
        format: "xhtml", // Puedes usar "xhtml" si prefieres HTML.
        style: "https://cdn.jsdelivr.net/npm/sceditor@3/minified/themes/content/default.min.css",
        width: "100%",
        height: "300px",
    });


    //Llenar datos en el modal para editar
    const botonesEditar = document.querySelectorAll("button[name='editar']");
    
    botonesEditar.forEach(boton => {
        boton.addEventListener("click", function (event) {
            event.preventDefault();
            const div = document.getElementById("archivoActual");
            div.style.display = "block";
    
            const avisoId = boton.value;
            const avisoEditar = avisos.find(aviso => aviso.Id === parseInt(avisoId));
            const documentoActual = documentos.find(documento => documento.Id === parseInt(avisoEditar.IdDocumento));
    
            if (avisoEditar) {
                // Llenar el formulario con los datos del aviso
                document.getElementById("IdAviso").value = avisoEditar.Id;
                document.getElementById("titulo").value = avisoEditar.Titulo;
                // Llenar SCEditor con el contenido del aviso
                const editorInstance = sceditor.instance(textarea);
                editorInstance.val(avisoEditar.Contenido); // Establecer el contenido
                
                document.getElementById("enviarPor").value = avisoEditar.MedioEnvio;
                handleSelectChange();
                document.getElementById("selectUsuario").value = avisoEditar.EnvioUsuario;
                document.getElementById("selectPuesto").value = avisoEditar.EnvioPuesto;
                document.getElementById("selectGenero").value = avisoEditar.EnvioGenero;
                document.getElementById("selectDepartamento").value = avisoEditar.EnvioDepartamento;
                document.getElementById("fechaEvento").value = avisoEditar.FechaEvento;
                
                // Mostrar el archivo asociado
                const archivoPreview = document.getElementById("archivoPreview");
                archivoPreview.innerHTML = "";
                
                if (documentoActual) {
                    document.getElementById("IdDocumentoActual").value = avisoEditar.IdDocumento;
                    if (documentoActual.URL.endsWith(".jpg") || documentoActual.URL.endsWith(".png") || documentoActual.URL.endsWith(".jpeg")) {
                        const imagen = document.createElement("img");
                        imagen.src = documentoActual.URL;
                        imagen.alt = "Vista previa del archivo";
                        imagen.style.maxWidth = "200px";
                        archivoPreview.appendChild(imagen);
                    }
                    else{
                        const enlaceArchivo = document.createElement("a");
                        enlaceArchivo.href = documentoActual.URL;
                        enlaceArchivo.textContent = "Ver Archivo Actual";
                        enlaceArchivo.target = "_blank";
    
                        archivoPreview.appendChild(enlaceArchivo);
    
                    }
                } else {
                    archivoPreview.textContent = "No hay archivo asociado.";
                    document.getElementById("IdDocumentoActual").value = 0;
                }
                // Muestra el modal
                const modalCrearAviso = new bootstrap.Modal(document.getElementById("modalCrearAviso"));
                modalCrearAviso.show();
            } else {
                console.error("Aviso no encontrado.");
            }
        });
    });
    const botonCrear = document.querySelectorAll("button[name='crear']");
    botonCrear .forEach(boton => {
        boton.addEventListener("click", function (event) {
            event.preventDefault();

            const editorInstance = sceditor.instance(textarea);
            editorInstance.val("");

            const div = document.getElementById("archivoActual");
            div.style.display = "none";

            document.getElementById("IdAviso").value = "";
            document.getElementById("titulo").value = "";
            document.getElementById("myTextArea").value = "";
            document.getElementById("enviarPor").value = "0";
            handleSelectChange();
            document.getElementById("selectUsuario").value = "";
            document.getElementById("selectPuesto").value = "";
            document.getElementById("selectGenero").value = "";
            document.getElementById("selectDepartamento").value = "";
            document.getElementById("fechaEvento").value = "";
            document.getElementById("IdDocumentoActual").value = "";
        });
    });

    
});


//Funcion para el funcionamiento y diseño de la zona de arrastrar y soltar archivos
document.addEventListener('DOMContentLoaded', function () {
    const dropZone = document.getElementById('drop-zone');
    const fileInput = document.getElementById('file-upload');

    // Función para mostrar los nombres de los archivos seleccionados
    function updateFileNames(files) {
        if (files.length > 0) {
            const fileNames = Array.from(files).map(file => file.name).join('<br>');
            dropZone.innerHTML = `<strong>Archivos seleccionados:</strong><br>${fileNames}`;
        } else {
            dropZone.innerHTML = 'No se seleccionó ningún archivo.';
        }
    }

    // Manejador de eventos para arrastrar y soltar
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(event => {
        dropZone.addEventListener(event, e => e.preventDefault());
        dropZone.addEventListener(event, e => e.stopPropagation());
    });

    ['dragenter', 'dragover'].forEach(event => {
        dropZone.addEventListener(event, () => dropZone.classList.add('dragover'));
    });

    ['dragleave', 'drop'].forEach(event => {
        dropZone.addEventListener(event, () => dropZone.classList.remove('dragover'));
    });

    dropZone.addEventListener('drop', event => {
        const files = event.dataTransfer.files;
        fileInput.files = files; // Vincula los archivos al input file
        updateFileNames(files); // Muestra los nombres en pantalla
    });

    // Manejador de eventos para seleccionar archivos mediante clic
    fileInput.addEventListener('change', () => {
        updateFileNames(fileInput.files); // Muestra los nombres en pantalla
    });

    dropZone.addEventListener('click', () => fileInput.click());
});


//Funcion para mostrar los selects secundarios según el valor seleccionado en el select principal
function handleSelectChange() {
    console.log("Evento onchange ejecutado");

    const enviarPor = document.getElementById("enviarPor").value;

    const enviarPorContainer = document.getElementById("enviarPorContainer");
    const usuarioContainer = document.getElementById("usuarioContainer");
    const puestoContainer = document.getElementById("puestoContainer");
    const generoContainer = document.getElementById("generoContainer");
    const departamentoContainer = document.getElementById("departamentoContainer");

    enviarPorContainer.style.width = "100%";

    // Reiniciar visibilidad
    usuarioContainer.style.display = "none";
    puestoContainer.style.display = "none";
    generoContainer.style.display = "none";
    departamentoContainer.style.display = "none";

    // Mostrar los select secundarios según el valor seleccionado
    switch (enviarPor) {
        case "1": 
            usuarioContainer.style.display = "block";
            enviarPorContainer.style.width = "calc(50% - .5vw)";
            break;
        case "2":
            generoContainer.style.display = "block";
            enviarPorContainer.style.width = "calc(50% - .5vw)";
            break;
        case "3": 
            puestoContainer.style.display = "block";
            enviarPorContainer.style.width = "calc(50% - .5vw)";
            break;
        case "5":
            departamentoContainer.style.display = "block";
            enviarPorContainer.style.width = "calc(50% - .5vw)";
            break;
        default: 
            break;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const modalCrearAviso = document.getElementById("modalCrearAviso");

    modalCrearAviso.addEventListener("shown.bs.modal", () => {
        const enviarPor = document.getElementById("enviarPor");
        enviarPor.addEventListener("change", handleSelectChange);
    });
});









// function crearAviso() {
//     var titulo = $("#titulo").val();
//     var contenido = $("#myTextArea").val();
//     var fecha = $("#fechaEvento").val();
//     var medioEnvio = $("#enviarPor").val();
//     var archivos = $("#file-upload").prop('files');

//     var info = {
//         Titulo: titulo,
//         // Contenido: "contenido",
//         // Fecha: fecha,
//         // MedioEnvio: medioEnvio
//     };

//     $.ajax({
//         type: "POST",
//         url: '/Avisos?handler=CrearAviso',
//         data: JSON.stringify(info),
//         beforeSend: function (xhr) {
//             xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
//             console.log($('input:hidden[name="__RequestVerificationToken"]').val())
//         },
//         contentType: "application/json; charset=utf-8",
//         dataType: "json"
//     }).done(function (data) {
//         console.log("funciona")
//     })

// }

