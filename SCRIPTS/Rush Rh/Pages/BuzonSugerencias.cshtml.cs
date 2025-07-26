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
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Rush_Rh.Pages
{
    public class BuzonSugerencias : PageModel
    {
        private readonly ILogger<BuzonSugerencias> _logger;
        string conexion = "";

        int puestoUsuario = 0;
        int usuario = 0;
        public bool admin = false;
        public List<UsuarioNombre> usuarios { get; set; } = new List<UsuarioNombre>();
        public List<TipoSugerencia> tiposSugerencias { get; set; } = new List<TipoSugerencia>();
        public List<Sugerencia> sugerencias { get; set; } = new List<Sugerencia>();
        [BindProperty]
        public Sugerencia agregarSugerencia { get; set; } = new Sugerencia();

        // Varibles para filtrar
        [BindProperty]
        public int TipoSugerenciaId { get; set; } = 0;
        [BindProperty]
        public DateTime? FechaRegistroDesde { get; set; }
        [BindProperty]
        public DateTime? FechaRegistroHasta { get; set; }
        [BindProperty]
        public int Anonimo { get; set; } = 0;
        [BindProperty]
        public int IdUsuario { get; set; } = 0;
        public BuzonSugerencias(ILogger<BuzonSugerencias> logger, IConfiguration configuration)
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
            puestoUsuario = HttpContext.Session.GetInt32("idPuesto") ?? 0;
            if(puestoUsuario == 3 || puestoUsuario == 5){
                admin = true;
            }
            if(admin){
                GetNombresUsuarios();
                GetSugerenciasAll();
            }
            else{
                GetSugerencias(usuario);
            }
            GetTiposSugerencias();
            return Page();
        }

        public void GetNombresUsuarios()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAllNombre", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            usuarios.Add(new UsuarioNombre
                            {
                                Id = int.Parse(dtr["Id"].ToString()),
                                Nombre = dtr.GetString("Nombre")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetTiposSugerencias(){
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTiposSugerenciasCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposSugerencias.Add(construirTiposSugerencias(dtr));
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

        public TipoSugerencia construirTiposSugerencias(DataTableReader dtr){
            TipoSugerencia tipoSugerencia = new()
            {
                Id = int.Parse(dtr["id"].ToString()),
                Nombre = dtr.GetString("nombre")
            };
            return tipoSugerencia;
        }

        public void GetSugerenciasFiltered(int usuarioIniciado){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuarioIniciado));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdTipoSugerencia", TipoSugerenciaId));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@FechaRegistroDesde", FechaRegistroDesde == null ? (object)DBNull.Value : FechaRegistroDesde));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@FechaRegistroHasta", FechaRegistroHasta == null ? (object)DBNull.Value : FechaRegistroHasta));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Anonimo", Anonimo));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.SugerenciasGetFilteredCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            sugerencias.Add(construirSugerencia(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetSugerencias(int usuarioIniciado){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuarioIniciado));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.SugerenciasGetCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            sugerencias.Add(construirSugerencia(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al cargar las sugerencias.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    _logger.LogError(ex, "Error al cargar sugerencias");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetSugerenciasAllFiltered(){
            using(SqlServer sqlConexion = new SqlServer()){
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdTipoSugerencia", TipoSugerenciaId));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@FechaRegistroDesde", FechaRegistroDesde == null ? (object)DBNull.Value : FechaRegistroDesde)); 
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@FechaRegistroHasta", FechaRegistroHasta == null ? (object)DBNull.Value : FechaRegistroHasta));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Anonimo", Anonimo));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", IdUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.SugerenciasGetFilteredCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            sugerencias.Add(construirSugerenciaAll(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al cargar las sugerencias.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    _logger.LogError(ex, "Error al cargar sugerencias");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetSugerenciasAll(){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.SugerenciasGetCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            sugerencias.Add(construirSugerenciaAll(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al cargar las sugerencias.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    _logger.LogError(ex, "Error al cargar sugerencias");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public Sugerencia construirSugerencia(DataTableReader dtr)
        {
            Sugerencia sugerencia = new()
            {
                Id = int.Parse(dtr["idSugerencia"].ToString()),
                Contenido = dtr.GetString("contenido"),
                TipoSugerencia = dtr.GetString("nombreSugerencia"),
                IdUsuario = int.Parse(dtr["idUsuario"].ToString()),
                FechaRegistro = dtr.GetDateTime("fechaRegistro"),
                Anonima = dtr.GetBoolean("anonimo"),
                Respuesta = dtr.GetString("respuesta")
            };
            return sugerencia;
        }

        public Sugerencia construirSugerenciaAll(DataTableReader dtr)
        {
            Sugerencia sugerencia = new()
            {
                Id = int.Parse(dtr["idSugerencia"].ToString()),
                Contenido = dtr.GetString("contenido"),
                TipoSugerencia = dtr.GetString("nombreSugerencia"),
                IdUsuario = int.Parse(dtr["idUsuario"].ToString()),
                Usuario = dtr.GetString("Nombre"),
                FechaRespuesta = dtr.GetDateTime("fechaUltimaRespuesta"),
                FechaRegistro = dtr.GetDateTime("fechaRegistro"),
                Anonima = dtr.GetBoolean("anonimo"),
                Respuesta = dtr.GetString("respuesta")
            };
            return sugerencia;
        }

        public IActionResult OnPostEliminarSugerencia(){
            int idSugerencia = int.Parse(Request.Form["idSugerencia"]);
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdSugerencia", idSugerencia));
                    sqlConexion.EjecutarStoreProcedure("dbo.SugerenciasEliminar", prmtrs);
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Sugerencia eliminada correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                }
                catch(Exception ex)
                {
                    // Manejo de excepciones
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al eliminar la sugerencia.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    _logger.LogError(ex, "Error al eliminar sugerencia");
                    return RedirectToPage();
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPostAgregarSugerencia()
        {
            agregarSugerencia.IdUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Contenido", agregarSugerencia.Contenido));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdTipoSugerencia", agregarSugerencia.TipoSugerencia));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", agregarSugerencia.IdUsuario));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Anonimo", agregarSugerencia.Anonima));

                    sqlConexion.EjecutarStoreProcedure("dbo.SugerenciasCrear", prmtrs);
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Sugerencia agregada correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    // Puedes registrar el error o mostrar un mensaje al usuario
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al agregar la sugerencia.");
                    _logger.LogError(ex, "Error al agregar sugerencia");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al agregar la sugerencia.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage();
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage("/BuzonSugerencias");
        }

        public IActionResult OnPostResponderSugerencia(){
            int idSugerencia = int.Parse(Request.Form["idSugerencia"]);
            string respuesta = Request.Form["respuesta"];
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdSugerencia", idSugerencia));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Respuesta", respuesta));
                    sqlConexion.EjecutarStoreProcedure("dbo.SugerenciasResponder", prmtrs);
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Sugerencia respondida correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al responder la sugerencia.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    _logger.LogError(ex, "Error al responder sugerencia");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            // Redirigir a la misma página para mostrar el mensaje
            return RedirectToPage();
        }

        public IActionResult OnPostFiltrarSugerencias(){
            
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if(usuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            puestoUsuario = HttpContext.Session.GetInt32("idPuesto") ?? 0;
            if(puestoUsuario == 3 || puestoUsuario == 5){
                admin = true;
            }
            if(admin){
                GetNombresUsuarios();
                GetSugerenciasAllFiltered();
            }
            else{
                GetSugerenciasFiltered(usuario);
            }
            GetTiposSugerencias();
            return Page();
        }

    }
}