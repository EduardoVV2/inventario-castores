using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using static Rush_Rh.Models.Formularios;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Rush_Rh.Pages
{
    public class PdfSolicitudesModel : PageModel
    {
        private readonly ILogger<PdfSolicitudesModel> _logger;
        private readonly string conexion;


        public VacacionDetalle Vacacion { get; set; }

        public LicenciaDetalle Licencia { get; set; }



        public List<Licencias> Licencias = new List<Licencias>();
        public List<Vacaciones> vacaciones = new List<Vacaciones>();


        public int DiasDisponiblesVacaciones { get; set; }
        public int TotalSolicitudesVacaciones { get; set; }
        public int TotalSolicitudesLicencias { get; set; }


        public PdfSolicitudesModel(ILogger<PdfSolicitudesModel> logger, IConfiguration configuration)
        {

            conexion = configuration.GetValue<string>("ConnectionStrings:Database");// Asigna la cadena de conexión real
            _logger = logger;
        }

         public IActionResult OnGet(int id, string tipo)
        {
            if (string.IsNullOrEmpty(tipo) || id <= 0)
                return BadRequest("Solicitud inválida.");

            string tipoNormalizado = tipo.Trim().ToLower();

            if (tipoNormalizado == "licencias") tipoNormalizado = "licencia";
            else if (tipoNormalizado == "vacaciones") tipoNormalizado = "vacacion";

            return tipoNormalizado switch
            {
                "vacacion" => OnGetGenerarPdfVacacion(id),
                "licencia" => OnGetGenerarPdfLicencia(id),
                _ => BadRequest("Tipo de solicitud no válido.")
            };
        }


        public IActionResult OnGetGenerarPdfVacacion(int id)
        {
            try
            {
                VacacionDetalle vacacion;
                int diasSinDisfrutar = 0;

                using (SqlConnection sqlConexion = new SqlConnection(conexion))
                {
                    sqlConexion.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.VacacionesGetById", sqlConexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdVacacion", id));
                        using (SqlDataReader dtr = cmd.ExecuteReader())
                        {
                            if (dtr.Read())
                            {
                                vacacion = construirVacacionDetalle(dtr);
                                GetDiasVacaciones(vacacion.IdUsuario);
                                GetTotalSolicitudes(vacacion.IdUsuario);

                                int TotalUsados = vacacion.TotalDiasUsados;
                                diasSinDisfrutar = DiasDisponiblesVacaciones - TotalUsados;

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    string rutaTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "Solicitud de vacaciones.pdf");

                                    PdfReader pdfReader = new PdfReader(rutaTemplate);
                                    PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
                                    PdfContentByte canvas = pdfStamper.GetOverContent(1);

                                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                    canvas.SetFontAndSize(bf, 10);
                                    canvas.SetColorFill(BaseColor.BLACK);

                                    AgregarTexto(canvas, 117, 700, vacacion.NombreUsuario ?? "");
                                    AgregarTexto(canvas, 117, 686, vacacion.Puesto ?? "");
                                    AgregarTexto(canvas, 387, 686, vacacion.Departamento ?? "");
                                    AgregarTexto(canvas, 182, 672, vacacion.NombreJefe ?? "");
                                    AgregarTexto(canvas, 182, 657, vacacion.FechaSolicitud.ToString("dd-MM-yyyy"));
                                    AgregarTexto(canvas, 122, 549, vacacion.FechaInicio.ToString("dd-MM-yyyy"));
                                    AgregarTexto(canvas, 248, 549, vacacion.FechaFin.ToString("dd-MM-yyyy"));
                                    AgregarTexto(canvas, 405, 549, vacacion.Reincorporacion.ToString("dd-MM-yyyy"));
                                    AgregarTexto(canvas, 505, 549, vacacion.DiasSolicitados.ToString());

                                    ColumnText ct = new ColumnText(canvas);
                                    ct.SetSimpleColumn(
                                        new Phrase(vacacion.Comentarios ?? "", new Font(Font.FontFamily.HELVETICA, 10)),
                                        52, 400, 520, 490, 14, Element.ALIGN_LEFT
                                    );
                                    ct.Go();

                                    AgregarTexto(canvas, 273, 592, vacacion.PeriodoVacacional ?? "");
                                    AgregarTexto(canvas, 205, 512, vacacion.TotalDiasUsados.ToString());
                                    AgregarTexto(canvas, 503, 512, DiasDisponiblesVacaciones.ToString());
                                    AgregarTexto(canvas, 204, 616, "X");
                                    AgregarTexto(canvas, 407, 512, diasSinDisfrutar.ToString());
                                    AgregarTexto(canvas, 438, 591, TotalSolicitudesVacaciones.ToString() + " permisos");

                                    pdfStamper.Close();
                                    pdfReader.Close();

                                    Response.Headers["Content-Disposition"] = "inline; filename=SolicitudDeVacaciones.pdf";
                                    return File(ms.ToArray(), "application/pdf");
                                }
                            }
                            else
                            {
                                return BadRequest("No se encontraron datos para la vacación.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al generar el PDF: " + ex.Message);
            }
        }



        private void AgregarTexto(PdfContentByte canvas, float x, float y, string texto)
        {
            canvas.BeginText();
            canvas.SetTextMatrix(x, y);
            canvas.ShowText(texto);
            canvas.EndText();
        }


        private VacacionDetalle construirVacacionDetalle(SqlDataReader vacacion)
        {
            return new VacacionDetalle()
            {
                Id = vacacion.GetInt32(vacacion.GetOrdinal("Id")),
                NombreJefe = vacacion["NombreCompletoJefe"].ToString(),
                NombreUsuario = vacacion["NombreCompletoUsuario"].ToString(),
                Comentarios = vacacion["Comentarios"].ToString(),
                Estatus = vacacion["Estatus"].ToString(),
                DiasSolicitados = vacacion.GetInt32(vacacion.GetOrdinal("DiasSolicitados")),
                FechaSolicitud = vacacion.GetDateTime(vacacion.GetOrdinal("FechaSolicitud")),
                FechaInicio = vacacion.GetDateTime(vacacion.GetOrdinal("FechaInicio")),
                FechaFin = vacacion.GetDateTime(vacacion.GetOrdinal("FechaFin")),
                Reincorporacion = vacacion.GetDateTime(vacacion.GetOrdinal("Reincorporacion")),
                Departamento = vacacion["NombreDepartamento"].ToString(),
                Puesto = vacacion["NombrePuesto"].ToString(),
                PeriodoVacacional = vacacion["PeriodoVacacional"].ToString(),
                TotalDiasUsados = vacacion.GetInt32(vacacion.GetOrdinal("TotalDiasUsados")),
                IdUsuario = Convert.ToInt32(vacacion["IdUsuario"]),
            };
        }
        private void GetTotalSolicitudes(int idUsuario)
        {
            using (SqlConnection sqlConexion = new SqlConnection(conexion))
            {
                sqlConexion.Open();
                using (SqlCommand cmd = new SqlCommand("GetTotalSolicitudesVacaciones", sqlConexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));

                    object resultado = cmd.ExecuteScalar();
                    if (resultado != null)
                    {
                        TotalSolicitudesVacaciones = Convert.ToInt32(resultado);
                    }
                }
            }
        }


        private void GetDiasVacaciones(int id)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    DateTime FechaInicio = DateTime.Now;
                    DateTime FechaActual = DateTime.Now;

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", id));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetFechaInicioKardex", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            if (DateTime.TryParse(dtr["FechaInicio"]?.ToString(), out DateTime resultado))
                            {
                                FechaInicio = resultado;
                            }
                        }
                    }

                    // Calcular años transcurridos
                    int AniosTranscurridos = FechaActual.Year - FechaInicio.Year;

                    // Ajustar si el aniversario no ha llegado este año
                    if (FechaInicio.AddYears(AniosTranscurridos) > FechaActual)
                    {
                        AniosTranscurridos--;
                    }
                    // Asignar días de vacaciones según la antigüedad
                    if (AniosTranscurridos == 0)
                        DiasDisponiblesVacaciones = 0;
                    else if (AniosTranscurridos == 1)
                        DiasDisponiblesVacaciones = 12;
                    else if (AniosTranscurridos == 2)
                        DiasDisponiblesVacaciones = 14;
                    else if (AniosTranscurridos == 3)
                        DiasDisponiblesVacaciones = 16;
                    else if (AniosTranscurridos == 4)
                        DiasDisponiblesVacaciones = 18;
                    else if (AniosTranscurridos == 5)
                        DiasDisponiblesVacaciones = 20;
                    else if (AniosTranscurridos > 5 && AniosTranscurridos < 11)
                        DiasDisponiblesVacaciones = 22;
                    else if (AniosTranscurridos > 10 && AniosTranscurridos < 16)
                        DiasDisponiblesVacaciones = 24;
                    else if (AniosTranscurridos > 15 && AniosTranscurridos < 21)
                        DiasDisponiblesVacaciones = 26;
                    else if (AniosTranscurridos > 20 && AniosTranscurridos < 26)
                        DiasDisponiblesVacaciones = 28;
                    else if (AniosTranscurridos >= 26)
                        DiasDisponiblesVacaciones = 30;
                }
                catch (Exception ex)
                {
                    throw new Exception("Hubo un error al obtener los días de vacaciones del usuario", ex);
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }
        public IActionResult OnGetGenerarPdfLicencia(int id)
        {
            try
            {
                // Obtener los detalles de la vacación desde la BD
                LicenciaDetalle licencia;
                using (SqlConnection sqlConexion = new SqlConnection(conexion))
                {
                    sqlConexion.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.LicenciasGetById", sqlConexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdLicencia", id));
                        using (SqlDataReader dtr = cmd.ExecuteReader())
                        {
                            if (dtr.Read())
                            {
                                licencia = construirLicenciaDetalle(dtr);
                                GetTotalSolicitudes(licencia.IdUsuario);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    // Ruta de la plantilla PDF en wwwroot/templates
                                    string rutaTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "Solicitud de licencias.pdf");

                                    // Cargar el PDF existente
                                    PdfReader pdfReader = new PdfReader(rutaTemplate);
                                    PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
                                    PdfContentByte canvas = pdfStamper.GetOverContent(1); // Página 1 del PDF

                                    // Configurar fuente y tamaño
                                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                    canvas.SetFontAndSize(bf, 10);
                                    canvas.SetColorFill(BaseColor.BLACK);

                                    AgregarTexto(canvas, 126, 678, licencia.NombreCompletoUsuario);
                                    AgregarTexto(canvas, 126, 663, licencia.NombrePuesto);
                                    AgregarTexto(canvas, 361, 663, licencia.NombreDepartamento);
                                    AgregarTexto(canvas, 126, 648, licencia.NombreCompletoJefe);
                                    AgregarTexto(canvas, 324, 693, licencia.FechaSolicitud.ToString("dd-MM-yyyy"));
                                    ColumnText ct = new ColumnText(canvas);
                                    ct.SetSimpleColumn(
                                        new Phrase(licencia.Comentarios ?? "", new Font(Font.FontFamily.HELVETICA, 10)),
                                        53,    //Izquierda
                                        315,   //Abajo
                                        420,    //Derecha
                                        377,    //Arriba
                                        14,     //espaciado
                                        Element.ALIGN_LEFT //Alineación del texto
                                    );
                                    ct.Go();
                                    ColumnText ect = new ColumnText(canvas);
                                    ct.SetSimpleColumn(
                                        new Phrase(licencia.Estatus ?? "", new Font(Font.FontFamily.HELVETICA, 10)),
                                        431,    //Izquierda
                                        280,   //Abajo
                                        540,    //Derecha
                                        327,    //Arriba
                                        14,     //espaciado
                                        Element.ALIGN_CENTER //Alineación del texto
                                    );
                                    ct.Go();
                                    AgregarTexto(canvas, 470, 356, licencia.DiasSolicitados.ToString() + " días");

                                    switch (licencia.IdTipoLicencia)
                                    {
                                        case 2:
                                            AgregarTexto(canvas, 290, 515, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 515, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 515, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 515, ("X"));

                                            break;
                                        case 1:
                                            AgregarTexto(canvas, 290, 423, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 423, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 423, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 423, ("X"));
                                            break;
                                        case 3:
                                            AgregarTexto(canvas, 290, 408, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 408, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 408, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 408, ("X"));
                                            break;
                                        case 4:
                                            AgregarTexto(canvas, 290, 573, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 573, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 573, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 573, ("X"));
                                            break;
                                        case 5:
                                            AgregarTexto(canvas, 290, 393, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 393, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 393, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 393, ("X"));
                                            break;
                                        case 6:
                                            AgregarTexto(canvas, 290, 530, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 530, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 530, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 530, ("X"));
                                            break;
                                        case 7:
                                            AgregarTexto(canvas, 290, 500, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 500, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 500, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 500, ("X"));
                                            break;
                                        case 8:
                                            AgregarTexto(canvas, 290, 439, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 439, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 439, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 439, ("X"));
                                            break;
                                        case 9:
                                            AgregarTexto(canvas, 290, 470, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 470, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 470, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 470, ("X"));
                                            break;
                                        case 10:
                                            AgregarTexto(canvas, 290, 573, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 573, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 573, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 573, ("X"));
                                            break;
                                        case 11:
                                            AgregarTexto(canvas, 290, 485, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 485, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 485, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 485, ("X"));
                                            break;
                                        case 12:
                                            AgregarTexto(canvas, 290, 454, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 454, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 454, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 454, ("X"));
                                            break;
                                        case 13:
                                            AgregarTexto(canvas, 290, 544, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 544, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 544, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 545, ("X"));
                                            break;
                                        case 14:
                                            AgregarTexto(canvas, 290, 573, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 573, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 573, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 575, ("X"));
                                            break;
                                        case 15:
                                            AgregarTexto(canvas, 290, 560, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 373, 560, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 560, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 560, ("X"));
                                            break;
                                        default:
                                            // Coordenadas por defecto
                                            AgregarTexto(canvas, 256, 573, licencia.FechaInicio.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 399, 573, licencia.FechaFin.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 455, 573, licencia.Reincorporacion.ToString("dd-MM-yyyy"));
                                            AgregarTexto(canvas, 248, 575, ("X"));
                                            break;
                                    }
                                    AgregarTexto(canvas, 461, 260, TotalSolicitudesLicencias.ToString() + " permisos" ?? "");
                                    // Guardar y cerrar el PDF
                                    pdfStamper.Close();
                                    pdfReader.Close();

                                    // Devolver el PDF generado
                                    Response.Headers["Content-Disposition"] = "inline; filename=SolicitudDeLicencia.pdf";
                                    return File(ms.ToArray(), "application/pdf");
                                }

                            }
                            else
                            {
                                return BadRequest("No se encontraron datos para la licencia.");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar el PDF: " + ex.Message);
            }
        }

        private LicenciaDetalle construirLicenciaDetalle(SqlDataReader licencia)
        {
            return new LicenciaDetalle()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                IdJefe = int.Parse(licencia["IdJefe"].ToString()),
                IdTipoLicencia = int.Parse(licencia["IdTipoLicencia"].ToString()),
                TipoPermiso = licencia["TipoPermiso"].ToString(),
                Comentarios = licencia["Comentarios"].ToString(),
                IdEstatus = int.Parse(licencia["IdEstatus"].ToString()),
                Estatus = licencia["Estatus"].ToString(),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"].ToString()),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),
                NombreCompletoJefe = licencia["NombreCompletoJefe"].ToString(),
                NombreCompletoUsuario = licencia["NombreCompletoUsuario"].ToString(),
                DiasSolicitados = int.Parse(licencia["DiasSolicitados"].ToString()),
                NombrePuesto = licencia["NombrePuesto"].ToString(),
                NombreDepartamento = licencia["NombreDepartamento"].ToString(),
                TotalDiasUsados = int.Parse(licencia["TotalDiasUsados"].ToString()),

            };
        }
        private void GetTotalSolicitudesDeLicencias(int idUsuario)
        {
            using (SqlConnection sqlConexion = new SqlConnection(conexion))
            {
                sqlConexion.Open();
                using (SqlCommand cmd = new SqlCommand("GetTotalSolicitudesLicencias", sqlConexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));

                    object resultado = cmd.ExecuteScalar();
                    if (resultado != null)
                    {
                        TotalSolicitudesLicencias = Convert.ToInt32(resultado);
                    }
                }
            }
        }
    }
}