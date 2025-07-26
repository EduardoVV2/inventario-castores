using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Rush_Rh.Pages
{
    public class SolicitudLicenciasVacacionesModel : PageModel
    {
        private readonly ILogger<SolicitudLicenciasVacacionesModel> _logger;

        public SolicitudLicenciasVacacionesModel(ILogger<SolicitudLicenciasVacacionesModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet(int? id)
        {
            // Simulación de datos para ejemplooooooo
            string nombreEmpleado = "Juan Pérez";
            string tipoSolicitud = "Vacaciones";
            string fechaInicio = "2025-03-01";
            string fechaFin = "2025-03-10";

            // Crear el PDF con iText7
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Agregar contenido al PDF
                document.Add(new Paragraph("Solicitud de Licencia/Vacaciones"));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Empleado: {nombreEmpleado}"));
                document.Add(new Paragraph($"Tipo de solicitud: {tipoSolicitud}"));
                document.Add(new Paragraph($"Fecha inicio: {fechaInicio}"));
                document.Add(new Paragraph($"Fecha fin: {fechaFin}"));

                // Cerrar el documento
                document.Close();
                pdf.Close();
                writer.Close();

                // **Reiniciar la posición del MemoryStream**
                ms.Position = 0;

                // Devolver el PDF como descarga
                return File(ms.ToArray(), "application/pdf", "Solicitud.pdf");
            }
        }
    }
}
