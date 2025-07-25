
let inputDocumentoAsistenciasChecador = document.getElementById("documentoAsistenciasChecador");
let btnlimpiarDatos = document.getElementById("btnlimpiarDatos");
let btnconfirmarInformacion = document.getElementById("btnconfirmarInformacion");
let spinner = document.getElementById("contenedorSpinnerAsistencias");


let datosGlobales = [];


inputDocumentoAsistenciasChecador.addEventListener("change", async function (event) {

    manejarEstadoEnvioAsistenciasDiseño();

    datosGlobales = await enviarDocumentoRetornarDatos();
    if (datosGlobales) {
        imprimirDatos(".contenedorTablaAsistencias"); 
        manejarEstadoLlegadaAsistenciasDiseño(); 
    } 
    
    
})



btnconfirmarInformacion.addEventListener("click", function (event) {
    event.preventDefault();

    manejarEstadoEnvioAsistenciasDiseño();
    form = document.getElementById("formGuardarAsistencias");
    form.submit();
});


async function enviarDocumentoRetornarDatos(){

    try{
        let token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF

        if (!inputDocumentoAsistenciasChecador.files.length) {
            throw new Error("No se ha seleccionado ningún archivo.");
        }
    
        let formData = new FormData();
        formData.append("DocumentoAsistenciasChecador", inputDocumentoAsistenciasChecador.files[0]);
    
        let url = "/ListaUsuarios?handler=AnalizarDocumentoAsistenciasChecador";
        let method = "POST";
    
        let dataRespuesta = await peticionFetchGenerica(formData, url, token, method);

        console.log(dataRespuesta);
        
        return dataRespuesta.datos;
    

    } catch (error) {
        console.log("Error: ", error); 
        manejarEstadoLlegadaAsistenciasDiseño();    
        limpiarDatos();
        swal.fire({title: "Error", text: error, icon: "error", confirmButtonText: "Aceptar",});
    }


}



async function peticionFetchGenerica(data = null, url = null, token = null, method = "GET"){

        if (!url) {
            console.error("URL no proporcionada");
            return;
        }

        const response = await fetch(url, {
            method: method,
            body: data,
            headers: {
                'XSRF-TOKEN': token 
            }
        })


        if(response.status === 401){
            throw new Error("No tienes permiso para acceder a esta página.");
        }

        if (!response.ok) {
            throw new Error(`Error en la petición: ${response.status} ${response.statusText}`);
        }

        
        const datajson = await response.json();


        if(!datajson.success){
            throw new Error(datajson.message);
        }


        return datajson;

}


async function obtenerDatosPaseLista(id){

    let token = $('input[name="__RequestVerificationToken"]').val(); // Obtener el token CSRF
    let url = "/ListaUsuarios?handler=DetallesPasesLista&idPaseLista=" + id;
    let method = "POST";

    let dataRespuesta = await peticionFetchGenerica(null, url, token, method);

    console.log(dataRespuesta);
    
    datosGlobales = dataRespuesta.datos;

    imprimirDatos(".contenedorTablaPaseLista")
}



function imprimirDatos(contenedorTabla){

    let contenedorTablaAsistencias = document.querySelector(contenedorTabla);

    contenedorTablaAsistencias.innerHTML = ""; // Limpiar el contenido existente
    let h2 = document.createElement("h2");
    h2.classList.add("tituloTablaAsistencias");
    h2.textContent = "Interpretación de datos";
    contenedorTablaAsistencias.appendChild(h2);

    //crear tabla
    let tabla = document.createElement("table");
    tabla.classList.add("table", "tablaAsistencias");
    tabla.innerHTML = `
        <thead>
            <tr class="align-middle">
                <th>Nombre</th>
                <th>Asistencias</th>
                <th>Retardos</th>
                <th>Faltas</th>
                <th>Licencias</th>
                <th>Dias festivos</th>
                <th>Vacaciones</th>
                <th>Más detalles</th>
            </tr>
        </thead>
        <tbody></tbody>
    `;
    contenedorTablaAsistencias.appendChild(tabla);
    
    let tbody = tabla.querySelector("tbody");
    tbody.innerHTML = ""; // Limpiar el contenido existente

    datosGlobales.forEach((dato, index) => {
        let fila = document.createElement("tr");
        fila.classList.add("align-middle");
        fila.innerHTML = `
            <td>${dato.nombre}</td>
            <td>${dato.numAsistencias}</td>
            <td>${dato.numRetardos}</td>
            <td>${dato.numFaltas}</td>
            <td>${dato.licencias}</td>
            <td>${dato.diasFestivos}</td>
            <td>${dato.vacaciones}</td>
            <td>
                <button class="btn buttonGeneral botonVerdePequeño" data-bs-toggle="modal" data-bs-target="#modalDetallesAsistencias" onclick="abrirModalDetallesAsistencias(${index})">Detalles</button>
            </td>
        `;
        tbody.appendChild(fila);
    });

}


