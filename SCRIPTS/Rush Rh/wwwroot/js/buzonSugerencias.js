document.addEventListener("DOMContentLoaded", function() {
    // Inicializa el editor SCEditor en el textarea
    var textarea = document.getElementById("myTextArea");
    sceditor.create(textarea, {
        format: "xhtml", // Formato de salida
        style: "https://cdn.jsdelivr.net/npm/sceditor@3/minified/themes/content/default.min.css",
        width: "100%",
        height: "300px",
    });
    // Configuración adicional de SCEditor, si es necesario
    // Puedes agregar más configuraciones aquí
    // Por ejemplo, habilitar la barra de herramientas
    // toolbar: "bold,italic,underline,strike,subscript,superscript,|,left,center,right,justify,|,font,size,color,bold,italic,underline,strike,|,image,email,url,youtube",

    // Llenar modal para responder sugerencia
    const botonesResponder = document.querySelectorAll("button[name='buttonResponder']");
    botonesResponder.forEach(boton => {
        boton.addEventListener("click", function (event) {
            event.preventDefault(); // Evitar el comportamiento predeterminado del botón
            const sugerenciaId = boton.value;
            const sugerenciaResponder = todasSugerencias.find(sugerencia => sugerencia.Id === parseInt(sugerenciaId));
    
            if (sugerenciaResponder) {
                // Llenar el formulario con los datos del aviso
                // Llenar SCEditor con el contenido del aviso
                document.getElementById("sugerenciaResponder").innerHTML = sugerenciaResponder.Contenido;
                
                // Llenar el campo de ID de la sugerencia
                document.getElementById("IdResponderSugerencia").value = sugerenciaResponder.Id;
            } else {
                console.error("sugerencia no encontrado.");
            }
        });
    });
});