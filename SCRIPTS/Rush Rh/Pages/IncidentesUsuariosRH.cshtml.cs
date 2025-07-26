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
using static Rush_Rh.Models.Incidentes;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Rush_Rh.Pages
{
    public class IncidentesUsuariosRH : PageModel
    {
        private readonly ILogger<IncidentesUsuariosRH> _logger;
        public string conexion = "";
        public int usuario = 5;
        public List<Incidencia> incidentes = new List<Incidencia>();
        public List<TiposIncidencias> tiposIncidencias = new List<TiposIncidencias>();
        public List<UsuarioGeneral> usuarios = new List<UsuarioGeneral>();
        public List<EstatusIncidencias> estatusIncidencias = new List<EstatusIncidencias>();

        // Variables para el filtrado
        [BindProperty]
        public int filtroUsuario { get; set; }
        [BindProperty]
        public int filtroEstado { get; set; }
        [BindProperty]
        public int filtroTipoIncidente { get; set; }
        [BindProperty]
        public DateTime? fechaInicio { get; set; }
        [BindProperty]
        public DateTime? fechaTermino { get; set; }
        [BindProperty]
        public int aceptada { get; set; } = 0;
        [BindProperty]
        public int rechazada { get; set; }
        [BindProperty]
        public string comentarios { get; set; }
        
        



        public IncidentesUsuariosRH(ILogger<IncidentesUsuariosRH> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
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
            GetUsuarios();
            GetIncidentes();
            GetEstatusIncidencias();
            GetTiposIncidencias(); 
            return Page();
        }

        private void GetUsuarios(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAllNombre", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            usuarios.Add(construirUsuario(dtr));
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private UsuarioGeneral construirUsuario(DataTableReader usuario){
            return new UsuarioGeneral()
            {
                Id = int.Parse(usuario["Id"].ToString()),
                Nombre = usuario["Nombre"].ToString(),
            };
        }

        public void GetIncidentesFiltrados(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", filtroUsuario));
                    prmtrs.Add(new SqlParameter("@Estatus", filtroEstado));
                    prmtrs.Add(new SqlParameter("@TipoIncidente", filtroTipoIncidente));
                    prmtrs.Add(new SqlParameter("@FechaInicio", fechaInicio));
                    prmtrs.Add(new SqlParameter("@FechaTermino", fechaTermino));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosGetFilteredCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            incidentes.Add(construirIncidencias(dtr));
                        }
                    }
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Error al obtener los incidentes.");
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetIncidentes(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosGetCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            incidentes.Add(construirIncidencias(dtr));
                        }
                    }
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Error al obtener los incidentes.");
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private Incidencia construirIncidencias(DataTableReader incidencia){
            return new Incidencia ()
            {
                Id = int.Parse(incidencia["idIncidencia"].ToString()),
                IdTipoIncidencia = int.Parse(incidencia["idTipoIncidencia"].ToString()),
                FechaRegistro = DateTime.Parse(incidencia["fechaRegistro"].ToString()),
                FechaIncidencia = DateTime.Parse(incidencia["fechaIncidencia"].ToString()),
                HoraSalida = incidencia["horaSalida"].ToString(),
                HoraEntrada = incidencia["horaEntrada"].ToString(),
                Descripcion = incidencia["descripcion"].ToString(),
                IdUsuario = int.Parse(incidencia["idUsuario"].ToString()),
                IdDocumento = incidencia.GetInt("idDocumento"),
                URLDocumento = incidencia["URL"].ToString(),
                IdEstatus = int.Parse(incidencia["idEstatusIncidencia"].ToString()),
                Estatus = incidencia["estatus"].ToString(),
                NombreUsuario = incidencia["nombreUsuario"]?.ToString(),
                ApellidoMaternoUsuario = incidencia["apellidoMaternoUsuario"]?.ToString(),
                ApellidoPaternoUsuario = incidencia["apellidoPaternoUsuario"]?.ToString(),
                Comentarios = incidencia["comentarios"].ToString()
            };
        }

        public void GetTiposIncidencias()
        {
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTiposIncidenciasCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposIncidencias.Add(construirTiposIncidencias(dtr));
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private TiposIncidencias construirTiposIncidencias(DataTableReader tipoIncidencia){
            return new TiposIncidencias ()
            {
                Id = int.Parse(tipoIncidencia["idTipo"].ToString()),
                Nombre = tipoIncidencia["nombreTipo"].ToString()
            };
        }

        public void GetEstatusIncidencias()
        {
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetEstatusIncidenciasCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            estatusIncidencias.Add(construirEstatusIncidencias(dtr));
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private EstatusIncidencias construirEstatusIncidencias(DataTableReader estatusIncidencia){
            return new EstatusIncidencias ()
            {
                Id = int.Parse(estatusIncidencia["idEstatus"].ToString()),
                Nombre = estatusIncidencia["nombreEstatus"].ToString()
            };
        }

        public IActionResult OnPostFiltrarIncidentes(){
            GetUsuarios();
            GetIncidentesFiltrados();
            GetEstatusIncidencias();
            GetTiposIncidencias();
            return Page();
        }

        public ActionResult OnPostAceptarRechazarIncidencia(){
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            
            if (aceptada > 0){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdIncidencia", aceptada));
                        prmtrs.Add(new SqlParameter("@IdUsuario", usuario)); //Cambiará cuando se implemente la autenticación
                        prmtrs.Add(new SqlParameter("@Comentarios", comentarios));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosMandarRevision", prmtrs))
                        {
                            //while (dtr.Read())
                            //{
                                
                            //}
                        }
                        TempData["Message"] = "¡Incidencia aceptada correctamente!";
                        TempData["MessageType"] = "success";
                    }
                    catch(Exception ex){
                        TempData["Message"] = "¡Incidencia no aceptada!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
            }
            else if (rechazada > 0){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdIncidencia", rechazada));
                        prmtrs.Add(new SqlParameter("@IdUsuario", usuario));
                        prmtrs.Add(new SqlParameter("@Comentarios", comentarios));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosRechazar", prmtrs))
                        {
                            //while (dtr.Read())
                            //{
                                
                            //}
                        }
                        TempData["Message"] = "¡Incidencia rechazada correctamente!";
                        TempData["MessageType"] = "success";
                    }
                    catch(Exception ex){
                        TempData["Message"] = "¡Incidencia no rechazada!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
            }
            else if (aceptada == 0 && rechazada == 0)
            {
                TempData["Message"] = "¡No se seleccionó ninguna acción!";
                TempData["MessageType"] = "error";
                return RedirectToPage();
            }
            return RedirectToPage();
        }

    }
}