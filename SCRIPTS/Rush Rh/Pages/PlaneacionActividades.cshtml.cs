using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static Rush_Rh.Models.Formularios;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace Rush_Rh.Pages
{
    public class PlaneacionActividades : PageModel
    {
        private readonly ILogger<PlaneacionActividades> _logger;
        string conexion = "";

        [BindProperty]
        public PlaneacionAnual planeacionAnual { get; set; }

        [BindProperty]
        public ActividadesPlaneacion actividades { get; set; }

        List<PlaneacionAnual> planeacionesRecuperadas = new List<PlaneacionAnual>();
        List<ActividadesPlaneacion> actividadesRecuperadas = new List<ActividadesPlaneacion>();


        public PlaneacionActividades(ILogger<PlaneacionActividades> logger,  IConfiguration configuration)
        {
            _logger = logger;
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
        }

        public IActionResult OnGet()
        {
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            if(puestoUsuario != 3 && puestoUsuario != 5){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes los permisos para acceder a esta página.";
                TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Home");
            }
            return Page();
        }


        public void messageAlert(string MessageTitle, string message, string type)
        {
            TempData["MessageTitle"] = MessageTitle;
            TempData["Message"] = message;
            TempData["MessageType"] = type; // Puede ser 'error', 'info', etc.
        }


        public IActionResult OnPostGuardarPlaneacionAnual()
        {
           using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);


                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Nombre", planeacionAnual.Nombre));
                    prmtrs.Add(new SqlParameter("@FechaHoraInicio", planeacionAnual.FechaHoraInicio));
                    prmtrs.Add(new SqlParameter("@FechaHoraFin", planeacionAnual.FechaHoraFin));
                    prmtrs.Add(new SqlParameter("@Año", planeacionAnual.Año));
                   
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PlaneacionAnualCrear", prmtrs))
                    {
                        if (dtr.Read())
                        {

                        }
                    }


                    messageAlert("¡Éxito!", "¡Planeacion creada exitosamente!", "success");
                    return RedirectToPage("/PlaneacionActividades", new { anio = planeacionAnual.Año });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar la planeacion");
                    messageAlert("¡Error!", "¡Ocurrió un error al guardar la planeacion!", "error");
                    return RedirectToPage("/PlaneacionActividades");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostEliminarPlaneacionAnual()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Id", planeacionAnual.Id));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PlaneacionAnualEliminar", prmtrs))
                    {
                        if (dtr.Read())
                        {

                        }
                    }

                    messageAlert("¡Éxito!", "¡Subplaneacion eliminada exitosamente!", "success");
                    return RedirectToPage("/PlaneacionActividades", new { anio = planeacionAnual.Año });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al eliminar la Subplaneacion");
                    messageAlert("¡Error!", "¡Ocurrió un error al eliminar la Subplaneacion!", "error");
                    return RedirectToPage("/PlaneacionActividades");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostEditarPlaneacionAnual(){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Id", planeacionAnual.Id));
                    prmtrs.Add(new SqlParameter("@Nombre", planeacionAnual.Nombre));
                    prmtrs.Add(new SqlParameter("@FechaHoraInicio", planeacionAnual.FechaHoraInicio));
                    prmtrs.Add(new SqlParameter("@FechaHoraFin", planeacionAnual.FechaHoraFin));
                    prmtrs.Add(new SqlParameter("@Año", planeacionAnual.Año));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PlaneacionAnualEditar", prmtrs))
                    {
                        if (dtr.Read())
                        {

                        }
                    }

                    messageAlert("¡Éxito!", "¡Subplaneacion editada exitosamente!", "success");
                    return RedirectToPage("/PlaneacionActividades", new { anio = planeacionAnual.Año, idPlaneacion = planeacionAnual.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al editar la Subplaneacion");
                    messageAlert("¡Error!", "¡Ocurrió un error al editar la Subplaneacion!", "error");
                    return RedirectToPage("/PlaneacionActividades");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostGuardarActividad()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    int? año = 0;

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@NombreActividad", actividades.NombreActividad));
                    prmtrs.Add(new SqlParameter("@FechaHoraInicio", actividades.FechaHoraInicio));
                    prmtrs.Add(new SqlParameter("@FechaHoraFin", actividades.FechaHoraFin));
                    prmtrs.Add(new SqlParameter("@DC3", actividades.DC3));
                    prmtrs.Add(new SqlParameter("@IdPlaneacion", actividades.IdPlaneacion));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ActividadesPlaneacionCrear", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            año = dtr.GetInt32(dtr.GetOrdinal("Año"));
                        }
                    }


                    messageAlert("¡Éxito!", "Actividad creada exitosamente!", "success");
                    return RedirectToPage("/PlaneacionActividades", new { anio = año , idPlaneacion = actividades.IdPlaneacion });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar la actividad");
                    messageAlert("¡Error!", "¡Ocurrió un error al guardar la actividad!", "error");
                    return RedirectToPage("/PlaneacionActividades");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



        //Función para mandar por ajax la planeacion anual y sus actividades asociadas
        public JsonResult OnGetPlaneacionAnual(int anio)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<SqlParameter> prmtrs = new List<SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Año", anio));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetPlaneacionAnualCollectionWithAño", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            PlaneacionAnual planeacion = new PlaneacionAnual
                            {
                                Id = dtr.GetInt64(dtr.GetOrdinal("Id")), // Cambiado a GetInt64
                                Nombre = dtr.GetString(dtr.GetOrdinal("Nombre")),
                                FechaHoraInicio = dtr.GetDateTime(dtr.GetOrdinal("FechaHoraInicio")),
                                FechaHoraFin = dtr.GetDateTime(dtr.GetOrdinal("FechaHoraFin")),
                                Año = dtr.GetInt32(dtr.GetOrdinal("Año"))
                            };
                            planeacionesRecuperadas.Add(planeacion);
                        }
                    }

                    List<SqlParameter> prmtrs1 = new List<SqlParameter>();
                    prmtrs1.Add(new SqlParameter("@Año", anio));
                    //Retornar todas las actividades asociadas a la planeacion anual
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetActividadesPlaneacionCollectionWithAño", prmtrs1))
                    {
                        while (dtr.Read())
                        {
                            ActividadesPlaneacion actividad = new ActividadesPlaneacion
                            {
                                Id = dtr.GetInt64(dtr.GetOrdinal("Id")),
                                NombreActividad = dtr.GetString(dtr.GetOrdinal("NombreActividad")),
                                FechaHoraInicio = dtr.GetDateTime(dtr.GetOrdinal("FechaHoraInicio")),
                                FechaHoraFin = dtr.GetDateTime(dtr.GetOrdinal("FechaHoraFin")),
                                DC3 = dtr.GetBoolean(dtr.GetOrdinal("DC3")),
                                IdPlaneacion = dtr.GetInt64(dtr.GetOrdinal("IdPlaneacion")) // Cambiado a GetInt64
                            };
                            actividadesRecuperadas.Add(actividad);
                        }
                    }

                    return new JsonResult(new
                    {
                        success = true,
                        planeaciones = planeacionesRecuperadas,
                        actividades = actividadesRecuperadas
                    });

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener la planeacion anual");
                    return new JsonResult(new { success = false, message = "Error al obtener la planeacion anual" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

    }
}