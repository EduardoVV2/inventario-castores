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
using static Rush_Rh.Models.Formularios;
using static Rush_Rh.Models.Usuarios;
using Microsoft.AspNetCore.Server.Kestrel.Core;


namespace Rush_Rh.Pages
{
    public class Avisos : PageModel
    {
        private readonly ILogger<Avisos> _logger;

        [BindProperty]
        public Aviso info { get; set; } 
        public Documento doc{ get; set; } = new Documento();
        [BindProperty]
        public string accion { get; set; }
        [BindProperty]
        public int eliminar { get; set; } = 0;
        [BindProperty]
        public int restaurar { get; set; } = 0;
        [BindProperty]
        public List<int> seleccionados { get; set; } = new List<int>();
        [BindProperty]
        public int fechaFiltro { get; set; } = 0;
        [BindProperty]
        public DateTime? fechaInicio{ get; set; }
        [BindProperty]
        public DateTime? fechaTermino{ get; set; }

        string conexion = "";
        int usuarioRH = 5;

        public List<MediosEnvio> mediosEnvio = new List<MediosEnvio>();
        public List<UsuarioGeneral> usuariosGenerales = new List<UsuarioGeneral>();
        public List<Departamentos> departamentos = new List<Departamentos>();

        public List<Puestos> puestos = new List<Puestos>();
        public List<Aviso> avisos = new List<Aviso>();
        public List<Generos> generos= new List<Generos>();
        public List<Documento> documentos= new List<Documento>();



        public Avisos(ILogger<Avisos> logger, IConfiguration configuration)
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
            //TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
            GetMediosEnvio();
            GetUsuarios(1);
            GetGeneros();
            GetDepartamentos();
            GetPuestos();
            GetAvisos();
            GetDocumentos();
            return Page();
        }


