using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models; //Agregamos el espacio de nombres de los modelos
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Rush_Rh.Models.Formularios;
using static Rush_Rh.Models.Usuarios;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text.Json;

namespace Rush_Rh.Pages
{
    public class MisAvisos : PageModel
    {
        private readonly ILogger<MisAvisos> _logger;
        string conexion = "";
        public int usuario = 0;
        public List<AvisoUsuario> avisos = new List<AvisoUsuario>();
        public MisAvisos(ILogger<MisAvisos> logger, IConfiguration configuration)
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
            GetAvisos(usuario);
            return Page();
        }

        public void GetAvisos(int usuario)
        {
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosGetCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            avisos.Add(construirAviso(dtr));
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

        public AvisoUsuario construirAviso(DataTableReader dtr){
            AvisoUsuario aviso = new()
            {
                Id = Convert.ToInt32(dtr["id"]),
                Titulo = dtr["titulo"].ToString(),
                Contenido = dtr["contenido"].ToString(),
                FechaEvento = Convert.ToDateTime(dtr["fechaEvento"]),
                MedioEnvio = dtr["medioEnvio"].ToString(),
                URL = dtr["URL"].ToString()
            };
            return aviso;
        }

    }
}