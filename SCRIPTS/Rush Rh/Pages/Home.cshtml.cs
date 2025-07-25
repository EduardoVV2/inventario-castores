using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Rush_Rh.Models.Formularios;
using static Rush_Rh.Models.Usuarios;

namespace Rush_Rh.Pages
{
    public class Home : PageModel
    {
        private readonly ILogger<Home> _logger;
        public string conexion = "";
        public int usuario = 0;                                 //Usuario de prueba
        public Documento doc { get; set; } = new Documento();
        public List<Recordatorios> recordatorios = new List<Recordatorios>();
        public List<ImagenCarruselAviso> imagenesCarrusel = new List<ImagenCarruselAviso>();
        public UsuarioSencillo datosUsuario = new UsuarioSencillo();
        public Home(ILogger<Home> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if (usuario == 0)
            {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            GetUsuario(usuario);
            Cumpleaños();
            GetAvisos(usuario);
            GetUrlImagenesCarrusel();
            return Page();
        }


        public IActionResult Cumpleaños()
        {
            if (datosUsuario.FechaNacimiento.Month == DateTime.Now.Month && datosUsuario.FechaNacimiento.Day == DateTime.Now.Day)
            {
                if (HttpContext.Session.GetInt32("Cumple") == null)
                {
                    HttpContext.Session.SetInt32("Cumple", 1);
                    return RedirectToPage();
                }
            }
            return new EmptyResult();
        }
        public ActionResult OnPostCumpleVisto()
        {
            HttpContext.Session.SetInt32("Cumple", 0);
            return RedirectToPage();
        }
        public void GetUsuario(int usuario)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioSencilloGetWithId", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            datosUsuario = new UsuarioSencillo()
                            {
                                Id = int.Parse(dtr["id"].ToString()),
                                Nombre = dtr["nombre"].ToString(),
                                ApellidoPaterno = dtr["apellidoPaterno"].ToString(),
                                ApellidoMaterno = dtr["apellidoMaterno"].ToString(),
                                IdTipoUsuario = int.Parse(dtr["idTipoUsuario"].ToString()),
                                TipoUsuario = dtr["tipoUsuario"].ToString(),
                                IdEstatusUsuario = int.Parse(dtr["idEstatus"].ToString()),
                                EstatusUsuario = dtr["nombreEstatus"].ToString(),
                                FechaNacimiento = DateTime.Parse(dtr["fechaNacimiento"].ToString()),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetAvisos(int usuario)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {

                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetRecordatoriosTablaAvisos", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            recordatorios.Add(construirRecordatorio(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public Recordatorios construirRecordatorio(DataTableReader dtr)
        {
            Recordatorios recordatorio = new()
            {
                Id = Convert.ToInt32(dtr["id"]),
                Titulo = dtr["titulo"].ToString(),
                FechaEvento = Convert.ToDateTime(dtr["fechaEvento"]),
            };
            return recordatorio;
        }

        public void GetUrlImagenesCarrusel()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ImagenesCarruselGetCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            imagenesCarrusel.Add(new ImagenCarruselAviso()
                            {
                                Id = Convert.ToInt32(dtr["id"]),
                                URL = dtr["URL"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las imágenes del carrusel: " + ex.Message);
                    // Manejar la excepción según sea necesario
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public IActionResult OnPostAgregarImagen()
        {

            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if (usuario == 0)
            {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }

            var usuarioRH = HttpContext.Session.GetInt32("idUsuario") ?? 0;

            string rutaDocumento;

            if (Request.Form.Files.Count != 0)
            {
                // Procesar archivo subido
                var archivo = Request.Form.Files[0];

                try
                {
                    var extensionesValidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" };
                    var extension = Path.GetExtension(archivo.FileName).ToLower();

                    if (!extensionesValidas.Contains(extension))
                    {
                        ModelState.AddModelError("Imagen", "Solo se permiten archivos de imagen.");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "Solo se permiten archivos de imagen.";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                        return RedirectToPage();
                    }
                    else if (archivo.Length > 2 * 1024 * 1024) // 2 MB
                    {
                        ModelState.AddModelError("Imagen", "El tamaño del archivo no debe exceder los 2 MB.");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "El tamaño del archivo no debe exceder los 2 MB.";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                        return RedirectToPage();
                    }
                    else if (archivo.Length == 0)
                    {
                        ModelState.AddModelError("Imagen", "El archivo está vacío.");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "El archivo está vacío.";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                        return RedirectToPage();
                    }
                    else
                    {
                        // Guardar archivo en el servidor 
                        var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                        var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/carrusel");
                        if (!Directory.Exists(rutaCarpeta))
                        {
                            Directory.CreateDirectory(rutaCarpeta);
                        }
                        var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        rutaDocumento = $"/uploads/carrusel/{nombreArchivo}"; // Ruta relativa para la base de datos
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al guardar el archivo: {ex.Message}");
                }

            }
            else
            {
                return BadRequest("No se ha subido ningún archivo.");
            }
            doc.IdUsuario = usuarioRH;// Cambiar por el ID del usuario actual
            doc.IdTipoDocumento = 38;
            doc.URL = rutaDocumento;
            // Guardar la ruta en la base de datos
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    if (Request.Form.Files.Count != 0)
                    {
                        // Insertar el documento y obtener el ID
                        List<System.Data.SqlClient.SqlParameter> parametrosDocumento =
                        [
                            new SqlParameter("@IdTipoDocumento", doc.IdTipoDocumento),
                            new SqlParameter("@DireccionDocumento", rutaDocumento),
                            new SqlParameter("@IdUsuario", doc.IdUsuario),
                        ];

                        int? idDocumento = null;
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoCrear", parametrosDocumento))
                        {
                            if (dtr.Read())
                            {
                                if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                                {
                                    idDocumento = result;
                                }
                            }
                            else
                            {
                                throw new Exception("No se pudo obtener el ID del documento.");
                            }
                        }
                        prmtrs.Add(new SqlParameter("@IdDocumento", idDocumento));

                    }
                    else
                    {
                        prmtrs.Add(new SqlParameter("@IdDocumento", DBNull.Value));
                    }
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ImagenesCarruselAgregar", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            // Aquí puedes manejar la respuesta si es necesario
                        }
                    }
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Imagen guardada correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al guardar el documento: {ex.Message}");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

            return RedirectToPage();
        }
        
        public IActionResult OnPostEliminarImagen(int id)
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if (usuario == 0)
            {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Id", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ImagenesCarruselEliminar", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            // Aquí puedes manejar la respuesta si es necesario
                        }
                    }
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Imagen eliminada correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al eliminar la imagen: {ex.Message}");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

            return RedirectToPage();
        }

    }
}