        /*Metodo para traer de la BD los MedioEnvio*/
        private void GetMediosEnvio(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetMediosEnvioCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            mediosEnvio.Add(construirMedioEnvio(dtr));
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

        /*Contructor para  MediosEnvio*/
        private MediosEnvio construirMedioEnvio(DataTableReader medioEnvio){
            return new MediosEnvio ()
            {
                Id = int.Parse(medioEnvio["Id"].ToString()),
                Nombre = medioEnvio["Nombre"].ToString()
            };
        }


        
        public void GetUsuarios( int estatus){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Estatus", estatus));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAll", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            usuariosGenerales.Add(construirUsuario(dtr));
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

        private UsuarioGeneral construirUsuario(DataTableReader usuarioGeneral){
            return new UsuarioGeneral()
            {
                Id = int.Parse(usuarioGeneral["Id"].ToString()),
                Nombre = usuarioGeneral["Nombre"].ToString(),
                Nick = usuarioGeneral["Nick"].ToString(),
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


        private void GetDepartamentos(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetDepartamentoCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            departamentos.Add(construirDepartamentos(dtr));
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


        private Departamentos construirDepartamentos(DataTableReader departamentos){
            return new Departamentos (){
                    Id = int.Parse(departamentos["Id"].ToString()),
                    Nombre = departamentos["Nombre"].ToString()
            };
        }


        private void GetPuestos(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetPuestosCollectionSimple", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            puestos.Add(construirPuestos(dtr));
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

        private Puestos construirPuestos(DataTableReader puestos){
            return new Puestos (){
                    Id = int.Parse(puestos["Id"].ToString()),
                    Nombre = puestos["Nombre"].ToString()
            };
        }

        private void GetDocumentos(){
            using (SqlServer sqlConexion = new SqlServer()){
                try {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@TipoDocumento", 5));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoGetCollectionWithTipo", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            documentos.Add(construirDocumentos(dtr));
                        }
                    }
                }
                catch(Exception ex){
                    throw new Exception("No se encontró un documento", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }
        private Documento construirDocumentos(DataTableReader documentos){
            return new Documento (){
                Id = int.Parse(documentos["id"].ToString()),
                URL = documentos["URL"].ToString()
            };
        }
        
        private void GetAvisosByDates(int filtro){
            if(fechaInicio <= new DateTime(1753, 1, 1) || fechaTermino <= new DateTime(1753, 1, 1) || fechaInicio >= new DateTime(9999, 1, 1) || fechaTermino >= new DateTime(9999, 1, 1)){
                TempData["Message"] = "¡No puede agregar esas fechas para filtrar! Por favor use fechas más coherentes";
                TempData["MessageType"] = "error";
                GetAvisos();
            }
            else{
                using (SqlServer sqlConexion = new SqlServer()){
                    try {
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@Filtro", filtro));
                        prmtrs.Add(new SqlParameter("@FechaInicio", fechaInicio));
                        prmtrs.Add(new SqlParameter("@FechaTermino", fechaTermino));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosGetFilteredCollection", prmtrs))
                        {
                            while (dtr.Read())
                            {
                                avisos.Add(construirAvisos(dtr));
                            }
                        }
                        TempData["Message"] = "";
                        TempData["MessageType"] = "";
                    }
                    catch(Exception ex){
                        throw new Exception("Hubo un error al recuperar los avisos", ex);
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
            }
        }

        private void GetAvisos(){
            using (SqlServer sqlConexion = new SqlServer()){
                try {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Mes", DateTime.Now.Month));
                    prmtrs.Add(new SqlParameter("@Anio", DateTime.Now.Year));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosGetCollectionWithMonth", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            avisos.Add(construirAvisos(dtr));
                        }
                    }
                }
                catch(Exception ex){
                    throw new Exception("Hubo un error un aviso", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private Aviso construirAvisos(DataTableReader avisos){
            Aviso aviso = new Aviso (){
                Id = int.Parse(avisos["id"].ToString()),
                Titulo = avisos["titulo"].ToString(),
                Contenido = avisos["contenido"].ToString(),
                MedioEnvio = int.Parse(avisos["medioEnvio"].ToString()),
                FechaEvento = avisos.GetDateOnly("fechaEvento"),
                FechaRegistro = DateTime.Parse(avisos["fechaRegistro"].ToString()),
                FechaEdicion = DateTime.Parse(avisos["fechaEdicion"].ToString()),
                FechaEnvio = avisos.GetDateTime("fechaEnvio"),
                IdDocumento = avisos.GetInt("idDocumento"),
                EnvioUsuario = avisos.GetInt("usuario"),
                EnvioGenero = avisos.GetInt("genero"),
                EnvioPuesto = avisos.GetInt("puesto"),
                EnvioDepartamento = avisos.GetInt("departamento"),
                Activo = bool.Parse(avisos["activo"].ToString())
            };
            var fechaEvento = aviso.FechaEvento.ToDateTime(TimeOnly.MinValue);
            //Filtro que convierte a inactivos los avisos de fechas de evento ya pasadas
            if (fechaEvento<DateTime.Today && aviso.Activo==true){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = [new SqlParameter("@Id", aviso.Id)];
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosEliminar", prmtrs))
                        TempData["Message"] = "¡Aviso eliminado por fecha vencida!\\n Puede modificar la fecha del evento en la sección de edición";
                        TempData["MessageType"] = "info";
                    }
                    catch(Exception ex){
                        throw new Exception("Hubo un error al eliminar un aviso", ex);
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                aviso.Activo = bool.Parse("false");
            }
            return aviso;
        }




        public ActionResult OnPostGuardarAviso(){
            usuarioRH = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            string rutaDocumento = "";
            if (info.Id == 0){ //En el caso de crear un nuevo aviso
                //En caso de que el Documento no sea nulo
                if (Request.Form.Files.Count != 0)
                {
                    // Procesar archivo subido
                    var archivo = Request.Form.Files[0];
                    
                    try
                    {
                        // Guardar archivo en el servidor
                        var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                        var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avisos");
                        if (!Directory.Exists(rutaCarpeta))
                        {
                            Directory.CreateDirectory(rutaCarpeta);
                        }
                        var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        rutaDocumento = $"/uploads/avisos/{nombreArchivo}"; // Ruta relativa para la base de datos
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Error al guardar el archivo: {ex.Message}");
                    }
    
                }
                

                
                // var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                // if (string.IsNullOrEmpty(userId))
                // {
                //     return new UnauthorizedObjectResult("No se pudo identificar al usuario logeado.");

                // }

                // // Asignar el ID del usuario al modelo
                // info.IdUsuario = Convert.ToInt64(userId);
                doc.IdUsuario=usuarioRH;    
                doc.IdTipoDocumento=5;

                
                using var sqlConexion = new SqlServer();

                SqlTransaction? transaction = null;

                try{
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    //En caso de que el Documento no sea nulo
                    if (Request.Form.Files.Count != 0) {
                        // Insertar el documento y obtener el ID
                        List<System.Data.SqlClient.SqlParameter> parametrosDocumento =
                        [
                            new SqlParameter("@IdTipoDocumento", doc.IdTipoDocumento),
                            new SqlParameter("@DireccionDocumento", rutaDocumento),
                            new SqlParameter("@IdUsuario", doc.IdUsuario),
                        ];

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



                    
                    prmtrs.Add(new SqlParameter("@Titulo", info.Titulo));
                    prmtrs.Add(new SqlParameter("@Contenido", info.Contenido));
                    prmtrs.Add(new SqlParameter("@MedioEnvio", info.MedioEnvio));
                    prmtrs.Add(new SqlParameter("@FechaEvento", info.FechaEvento.ToDateTime(TimeOnly.MinValue)));
                    switch (info.MedioEnvio){
                        case 1://Usuario
                            prmtrs.Add(new SqlParameter("@Usuario", info.EnvioUsuario));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 2://Genero
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", info.EnvioGenero));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 3://Puesto
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", info.EnvioPuesto));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 4://A todos
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        default://5--- Departamento
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", info.EnvioDepartamento));
                        break;
                    }

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosCrear", prmtrs, transaction))
                    {
                        //while (dtr.Read())
                        //{
                            
                        //}
                    }
                    transaction.Commit();
                    TempData["Message"] = "¡Aviso creado correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    transaction?.Rollback(); // Revertir en caso de error
                    //Eliminar el archivo guardado
                    if (System.IO.File.Exists(rutaDocumento))
                    {
                        System.IO.File.Delete(rutaDocumento);
                    }
                    throw new Exception("Hubo un error al Registrar un aviso", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            else{ //En el caso de editar un aviso ya existente
                //En caso de que el Documento no sea nulo
                if (Request.Form.Files.Count != 0)
                {
                    // Procesar archivo subido
                    var archivo = Request.Form.Files[0];
                    
                    try
                    {
                        // Guardar archivo en el servidor
                        var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                        var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/avisos");
                        if (!Directory.Exists(rutaCarpeta))
                        {
                            Directory.CreateDirectory(rutaCarpeta);
                        }
                        var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        rutaDocumento = $"/uploads/avisos/{nombreArchivo}"; // Ruta relativa para la base de datos
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Error al guardar el archivo: {ex.Message}");
                    }
    
                }


                doc.IdUsuario=usuarioRH;    
                doc.IdTipoDocumento=5;

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
                        if (info.IdDocumento!=0){
                            prmtrs.Add(new SqlParameter("@IdDocumento", info.IdDocumento));
                        }
                        else{
                            prmtrs.Add(new SqlParameter("@IdDocumento", DBNull.Value));
                        }
                    }


                    prmtrs.Add(new SqlParameter("@Id", info.Id));
                    prmtrs.Add(new SqlParameter("@Titulo", info.Titulo));
                    prmtrs.Add(new SqlParameter("@Contenido", info.Contenido));
                    prmtrs.Add(new SqlParameter("@MedioEnvio", info.MedioEnvio));
                    prmtrs.Add(new SqlParameter("@FechaEvento", info.FechaEvento.ToDateTime(TimeOnly.MinValue)));
                    switch (info.MedioEnvio){
                        case 1://Usuario
                            prmtrs.Add(new SqlParameter("@Usuario", info.EnvioUsuario));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 2://Genero
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", info.EnvioGenero));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 3://Puesto
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", info.EnvioPuesto));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        case 4://A todos
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", DBNull.Value));
                        break;
                        default://5--- Departamento
                            prmtrs.Add(new SqlParameter("@Usuario", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Genero", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Puesto", DBNull.Value));
                            prmtrs.Add(new SqlParameter("@Departamento", info.EnvioDepartamento));
                        break;
                    }

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosEditar", prmtrs, transaction))
                    {
                        //while (dtr.Read())
                        //{
                            
                        //}
                    }
                    transaction.Commit();
                    TempData["Message"] = "¡Aviso editado correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    transaction?.Rollback(); // Revertir en caso de error
                    //Eliminar el archivo guardado
                    if (System.IO.File.Exists(rutaDocumento))
                    {
                        System.IO.File.Delete(rutaDocumento);
                    }
                    throw new Exception("Hubo un error al Editar el aviso", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }

            }

            return RedirectToPage();
        }
        //Acciones que se pueden realizar a un aviso
        public ActionResult OnPostAccionesAvisos(){
            if(eliminar!=0){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@Id", eliminar));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosEliminar", prmtrs))
                        {
                            // while (dtr.Read())
                            // {
                            //     avisos.Add(construirAvisos(dtr));
                            // }
                        }
                        TempData["Message"] = "¡Aviso eliminado correctamente!";
                        TempData["MessageType"] = "success";
                    }
                    catch(Exception ex){
                        throw new Exception("Hubo un error al eliminar un aviso", ex);
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage();
            }
            if(restaurar!=0){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@Id", restaurar));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosRestaurar", prmtrs))
                        {
                            // while (dtr.Read())
                            // {
                            //     avisos.Add(construirAvisos(dtr));
                            // }
                        }
                        TempData["Message"] = "¡Aviso restaurado correctamente, si lo necesita, modifique los datos correspondientes!";
                        TempData["MessageType"] = "info";
                    }
                    catch(Exception ex){
                        throw new Exception("Hubo un error al restaurar un aviso", ex);
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage();
            }
            switch (accion)
            {
                case "enviar":
                    if (seleccionados == null || !seleccionados.Any())
                    {
                        TempData["Message"] = "No se seleccionó ningún elemento.";
                        TempData["MessageType"] = "error";
                        return RedirectToPage();
                    }
                    using (SqlServer sqlConexion = new SqlServer()){
                        try{
                            sqlConexion.Conectar(conexion);
                            string ids = string.Join(",", seleccionados);
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>{ new SqlParameter("@Ids", ids) };
                            using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.AvisosEnviar", prmtrs))
                            {
                                // while (dtr.Read())
                                // {
                                //     avisos.Add(construirAvisos(dtr));
                                // } 
                            }
                            TempData["Message"] = "¡Avisos enviados correctamente!";
                            TempData["MessageType"] = "success";
                        }
                        catch(Exception ex){
                            throw new Exception("Error al enviar los avisos seleccionados", ex);
                        }
                        finally{
                            sqlConexion.Desconectar();
                        }
                    }
                    break;
                
                case "filtrar":
                    GetMediosEnvio();
                    GetUsuarios(1);
                    GetGeneros();
                    GetDepartamentos();
                    GetPuestos();
                    GetDocumentos();
                    GetAvisosByDates(fechaFiltro);
                    return Page();
                default:
                    TempData["Mensaje"] = "Acción no válida." + accion;
                    break;
            }

            return RedirectToPage();
        }



    }
    //Esto es una clase que funciona para el manejo de los valores nulos que lleguen de la base de datos
    public static class DataReaderExtensions
    {
        public static int GetInt(this IDataRecord record, string columnName, int defaultValue = 0)
        {
            return record[columnName] != DBNull.Value ? Convert.ToInt32(record[columnName]) : defaultValue;
        }

        public static string GetString(this IDataRecord record, string columnName, string defaultValue = "")
        {
            return record[columnName] != DBNull.Value ? record[columnName].ToString() : defaultValue;
        }

        public static DateTime GetDateTime(this IDataRecord record, string columnName, DateTime defaultValue = default)
        {
            return record[columnName] != DBNull.Value ? Convert.ToDateTime(record[columnName]) : defaultValue;
        }
        public static DateOnly GetDateOnly(this IDataRecord record, string columnName, DateOnly defaultValue = default)
        {
            return record[columnName] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(record[columnName])) : defaultValue;
        }
    }
}