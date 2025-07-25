using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rush_Rh.Models
{
    public class Incidentes
    {
        public class EstatusIncidencias
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
        public class TiposIncidencias
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
        public class Incidencia{
            public int Id { get; set; }
            public int IdTipoIncidencia { get; set; }
            public DateTime FechaRegistro { get; set; }
            public DateTime FechaIncidencia { get; set; }
            public string? HoraSalida { get; set; }
            public string? HoraEntrada { get; set; }
            public string Descripcion { get; set; }
            public int IdUsuario { get; set; }
            public int? IdDocumento { get; set; }
            public string? URLDocumento { get; set; }
            public int IdEstatus { get; set; }
            public string Estatus { get; set; }
            public string? NombreUsuario { get; set; }
            public string? ApellidoMaternoUsuario { get; set; }
            public string? ApellidoPaternoUsuario { get; set; }
            public string? Comentarios { get; set; }

        }
    }
}