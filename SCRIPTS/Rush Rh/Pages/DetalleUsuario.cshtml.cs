using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace Rush_Rh.Pages
{
    public class DetalleUsuario : PageModel
    {
        private readonly string conexion;
        private readonly ILogger<DetalleUsuario> _logger;

        public UsuarioDetalle Usuario { get; set; }
        public DomicilioDetalle Domicilio { get; set; } // Objeto deserializado del JSON de domicilio.

        public DetalleUsuario(IConfiguration configuration, ILogger<DetalleUsuario> logger)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public void OnGet(int id)
        {
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

        private UsuarioDetalle ObtenerDetallesUsuario(int idUsuario)
        {
            UsuarioDetalle usuarioDetalle = null;
            List<DocumentoDetalle> documentos = new List<DocumentoDetalle>();
            List<Idiomas> idiomas = new List<Idiomas>();

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
                            if (reader["NombreIdioma"] != DBNull.Value)
                            {
                                idiomas.Add(new Idiomas
                                {
                                    Nombre = reader["DireccionDocumento"]?.ToString(),
                                    NivelEscrito = int.Parse(reader["NivelHablado"]?.ToString()),
                                    NivelHablado = int.Parse(reader["NivelEscrito"]?.ToString())
                                });
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
                Domicilio = reader["Domicilio"]?.ToString(), // JSON del domicilio.
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
                NombreTipo = reader["NombreTipo"]?.ToString(),
                NombreContactoEmergencia = reader["NombreContactoEmergencia"]?.ToString(),
                CelularContactoEmergencia = reader["CelularContactoEmergencia"]?.ToString(),
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
                // NombreIdioma = reader["NombreIdioma"]?.ToString(),
                // NivelHablado = reader["NivelHablado"]?.ToString(),
                // NivelEscrito = reader["NivelEscrito"]?.ToString(),
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
    }

    // Modelo para deserializar el JSON de domicilio
    public class DomicilioDetalle
    {
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
    }
}
