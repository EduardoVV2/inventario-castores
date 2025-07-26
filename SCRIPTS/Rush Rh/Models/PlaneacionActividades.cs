using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Rush_Rh.Models
{
    public class PlaneacionAnual
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int Año { get; set; }
        public bool Activo { get; set; }
    }

    public class ActividadesPlaneacion
    {
        public long Id { get; set; }

        public string NombreActividad { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public bool DC3 { get; set; }
        public long IdPlaneacion { get; set; }
        public bool Activo { get; set; }
    }


    public class IntegrantesActividad {
        public long Id { get; set; }
        public long IdActividad { get; set; }
        public long IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public string RFC { get; set; }
        public string NombrePuesto { get; set; }
        public bool Activo { get; set; }
    }

}