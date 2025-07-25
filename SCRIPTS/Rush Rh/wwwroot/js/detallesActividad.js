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
