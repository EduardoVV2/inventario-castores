// Write your JavaScript code.
function handleBuscar (){
    var selectMes = $("#selectMes").val();
    var selectAnio = $("#selectAnio").val();

    // Validar que todos los campos estén llenos
    if (
        !selectMes || !selectAnio 
    ) {
        alert("Por favor, completa todos los campos antes de enviar el formulario.");
        return false; // Detener la ejecución si falta algún dato
    }
    
    var Info = { 
        Mes : selectMes,
        Año : selectAnio
    }
    $.ajax({
        type: "POST",
        url: '/PausasActivas?handler=PausasActivas',
        data: JSON.stringify(Info),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    }).done(function (data) {
        if (data.success) {
            console.log("Datos recibidos:", data.pausasActivas);

            // Actualizar la variable `registros` global
            registros = data.pausasActivas;

            // Actualizar el calendario con el mes, año y nuevos registros
            fillCalendar(Info.Año, Info.Mes, registros); // Mes es 0-indexado en JS
        } else {
            console.error("Error en la solicitud:", error);
        }
    
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("Error en la solicitud AJAX:");
        console.error("Estado:", textStatus);
        console.error("Error:", errorThrown);
        console.error("Respuesta:", jqXHR.responseText);
    });

    // Cierra el collapse
    const collapseElement = document.getElementById("collapseCabecera");
    const bsCollapse = new bootstrap.Collapse(collapseElement, {
        toggle: false, // Evita que lo abra si ya está cerrado
    });
    bsCollapse.hide(); // Cierra el collapse
}

