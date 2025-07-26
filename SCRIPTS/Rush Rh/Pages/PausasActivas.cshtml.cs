using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using static Rush_Rh.Models.Pausas;
using static Rush_Rh.Models.Usuarios;


namespace Rush_Rh.Pages
{
    public class PausasActivas : PageModel
    {
        [BindProperty]
        public Pausas info { get; set; }

        string conexion = "";
        public int usuario { get; set; } 
        public int idPuesto { get; set; }
        public List<UsuariosFotos> usuarios = new List<UsuariosFotos>(); 
        public List<Pausas> pausasActivas = new List<Pausas>();
        private readonly ILogger<PausasActivas> _logger;

        public PausasActivas(ILogger<PausasActivas> logger, IConfiguration configuration)
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

            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

            GetUsuarios();
            FechasCalendario fecha = new FechasCalendario
            {
                Mes = DateTime.Now.Month - 1,  // Mes actual
                Año = DateTime.Now.Year    // Año actual
            };
            OnPostPausasActivas(fecha);
            return Page();
        }

        private void GetUsuarios(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosFotoGet", prmtrs))
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

        private UsuariosFotos construirUsuario(DataTableReader usuario){
            return new UsuariosFotos()
            {
                Id = int.Parse(usuario["Id"].ToString()),
                Nombre = usuario["Nombre"].ToString(),
                RutaFoto = usuario["RutaFoto"].ToString(),
                Nick = usuario["Nick"].ToString()
            };
        }

        public IActionResult OnPostPausasActivas( [FromBody] FechasCalendario fecha)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {   
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Mes", fecha.Mes + 1));
                    prmtrs.Add(new SqlParameter("@Año", fecha.Año));
                    
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PausasActivasGetByMonth", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            pausasActivas.Add(construirPausaActiva(dtr));
                        }
                    } 
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { Error = ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }

                // Devuelve las pausas activas como JSON
                return new JsonResult(new { success = true, pausasActivas })
                {
                    ContentType = "application/json"
                };
            }
            
            //return RedirectToPage();
            
        }

        private Pausas construirPausaActiva(DataTableReader pausa){
            return new Pausas(){
                Id = int.Parse(pausa["Id"].ToString()),
                Nick = pausa["Nick"].ToString(),
                Fecha = Convert.ToDateTime(pausa["Fecha"]),
                IdUsuario = int.Parse(pausa["IdUsuario"].ToString()),
                RutaFoto = pausa["RutaFoto"].ToString(),
                Hora = pausa["Hora"] is TimeSpan tiempo
                    ? DateTime.Today.Add(tiempo)  // Añadir el TimeSpan al DateTime
                    : DateTime.MinValue  // Si no es válido, asigna el valor por defecto (o maneja el error)
                
            };
        }

        public IActionResult OnPostGuardarPausaActiva(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdPausaActiva", info.Id));
                    prmtrs.Add(new SqlParameter("@Fecha", info.Fecha));
                    prmtrs.Add(new SqlParameter("@Hora", info.Hora));
                    prmtrs.Add(new SqlParameter("@IdUsuario", info.IdUsuario));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PausasActivasCrear", prmtrs)){
                      
                    }
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["AlertMessage"] = "Registro guardado exitosamente";
                    TempData["AlertType"] = "success";
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Error al guardar registro");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["AlertMessage"] = "Ocurrió un error al guardar el registro";
                    TempData["AlertType"] = "error";
                }
                finally{
                    sqlConexion.Desconectar();
                }

                // Redirigir a la misma página
                return RedirectToPage();
            }
            
        }
    }
}