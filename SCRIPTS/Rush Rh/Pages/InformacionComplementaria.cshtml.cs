using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static Rush_Rh.Models.Formularios;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;


namespace Rush_Rh.Pages
{
    public class InformacionComplementaria : PageModel
    {
        private readonly ILogger<InformacionComplementaria> _logger;

        string conexion = "";

        //Para rellenado de selects
        public List<Pais> paises = new List<Pais>();
        public List<TipoSangre> tiposSangre = new List<TipoSangre>();
        public List<Parentesco> parentescos = new List<Parentesco>();
        public List<NivelesAcademicos> nivelesAcademicos = new List<NivelesAcademicos>();
        public List<EstatusEstudio> estatusEstudio = new List<EstatusEstudio>();
        public List<TiposExperiencias> tiposExperiencias = new List<TiposExperiencias>();


        //Private porque no la muestro directamente, respondo con json
        private List<Estado> estados = new List<Estado>();
        private List<Municipio> municipios = new List<Municipio>();
        private List<Colonia> colonias = new List<Colonia>();


        //Para envio de datos
        [BindProperty]
        public Direccion direccion { get; set; }

        [BindProperty]
        public DatosPersonales datosPersonales { get; set; }

        [BindProperty]
        public DatosMedicos datosMedicos { get; set; }

        [BindProperty]
        public ContactoEmergencia contactoEmergencia { get; set; }

        [BindProperty]
        public DatosSocioeconomicos datosSocioeconomicos { get; set; }

        [BindProperty]
        public List<Idiomas> idiomas { get; set; }

        [BindProperty]
        public List<DatosAcademicos> estudios { get; set; }

        [BindProperty]
        public List<Documento> documentos { get; set; }

        [BindProperty]
        public List<ExperienciasLaborales> experiencias { get; set; }

        [BindProperty]
        public Documentacion documentacion { get; set; }

        [BindProperty]
        public DocumentacionExpediente documentacionExpediente { get; set; }




        //Recuperacion de informacion que ya esta en la base de datos
        public UsuarioDetalle Usuario { get; set; }
        public DomicilioDetalle Domicilio { get; set; } // Objeto deserializado del JSON de domicilio.
        public List<Idiomas> idiomasRecuperados = new List<Idiomas>();
        public List<DatosAcademicos> datosAcademicosRecuperados = new List<DatosAcademicos>();
        public List<ExperienciasLaborales> experienciasRecuperadas = new List<ExperienciasLaborales>();
        public List<Documento> documentosRecuperados = new List<Documento>();
        



        // Para enlazar automáticamente desde la URL
        [BindProperty(SupportsGet = true)] 
        public long idUsuarioActual { get; set; }


        public InformacionComplementaria(ILogger<InformacionComplementaria> logger, IConfiguration configuration)
        {
            _logger = logger;
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
        }


        public void OnGet(long id)
        {
            idUsuarioActual = id;
            GetPaises();
            GetTipoSangre();
            GetParentescos();
            GetNivelesAcademicos();
            GetEstatusEstudio();
            GetTiposExperiencias();
            

            if (id > 0)
            {
                Usuario = ObtenerDetallesUsuario(id);

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
            }

        }

        protected UsuarioDetalle ObtenerDetallesUsuario(long idUsuario)
        {
            UsuarioDetalle usuarioDetalle = null;
            List<DocumentoDetalle> documentos = new List<DocumentoDetalle>();

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
                            if (usuarioDetalle == null)
                            {
                                // Construir el usuario solo una vez
                                usuarioDetalle = ConstruirUsuarioDetalle(reader);
                            }

                            // Construir y agregar los documentos asociados
                            if (reader["DireccionDocumento"] != DBNull.Value)
                            {
                                documentos.Add(new DocumentoDetalle
                                {
                                    DireccionDocumento = reader["DireccionDocumento"]?.ToString(),
                                    NombreTipoDocumento = reader["NombreTipoDocumento"]?.ToString(),
                                    DescripcionClaseDocumento = reader["DescripcionClaseDocumento"]?.ToString(),
                                });
                            }

                            // Construir y agregar los idiomas asociados
                            if (reader["NombreIdioma"] != DBNull.Value &&
                                reader["ActivoIdiomas"] != DBNull.Value &&
                                Convert.ToBoolean(reader["ActivoIdiomas"]) && // Convertimos el bit a booleano
                                !idiomasRecuperados.Any(i => i.Id == Convert.ToInt32(reader["IdIdioma"])))
                            {
                                idiomasRecuperados.Add(new Idiomas
                                {
                                    Id = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 0,
                                    Nombre = reader["NombreIdioma"]?.ToString(),
                                    NivelEscrito = reader["NivelEscrito"] != DBNull.Value ? Convert.ToInt32(reader["NivelEscrito"]) : 0,
                                    NivelHablado = reader["NivelHablado"] != DBNull.Value ? Convert.ToInt32(reader["NivelHablado"]) : 0,
                                });
                            }


                            // Construir y agregar los datos académicos asociados
                            if (reader["NombreTitulo"] != DBNull.Value &&
                                reader["ActivoDatosEstudio"] != DBNull.Value &&
                                Convert.ToBoolean(reader["ActivoDatosEstudio"]) && // Convertimos el bit a booleano
                                !datosAcademicosRecuperados.Any(i => i.Id == Convert.ToInt32(reader["IdDatosEstudio"])))
                            {
                                datosAcademicosRecuperados.Add(new DatosAcademicos
                                {
                                    Id = reader["IdDatosEstudio"] != DBNull.Value ? Convert.ToInt32(reader["IdDatosEstudio"]) : 0,
                                    NombreTitulo = reader["NombreTitulo"]?.ToString(),
                                    Cedula = reader["Cedula"]?.ToString(),
                                    IdNivelAcademico = reader["IdNivelAcademico"] != DBNull.Value ? Convert.ToInt32(reader["IdNivelAcademico"]) : 0,
                                    IdEstatusEstudio = reader["IdEstatusEstudio"] != DBNull.Value ? Convert.ToInt32(reader["IdEstatusEstudio"]) : 0,
                                    IdDocumento = reader["IdDocumentoDatosEstudio"] != DBNull.Value ? new Documento { Id = Convert.ToInt32(reader["IdDocumentoDatosEstudio"]), URL = reader["DocumentoURLDatosEstudio"]?.ToString() } : null,
                                    Activo = Convert.ToBoolean(reader["ActivoDatosEstudio"]) ? 1 : 0, // Guardar como int si es necesario
                                });

                            }
                            
                            // Construir y agregar las experiencias laborales asociadas
                            if (reader["IdExperiencia"] != DBNull.Value &&
                                reader["ActivoExperienciasLaborales"] != DBNull.Value &&
                                Convert.ToBoolean(reader["ActivoExperienciasLaborales"]) && // Convertimos el bit a booleano
                                !experienciasRecuperadas.Any(i => i.Id == Convert.ToInt32(reader["IdExperiencia"])))
                            {
                                experienciasRecuperadas.Add(new ExperienciasLaborales
                                {
                                    Id = reader["IdExperiencia"] != DBNull.Value ? Convert.ToInt32(reader["IdExperiencia"]) : 0,
                                    Descripcion = reader["ExperienciaDescripcion"]?.ToString(),
                                    FechaInicio = reader["ExperienciaFechaInicio"] != DBNull.Value ? Convert.ToDateTime(reader["ExperienciaFechaInicio"]) : DateTime.MinValue,
                                    FechaTermino = reader["ExperienciaFechaTermino"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ExperienciaFechaTermino"]) : null,
                                    NombreEmpleador = reader["ExperienciaNombreEmpleador"]?.ToString(),
                                    CargoOcupado = reader["ExperienciaCargoOcupado"]?.ToString(),
                                    IdDocumento = reader["IdDocumentoExperienciasLaborales"] != DBNull.Value ? new Documento { Id = Convert.ToInt32(reader["IdDocumentoExperienciasLaborales"]), URL = reader["DocumentoURLExperienciasLaborales"]?.ToString() } : null,
                                    IdEstatusTipoExperiencia = reader["IdEstatusTipoExperiencia"] != DBNull.Value ? Convert.ToInt32(reader["IdEstatusTipoExperiencia"]) : 0,
                                    Activo = Convert.ToBoolean(reader["ActivoExperienciasLaborales"]) ? 1 : 0, // Guardar como int si es necesario
                                });
                            }

                            // Construir y agregar los documentos asociados
                        if (reader["IdDocumento"] != DBNull.Value && 
                            reader["ActivoDocumentos"] != DBNull.Value &&
                            Convert.ToBoolean(reader["ActivoDocumentos"]) && // Convertimos el bit a booleano
                            !documentosRecuperados.Any(d => d.Id == Convert.ToInt32(reader["IdDocumento"])))
                        {
                            documentosRecuperados.Add(new Documento
                            {
                                Id = reader["IdDocumento"] != DBNull.Value ? Convert.ToInt64(reader["IdDocumento"]) : 0,
                                URL = reader["DireccionDocumento"]?.ToString(),
                                IdTipoDocumento = reader["IdTipoDocumento"] != DBNull.Value ? Convert.ToInt32(reader["IdTipoDocumento"]) : 0,
                            });
                        }


                        }

                    }

