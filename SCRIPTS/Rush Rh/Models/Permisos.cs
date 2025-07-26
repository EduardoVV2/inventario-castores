using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rush_Rh.Models
{
    public class LicenciaRegistro
    {
        public int IdUsuario { get; set; }
        public int IdJefeDirecto { get; set; }
        public int IdTipoLicencia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReincorporacion { get; set; }
        public string Comentarios { get; set; }
        public int DiasUsados { get; set; }
    }

    public class Licencias
    {
        public int Id { get; set; }
        public int IdJefe { get; set; }
        public string TipoLicencia { get; set; }
        public int IdTipoLicencia { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Estatus { get; set; }
        public int IdEstatus { get; set; }
        public bool Pendiente { get; set; }
    }

    public class LicenciaDetalle
    {
    
        public int Id { get; set; }
        public int IdJefe { get; set; }
        public int IdTipoLicencia {get; set;}
        public string Usuario { get; set; }
        public string TipoPermiso {get; set;}
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }
        public string ComentariosRH { get; set; }
        public string ComentariosGerente { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public string NombreCompletoJefe { get; set; } 
        public string NombreCompletoUsuario { get; set; }
        public int DiasTotalUsados { get; set; } 
        public string NombrePuesto { get; set; }
        public string NombreDepartamento { get; set; }
        public int DiasSolicitados { get; set; }
        public int TotalDiasUsados { get; set; } 
        public int IdUsuario { get; set; }
       
    }

    public class LicenciaDetalleJefe
    {
         public int Id { get; set; }
        public int IdJefe { get; set; }
        public int IdTipoLicencia {get; set;}
        public string Usuario { get; set; }
        public string TipoPermiso {get; set;}
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }

        public string Estatus { get; set; }
        public string NombreCompletoJefe { get; set; } 
        public string NombreCompletoUsuario { get; set; }
        public int DiasTotalUsados { get; set; } 
        public string NombrePuesto { get; set; }
        public string NombreDepartamento { get; set; }
        public int DiasSolicitados { get; set; }
        
    }

    public class LicenciaDetalleRhGerente
    {
         public int Id { get; set; }
        public int IdJefe { get; set; }
        public int IdTipoLicencia {get; set;}
        public string Usuario { get; set; }
        public string TipoPermiso {get; set;}
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }
        public string ComentariosRH { get; set; }
        public string ComentariosGerente { get; set; }
        public string Jefe { get; set;  }

        public string Estatus { get; set; }
        public string NombreCompletoJefe { get; set; } 
        public string NombreCompletoUsuario { get; set; }
        public int DiasTotalUsados { get; set; } 
        public string NombrePuesto { get; set; }
        public string NombreDepartamento { get; set; }
        public int DiasSolicitados { get; set; }
        

    }

    public class VacacionRegistro
    {
        public int IdUsuario { get; set; }
        public int IdJefeDirecto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaReincorporacion { get; set; }
        public string Comentarios { get; set; }
        public int DiasUsados { get; set; }
    }

    public class Vacaciones
    {
        public int Id { get; set; }
        public int IdJefe { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Estatus { get; set; }
        public int IdEstatus { get; set; }
        public int DiasUsados { get; set; }
        
        public bool Pendiente { get; set; }
    }

    public class VacacionDetalle
    {
        public int Id { get; set; }
        public int IdJefe { get; set; }
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }
        public string ComentariosRH { get; set; }
        public string ComentariosGerente { get; set; }
        public string Estatus { get; set; }
        public int IdEstatus { get; set; }
        public int DiasSolicitados { get; set; }
        public string NombreJefe { get; set; } 
        public string NombreUsuario { get; set; } 
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public string PeriodoVacacional { get; set; }
        public int TotalDiasUsados { get; set; }
        public string DiasDisponiblesVacaciones { get; set; }
        public int IdUsuario { get; set; }
        public int TotalSolicitudesVacaciones { get; set; }



        
        
    }

    public class VacacionDetalleJefe
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public int IdJefe { get; set; }
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }
        public string Estatus { get; set; }
        public int IdEstatus { get; set; }
        public int DiasUsados { get; set; }
         public string NombreJefe { get; set; } 
        public string NombreUsuario { get; set; } 
        public string Departamento { get; set; }
        public string Puesto { get; set; }
         public string PeriodoVacacional { get; set; }
        
    }

    public class VacacionDetalleRhGerente
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Jefe { get; set; }
        public int IdJefe { get; set; }
        public DateTime FechaSolicitud { get; set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime Reincorporacion { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosJefe { get; set; }
        public string ComentariosRH { get; set; }
        public string ComentariosGerente { get; set; }
        public string Estatus { get; set; }
        public int DiasUsados { get; set; }
        public string NombreJefe { get; set; } 
        public string NombreUsuario { get; set; } 
        public string Departamento { get; set; }
        public string Puesto { get; set; }
         public string PeriodoVacacional { get; set; }
        
    }

    public class DiasFestivos 
    {
        public DateTime Fecha { get; set; }
    }

    public class FechasVacaciones
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; } 
    }

    public class FechasLicencias
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; } 
        public string TipoLicencia { get; set; }
    }

    public class PermisosCalendario
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; } 
        public string Nick { get; set; }
        public string TipoSolicitud { get; set; }
    }

    
}