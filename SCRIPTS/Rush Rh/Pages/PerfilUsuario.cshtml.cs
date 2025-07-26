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
using System.Text.Json;

namespace Rush_Rh.Pages
{
    public class PerfilUsuario : PageModel
    {
        private readonly ILogger<PerfilUsuario> _logger;
        string conexion = "";
        // public int usuario = 0; Descomentar
        public bool paginaPropia = false;
        public int usuario = 20;                                 //Usuario de prueba

        public List<EstatusUsuario> estatusUsuario = new List<EstatusUsuario>();
        public List<Puestos> puestos = new List<Puestos>();
        public List<Departamentos> departamentos = new List<Departamentos>();
        public List<Oficinas> oficinas = new List<Oficinas>();
        public List<TiposUsuarios> tiposUsuarios = new List<TiposUsuarios>();
        //public List<Cardex> cardx = new List<Cardex>();
        
        //Datos de un usuario
        public UsuarioDetalle? Usuario { get; set; }
        public List<Idiomas> IdiomasUsuario { get; set; } = new List<Idiomas>();
        public List<DatosAcademicos> datosAcademicosUsuario { get; set; } = new List<DatosAcademicos>();
        public List<Documento> documentosUsuario { get; set; } = new List<Documento>();
        public List<ExperienciasLaborales> experienciasLaborales { get; set; } = new List<ExperienciasLaborales>();
        public DomicilioDetalle Domicilio { get; set; }

        [BindProperty]
        public UsuarioDetalle editar { get; set; }
        public Cardex cardx = new Cardex();
        [BindProperty]
        public Cardex cardex { get; set; }
        [BindProperty]
        public bool cardexLleno { get; set; } = true;
        [BindProperty]
        public bool cardexBaja { get; set; } = false;
        [BindProperty]
        public bool infoContacto { get; set; } = true;
        [BindProperty]
        public string nuevaContrasenia { get; set; }

