using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Rush_Rh.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        string conexion = "";
        public LoginUsuario info = new LoginUsuario() { Nick = "", Password=""};

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");

        }

        public void OnGet()
        {
        }


        public IActionResult OnPostDoLogin(LoginUsuario info)
        {
            
            byte[] hash = info.Password.EncriptarContrasenia();
            using (SqlServer sqlConexion = new SqlServer())
            {
                var user = new Usuarios();
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Nick", info.Nick));
                    prmtrs.Add(new SqlParameter("@Pass", SqlDbType.VarBinary, 64) { Value = hash });
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.Login_get", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            user = construirUsuarioCompleto(dtr);
                        }
                    }
                    if(user.Id == 0)
                    {
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrió un error al iniciar sesión!\\n Verifica tus datos.";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    }
                    else if(user.Id > 0)
                    {
                        HttpContext.Session.SetString("session", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                        HttpContext.Session.SetInt32("idUsuario", user.Id);
                        List<System.Data.SqlClient.SqlParameter> parameter = new List<System.Data.SqlClient.SqlParameter>();
                        parameter.Add(new SqlParameter("@idUsuario", user.Id));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexGetPuestoWithUsuario", parameter))
                        {
                            if (dtr.Read())
                            {
                                HttpContext.Session.SetInt32("idPuesto", int.Parse(dtr["idPuesto"].ToString()));
                            }
                        }
                        return RedirectToPage("/Home");
                    }
                }        
                catch(Exception ex) {
                    return RedirectToAction("Login");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
                return RedirectToPage();
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
    public static class PasswordExtensions
    {
        public static byte[] EncriptarContrasenia(this string contraseña)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                    return hashBytes;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Hubo un problema al encriptar la contraseña.", ex);
            }
        }
    }
}
