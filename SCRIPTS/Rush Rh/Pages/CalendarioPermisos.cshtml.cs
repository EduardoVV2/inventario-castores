using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models;
using RushtecRH.Clases;

namespace Rush_Rh.Pages
{
    public class CalendarioPermisos : PageModel
    {
        private readonly ILogger<CalendarioPermisos> _logger;

        [BindProperty]
        public LicenciaDetalleRhGerente Licencia { get; set; }

        [BindProperty]
        public VacacionDetalleRhGerente Vacacion { get; set; }

        public List<PermisosCalendario> permisos = new List<PermisosCalendario>();

        public int year { get; set; }
        public int month { get; set; }
        public string tipoPermiso { get; set; }
        string conexion = ""; 

        public CalendarioPermisos(ILogger<CalendarioPermisos> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public IActionResult OnGet(string? tipo, int mes, int año)
        {   
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            if(puestoUsuario != 3 && puestoUsuario != 5){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes los permisos para acceder a esta página.";
                TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Home");
            }

            tipo ??= "licencias";
            if (mes == 0) mes = DateTime.Now.Month;  
            if (año == 0) año = DateTime.Now.Year;

            if(tipo == "licencias")
            {
                GetCalendarioLicencias(año, mes);
            }
            else if(tipo == "vacaciones")
            {
                GetCalendarioVacaciones(año, mes);
            }
            else if(tipo == "permisos")
            {
                GetCalendarioPermisos(año, mes);
            }
            return Page();
        }

        public void GetCalendarioLicencias(int año, int mes)
        {
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetCalendario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            permisos.Add(contruirPermisos(dtr)); 
                        }
                    }

                    month = mes;
                    year = año;
                    tipoPermiso = "Licencias";
                }
                catch(Exception ex){

                }
                finally{

                }
            }
        }

        public void GetCalendarioVacaciones(int año, int mes)
        {
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetCalendario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            permisos.Add(contruirPermisos(dtr)); 
                        }
                    }

                    month = mes;
                    year = año;
                    tipoPermiso = "Vacaciones";
                }
                catch(Exception ex){

                }
                finally{

                }
            }
        }

        //aqui busco las licencias y las vacaciones
        public void GetCalendarioPermisos(int año, int mes)
        {
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PermisosGetCalendarioLicenciasVacaciones", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            permisos.Add(contruirPermisos(dtr)); 
                        }
                    }

                    month = mes;
                    year = año;
                    tipoPermiso = "Permisos";
                }
                catch(Exception ex){

                }
                finally{

                }
            }
        }

        private PermisosCalendario contruirPermisos (DataTableReader permiso){
            return new PermisosCalendario()
            {
                Id = int.Parse(permiso["Id"].ToString()),
                FechaInicio = Convert.ToDateTime(permiso["FechaInicio"]),
                FechaFin = Convert.ToDateTime(permiso["FechaFin"]),
                Nick = permiso["Nick"].ToString(),
                TipoSolicitud = permiso["TipoSolicitud"].ToString(),
            };
        }

        public IActionResult OnGetDetalleLicencia(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetByIdParaConfirmarRHyGerente", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            Licencia = construirLicenciaDetalle(dtr);
                        }

                        return new JsonResult(new
                        {
                            usuario = Licencia.Usuario,
                            jefe = Licencia.NombreCompletoJefe,
                            tipoLicencia = Licencia.TipoPermiso, 
                            fechaSolicitud = Licencia.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = Licencia.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = Licencia.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = Licencia.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = Licencia.Comentarios,
                        });
                    }
                }
                catch(Exception ex){
                    _logger.LogError(ex, "Hubo un error al buscar la licencia");
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private LicenciaDetalleRhGerente construirLicenciaDetalle (DataTableReader licencia){
            return new LicenciaDetalleRhGerente()
            {
                Usuario = licencia["Usuario"].ToString(),
                NombreCompletoJefe = licencia["Jefe"].ToString(),
                TipoPermiso = licencia["TipoLicencia"].ToString(),
                Comentarios = licencia["Comentarios"].ToString(),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"].ToString()),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),
            };
        }

        public IActionResult OnGetDetalleVacacion( int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetByIdConfirmarRHyGerente", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            Vacacion = construirVacacionDetalle(dtr);
                        }

                        return new JsonResult(new
                        {
                            usuario = Vacacion.Usuario,
                            jefe = Vacacion.Jefe,
                            fechaSolicitud = Vacacion.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = Vacacion.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = Vacacion.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = Vacacion.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = Vacacion.Comentarios,
                            diasUsados = Vacacion.DiasUsados,
                        });
                    }
                }
                catch(Exception ex){
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private VacacionDetalleRhGerente construirVacacionDetalle (DataTableReader vacacion){
            return new VacacionDetalleRhGerente()
            {
                Usuario = vacacion["Usuario"].ToString(),
                Jefe = vacacion["Jefe"].ToString(),
                Comentarios = vacacion["Comentarios"].ToString(),
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
            };
        }
    }
}