        public PerfilUsuario(ILogger<PerfilUsuario> logger, IConfiguration configuration)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public IActionResult OnGet(int id)
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            usuario = idusuario ?? 0;
            if(usuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            if (usuario > 0 && id < 1){
                id = usuario;
                paginaPropia = true;
            }
            if (id > 0)
            {
                if(!paginaPropia && puestoUsuario != 3 && puestoUsuario != 5){
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "No tienes los permisos para acceder a esta página.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage("/Home");
                }
                if (GetCardex(id)==1)
                {
                    cardexLleno = true;
                }
                else
                {
                    cardexLleno = false;
                }
                ObtenerDetallesUsuario(id);

                // Deserializar el JSON del domicilio si existe
                if (!string.IsNullOrEmpty(Usuario?.Domicilio))
                {
                    try
                    {
                        Domicilio = JsonSerializer.Deserialize<DomicilioDetalle>(Usuario.Domicilio);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error al deserializar el JSON de domicilio: {ex.Message}");
                        Domicilio = null; // En caso de error, dejamos el objeto como nulo
                    }
                }
                GetPuestos();
                GetOficinas();
                GetDepartamentos();
                GetTiposUsuario();
                GetEstatusUsuario();
            }
            else{
                return RedirectToPage("/ListaUsuarios");
            }
            return Page();
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
                            tiposUsuarios.Add(construirTipo(dtr));
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

        private void GetEstatusUsuario(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetEstatusUsuarioCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            estatusUsuario.Add(construirEstatus(dtr));
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
        private EstatusUsuario construirEstatus(DataTableReader tiposUsuario){
            return new EstatusUsuario ()
            {
                Id = int.Parse(tiposUsuario["id"].ToString()),
                Nombre = tiposUsuario["nombre"].ToString()
            };
        }

        private void ObtenerDetallesUsuario(int idUsuario)
        {

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter("@IdUsuario", idUsuario)
                    };

                    using (DataTableReader reader = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetUsuarioCompleto", parametros))
                    {
                        while (reader.Read())
                        {
                            // Construir el usuario solo una vez
                            Usuario ??= ConstruirUsuarioDetalle(reader);

                            // // Construir y agregar los documentos asociados
                            // if (reader["DireccionDocumento"] != DBNull.Value)
                            // {
                            //     documentosUsuario.Add(new Documento
                            //     {
                            //         URL = reader["DireccionDocumento"]?.ToString(),
                            //         IdTipoDocumento = int.Parse(reader["IdTipoDocumento"]?.ToString()),
                            //         //DescripcionClaseDocumento = reader["DescripcionClaseDocumento"]?.ToString(),
                            //     });
                            // }
                            // Construir y agregar los idiomas asociados
                            if (reader["IdIdioma"] != DBNull.Value && !IdiomasUsuario.Any(i => i.Id == int.Parse(reader["IdIdioma"].ToString())))
                            {
                                IdiomasUsuario.Add(new Idiomas
                                {
                                    Id = int.Parse(reader["IdIdioma"]?.ToString()),
                                    Nombre = reader["NombreIdioma"]?.ToString(),
                                    NivelEscrito = int.Parse(reader["NivelHablado"]?.ToString()),
                                    NivelHablado = int.Parse(reader["NivelEscrito"]?.ToString()),
                                });
                            }
                            // // Construir y agregar los datos de estudios asociados
                            // if (reader["IdDatosEstudio"] != DBNull.Value && !datosAcademicosUsuario.Any(i => i.Id == int.Parse(reader["IdDatosEstudio"].ToString())))
                            // {
                            //     datosAcademicosUsuario.Add(new DatosAcademicos
                            //     {
                            //         Id = int.Parse(reader["IdDatosEstudio"]?.ToString()),
                            //         NombreTitulo = reader["NombreTitulo"]?.ToString(),
                            //         Cedula = reader["Cedula"]?.ToString(),
                            //         IdEstatusEstudio = int.Parse(reader["IdEstatusEstudio"]?.ToString()),
                            //         IdNivelAcademico = int.Parse(reader["IdNivelAcademico"]?.ToString()),
                            //     });
                            // }
                            // // Construir y agregar las expoeriencias laborales
                            // if(reader["IdExperiencia"] != DBNull.Value){
                            //     experienciasLaborales.Add(new ExperienciasLaborales 
                            //     {
                            //         IdExperiencia = int.Parse(reader["IdExperiencia"]?.ToString()),
                            //         Descripcion = reader["ExperienciaDescripcion"]?.ToString(),
                            //         FechaInicio = DateTime.Parse(reader["ExperienciaFechaInicio"]?.ToString()),
                            //         FechaTermino = DateTime.Parse(reader["ExperienciaFechaTermino"]?.ToString()),
                            //         NombreEmpleador = reader["ExperienciaNombreEmpleador"]?.ToString(),
                            //         CargoOcupado = reader["ExperienciaCargoOcupado"]?.ToString(),
                            //         IdEstatusTipoExperiencia = int.Parse(reader["IdEstatusTipoExperiencia"]?.ToString()),
                            //     });
                            // }

                            if(reader["Domicilio"] != DBNull.Value){
                                Usuario.Domicilio = reader["Domicilio"]?.ToString(); // JSON del domicilio.
                                Usuario.Celular = reader["Celular"]?.ToString();
                                Usuario.CorreoElectronico = reader["CorreoElectronico"]?.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al obtener los detalles del usuario: {ex.Message}");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

        }
        

        private UsuarioDetalle ConstruirUsuarioDetalle(DataTableReader reader)
        {
            return new UsuarioDetalle
            {
                // Hijos = reader["Hijos"] as int?,
                // ViviendaPropia = reader["ViviendaPropia"] as bool? ?? false,
                // VehiculoPropio = reader["VehiculoPropio"] as bool? ?? false,
                // RentaCasa = reader["RentaCasa"] as bool? ?? false,
                // Deudas = reader["Deudas"] as bool? ?? false,
                // Mascotas = reader["Mascotas"] as bool? ?? false,
                // IngresosMensuales = reader["IngresosMensuales"] != DBNull.Value ? Convert.ToDecimal(reader["IngresosMensuales"]) : (decimal?)null,
                // GastoMensual = reader["GastoMensual"] != DBNull.Value ? Convert.ToDecimal(reader["GastoMensual"]) : (decimal?)null,
                // FrecuenciaActividadesRecreativas = reader["FrecuenciaActividadesRecreativas"]?.ToString(),
                // PersonasVivenCasa = reader["PersonasVivenCasa"] as int?,
                // PersonasDependientes = reader["PersonasDependientes"] as int?,
                
                // NSS = reader["NSS"]?.ToString(),
                // Alergia = reader["Alergia"] as bool? ?? false,
                // ConsumeMedicamentos = reader["ConsumeMedicamentos"] as bool? ?? false,
                // EnfermedadCronica = reader["EnfermedadCronica"] as bool? ?? false,
                // DetalleAlergia = reader["DetalleAlergia"]?.ToString(),
                // DetalleMedicamentos = reader["DetalleMedicamentos"]?.ToString(),
                // FrecuenciaMedica = reader["FrecuenciaMedica"] as int?,
                // DetalleEnfermedad = reader["DetalleEnfermedad"]?.ToString(),
                // NombreTipo = reader["NombreTipo"]?.ToString(),
                // NombreContactoEmergencia = reader["NombreContactoEmergencia"]?.ToString(),
                // CelularContactoEmergencia = reader["CelularContactoEmergencia"]?.ToString(),

                // // NombreTitulo = reader["NombreTitulo"]?.ToString(),
                // // Cedula = reader["Cedula"]?.ToString(),
                // // NombreNivelAcademico = reader["NombreNivelAcademico"]?.ToString(),
                // // NombreEstatusEstudio = reader["NombreEstatusEstudio"]?.ToString(),

                Nombre = reader["Nombre"]?.ToString(),
                ApellidoMaterno = reader["ApellidoMaterno"]?.ToString(),
                ApellidoPaterno = reader["ApellidoPaterno"]?.ToString(),
                FechaNacimiento = reader["FechaNacimiento"] as DateTime?,
                RFC = reader["RFC"]?.ToString(),
                CURP = reader["CURP"]?.ToString(),
                Nick = reader["Nick"]?.ToString(),
                NUE = reader["NUE"]?.ToString(),
                IdTipoUsuario = int.Parse(reader["IdTipoUsuario"]?.ToString()),
                IdEstatusUsuario = int.Parse(reader["IdEstatus"]?.ToString()),

                NombreGenero = reader["NombreGenero"]?.ToString(),
                NombreSexo = reader["NombreSexo"]?.ToString(),
                NombreEstado = reader["NombreEstado"]?.ToString(),
                NombreEstatus = reader["NombreEstatus"]?.ToString(),
                
                IdDocumento = reader["IdDocumentoDatosPersonales"] != DBNull.Value ? new Documento { Id = Convert.ToInt32(reader["IdDocumentoDatosPersonales"]), URL = reader["DocumentoURLFotoPerfil"]?.ToString(), IdTipoDocumento = reader["IdTipoDocumento"] != DBNull.Value ? Convert.ToInt32(reader["IdTipoDocumento"]) : 0 } : new Documento { Id = 0 },

                // // Not good
                // // ExperienciaDescripcion = reader["ExperienciaDescripcion"]?.ToString(),
                // // ExperienciaFechaInicio = reader["ExperienciaFechaInicio"]?.ToString(),
                // // ExperienciaFechaTermino = reader["ExperienciaFechaTermino"]?.ToString(),
                // // ExperienciaNombreEmpleador = reader["ExperienciaNombreEmpleador"]?.ToString(),
                // // ExperienciaCargoOcupado = reader["ExperienciaCargoOcupado"]?.ToString(),
                // // NombreTipoExperiencia = reader["NombreTipoExperiencia"]?.ToString(),


                // FechaInicioContrato = reader["FechaInicioContrato"] as DateTime?,
                // HorarioSalidaTurno = reader["HorarioSalidaTurno"] as DateTime?,
                // FechaIngreso = reader["FechaIngreso"] as DateTime?,
                // HorarioEntradaTurno = reader["HorarioEntradaTurno"] as DateTime?,
                // FechaTerminoContrato = reader["FechaTerminoContrato"] as DateTime?,
                // NombreTipoContrato = reader["NombreTipoContrato"]?.ToString(),
                // NombreDepartamento = reader["NombreDepartamento"]?.ToString(),
                // DescripcionDepartamento = reader["DescripcionDepartamento"]?.ToString(),
            };
        }

        public int GetCardex(int id_usuario){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", id_usuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexGet", prmtrs))
                    {
                        if (dtr.Read()){
                            cardx=construirCardex(dtr);
                            return 1;
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
                return 0;
            }
        }

        private Cardex construirCardex(DataTableReader cardex){
            return new Cardex (){
                Id = int.Parse(cardex["id"].ToString()),
                IdOficina = int.Parse(cardex["idOficina"].ToString()),
                IdPuesto = int.Parse(cardex["idPuesto"].ToString()),
                IdUsuario = int.Parse(cardex["idUsuario"].ToString()),
                FechaInicio = DateTime.Parse(cardex["fechaInicio"].ToString()),
                FechaTermino = DateTime.Parse(cardex["fechaTermino"].ToString()),
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
                Id = int.Parse(puestos["id"].ToString()),
                Nombre = puestos["nombre"].ToString(),
                IdDepartamento = int.Parse(puestos["idDepartamento"].ToString())
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
        private Departamentos construirDepartamentos(DataTableReader departamento){
            return new Departamentos (){
                Id = int.Parse(departamento["id"].ToString()),
                Nombre = departamento["nombre"].ToString()
            };
        }

        private void GetOficinas(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetOficinasCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            oficinas.Add(construirOficinas(dtr));
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

        private Oficinas construirOficinas(DataTableReader oficinas){
            return new Oficinas (){
                    Id = int.Parse(oficinas["id"].ToString()),
                    Nombre = oficinas["nombre"].ToString()
            };
        }

        public IActionResult OnPostGuardarCardex()
        {
            using (SqlServer sqlConexion = new SqlServer()) {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdOficina", cardex.IdOficina));
                    prmtrs.Add(new SqlParameter("@IdPuesto", cardex.IdPuesto));
                    prmtrs.Add(new SqlParameter("@FechaInicio", cardex.FechaInicio));
                    prmtrs.Add(new SqlParameter("@FechaTermino", cardex.FechaTermino));
                    prmtrs.Add(new SqlParameter("@IdUsuario", cardex.IdUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexCrear", prmtrs))
                    {
                        // while (dtr.Read())
                        // {
                        //     avisos.Add(construirAvisos(dtr));
                        // }
                    }
                    TempData["Message"] = "¡Cardex Creado correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch(Exception ex){
                    throw new Exception("Hubo un error al crear el cardex", ex);
                }
                finally {
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage("PerfilUsuario", new { id = cardex.IdUsuario });
        }
        public IActionResult OnPostBajaCardex(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdCardex", cardex.Id));
                    prmtrs.Add(new SqlParameter("@JustCardexBaja", cardexBaja));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexBaja", prmtrs))
                    {
                    }
                    TempData["Message"] = "¡Cardex dado de baja correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    throw new Exception("Hubo un error al dar de baja el cardex", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage("PerfilUsuario", new { id = cardex.IdUsuario });
        }
        public IActionResult OnPostEditarDatos(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Id", editar.Id));
                    prmtrs.Add(new SqlParameter("@Nombre", editar.Nombre));
                    prmtrs.Add(new SqlParameter("@ApellidoPaterno", editar.ApellidoPaterno));
                    prmtrs.Add(new SqlParameter("@ApellidoMaterno", editar.ApellidoMaterno));

                    prmtrs.Add(new SqlParameter("@Cardex", cardexLleno));
                    prmtrs.Add(new SqlParameter("@IdPuesto", cardex.IdPuesto));
                    // prmtrs.Add(new SqlParameter("@IdEstatusUsuario", editar.IdEstatusUsuario));
                    prmtrs.Add(new SqlParameter("@NUE", editar.NUE));

                    prmtrs.Add(new SqlParameter("@Contacto", infoContacto));
                    prmtrs.Add(new SqlParameter("@CorreoElectronico", editar.CorreoElectronico));
                    prmtrs.Add(new SqlParameter("@Celular", editar.Celular));
                    prmtrs.Add(new SqlParameter("@IdTipoUsuario", editar.IdTipoUsuario));
                    
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioPerfilEditar", prmtrs))
                    {
                        // while (dtr.Read())
                        // {
                        //     avisos.Add(construirAvisos(dtr));
                        // }
                    }
                    TempData["Message"] = "¡Datos editados correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    throw new Exception("Hubo un error al editar los datos", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage("PerfilUsuario", new { id = editar.Id });
        }

        public IActionResult OnPostCambiarPassword(){
            byte [] contraseniaEncriptada = nuevaContrasenia.EncriptarContrasenia();
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            usuario = idusuario ?? 0;
            if(usuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }
            editar.Id = usuario;
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", editar.Id));
                    prmtrs.Add(new SqlParameter("@NuevaContrasenia", SqlDbType.VarBinary, 64) { Value = contraseniaEncriptada });
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioCambiarContrasenia", prmtrs))
                    {}
                    TempData["Message"] = "¡Contraseña cambiada correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    throw new Exception("Hubo un error al cambiar la contraseña", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage();
        }
        public IActionResult OnPostRestablecerPassword(){
            byte [] contraseniaEncriptada = "123".EncriptarContrasenia();
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", editar.Id));
                    prmtrs.Add(new SqlParameter("@NuevaContrasenia", SqlDbType.VarBinary, 64) { Value = contraseniaEncriptada });
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuarioCambiarContrasenia", prmtrs))
                    {}
                    TempData["Message"] = "¡Contraseña cambiada correctamente!";
                    TempData["MessageType"] = "success";
                }
                catch (Exception ex){
                    throw new Exception("Hubo un error al cambiar la contraseña", ex);
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage("PerfilUsuario", new { id = editar.Id });
        }
    }
}