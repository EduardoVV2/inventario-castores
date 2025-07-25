using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;

namespace Rush_Rh.Pages
{
    public class LoginModel : PageModel
    {
        string conexion = "";
        public LoginUsuario info = new LoginUsuario() { Nick = "", Password=""};
        public LoginModel(IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
        }

        public string Nombre { get; set; }  
        public void OnGet()
        {
        }

        public ActionResult OnPostDoLogin(LoginUsuario info)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                var user = new Usuarios();
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Nick", info.Nick));
                    prmtrs.Add(new SqlParameter("@Pass", info.Password));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.Login_get", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            user = construirUsuarioCompleto(dtr);
                        }
                    }
                    if(user.Id == 0)
                    {
                        ViewData["mensaje"] = "Error de usuario o contraseña verificar los datos";
                        return RedirectToPage("/Login");
                    }
                    else if(user.Id > 0)
                    {
                        HttpContext.Session.SetString("session", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                        return RedirectToPage("/Principal/Index");
                    }
                    return RedirectToPage("/Login");
                }
               
                catch(Exception ex) {
                    return RedirectToAction("Login");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }         
        }

        private Usuarios construirUsuarioCompleto(DataTableReader Usuario)
        {
            return new Usuarios()
            {
                Nick = Usuario["nickUsuario"].ToString(),
                Id = int.Parse(Usuario["idUsuario"].ToString())
            };

        }
    }
}
