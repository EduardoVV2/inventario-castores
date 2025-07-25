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
    public class IncidentesUsuario : PageModel
    {
        private readonly ILogger<IncidentesUsuario> _logger;
        string conexion = "";
        int usuario = 0; //Cambiará cuando se implemente la autenticación

        public List<TiposIncidencias> tiposIncidencias = new List<TiposIncidencias>();
        public Documento doc { get; set; } = new Documento();
        [BindProperty]
        public Incidencia incidencia { get; set; }
        public List<Incidencia> incidencias = new List<Incidencia>();
        public List<Incidencia> incidenciasParaAceptar = new List<Incidencia>();
        [BindProperty]
        public string accion { get; set; }
        [BindProperty]
        public int aceptada { get; set; } = 0;
        [BindProperty]
        public int rechazada { get; set; } = 0;
        [BindProperty]
        public List<int> incidenciasSeleccionadas { get; set; } = new List<int>();

        public IncidentesUsuario(ILogger<IncidentesUsuario> logger, IConfiguration configuration)
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
            GetTiposIncidencias();
            GetIncidencias(usuario);
            GetIncidenciasParaAceptar(); ////Está en error //Se acualizará cuando Pepito termine el organigrama 
            return Page();
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

        public void GetIncidenciasParaAceptar() ////Se acualizará cuando Pepito termine el organigrama
        {
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", usuario)); //Cambiará cuando se implemente la autenticación
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosGetCollectionWithPuestoUsuario", prmtrs)) //Está en error
                    {
                        while (dtr.Read())
                        {
                            incidenciasParaAceptar.Add(construirIncidenciasParaAceptar(dtr)); 
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

        public void GetIncidencias(int usuario)
        {
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", usuario)); //Cambiará cuando se implemente la autenticación
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosGetCollectionWithUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            incidencias.Add(construirIncidencias(dtr));
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

        private Incidencia construirIncidenciasParaAceptar(DataTableReader incidencia){
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
                ApellidoPaternoUsuario = incidencia["apellidoPaternoUsuario"]?.ToString()
            };
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
                Comentarios = incidencia["comentarios"].ToString()
            };
        }


        public ActionResult OnPostGuardarIncidencia()
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;

            string rutaDocumento = "";
            //En caso de que el Documento no sea nulo
            if (Request.Form.Files.Count != 0)
            {
                // Procesar archivo subido
                var archivo = Request.Form.Files[0];
                
                try
                {
                    // Guardar archivo en el servidor
                    var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                    var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/incidentes");
                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }
                    var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        archivo.CopyTo(stream);
                    }
                    rutaDocumento = $"/uploads/incidentes/{nombreArchivo}"; // Ruta relativa para la base de datos
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al guardar el archivo: {ex.Message}");
                }

            }
            doc.IdUsuario=usuario;

            doc.IdTipoDocumento=35;//35 es el id de tipo de documento "justificante de incidencia"

            
            using var sqlConexion = new SqlServer();

            SqlTransaction? transaction = null;

            try{
                sqlConexion.Conectar(conexion);
                transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción
                List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                //En caso de que el Documento no sea nulo
                if (Request.Form.Files.Count != 0) {
                    // Insertar el documento y obtener el ID
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

                }
                else {
                    prmtrs.Add(new SqlParameter("@IdDocumento", DBNull.Value));
                }


                prmtrs.Add(new SqlParameter("@IdUsuario", doc.IdUsuario));
                prmtrs.Add(new SqlParameter("@Descripcion", incidencia.Descripcion));
                prmtrs.Add(new SqlParameter("@IdTipoIncidencia", incidencia.IdTipoIncidencia));
                prmtrs.Add(new SqlParameter("@FechaIncidencia", incidencia.FechaIncidencia));
                switch (incidencia.IdTipoIncidencia){
                    case 1://Falta
                        
                    break;
                    case 2://Retardo
                        prmtrs.Add(new SqlParameter("@HoraEntrada", incidencia.HoraEntrada));
                    break;
                    case 3://Salida Temprana
                        prmtrs.Add(new SqlParameter("@HoraSalida", incidencia.HoraSalida));
                    break;
                    case 4://Ausencia Temporal
                        prmtrs.Add(new SqlParameter("@HoraEntrada", incidencia.HoraEntrada));
                        prmtrs.Add(new SqlParameter("@HoraSalida", incidencia.HoraSalida));
                    break;
                    default:
                                            
                    break;
                }

                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosCrear", prmtrs, transaction))
                {
                    //while (dtr.Read())
                    //{
                        
                    //}
                }
                transaction.Commit();
                TempData["Message"] = "¡Incidencia creada correctamente!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex){
                transaction?.Rollback(); // Revertir en caso de error
                //Eliminar el archivo guardado
                if (System.IO.File.Exists(rutaDocumento))
                {
                    System.IO.File.Delete(rutaDocumento);
                }
                TempData["Message"] = "¡Incidencia no creada!";
                TempData["MessageType"] = "fail";
            }
            finally{
                sqlConexion.Desconectar();
            }
            return RedirectToPage();
        }
        
        public ActionResult OnPostAcciones()
        {
            if (incidenciasSeleccionadas.Count == 0)
            {
                TempData["Message"] = "¡No se seleccionó ninguna incidencia!";
                TempData["MessageType"] = "error";
                return RedirectToPage();
            }
            switch (accion)
            {
                case "enviar":
                    using (SqlServer sqlConexion = new SqlServer()){

                        try{
                            sqlConexion.Conectar(conexion);
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                            foreach (var idIncidencia in incidenciasSeleccionadas)
                            {
                                prmtrs.Add(new SqlParameter("@IdIncidencia", idIncidencia));
                                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosEnviar", prmtrs))
                                {
                                    //while (dtr.Read())
                                    //{
                                        
                                    //}
                                }
                                prmtrs.Clear();
                            }
                            TempData["Message"] = "¡Incidencia(s) enviada(s) correctamente!";
                            TempData["MessageType"] = "success";
                        }
                        catch(Exception ex){
                            TempData["Message"] = "¡Incidencia(s) no enviada(s)!\\n Verifique que las incidencias seleccionadas no hayan sido ya enviadas previamente.";
                            TempData["MessageType"] = "error";
                        }
                        finally{
                            sqlConexion.Desconectar();
                        }
                    }
                break;
                case "eliminar":   
                    using (SqlServer sqlConexion = new SqlServer()){

                        try{
                            sqlConexion.Conectar(conexion);
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                            foreach (var idIncidencia in incidenciasSeleccionadas)
                            {
                                prmtrs.Add(new SqlParameter("@IdIncidencia", idIncidencia));
                                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.IncidenciasUsuariosEliminar", prmtrs))
                                {
                                    //while (dtr.Read())
                                    //{
                                        
                                    //}
                                }
                                prmtrs.Clear();
                            }
                            TempData["Message"] = "¡Incidencia(s) eliminada(s) correctamente!";
                            TempData["MessageType"] = "success";
                        }
                        catch(Exception ex){
                            TempData["Message"] = "¡Incidencia(s) no eliminada(s)!";
                            TempData["MessageType"] = "error";
                        }
                        finally{
                            sqlConexion.Desconectar();
                        }
                    }
                break;
                default:
                    TempData["Message"] = "¡Acción no válida!";
                    TempData["MessageType"] = "error";
                    return RedirectToPage();
            }
            return RedirectToPage();
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