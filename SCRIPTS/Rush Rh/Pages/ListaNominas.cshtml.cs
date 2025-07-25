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
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Rush_Rh.Pages
{
    public class ListaNominas : PageModel
    {
        private readonly ILogger<ListaNominas> _logger;
        public string conexion = "";
        public int usuario = 5;                                 //Usuario de prueba
        public List<int> anios = new List<int>();
        public List<Meses> meses = new List<Meses>();
        public List<NominasSubidas> nominas = new List<NominasSubidas>();
        public UsuarioSencillo datosUsuario = new UsuarioSencillo();
        [BindProperty]
        public int Year {get; set;} = DateTime.Now.Year;

        public ListaNominas(ILogger<ListaNominas> logger, IConfiguration configuration)
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
            GetAnios();
            GetMeses();
            GetNominas(Year, usuario);
            GetUsuario(usuario);
            // Cumpleaños();
            return Page();
        }

        // public void Cumpleaños(){
        //     if(datosUsuario.FechaNacimiento.Month == DateTime.Now.Month && datosUsuario.FechaNacimiento.Day == DateTime.Now.Day){
        //         if(HttpContext.Session.GetInt32("Cumple") == null){
        //             HttpContext.Session.SetInt32("Cumple", 1);
        //         }
        //     }
        // }
        // public ActionResult OnPostCumpleVisto(){
        //     HttpContext.Session.SetInt32("Cumple", 0);
        //     return RedirectToPage();
        // }
        public void GetUsuario(int usuario){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioSencilloGetWithId", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            datosUsuario = new UsuarioSencillo(){
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
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        public void GetAnios(){
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetAniosTranscurridosCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            anios.Add(int.Parse(dtr["año"].ToString()));
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

        public void GetMeses(){
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetMesesCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            meses.Add(construirMeses(dtr));
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

        private Meses construirMeses(DataTableReader meses){
            return new Meses ()
            {
                Id = int.Parse(meses["Id"].ToString()),
                Nombre = meses["Nombre"].ToString()
            };
        }

        public void GetNominas(int year, int usuario){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@Anio", year));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.NominasGetByUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            nominas.Add(construirNominas(dtr));
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

        private NominasSubidas construirNominas(DataTableReader nominas){
            return new NominasSubidas ()
            {
                Id = int.Parse(nominas["Id"].ToString()),
                IdMes = int.Parse(nominas["IdMes"].ToString()),
                IdDocumento = int.Parse(nominas["IdDocumento"].ToString()),
                URL = nominas["URL"].ToString(),
                Año = int.Parse(nominas["Año"].ToString())
            };
        }

        

        public IActionResult OnPostBuscarNominas(){
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if(usuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            GetAnios();
            GetMeses();
            GetNominas(Year, usuario);
            GetUsuario(usuario);
            return Page();
        }

    }
}