                    //Si documentosRecuperados esta vacio, se le agrega uno vacio para que se pueda agregar un nuevo documento
                    if (documentosRecuperados.Count <= 0)
                    {
                        documentosRecuperados.Add(new Documento());
                    }

                }
                catch (Exception ex){
                    _logger.LogError($"Error al obtener los detalles del usuario: {ex.Message}");
                }
                finally 
                {
                    sqlConexion.Desconectar();
                }
            }

            return usuarioDetalle;
        }


        private UsuarioDetalle ConstruirUsuarioDetalle(DataTableReader reader)
        {
            return new UsuarioDetalle
            {
                Hijos = reader["Hijos"] as int?,
                ViviendaPropia = reader["ViviendaPropia"] as bool? ?? false,
                VehiculoPropio = reader["VehiculoPropio"] as bool? ?? false,
                RentaCasa = reader["RentaCasa"] as bool? ?? false,
                Deudas = reader["Deudas"] as bool? ?? false,
                Mascotas = reader["Mascotas"] as bool? ?? false,
                IngresosMensuales = reader["IngresosMensuales"] != DBNull.Value ? Convert.ToDecimal(reader["IngresosMensuales"]) : (decimal?)null,
                GastoMensual = reader["GastoMensual"] != DBNull.Value ? Convert.ToDecimal(reader["GastoMensual"]) : (decimal?)null,
                FrecuenciaActividadesRecreativas = reader["FrecuenciaActividadesRecreativas"] as int?,
                PersonasVivenCasa = reader["PersonasVivenCasa"] as int?,
                PersonasDependientes = reader["PersonasDependientes"] as int?,
                Domicilio = reader["Domicilio"]?.ToString(),
                IdDocumento = reader["IdDocumentoDatosPersonales"] != DBNull.Value ? new Documento { Id = Convert.ToInt32(reader["IdDocumentoDatosPersonales"]), URL = reader["DocumentoURLFotoPerfil"]?.ToString(), IdTipoDocumento = reader["IdTipoDocumento"] != DBNull.Value ? Convert.ToInt32(reader["IdTipoDocumento"]) : 0 } : new Documento { Id = 0 },
                NSS = reader["NSS"]?.ToString(),
                Celular = reader["Celular"]?.ToString(),
                CorreoElectronico = reader["CorreoElectronico"]?.ToString(),
                Alergia = reader["Alergia"] as bool? ?? false,
                ConsumeMedicamentos = reader["ConsumeMedicamentos"] as bool? ?? false,
                EnfermedadCronica = reader["EnfermedadCronica"] as bool? ?? false,
                DetalleAlergia = reader["DetalleAlergia"]?.ToString(),
                DetalleMedicamentos = reader["DetalleMedicamentos"]?.ToString(),
                FrecuenciaMedica = reader["FrecuenciaMedica"] as int?,
                DetalleEnfermedad = reader["DetalleEnfermedad"]?.ToString(),
                IdTipoSanguineo = reader["IdTipoSanguineo"] as int?,
                NombreTipo = reader["NombreTipo"]?.ToString(),
                NombreContactoEmergencia = reader["NombreContactoEmergencia"]?.ToString(),
                CelularContactoEmergencia = reader["CelularContactoEmergencia"]?.ToString(),
                IdParentesco = reader["IdParentesco"] as int?,
                NombreTitulo = reader["NombreTitulo"]?.ToString(),
                Cedula = reader["Cedula"]?.ToString(),
                NombreNivelAcademico = reader["NombreNivelAcademico"]?.ToString(),
                NombreEstatusEstudio = reader["NombreEstatusEstudio"]?.ToString(),
                Nombre = reader["Nombre"]?.ToString(),
                ApellidoMaterno = reader["ApellidoMaterno"]?.ToString(),
                ApellidoPaterno = reader["ApellidoPaterno"]?.ToString(),
                FechaNacimiento = reader["FechaNacimiento"] as DateTime?,
                RFC = reader["RFC"]?.ToString(),
                CURP = reader["CURP"]?.ToString(),
                Nick = reader["Nick"]?.ToString(),
                NombreGenero = reader["NombreGenero"]?.ToString(),
                NombreSexo = reader["NombreSexo"]?.ToString(),
                NombreEstado = reader["NombreEstado"]?.ToString(),
                NombreEstatus = reader["NombreEstatus"]?.ToString(),
                ExperienciaDescripcion = reader["ExperienciaDescripcion"]?.ToString(),
                ExperienciaFechaInicio = reader["ExperienciaFechaInicio"]?.ToString(),
                ExperienciaFechaTermino = reader["ExperienciaFechaTermino"]?.ToString(),
                ExperienciaNombreEmpleador = reader["ExperienciaNombreEmpleador"]?.ToString(),
                ExperienciaCargoOcupado = reader["ExperienciaCargoOcupado"]?.ToString(),
                NombreTipoExperiencia = reader["NombreTipoExperiencia"]?.ToString(),
                FechaInicioContrato = reader["FechaInicioContrato"] as DateTime?,
                HorarioSalidaTurno = reader["HorarioSalidaTurno"] as DateTime?,
                FechaIngreso = reader["FechaIngreso"] as DateTime?,
                HorarioEntradaTurno = reader["HorarioEntradaTurno"] as DateTime?,
                FechaTerminoContrato = reader["FechaTerminoContrato"] as DateTime?,
                NombreTipoContrato = reader["NombreTipoContrato"]?.ToString(),
                NombreDepartamento = reader["NombreDepartamento"]?.ToString(),
                DescripcionDepartamento = reader["DescripcionDepartamento"]?.ToString(),
            };
        }


        public class DomicilioDetalle
        {
            public int? idPais { get; set; }
            public string? Pais { get; set; }
            public int? idEstado { get; set; }
            public string? Estado { get; set; }
            public int? idMunicipio { get; set; }
            public string? Municipio { get; set; }
            public int? idColonia { get; set; }
            public string? Colonia { get; set; }
            public string? Calle { get; set; }
            public string? Numero { get; set; }
        }


        public List<Idiomas> ConstruirIdiomas(DataTableReader reader)
        {
            var idiomas = new List<Idiomas>();

            while (reader.Read())
            {
                idiomas.Add(new Idiomas
                {
                    Nombre = reader["NombreIdioma"]?.ToString(),
                    NivelEscrito = reader["NivelEscrito"] != DBNull.Value ? Convert.ToInt32(reader["NivelEscrito"]) : 0,
                    NivelHablado = reader["NivelHablado"] != DBNull.Value ? Convert.ToInt32(reader["NivelHablado"]) : 0,
                });
            }

            return idiomas;
        }


    

        //-----------------------------------Metodos para traer selects primera carga de la pagina
        private void GetPaises(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetPaisesCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            paises.Add(construirPais(dtr));
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

        private Pais construirPais(DataTableReader pais){
            return new Pais ()
            {
                Id = int.Parse(pais["Id"].ToString()),
                Nombre = pais["Nombre"].ToString()
            };
        }


        private void GetTipoSangre(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTipoSangreCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposSangre.Add(construirTipoSangre(dtr));
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


        private TipoSangre construirTipoSangre(DataTableReader tipoSangre){
            return new TipoSangre ()
            {
                Id = int.Parse(tipoSangre["Id"].ToString()),
                Nombre = tipoSangre["Nombre"].ToString()
            };
        }


        private void GetParentescos(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetParentescosCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            parentescos.Add(construirParentesco(dtr));
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


        private Parentesco construirParentesco(DataTableReader parentesco){
            return new Parentesco ()
            {
                Id = int.Parse(parentesco["Id"].ToString()),
                Nombre = parentesco["Nombre"].ToString()
            };
        }
        

        public void GetNivelesAcademicos(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetNivelesAcademicosCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            nivelesAcademicos.Add(construirNivelAcademico(dtr));
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

        public NivelesAcademicos construirNivelAcademico(DataTableReader nivelAcademico){
            return new NivelesAcademicos ()
            {
                Id = int.Parse(nivelAcademico["Id"].ToString()),
                Nombre = nivelAcademico["Nombre"].ToString()
            };
        }

        public void GetEstatusEstudio(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetEstatusEstudioCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            estatusEstudio.Add(construirEstatusEstudio(dtr));
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

        public EstatusEstudio construirEstatusEstudio(DataTableReader estatusEstudio){
            return new EstatusEstudio ()
            {
                Id = int.Parse(estatusEstudio["Id"].ToString()),
                Nombre = estatusEstudio["Nombre"].ToString()
            };
        }

        public void GetTiposExperiencias(){
            
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                     using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTiposExperienciasCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposExperiencias.Add(construirTipoExperiencia(dtr));
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

        public TiposExperiencias construirTipoExperiencia(DataTableReader tipoExperiencia){
            return new TiposExperiencias ()
            {
                Id = int.Parse(tipoExperiencia["Id"].ToString()),
                Nombre = tipoExperiencia["Nombre"].ToString()
            };
        }


        //---------------------------Para peticiones ajax, son los selects dinamicos 
        public JsonResult OnGetEstados(int paisId)
        {

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdPais", paisId));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetEstadosPaisCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            estados.Add(construirEstado(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return new JsonResult(estados);
        }

        private Estado construirEstado(DataTableReader estado)
        {
            return new Estado()
            {
                Id = int.Parse(estado["Id"].ToString()),
                Nombre = estado["Nombre"].ToString()
            };
        }

        public JsonResult OnGetMunicipios(int estadoId)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdEstado", estadoId));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetMunicipiosEstadoCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            municipios.Add(construirMunicipio(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return new JsonResult(municipios);
        }

        private Municipio construirMunicipio(DataTableReader municipio)
        {
            return new Municipio()
            {
                Id = int.Parse(municipio["Id"].ToString()),
                Nombre = municipio["Nombre"].ToString()
            };
        }


        public JsonResult OnGetColonias(int municipioId)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdMunicipio", municipioId));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetColoniasMunicipioCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            colonias.Add(construirColonia(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return new JsonResult(colonias);
        }

        private Colonia construirColonia(DataTableReader colonia)
        {
            return new Colonia()
            {
                Id = int.Parse(colonia["Id"].ToString()),
                Nombre = colonia["Nombre"].ToString()
            };
        }


        //----------------------------------------------Funciones propias----------------------------------------------
        public int subirDocumento(IFormFile archivo, int idTipoDocumento, long idUsuario, string nombreCarpetaGuardar, SqlServer sqlConexion, SqlTransaction transaction)
        {
            var filePath = "";
            int? idDocumento = null;

            try
            {
                if (archivo != null)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/uploads/{nombreCarpetaGuardar}");
                    Directory.CreateDirectory(uploadsPath); // Asegura que la ruta existe

                    // Usa FileName para obtener el nombre del archivo
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(archivo.FileName)}";
                    filePath = Path.Combine(uploadsPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        archivo.CopyTo(fileStream); // Copia el archivo al destino
                    }

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdTipoDocumento", idTipoDocumento));
                    prmtrs.Add(new SqlParameter("@DireccionDocumento", $"/uploads/{nombreCarpetaGuardar}/{uniqueFileName}"));
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoCrear", prmtrs, transaction))
                    {
                        if (dtr.Read())
                        {
                            //INtenta convertir el valor a entero, si no se puede asigna null
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
                }
            }
            catch (Exception ex)
            {
                //Eliminar el archivo guardado
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _logger.LogError(ex, "Error al subir documento");
            }

            return idDocumento ?? 0;
        }

        public void messageAlert(string MessageTitle, string message, string type, string? pagina)
        {
            TempData["MessageTitle"] = MessageTitle;
            TempData["Message"] = message;
            TempData["MessageType"] = type; // Puede ser 'error', 'info', etc.
            TempData["Pagina"] = pagina; // Puede ser 'error', 'info', etc.
        }


        //Para sacar el nombre de las url
        // Función para extraer el nombre después del primer guion bajo
        public string ObtenerNombreArchivo(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            var fileName = Path.GetFileName(url); // Obtener solo el nombre del archivo
            var firstUnderscoreIndex = fileName.IndexOf('_'); // Buscar el primer guion bajo

            return firstUnderscoreIndex >= 0 
                ? fileName.Substring(firstUnderscoreIndex + 1) // Retornar todo después del primer guion bajo
                : fileName; // Si no hay guion bajo, retornar el nombre completo
        }



        //----------------------------------------------Datos personales----------------------------------------------
        public IActionResult OnPostGuardarDatosPersonales()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {

                SqlTransaction? transaction = null;

                try
                {
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción


                    // Serializar la dirección como JSON
                    string direccionJson = JsonSerializer.Serialize(new
                    {
                        idPais = direccion.Pais,
                        Pais = direccion.NombrePais,
                        idEstado = direccion.Estado,
                        Estado = direccion.NombreEstado,
                        idMunicipio = direccion.Municipio,
                        Municipio = direccion.NombreMunicipio,
                        idColonia = direccion.Colonia,
                        Colonia = direccion.NombreColonia,
                        Calle = direccion.Calle,
                        Numero = direccion.Numero
                    });

                    int idTipoDocumento = 36; //Tipo documento academico
                    long? idDocumento = null;
                    

                    //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                    if(datosPersonales.IdDocumento.Archivo != null){
                        //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                        if (datosPersonales.IdDocumento.Id < 0 || datosPersonales.IdDocumento.Id == 0)
                        {
                            idDocumento = subirDocumento(datosPersonales.IdDocumento.Archivo, idTipoDocumento, idUsuarioActual, "fotosPerfil", sqlConexion, transaction);
                            if (idDocumento == 0)
                            {
                                throw new Exception($"No se pudo guardar el documento en la ruta");
                            }
                            //Elimina el documento anterior si es que existia
                            if(datosPersonales.IdDocumento.Id < 0){//quiere decir que si existia
                            
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", (datosPersonales.IdDocumento.Id*-1)) //LO convertimos a positivo
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            }
                        }
                        else //Si viene en posityivo y no lo tengo que cambiar solo lo asigno
                        {
                            idDocumento = datosPersonales.IdDocumento.Id;
                        }
                    } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                        if (datosPersonales.IdDocumento.Eliminado){
                            // Eliminar el documento asociado al datosPersonales
                            var parametrosDocumento = new List<SqlParameter>
                            {
                                new SqlParameter("@IdDocumento", datosPersonales.IdDocumento.Id)
                            };

                            idDocumento = null; //Si se elimino el documento asigno null para despues que haga la actualizacion

                            sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                        } else {
                            if (datosPersonales.IdDocumento.Id > 0){ //Si recibo un id es porque si habia un archivo
                                idDocumento = datosPersonales.IdDocumento.Id; //Si no se elimino lo mantengo
                            }
                        }
                    } 
                      

    
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Celular", datosPersonales.Celular));
                    prmtrs.Add(new SqlParameter("@DireccionJson", direccionJson));
                    prmtrs.Add(new SqlParameter("@NSS", datosPersonales.NSS));
                    prmtrs.Add(new SqlParameter("@CorreoElectronico", datosPersonales.CorreoElectronico));
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                    prmtrs.Add(new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value)); //Si es null se envia como DBNull


                   //Compropbar si el usuario ya tiene datos personales para en lugar de crearlos actualizarlos
                    List<System.Data.SqlClient.SqlParameter> prmtrsBuscar = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrsBuscar.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosPersonalesBuscarWithIdUsuario", prmtrsBuscar, transaction))
                    {
                        if (dtr.Read())
                        {
                            sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosPersonalesEditar", prmtrs, transaction);

                            messageAlert("¡Éxito!", "¡Datos personales actualizados correctamente!", "success", "DatosPersonales");
                            transaction.Commit();

                            return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                        }
                    }

                    // Si no se encontraron datos personales, crearlos
                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosPersonalesCrear", prmtrs, transaction);

                    messageAlert("¡Éxito!", "¡Datos personales registrados correctamente!", "success", "DatosPersonales");
                    transaction.Commit();
                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error

                    _logger.LogError(ex, "Error al guardar datos personales");
                    messageAlert("¡Error!", "¡Ocurrió un error al guardar los datos personales!", "error", "DatosPersonales");
                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        //----------------------------------------------Datos médicos----------------------------------------------

        public IActionResult OnPostGuardarDatosMedicos()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                int? idContactoEmergencia = null;

                SqlTransaction? transaction = null;


                try
                {
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción
                    bool existe = false;

                    List<System.Data.SqlClient.SqlParameter> prmtrsBuscar = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrsBuscar.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosMedicosBuscarWithIdUsuario", prmtrsBuscar, transaction))
                    {
                        if (dtr.Read())
                        {
                            existe = true;
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["IdContactoEmergencia"]?.ToString(), out int result))
                            {
                                idContactoEmergencia = result;
                            }
                        }
                    }

                        // Primero guardamos los datos del contacto de emergencia
                    List<System.Data.SqlClient.SqlParameter> prmtrsContacto = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrsContacto.Add(new SqlParameter("@Nombre", contactoEmergencia.Nombre));
                    prmtrsContacto.Add(new SqlParameter("@Telefono", contactoEmergencia.Telefono));
                    prmtrsContacto.Add(new SqlParameter("@IdParentesco", contactoEmergencia.IdParentesco));


                    if(existe){
                        prmtrsContacto.Add(new SqlParameter("@IdContactoEmergencia", idContactoEmergencia));
                        sqlConexion.EjecutarReaderStoreProcedure("dbo.ContactoEmergenciaEditar", prmtrsContacto, transaction);
                    } else {

                        // Ejecutar el procedimiento almacenado y obtener el ID generado
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ContactoEmergenciaCrear", prmtrsContacto, transaction))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["NuevoId"]?.ToString(), out int result))
                                {
                                    idContactoEmergencia = result;
                                }
                            }
                        }

                    }

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdTipoSanguineo", datosMedicos.IdTipoSanguineo));
                    prmtrs.Add(new SqlParameter("@Alergias", datosMedicos.Alergias));
                    prmtrs.Add(new SqlParameter("@DetalleAlergia", (datosMedicos.Alergias) ? datosMedicos.DetalleAlergia : null));
                    prmtrs.Add(new SqlParameter("@EnfermedadCronica", datosMedicos.EnfermedadCronica));
                    prmtrs.Add(new SqlParameter("@DetalleEnfermedad", (datosMedicos.EnfermedadCronica) ? datosMedicos.DetalleEnfermedad : null));
                    prmtrs.Add(new SqlParameter("@ConsumeMedicamentos", datosMedicos.ConsumeMedicamentos));
                    prmtrs.Add(new SqlParameter("@DetalleMedicamentos", (datosMedicos.ConsumeMedicamentos) ? datosMedicos.DetalleMedicamentos : null));
                    prmtrs.Add(new SqlParameter("@IdContactoEmergencia", idContactoEmergencia));     
                    prmtrs.Add(new SqlParameter("@FrecuenciaMedica", datosMedicos.FrecuenciaMedica));
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));


                    if (existe){

                        sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosMedicosEditar", prmtrs, transaction);
                        messageAlert("¡Éxito!", "¡Datos médicos actualizados correctamente!", "success", "DatosMedicos");

                    } else {

                        sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosMedicosCrear", prmtrs, transaction);
                        messageAlert("¡Éxito!", "¡Datos médicos registrados correctamente!", "success", "DatosMedicos");

                    }

                    transaction.Commit();
                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });

                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error

                    _logger.LogError(ex, "Error al guardar datos médicos");

                    messageAlert("¡Error!", "¡Ocurrió un error al guardar los datos médicos!", "error", "DatosMedicos");

                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        //----------------------------------------------Idiomas----------------------------------------------

        public IActionResult OnPostAgregarIdioma()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    foreach (var idioma in idiomas)
                    {
                        //Si un idioma tien id 0 es porque es nuevo entonces se crea, si tiene un id mayor a 0 es porque ya existe y se actualiza y si tiene negativo es porque se elimino
                        if (idioma.Id == 0)
                        {
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                            prmtrs.Add(new SqlParameter("@Nombre", idioma.Nombre));
                            prmtrs.Add(new SqlParameter("@NivelEscrito", idioma.NivelEscrito));
                            prmtrs.Add(new SqlParameter("@NivelHablado", idioma.NivelHablado));
                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                            sqlConexion.EjecutarReaderStoreProcedure("dbo.IdiomasCrear", prmtrs);
                        }
                        else if (idioma.Id > 0)
                        {
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                            prmtrs.Add(new SqlParameter("@IdIdioma", idioma.Id));
                            prmtrs.Add(new SqlParameter("@Nombre", idioma.Nombre));
                            prmtrs.Add(new SqlParameter("@NivelEscrito", idioma.NivelEscrito));
                            prmtrs.Add(new SqlParameter("@NivelHablado", idioma.NivelHablado));
                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                            sqlConexion.EjecutarReaderStoreProcedure("dbo.IdiomasEditar", prmtrs);
                        }
                        else if (idioma.Id < 0)
                        {
                            //convertir el numero negativo a positivo
                            idioma.Id = idioma.Id * -1;
                            List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                            prmtrs.Add(new SqlParameter("@IdIdioma", idioma.Id));
                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                            sqlConexion.EjecutarReaderStoreProcedure("dbo.IdiomasEliminar", prmtrs);
                        }
                    }

                    messageAlert("¡Éxito!", "¡Operacion realizada exitosamente!", "success", "Idiomas");

                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
               
                catch(Exception ex) {
                    _logger.LogError(ex, "Error al guardar idiomas");

                    messageAlert("¡Error!", "¡Ocurrió un error al guardar los idiomas!", "error", "Idiomas");

                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });              
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }         
        }


        //----------------------------------------------Datos académicos----------------------------------------------


        public async Task<IActionResult> OnPostGuardarDatosAcademicos()
        {
            using var sqlConexion = new SqlServer();

            SqlTransaction? transaction = null;

            try
            {
                sqlConexion.Conectar(conexion);
                transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción
                
                int idTipoDocumento = 3; //Tipo documento academico
                long? idDocumento = null;

                foreach (var estudio in estudios)
                {    
                    idDocumento = null;   

                    //Si un estudio tien id 0 es porque es nuevo entonces se crea, si tiene un id mayor a 0 es porque ya existe y se actualiza y si tiene negativo es porque se elimino
                    if (estudio.Id == 0)
                    {
                        if (estudio.IdDocumento != null && estudio.IdDocumento.Archivo != null)
                        {
                            idDocumento = subirDocumento(estudio.IdDocumento.Archivo, idTipoDocumento, idUsuarioActual, "datosAcademicos", sqlConexion, transaction);
                            if (idDocumento == 0)
                            {
                                throw new Exception($"No se pudo guardar el documento en la ruta");
                            }
                        }
                    
                        // Guardar los datos del estudio, incluyendo el ID del documento
                        var parametrosEstudio = new List<SqlParameter>
                        {
                            new SqlParameter("@NombreTitulo", estudio.NombreTitulo),
                            new SqlParameter("@Cedula", estudio.Cedula ?? (object)DBNull.Value),
                            new SqlParameter("@IdNivelAcademico", estudio.IdNivelAcademico),
                            new SqlParameter("@IdEstatusEstudio", estudio.IdEstatusEstudio),
                            new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value),
                            new SqlParameter("@IdUsuario", idUsuarioActual)
                        };

                        sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosEstudioCrear", parametrosEstudio, transaction);
                    }
                    else if (estudio.Id > 0)
                    {
                        //Si hay un documento
                        if (estudio.IdDocumento != null)
                        {
                            //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                            if(estudio.IdDocumento.Archivo != null){
                                //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                                if (estudio.IdDocumento.Id < 0 || estudio.IdDocumento.Id == 0)
                                {
                                    idDocumento = subirDocumento(estudio.IdDocumento.Archivo, idTipoDocumento, idUsuarioActual, "datosAcademicos", sqlConexion, transaction);
                                    if (idDocumento == 0)
                                    {
                                        throw new Exception($"No se pudo guardar el documento en la ruta");
                                    }
                                    //Elimina el documento anterior si es que existia
                                    if(estudio.Id < 0){//quiere decir que si existia
                                    
                                        var parametrosDocumento = new List<SqlParameter>
                                        {
                                            new SqlParameter("@IdDocumento", (estudio.Id*-1)) //LO convertimos a positivo
                                        };

                                        sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                    }
                                }
                                else //Si viene en posityivo y no lo tengo que cambiar solo lo asigno
                                {
                                    idDocumento = estudio.IdDocumento.Id;
                                }
                            } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                                if (estudio.IdDocumento.Eliminado){
                                    // Eliminar el documento asociado al estudio
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", estudio.IdDocumento.Id)
                                    };

                                    idDocumento = null; //Si se elimino el documento asigno null para despues que haga la actualizacion

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                } else {
                                    if (estudio.IdDocumento.Id > 0){ //Si recibo un id es porque si habia un archivo
                                        idDocumento = estudio.IdDocumento.Id; //Si no se elimino lo mantengo
                                    }
                                }
                            }


                        }

                        // Actualizar los datos del estudio, incluyendo el ID del documento
                        var parametrosEstudio = new List<SqlParameter>
                        {
                            new SqlParameter("@IdEstudio", estudio.Id),
                            new SqlParameter("@NombreTitulo", estudio.NombreTitulo),
                            new SqlParameter("@Cedula", estudio.Cedula ?? (object)DBNull.Value),
                            new SqlParameter("@IdNivelAcademico", estudio.IdNivelAcademico),
                            new SqlParameter("@IdEstatusEstudio", estudio.IdEstatusEstudio),
                            new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value),
                            new SqlParameter("@IdUsuario", idUsuarioActual)
                        };

                        sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosEstudioEditar", parametrosEstudio, transaction);
                    }
                    else if (estudio.Id < 0)
                    {
                        //convertir el numero negativo a positivo
                        estudio.Id = estudio.Id * -1;

                        // Eliminar los datos del estudio
                        var parametrosEstudio = new List<SqlParameter>
                        {
                            new SqlParameter("@IdEstudio", estudio.Id),
                            new SqlParameter("@IdUsuario", idUsuarioActual)
                        };

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosEstudioEliminar", parametrosEstudio, transaction))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                                {
                                    idDocumento = result;
                                }

                                if (idDocumento != null)
                                {
                                    // Eliminar el documento asociado al estudio
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", idDocumento)
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                }
                            }
                        }

                    }

                }
                
                transaction.Commit();

                messageAlert("¡Éxito!", "¡Operación realizada exitosamente!", "success", "DatosAcademicos");  
                
                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });

            }
            catch (Exception ex)
            {
                transaction?.Rollback(); // Revertir en caso de error

                _logger.LogError(ex, "Error al guardar datos académicos");

                messageAlert("¡Error!", "¡Ocurrió un error al guardar los datos académicos!", "error", "DatosAcademicos");

                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
            }
            finally
            {
                sqlConexion.Desconectar();
            }
        }

        //----------------------------------------------Documentación----------------------------------------------
        public IActionResult OnPostGuardarDocumentacion()
        {
            using var sqlConexion = new SqlServer();

            SqlTransaction? transaction = null;

            try
            {
                sqlConexion.Conectar(conexion);
                transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción

                long? idDocumento = 0;

                // Lista de documentos con sus propiedades, tipos y rutas
                var documentos = new List<(Documento? doc, int tipo, string ruta)>
                {
                    (documentacion.CURP, 1, "documentacion/CURP"),
                    (documentacion.RFC, 11, "documentacion/RFC"),
                    (documentacion.ActaNacimiento, 8, "documentacion/ActaNacimiento"),
                    (documentacion.ComprobanteDomicilio, 14, "documentacion/ComprobanteDomicilio"),
                    (documentacion.ComprobanteSitacionFiscal, 10, "documentacion/ComprobanteSituacionFiscal"),
                    (documentacion.INE, 9, "documentacion/INE"),
                    (documentacion.CartaSolicitudRegimenHonorarios, 20, "documentacion/CartaSolicitudRegimenHonorarios"),
                    (documentacion.CV, 15, "documentacion/CV"),
                    (documentacion.EstadoCuenta, 17, "documentacion/EstadoCuenta"),
                    (documentacion.SolicitudEmpleo, 16, "documentacion/SolicitudEmpleo"),
                    (documentacion.ComprobanteVigenciaDerechos, 13, "documentacion/ComprobanteVigenciaDerechos"),
                    (documentacion.FolioBeca, 33, "documentacion/FolioBeca"),  
                    (documentacion.ComprobanteEstudios, 12, "documentacion/ComprobanteEstudios"), 
                };

                // Procesa cada documento en la lista
                foreach (var (doc, tipo, ruta) in documentos)
                {

                    //Si hay un documento
                    if (doc!= null)
                    {
                        //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                        if(doc.Archivo != null){
                            //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                            if (doc.Id < 0 || doc.Id == 0)
                            {
                                idDocumento = subirDocumento(doc.Archivo, tipo, idUsuarioActual, ruta, sqlConexion, transaction);
                                if (idDocumento == 0)
                                {
                                    throw new Exception($"No se pudo guardar el documento en la ruta");
                                }
                                //Elimina el documento anterior si es que existia
                                if(doc.Id < 0){//quiere decir que si existia
                                
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", (doc.Id*-1)) //LO convertimos a positivo
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                }
                            }
                            else //no lo tengo que cambiar solo lo asigno
                            {
                                idDocumento = doc.Id;
                            }
                        } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                            if (doc.Eliminado){
                                // Eliminar el documento asociado al doc
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", doc.Id)
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            } else {
                                if (doc.Id > 0){ //Si recibo un id es porque si habia un archivo
                                    idDocumento = doc.Id; //Si no se elimino lo mantengo
                                }
                            }
                        }
                    }  

                }

                transaction.Commit();

                messageAlert("¡Éxito!", "¡Documentación registrada correctamente!", "success", "DocumentacionIngreso");
                
                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });

            }
            catch (Exception ex)
            {
                transaction?.Rollback(); // Revertir en caso de error

                _logger.LogError(ex, "Error al guardar documentación");

                messageAlert("¡Error!", "¡Ocurrió un error al guardar la documentación!", "error", "DocumentacionIngreso");
                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
            }
            finally
            {
                sqlConexion.Desconectar();
            }
        }


        //----------------------------------------------Documentación expediente----------------------------------------------

 public IActionResult OnPostGuardarDocumentacionExpediente()
        {
            using var sqlConexion = new SqlServer();
            SqlTransaction? transaction = null;

            try
            {
                sqlConexion.Conectar(conexion);
                transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción

                long idDocumento = 0;

                // Procesar documentos únicos
                var documentosUnicos = new List<(Documento? doc, int tipo, string ruta)>
                {
                    (documentacionExpediente.ContratoPrestacionServicios, 21, "documentacionExpediente/ContratoPrestacionServicios"),
                    (documentacionExpediente.FormularioIngreso, 22, "documentacionExpediente/FormularioIngreso"),
                    (documentacionExpediente.FormatoInformacionGeneral, 23, "documentacionExpediente/FormatoInformacionGeneral"),
                    (documentacionExpediente.FormatoReclutamiento, 24, "documentacionExpediente/FormatoReclutamiento"),
                    (documentacionExpediente.ContratoConfidencialidad, 25, "documentacionExpediente/ContratoConfidencialidad"),
                    (documentacionExpediente.EvaluacionCompetencias, 26, "documentacionExpediente/EvaluacionCompetencias"),
                };

                foreach (var (doc, tipo, ruta) in documentosUnicos)
                {
                    //Si hay un documento
                    if (doc!= null)
                    {
                        //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                        if(doc.Archivo != null){
                            //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                            if (doc.Id < 0 || doc.Id == 0)
                            {
                                idDocumento = subirDocumento(doc.Archivo, tipo, idUsuarioActual, ruta, sqlConexion, transaction);
                                if (idDocumento == 0)
                                {
                                    throw new Exception($"No se pudo guardar el documento en la ruta");
                                }
                                //Elimina el documento anterior si es que existia
                                if(doc.Id < 0){//quiere decir que si existia
                                
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", (doc.Id*-1)) //LO convertimos a positivo
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                }
                            }
                            else //no lo tengo que cambiar solo lo asigno
                            {
                                idDocumento = doc.Id;
                            }
                        } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                            if (doc.Eliminado){
                                // Eliminar el documento asociado al doc
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", doc.Id)
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            } else {
                                if (doc.Id > 0){ //Si recibo un id es porque si habia un archivo
                                    idDocumento = doc.Id; //Si no se elimino lo mantengo
                                }
                            }
                        }
                    }  
                }

                // Procesar documentos múltiples
                var documentosMultiples = new List<(List<Documento> docs, int tipo, string ruta)>
                {
                    (documentacionExpediente.JustificacionPermiso, 29, "documentacionExpediente/JustificacionPermiso"),
                    (documentacionExpediente.ConstanciaCursosTalleres, 30, "documentacionExpediente/ConstanciaCursosTalleres"),
                    (documentacionExpediente.PruebasPsicometricas, 27, "documentacionExpediente/PruebasPsicometricas"),
                    (documentacionExpediente.ActasAdministrativas, 28, "documentacionExpediente/ActasAdministrativas"),
                };


                /*TENGO QUE HACER QUE EL UNICO INPUT PUEDA SUBIR MULTIPLES ARCHIVOS*/
                foreach (var (docs, tipo, ruta) in documentosMultiples)
                {
                    foreach (var doc in docs)
                    {
                        //Si hay un documento
                        if (doc!= null)
                        {
                            //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                            if(doc.Archivo != null){

                                //Codigo para subir multiples documentos de un solo input
                                //quitandole al string ruta de la lista lo que esta antes de la ultima /
                                var rutaSinNombreCarpeta = ruta.Substring(ruta.LastIndexOf("/") + 1);

                                // Filtrar los archivos enviados para esta propiedad
                                var archivos = Request.Form.Files
                                    .Where(f => doc?.Archivo != null && f.Name == nameof(documentacionExpediente) + "." + rutaSinNombreCarpeta + "[0].Archivo")
                                    .ToList();

                                foreach (var archivo in archivos)
                                {
                                    var nuevoDocumento = new Documento
                                    {
                                        Archivo = archivo,
                                        IdTipoDocumento = tipo,
                                        IdUsuario = idUsuarioActual,
                                        URL = ruta
                                    };

                                    idDocumento = subirDocumento(nuevoDocumento.Archivo, nuevoDocumento.IdTipoDocumento, nuevoDocumento.IdUsuario, nuevoDocumento.URL, sqlConexion, transaction);
                                    if (idDocumento == 0)
                                    {
                                        throw new Exception($"No se pudo guardar el archivo en la ruta: {ruta}");
                                    }
                                }

                            } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                                if (doc.Eliminado){
                                    // Eliminar el documento asociado al doc
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", doc.Id)
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                } else {
                                    if (doc.Id > 0){ //Si recibo un id es porque si habia un archivo
                                        idDocumento = doc.Id; //Si no se elimino lo mantengo
                                    }
                                }
                            }
                        }  
                    }
                }

                 
                transaction.Commit();

                messageAlert("¡Éxito!", "¡Documentación registrada correctamente!", "success", "DocumentacionExpediente");
                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
            }
            catch (Exception ex)
            {
                transaction?.Rollback();

                _logger.LogError(ex, "Error al guardar documentación");

                messageAlert("¡Error!", "¡Ocurrió un error al guardar la documentación!", "error", "DocumentacionExpediente");
                return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
            }
            finally
            {
                sqlConexion.Desconectar();
            }
        }

        




