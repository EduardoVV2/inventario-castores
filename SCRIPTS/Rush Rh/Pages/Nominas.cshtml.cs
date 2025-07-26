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
using static Rush_Rh.Models.Formularios;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Rush_Rh.Pages
{
    public class Nominas : PageModel
    {
        private readonly ILogger<Nominas> _logger;

        [BindProperty]
        public RegistroNominas infoNominas { get; set; }
        public Documento doc { get; set; } = new Documento();

        string conexion = "";

        public List<UsuarioNombre> usuarios = new List<UsuarioNombre>();
        public List<Meses> meses = new List<Meses>();
        public List<NominasSubidas> nominas = new List<NominasSubidas>();
        public List<int> años = new List<int>();

        public Nominas(ILogger<Nominas> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public void OnGet(int? usuario)
        {
            GetUsuarios();
            GetMeses();
            GetAnios();

            if(usuario.HasValue)
            {
                GetNominas(usuario.Value);
            }
            
        }

        private void GetUsuarios(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Estatus", 1));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAll", prmtrs))
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

        private UsuarioNombre construirUsuario(DataTableReader usuario){
            return new UsuarioNombre()
            {
                Id = int.Parse(usuario["Id"].ToString()),
                Nombre = usuario["Nombre"].ToString(),
            };
        }

        private void GetMeses(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetMesesCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            meses.Add(construirMes(dtr));
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

        private Meses construirMes(DataTableReader mes){
            return new Meses()
            {
                Id = int.Parse(mes["id"].ToString()),
                Nombre = mes["nombre"].ToString(),
            };
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
                            años.Add(int.Parse(dtr["año"].ToString()));
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
        
        private void GetNominas(int usuario){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new System.Data.SqlClient.SqlParameter("@IdUsuario", usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.NominasGetAllByUsuario", prmtrs))
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

        private NominasSubidas construirNominas(DataTableReader nomina){
            return new NominasSubidas ()
            {
                Id = int.Parse(nomina["Id"].ToString()),
                IdMes = int.Parse(nomina["IdMes"].ToString()),
                Mes = nomina["Mes"].ToString(),
                IdDocumento = int.Parse(nomina["IdDocumento"].ToString()),
                URL = nomina["URL"].ToString(),
                Año = int.Parse(nomina["Año"].ToString())
            };
        }

        public IActionResult OnPostGuardarNomina(){

            using var sqlConexion = new SqlServer();
            try{
                //reviso si en el mes y año que quiero subir la nomina no tiene otra asiganada
                int? NumeroNominas = null;
                sqlConexion.Conectar(conexion);
                List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                prmtrs.Add(new SqlParameter("@IdUsuario", infoNominas.IdUsuario));
                prmtrs.Add(new SqlParameter("@IdMes", infoNominas.IdMes));
                prmtrs.Add(new SqlParameter("@Anio", infoNominas.Año));
                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.NominasGetNumerosNominasByMonth", prmtrs))
                {
                    if (dtr.Read())
                    {
                        if(int.TryParse(dtr["nominas"]?.ToString(), out int result))
                        {
                            NumeroNominas = result;
                        }
                    }
                } 

                if(NumeroNominas == 0)
                {
                    string rutaDocumento = "";
                    //si mando un documento
                    if (Request.Form.Files.Count != 0)
                    {
                        //Procesar el archivo subido
                        var archivo = Request.Form.Files[0];

                        try{
                            //Guardar archivo en el servidor
                            var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                            var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/nominas");
                            if (!Directory.Exists(rutaCarpeta))
                            {
                                Directory.CreateDirectory(rutaCarpeta);
                            }
                            var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                            using ( var stream = new FileStream(rutaCompleta, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }
                            rutaDocumento = $"/uploads/nominas/{nombreArchivo}";  // Ruta relativa para la base de datos

                        }catch (Exception ex)
                        {
                            _logger.LogError(ex, "Hubo un error al guardar el archivo");
                            TempData["MessageTitle"] = "¡Error!";
                            TempData["Message"] = "¡Hubo un error al subir el archivo!";
                            TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                            return RedirectToPage();
                        }

                        doc.IdUsuario = infoNominas.IdUsuario;
                        doc.IdTipoDocumento = 34;

                        SqlTransaction? transaction = null;

                        try{
                            transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción
                            //si existe el documento
                            if(Request.Form.Files.Count != 0){
                                //Guardo en la base de datos el documento y obtengo su id para ponerlo en el registro de la nomina
                                List<System.Data.SqlClient.SqlParameter> parametrosDocumento = new List<System.Data.SqlClient.SqlParameter>();
                                parametrosDocumento.Add(new SqlParameter("@IdTipoDocumento", doc.IdTipoDocumento));
                                parametrosDocumento.Add(new SqlParameter("@DireccionDocumento", rutaDocumento));
                                parametrosDocumento.Add(new SqlParameter("@IdUsuario", doc.IdUsuario));

                                int? idDocumento = null;
                                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoCrear", parametrosDocumento, transaction))
                                {
                                    if (dtr.Read())
                                    {
                                        if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                                        {
                                            idDocumento = result;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No se pudo obtener el ID del documento.");
                                    }
                                }

                                prmtrs.Add(new SqlParameter("@IdDocumento", idDocumento));
                                prmtrs.Add(new SqlParameter("@IdUsuario", infoNominas.IdUsuario));
                                prmtrs.Add(new SqlParameter("@IdMes", infoNominas.IdMes));
                                prmtrs.Add(new SqlParameter("@Anio", infoNominas.Año));
                                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.NominasCrear", prmtrs, transaction))
                                {

                                }                   
                                
                                TempData["MessageTitle"] = "¡Éxito!";
                                TempData["Message"] = "¡Nómina subida correctamente!";
                                TempData["MessageType"] = "success";
                                transaction.Commit();

                            }else{
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡No se pudo recibir el archivo de nomina!";
                                TempData["MessageType"] = "error";
                            }
                            
                        }catch(Exception ex){
                            transaction?.Rollback(); // Revertir en caso de error
                            //Eliminar el archivo guardado
                            if (System.IO.File.Exists(rutaDocumento))
                            {
                                System.IO.File.Delete(rutaDocumento);
                            }
                            _logger.LogError(ex, "Hubo un error al subir la nómina");
                            TempData["MessageTitle"] = "¡Error!";
                            TempData["Message"] = "¡Hubo un error al subir la nómina!";
                            TempData["MessageType"] = "error";
                        }finally{
                            sqlConexion.Desconectar();
                        }
                    }
                    else{
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡No se pudo recibir el archivo de nomina!";
                        TempData["MessageType"] = "error";
                    }
                }
                else{
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Este usuario ya tiene una nómina en las fechas seleccionadas!";
                    TempData["MessageType"] = "error";
                }
            }catch(Exception ex){
                _logger.LogError(ex, "Hubo un error al subir la nómina");
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "¡Hubo un error al subir la nómina!";
                TempData["MessageType"] = "error";
            }finally{
                sqlConexion.Desconectar();
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostEliminarNominas(int id, string url){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdNomina", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.NominasBorrar", prmtrs))
                    {
                      
                    } 

                    //Eliminar el archivo guardado
                    if (System.IO.File.Exists(url))
                    {
                        System.IO.File.Delete(url);
                    }

                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Nómina eliminada con éxito";
                    TempData["MessageType"] = "success";
                }catch(Exception ex){
                     _logger.LogError(ex, "Hubo un error al eliminar la nómina");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al eliminar la nómina!";
                    TempData["MessageType"] = "error";
                }finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage();
        }
    }
}   

