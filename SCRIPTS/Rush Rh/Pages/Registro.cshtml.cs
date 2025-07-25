
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using static Rush_Rh.Models.Formularios;
using System.Security.Cryptography;
using System.Text;

namespace Rush_Rh.Pages
{
    public class Registro : PageModel
    {
        [BindProperty]
        public UsuarioRegistro info { get; set; }
        string conexion = "";

        /*listas para guardar los que nos manda el store y mandarla al front  */
        public List<Sexos> sexos = new List<Sexos>();
        public List<EstadosCiviles> estadosCiviles = new List<EstadosCiviles>();
        public List<Nacionalidades> nacionalidades= new List<Nacionalidades>();
        public List<Generos> generos= new List<Generos>();
        public List<TiposUsuarios> tiposUsuario = new List<TiposUsuarios>();

        public Registro(IConfiguration configuration, ILogger<Registro> logger)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }
        private readonly ILogger<Registro> _logger;

        

        public IActionResult OnGet()
        {
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            if(puestoUsuario != 3 && puestoUsuario != 5){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes los permisos para acceder a esta página.";
                TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Home");
            }
            GetSexos();
            GetEstadosCiviles();
            GetNacionalidades();
            GetGeneros();
            GetTiposUsuario();
            return Page();
        }

        /*Metodo para traer de la BD los sexos*/
        private void GetSexos(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetSexosColeccion", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            sexos.Add(construirSexo(dtr));
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

        /*Contructor para sexo*/
        private Sexos construirSexo(DataTableReader sexo){
            return new Sexos ()
            {
                Id = int.Parse(sexo["Id"].ToString()),
                Nombre = sexo["Nombre"].ToString()
            };
        }

        private void GetTiposUsuario(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTiposUsuarioCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposUsuario.Add(construirTipo(dtr));
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

        /*Contructor para tipo de usuario*/
        private TiposUsuarios construirTipo(DataTableReader tiposUsuario){
            return new TiposUsuarios ()
            {
                Id = int.Parse(tiposUsuario["id"].ToString()),
                Nombre = tiposUsuario["nombre"].ToString()
            };
        }

        private void GetEstadosCiviles(){
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetEstadoCivilCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            estadosCiviles.Add(construirEstadoCivil(dtr));
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

        private EstadosCiviles construirEstadoCivil(DataTableReader estadoCivil){
            return new EstadosCiviles (){
                Id = int.Parse(estadoCivil["Id"].ToString()),
                Nombre = estadoCivil["Nombre"].ToString()
            };
        }
        private void GetNacionalidades(){
            using (SqlServer sqlConexion = new SqlServer()){
                 try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetNacionalidadesCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            nacionalidades.Add(construirNacionalidades(dtr));
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

        private Nacionalidades construirNacionalidades(DataTableReader nacionalidades){
            return new Nacionalidades (){
                    Id = int.Parse(nacionalidades["Id"].ToString()),
                    Nombre = nacionalidades["Nombre"].ToString()
            };
        }

        private void GetGeneros(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetGenerosCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            generos.Add(construirGeneros(dtr));
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

        private Generos construirGeneros(DataTableReader generos){
            return new Generos (){
                    Id = int.Parse(generos["Id"].ToString()),
                    Nombre = generos["Nombre"].ToString()
            };
        }

        public IActionResult OnPostRegistrarUsuario(){
            using (SqlServer sqlConexion = new SqlServer()){
                 try{
                    string nick = "";
                    nick = ((info.Nombre.Trim()).Substring(0, 1)).ToLower();
                    nick = nick + info.ApellidoPaterno;
                    nick = ValidarNick(nick);
                    //ValidarNick(nick);
                    long NuevoId = 0;

                    byte[] contraseñaEncriptada = "123".EncriptarContrasenia();

                    sqlConexion.Conectar(conexion);
                    //var fecha = DateTime.Parse(usuarioRegistro.FechaNacimiento);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Nombre", info.Nombre));
                    prmtrs.Add(new SqlParameter("@ApellidoMaterno", info.ApellidoMaterno));
                    prmtrs.Add(new SqlParameter("@ApellidoPaterno", info.ApellidoPaterno));
                    prmtrs.Add(new SqlParameter("@FechaNacimiento", info.FechaNacimiento)); 
                    prmtrs.Add(new SqlParameter("@IdGenero", info.IdGenero));
                    prmtrs.Add(new SqlParameter("@IdSexo", info.IdSexo));
                    prmtrs.Add(new SqlParameter("@RFC", info.RFC));
                    prmtrs.Add(new SqlParameter("@CURP", info.CURP));
                    prmtrs.Add(new SqlParameter("@Nacionalidad", info.Nacionalidad));
                    prmtrs.Add(new SqlParameter("@IdEstadoCivil", info.IdEstadoCivil));
                    prmtrs.Add(new SqlParameter("@Nick", nick));
                    prmtrs.Add(new SqlParameter("@Contraseña", contraseñaEncriptada));
                    prmtrs.Add(new SqlParameter("@NUE", info.NUE));
                    prmtrs.Add(new SqlParameter("@IdTipoUsuario", info.IdTipoUsuario));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioGuardar", prmtrs))
                    {
                        
                        while (dtr.Read())
                        {
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["NuevoId"]?.ToString(), out int result))
                            {
                                NuevoId = result;
                            }
                        }
                    }

                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "¡Usuario registrado correctamente!";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage("/InformacionComplementaria", new { id = NuevoId });

                }
                
                catch(Exception ex){
                    _logger.LogError(ex, "Error al guardar usuario");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrió un error al guardar el usuario!";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    //throw ex;

                }
                finally{
                    sqlConexion.Desconectar();
                }

                // Redirigir a la misma página
                return RedirectToPage();
            }
           
        }
        private string ValidarNick(string Nick){
            using (SqlServer sqlConexion = new SqlServer()){
            try{
                sqlConexion.Conectar(conexion);
                List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Nick", Nick));

                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ValidarNick", prmtrs))
                {
                    
                    if (dtr.Read())
                    {
                        int valor = dtr.GetInt32(0);
                        if (valor > 0){
                            valor += 1;  
                            Nick = Nick + valor.ToString();
                        }  
                        
                    }
                    Nick = new string(Nick.Normalize(System.Text.NormalizationForm.FormD)
                    .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    .ToArray());
                } 

                Nick = Nick.ToLower();
            }
            catch(Exception ex){

            }
            finally{
                sqlConexion.Desconectar();
            }

            return Nick;
            }
        }
        
        // private string EncriptarContraseña(string contraseña)
        // {
        //     try
        //     {
        //         using (SHA256 sha256 = SHA256.Create())
        //         {
        //             byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
        //             return Convert.ToBase64String(hashBytes);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"Error al encriptar contraseña: {ex.Message}");
        //         throw new InvalidOperationException("Hubo un problema al encriptar la contraseña.", ex);
        //     }
        // }
    }
}