//----------------------------------------------Experiencias laborales----------------------------------------------

    public async Task<IActionResult> OnPostGuardarHistorialProfesional()
    {
        using var sqlConexion = new SqlServer();

        SqlTransaction? transaction = null;

        try
        {
            sqlConexion.Conectar(conexion);
            transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción

            int idTipoDocumento = 6; // Tipo documento experiencia laboral
            long? idDocumento = null;

            foreach (var experiencia in experiencias)
            {
                idDocumento = null;

                // Si la experiencia tiene Id = 0, es nueva y debe crearse
                if (experiencia.Id == 0)
                {
                    if (experiencia.IdDocumento != null && experiencia.IdDocumento.Archivo != null)
                    {
                        idDocumento = subirDocumento(experiencia.IdDocumento.Archivo, idTipoDocumento, idUsuarioActual, "experienciasLaborales", sqlConexion, transaction);
                        if (idDocumento == 0)
                        {
                            throw new Exception($"No se pudo guardar el documento en la ruta");
                        }
                    }

                    var parametrosExperiencia = new List<SqlParameter>
                    {
                        new SqlParameter("@NombreEmpleador", experiencia.NombreEmpleador ?? (object)DBNull.Value),
                        new SqlParameter("@CargoOcupado", experiencia.CargoOcupado),
                        new SqlParameter("@Descripcion", experiencia.Descripcion),                       
                        new SqlParameter("@FechaInicio", experiencia.FechaInicio),
                        new SqlParameter("@FechaFin", experiencia.FechaTermino ?? (object)DBNull.Value),
                        new SqlParameter("@IdEstatusTipoExperiencia", experiencia.IdEstatusTipoExperiencia),
                        new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value),
                        new SqlParameter("@IdUsuario", idUsuarioActual)
                    };

                    sqlConexion.EjecutarReaderStoreProcedure("dbo.ExperienciasCrear", parametrosExperiencia, transaction);
                }
                else if (experiencia.Id > 0) // Si la experiencia tiene un Id > 0, se debe actualizar
                {
                    if (experiencia.IdDocumento != null)
                    {
                        if (experiencia.IdDocumento.Archivo != null)
                        {
                            if (experiencia.IdDocumento.Id < 0 || experiencia.IdDocumento.Id == 0) //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)

                            {
                                idDocumento = subirDocumento(experiencia.IdDocumento.Archivo, idTipoDocumento, idUsuarioActual, "experienciasLaborales", sqlConexion, transaction);
                                if (idDocumento == 0)
                                {
                                    throw new Exception($"No se pudo guardar el documento en la ruta");
                                }
                                //Elimina el documento anterior si es que existia
                                if(experiencia.Id < 0){//quiere decir que si existia
                                
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", (experiencia.Id*-1)) //LO convertimos a positivo
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                }
                            }
                            else
                            {
                                idDocumento = experiencia.IdDocumento.Id;
                            }
                        }
                        else if (experiencia.IdDocumento.Eliminado)
                        {
                            var parametrosDocumento = new List<SqlParameter>
                            {
                                new SqlParameter("@IdDocumento", experiencia.IdDocumento.Id)
                            };

                            sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            idDocumento = null; // Documento eliminado
                        }
                        else
                        {
                            if (experiencia.IdDocumento.Id > 0)
                            {
                                idDocumento = experiencia.IdDocumento.Id;
                            }
                        }
                    }

                    var parametrosExperiencia = new List<SqlParameter>
                    {
                        new SqlParameter("@IdExperiencia", experiencia.Id),
                        new SqlParameter("@NombreEmpleador", experiencia.NombreEmpleador ?? (object)DBNull.Value),
                        new SqlParameter("@CargoOcupado", experiencia.CargoOcupado),
                        new SqlParameter("@Descripcion", experiencia.Descripcion),
                        new SqlParameter("@FechaInicio", experiencia.FechaInicio),
                        new SqlParameter("@FechaFin", experiencia.FechaTermino ?? (object)DBNull.Value),
                        new SqlParameter("@IdEstatusTipoExperiencia", experiencia.IdEstatusTipoExperiencia),
                        new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value),
                        new SqlParameter("@IdUsuario", idUsuarioActual)
                    };

                    sqlConexion.EjecutarReaderStoreProcedure("dbo.ExperienciasEditar", parametrosExperiencia, transaction);
                }
                else if (experiencia.Id < 0) // Si la experiencia tiene Id < 0, se debe eliminar
                {
                    experiencia.Id = Math.Abs(experiencia.Id);

                    var parametrosExperiencia = new List<SqlParameter>
                    {
                        new SqlParameter("@IdExperiencia", experiencia.Id),
                        new SqlParameter("@IdUsuario", idUsuarioActual)
                    };

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ExperienciasEliminar", parametrosExperiencia, transaction))
                    {
                        if (dtr.Read())
                        {
                            if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                            {
                                idDocumento = result;
                            }

                            if (idDocumento != null)
                            {
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", idDocumento)
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            }
                        }
                    }
                }
            }

            transaction.Commit();

            messageAlert("¡Éxito!", "¡Experiencias laborales registradas correctamente!", "success", "ExperienciasLaborales");
            return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
        }
        catch (Exception ex)
        {
            transaction?.Rollback();
            _logger.LogError(ex, "Error al guardar experiencias laborales");
            messageAlert("¡Error!", "¡Ocurrió un error al guardar las experiencias laborales!", "error", "ExperienciasLaborales");
            return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
        }
        finally
        {
            sqlConexion.Desconectar();
        }
    }

                

        //----------------------------------------------Datos socioeconómicos----------------------------------------------
        public IActionResult OnPostGuardarDatosSocioeconomicos()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();

                    // Asignamos los valores de los campos del modelo "datosSocioeconomicos"
                    prmtrs.Add(new SqlParameter("@ViviendaPropia", datosSocioeconomicos.ViviendaPropia));
                    prmtrs.Add(new SqlParameter("@VehiculoPropio", datosSocioeconomicos.VehiculoPropio));
                    prmtrs.Add(new SqlParameter("@RentaCasa", datosSocioeconomicos.RentaCasa));
                    prmtrs.Add(new SqlParameter("@PersonasVivenCasa", datosSocioeconomicos.PersonasVivenCasa));
                    prmtrs.Add(new SqlParameter("@PersonasDependientes", datosSocioeconomicos.PersonasDependientes));
                    prmtrs.Add(new SqlParameter("@Mascotas", datosSocioeconomicos.Mascotas));
                    prmtrs.Add(new SqlParameter("@IngresosMensuales", datosSocioeconomicos.IngresosMensuales));
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                    prmtrs.Add(new SqlParameter("@Hijos", datosSocioeconomicos.Hijos));
                    prmtrs.Add(new SqlParameter("@GastoMensual", datosSocioeconomicos.GastoMensual));
                    prmtrs.Add(new SqlParameter("@FrecuenciaActividadesRecreativas", datosSocioeconomicos.FrecuenciaActividadesRecreativas));
                    prmtrs.Add(new SqlParameter("@Deudas", datosSocioeconomicos.Deudas));


                    //Compropbar si el usuario ya tiene datos socieconomicos para en lugar de crearlos actualizarlos
                    List<System.Data.SqlClient.SqlParameter> prmtrsBuscar = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrsBuscar.Add(new SqlParameter("@IdUsuario", idUsuarioActual));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosSocioeconomicosBuscarWithIdUsuario", prmtrsBuscar))
                    {
                        if (dtr.Read())
                        {
                            sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosSocioeconomicosEditar", prmtrs);

                            messageAlert("¡Éxito!", "¡Datos socioeconómicos actualizados correctamente!", "success", "DatosSocioeconomicos");
                            return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                        }
                    }

                    // Ejecutamos el procedimiento almacenado usando los parámetros que definimos
                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DatosSocioeconomicosCrear", prmtrs);

                    messageAlert("¡Éxito!", "¡Datos socioeconómicos registrados correctamente!", "success", "DatosSocioeconomicos");

                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar datos socioeconómicos");

                    messageAlert("¡Error!", "¡Ocurrió un error al guardar los datos socioeconómicos!", "error", "DatosSocioeconomicos");
                     
                    return RedirectToPage("/InformacionComplementaria", new { id = idUsuarioActual });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

    }
}