function abrirModalDetallesAsistencias(index) {

    const listaDetalles = datosGlobales[index].detalles;

    // Ahora puedes llenar el modal con esos detalles (esto es solo un ejemplo)
    let contenedorTablaDetalles = document.querySelector(".contenedorTablaDetalles");

    contenedorTablaDetalles.innerHTML = ""; // limpiar contenido anterior

    let tablaDetalles = document.createElement("table");
    tablaDetalles.classList.add("table", "tablaDetalles");
    tablaDetalles.innerHTML = `
        <thead>
            <tr class="align-middle">
                <th>Fecha</th>
                <th>Tipo</th>
                <th>Detalles</th>
            </tr>
        </thead>
        <tbody></tbody>
    `;
    contenedorTablaDetalles.appendChild(tablaDetalles);

    let tbodyDetalles = tablaDetalles.querySelector("tbody");
    tbodyDetalles.innerHTML = ""; // Limpiar el contenido existente
    listaDetalles.forEach((detalle) => {
        let filaDetalles = document.createElement("tr");
        filaDetalles.innerHTML = `
            <td>${detalle.fecha}</td>
            <td>${detalle.tipo}</td>
            <td>${detalle.motivo}</td>
        `;
        tbodyDetalles.appendChild(filaDetalles);
    });

}




function manejarEstadoEnvioAsistenciasDiseño() {
    btnlimpiarDatos.disabled = true;
    btnconfirmarInformacion.disabled = true;
    inputDocumentoAsistenciasChecador.disabled = true; 
    inputDocumentoAsistenciasChecador.classList.add("disabled");
    spinner.style.display = "flex";
    let contenedorTablaAsistencias = document.querySelector(".contenedorTablaAsistencias");
    contenedorTablaAsistencias.innerHTML = ""; // Limpiar el contenido existente
}

function manejarEstadoLlegadaAsistenciasDiseño() {
    btnlimpiarDatos.disabled = false;
    btnconfirmarInformacion.disabled = false;
    spinner.style.display = "none";
    inputDocumentoAsistenciasChecador.disabled = false;
    inputDocumentoAsistenciasChecador.classList.remove("disabled");
}




const btnLimpiarDatos = document.getElementById("btnlimpiarDatos");
btnLimpiarDatos.addEventListener("click", function () {
    limpiarDatos();
    swal.fire({title: "Éxito", text: "Datos limpiados correctamente", icon: "success", confirmButtonText: "Aceptar",});
});


function limpiarDatos() {
    inputDocumentoAsistenciasChecador.value = "";
    let contenedorTablaAsistencias = document.querySelector(".contenedorTablaAsistencias");
    if(contenedorTablaAsistencias){
        contenedorTablaAsistencias.innerHTML = ""; // Limpiar el contenido existente
    }
    spinner.style.display = "none";
    datosGlobales = [];
    btnconfirmarInformacion.disabled = true;
    btnlimpiarDatos.disabled = true;
}


async function confirmacionEliminarPaseLista(idPaseLista) {
    const opcion = await messageAlert("¿Estás seguro?", "Se eliminará este pase de lista, esta acción no se puede deshacer.", "warning", "Sí, eliminar", "Cancelar");
    
    if(opcion) {
        form = document.getElementById("formEliminarPaseLista");
        form.submit();
    }


}

function messageAlert(title, text, icon, confirmButtonText, cancelButtonText = null) {
    return Swal.fire({ 
        title,
        text,
        icon,
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText,
        ...(cancelButtonText && { cancelButtonText }),
        showCancelButton: !!cancelButtonText // Mostrar botón solo si hay texto
    }).then((result) => {
        return result.isConfirmed; // Devuelve true si confirmó, false si canceló
    });
}



