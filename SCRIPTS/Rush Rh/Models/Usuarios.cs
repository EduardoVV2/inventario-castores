using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using static Rush_Rh.Models.Formularios; // Assuming Puestos and Departamentos are in the same namespace


namespace Rush_Rh.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Genero { get; set; }
        public string Sexo { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string Nacionalidad { get; set; }
        public string EstadoCivil { get; set; }
        public int Estatus { get; set; }
        public string Nick { get; set; }
    }

    public class LoginUsuario
    {
        public required string Nick { get; set; }
        public required string Password { get; set; }
    }

    public class UsuarioRegistro
    {
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdGenero { get; set; }
        public int IdSexo { get; set; }
        public string RFC { get; set; }
        public string CURP { get; set; }
        public int Nacionalidad { get; set; }
        public string NUE { get; set; }
        public int IdEstadoCivil { get; set; }
        public int IdTipoUsuario { get; set; }

    }

    public class UsuarioGeneral
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CURP { get; set; }
        public string Estatus { get; set; }
        public string Nick { get; set; }
        public string CorreoElectronico { get; set; }
    }

    public class UsuarioSencillo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoMaterno { get; set; }
        public string ApellidoPaterno { get; set; }
        public int IdTipoUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public int IdEstatusUsuario { get; set; }
        public string EstatusUsuario { get; set; }

        public DateTime FechaNacimiento { get; set; }

    }


    public class UsuarioNombre
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
    }


    public class Direccion
    {
        public int Pais { get; set; }
        public string NombrePais { get; set; }
        public int Estado { get; set; }
        public string NombreEstado { get; set; }
        public int Municipio { get; set; }
        public string NombreMunicipio { get; set; }
        public int Colonia { get; set; }
        public string NombreColonia { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
    }


    public class DatosPersonales
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; } //Para los datos en json
        public int NSS { get; set; }
        public string CorreoElectronico { get; set; }
        public Documento? IdDocumento { get; set; }
    }


    public class DatosMedicos
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoSanguineo { get; set; }
        public bool Alergias { get; set; }
        public string? DetalleAlergia { get; set; }
        public bool EnfermedadCronica { get; set; }
        public string? DetalleEnfermedad { get; set; }
        public bool ConsumeMedicamentos { get; set; }
        public string? DetalleMedicamentos { get; set; }
        public int IdContactoEmergencia { get; set; }
        public int FrecuenciaMedica { get; set; }
    }

    public class ContactoEmergencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public int IdParentesco { get; set; }
    }


    public class Idiomas
    {
        public int Id { get; set; }
        public long IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int NivelEscrito { get; set; }
        public int NivelHablado { get; set; }
        public bool Activo { get; set; }

    }


    public class DatosSocioeconomicos
    {
        public bool ViviendaPropia { get; set; }
        public bool VehiculoPropio { get; set; }
        public bool RentaCasa { get; set; }
        public int PersonasVivenCasa { get; set; }
        public int PersonasDependientes { get; set; }
        public bool Mascotas { get; set; }
        public float IngresosMensuales { get; set; }
        public long IdUsuario { get; set; }
        public long IdDatosSocioeconomicos { get; set; }
        public int Hijos { get; set; }
        public float GastoMensual { get; set; }
        public int FrecuenciaActividadesRecreativas { get; set; }
        public bool Deudas { get; set; }
    }


    public class DatosAcademicos
    {
        public long Id { get; set; }
        public string? NombreTitulo { get; set; }
        public string? Cedula { get; set; }
        public Documento? IdDocumento { get; set; }
        public int IdEstatusEstudio { get; set; }
        public int IdNivelAcademico { get; set; }
        public long IdUsuario { get; set; }
        public int Activo { get; set; }
    }

    public class Documento
    {
        //Para guardar un documento
        public long Id { get; set; }
        public IFormFile? Archivo { get; set; }
        public int IdTipoDocumento { get; set; }
        public string URL { get; set; } // Ruta del archivo
        public long IdUsuario { get; set; }
        public bool Eliminado { get; set; }  // Lo utilizo para saber si el docuemento se elimino o no, cuando ya tengo un precargado en el formulario y lo envio de nuevo
    }



    public class ExperienciasLaborales
    {
        public long Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public string? NombreEmpleador { get; set; }
        public string CargoOcupado { get; set; }
        public Documento? IdDocumento { get; set; }
        public int IdEstatusTipoExperiencia { get; set; }
        public long IdUsuario { get; set; }
        public int Activo { get; set; }
    }


    public class Documentacion
    {
        public Documento? CURP { get; set; }
        public Documento? RFC { get; set; }
        public Documento? ActaNacimiento { get; set; }
        public Documento? ComprobanteDomicilio { get; set; }
        public Documento? ComprobanteSitacionFiscal { get; set; }
        public Documento? INE { get; set; }
        public Documento? CartaSolicitudRegimenHonorarios { get; set; }
        public Documento? CV { get; set; }
        public Documento? EstadoCuenta { get; set; }
        public Documento? SolicitudEmpleo { get; set; }
        public Documento? ComprobanteVigenciaDerechos { get; set; }
        public Documento? ComprobanteEstudios { get; set; }
        public Documento? FolioBeca { get; set; }

    }
    public class Cardex
    {
        public long Id { get; set; }
        public int IdOficina { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public long IdUsuario { get; set; }
        public int IdPuesto { get; set; }
        public bool Activo { get; set; }
    }
    public class UsuarioDetalle
    {
        public long Id { get; set; } //Pus el Id del usuario xd
        // Datos socioeconómicos
        public int? Hijos { get; set; }
        public bool ViviendaPropia { get; set; }
        public bool VehiculoPropio { get; set; }
        public bool RentaCasa { get; set; }
        public bool Deudas { get; set; }
        public bool Mascotas { get; set; }
        public decimal? IngresosMensuales { get; set; }
        public decimal? GastoMensual { get; set; }
        public int? FrecuenciaActividadesRecreativas { get; set; }
        public int? PersonasVivenCasa { get; set; }
        public int? PersonasDependientes { get; set; }

        public Documento? IdDocumento { get; set; } //Para la foto de perfil

        public string? Domicilio { get; set; }
        public string? NSS { get; set; }
        public string? Celular { get; set; }
        public string? CorreoElectronico { get; set; }

        // Datos médicos
        public bool Alergia { get; set; }
        public bool ConsumeMedicamentos { get; set; }
        public bool EnfermedadCronica { get; set; }
        public string? DetalleAlergia { get; set; }
        public string? DetalleMedicamentos { get; set; }
        public int? FrecuenciaMedica { get; set; }
        public string? DetalleEnfermedad { get; set; }
        public int? IdTipoSanguineo { get; set; }

        public string? NombreTipo { get; set; }

        public string? NombreContactoEmergencia { get; set; }
        public string? CelularContactoEmergencia { get; set; }
        public int? IdParentesco { get; set; }

        // Datos de estudio
        public string NombreTitulo { get; set; }
        public string Cedula { get; set; }
        public string NombreEstatusEstudio { get; set; }
        public string NombreNivelAcademico { get; set; }

        // Datos de usuario generales
        public string? Nombre { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? ApellidoPaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string RFC { get; set; }
        public string Nick { get; set; }
        public string CURP { get; set; }
        public string NombreGenero { get; set; }
        public string NombreSexo { get; set; }
        public string NombreEstado { get; set; }
        public string NombreEstatus { get; set; }
        public string NUE { get; set; }
        public int IdTipoUsuario { get; set; }
        public int IdEstatusUsuario { get; set; }

        //Datos Experiencias Laborales
        public string ExperienciaDescripcion { get; set; }
        public string ExperienciaFechaInicio { get; set; }
        public string ExperienciaFechaTermino { get; set; }
        public string ExperienciaNombreEmpleador { get; set; }
        public string ExperienciaCargoOcupado { get; set; }
        public string NombreTipoExperiencia { get; set; }

        //Datos de trabajo
        public DateTime? FechaInicioContrato { get; set; }

        public DateTime? HorarioSalidaTurno { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? HorarioEntradaTurno { get; set; }
        public DateTime? FechaTerminoContrato { get; set; }
        public string? NombreTipoContrato { get; set; }
        public string? NombreDepartamento { get; set; }
        public string? DescripcionDepartamento { get; set; }
        public string? DireccionDocumento { get; set; }



    }
    public class DocumentoDetalle
    {
        public string DireccionDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string DescripcionClaseDocumento { get; set; }
    }



    public class DocumentacionExpediente
    {
        public Documento? ContratoPrestacionServicios { get; set; }
        public Documento? FormularioIngreso { get; set; }
        public Documento? FormatoInformacionGeneral { get; set; }
        public Documento? FormatoReclutamiento { get; set; }
        public Documento? ContratoConfidencialidad { get; set; }
        public Documento? EvaluacionCompetencias { get; set; }

        // Para manejar múltiples archivos, añadimos estas propiedades específicas
        public List<Documento> JustificacionPermiso { get; set; } = new List<Documento>();
        public List<Documento> ConstanciaCursosTalleres { get; set; } = new List<Documento>();
        public List<Documento> PruebasPsicometricas { get; set; } = new List<Documento>();
        public List<Documento> ActasAdministrativas { get; set; } = new List<Documento>();
    }

    public class EstatusUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class RegistroNominas
    {
        public int IdUsuario { get; set; }
        public int IdMes { get; set; }
        public int IdDocumento { get; set; }
        public int Año { get; set; }
    }

    public class NominasSubidas
    {
        public int Id { get; set; }
        public string? Mes { get; set; }
        public int IdMes { get; set; }
        public int IdDocumento { get; set; }
        public string URL { get; set; }
        public int Año { get; set; }
    }

    public class Recordatorios
    {
        //Los recordatorios son los avisos para los usuarios pero solo con titulo y fecha
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaEvento { get; set; }

    }

    public class OrganigramaInfo
    {
        public long Id { get; set; }
        public long? IdPadre { get; set; }
        public long? IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public Puestos? IdPuesto { get; set; }
        public Departamentos? IdDepartamento { get; set; }

        public Documento? IdDocumento { get; set; }
        public string? Tipo { get; set; }
    }

    public class UsuariosFotos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaFoto { get; set; }
        public string Nick { get; set; }
    }




    public class RegistroAsistencia
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Time { get; set; }
        [CsvHelper.Configuration.Attributes.Name("Attendance Status")]
        public string AttendanceStatus { get; set; }
    }



    public class Asistencia
    {
        public string Nombre { get; set; }
        public int departamentoId { get; set; }
        public string? departamento { get; set; }
        public int numAsistencias { get; set; }
        public int numRetardos { get; set; }
        public int numFaltas { get; set; }
        public int licencias { get; set; }
        public int diasFestivos { get; set; }
        public int vacaciones { get; set; }
        public long? IdUsuario { get; set; }
        public List<DetalleAsistencia> detalles { get; set; }
    }

    public class DetalleAsistencia
    {
        public int? Id { get; set; }
        public string fecha { get; set; }
        public string tipo { get; set; }
        public int tipoId { get; set; }

        public string motivo { get; set; }


    }


    public class HistorialPaseLista
    {
        public int IdPaseLista { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public int TotalRegistros { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }





    public class TiposPaseLista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }
    public class UsuarioInfoGeneral
    {
        public long? IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? ApellidoPaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? NombreSexo { get; set; }
        public int Edad { get; set; }
        public string? NombreNacionalidad { get; set; }
        public string? CURP { get; set; }
        public string? NombreEstado { get; set; }
        public string? CRF { get; set; }
        public int? NSS { get; set; }
        public string? Domicilio { get; set; }
        public string? CorreoElectronico { get; set; }
        public int? Celular { get; set; }
        public string? NombrePuesto { get; set; }
        public int? ViviendaPropia { get; set; }
        public int? VehiculoPropio { get; set; }
        public int? RentaCasa { get; set; }
        public string? IngresosMensuales { get; set; }
        public int? PersonasDependientes { get; set; }
        public int? Hijos { get; set; }
        public string? RealizaActividadesRecreativas { get; set; }
        public int? FrecuenciaActividadesRecreativas { get; set; }
        public int? Deudas { get; set; }
        public string? GastoMensual { get; set; }
        public int? PersonasVivenCasa { get; set; }
        public int? Mascotas { get; set; }
        public int? Alergia { get; set; }
        public int? EnfermedadCronica { get; set; }
        public string? NombreTipo { get; set; }
        public int? ConsumeMedicamentos { get; set; }
        public string? DetalleMedicamentos { get; set; }
        public int? FrecuenciaMedica { get; set; }
        public string? NombreContactoEmergencia { get; set; }
        public int? CelularContactoEmergencia { get; set; }
        public string? NombreParentesco { get; set; }
        public string? NombreTitulo { get; set; }
        public string? NombreNivelAcademico { get; set; }
        public string? NombreEstatus { get; set; }
        public string? NombreIdioma { get; set; }
            

        
        

        
    }

}
