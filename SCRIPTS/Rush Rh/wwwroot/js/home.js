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