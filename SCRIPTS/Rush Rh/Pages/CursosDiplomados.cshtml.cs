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
using Microsoft.AspNetCore.Mvc.Rendering;
using static Rush_Rh.Models.Formularios;
using static Rush_Rh.Models.Usuarios;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Rush_Rh.Pages
{
    public class CursosDiplomados : PageModel
    {
        private readonly ILogger<CursosDiplomados> _logger;
        string conexion = "";
        public int usuario = 0;
        public List<ActividadesPlaneacion> cursosDiplomados = new List<ActividadesPlaneacion>();
        //Variables necesarias para el filtro de actividades
        [BindProperty]
        public DateTime fechaInicio { get; set; }
        [BindProperty]
        public DateTime fechaFin { get; set; }
        [BindProperty]
        public int DC3 { get; set; }
        public CursosDiplomados(ILogger<CursosDiplomados> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if(usuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            GetCursosDiplomados(usuario);
            return Page();
        }

        public void GetCursosDiplomados(int usuario)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ActividadesPlaneacionGetCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            cursosDiplomados.Add(construirCursoDiplomado(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los cursos y diplomados del usuario.");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }
        public ActividadesPlaneacion construirCursoDiplomado(DataTableReader dtr)
        {
            ActividadesPlaneacion cursoDiplomado = new ActividadesPlaneacion();
            cursoDiplomado.Id = Convert.ToInt64(dtr["idActividad"]);
            cursoDiplomado.NombreActividad = dtr["nombreActividad"].ToString();
            cursoDiplomado.FechaHoraInicio = Convert.ToDateTime(dtr["fechaHoraInicio"]);
            cursoDiplomado.FechaHoraFin = Convert.ToDateTime(dtr["fechaHoraFin"]);
            cursoDiplomado.DC3 = Convert.ToBoolean(dtr["DC3"]);
            cursoDiplomado.IdPlaneacion = Convert.ToInt64(dtr["idPlaneacion"]);
            cursoDiplomado.Activo = Convert.ToBoolean(1);                                                                                                                                                                        
            return cursoDiplomado;
        }
        public IActionResult OnPostBuscarActividades()
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", usuario));
                    prmtrs.Add(new SqlParameter("@FechaInicio", fechaInicio));
                    prmtrs.Add(new SqlParameter("@FechaFin", fechaFin));
                    prmtrs.Add(new SqlParameter("@DC3", DC3));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ActividadesPlaneacionGetFilteredCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            cursosDiplomados.Add(construirCursoDiplomado(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los cursos y diplomados del usuario.");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return Page();
        }
    }
}