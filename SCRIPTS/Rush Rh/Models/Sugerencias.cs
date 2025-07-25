using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rush_Rh.Models
{
    public class Sugerencia
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public string Respuesta { get; set; }
        public string TipoSugerencia { get; set; }
        public int IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public bool Anonima { get; set; }
    }

    public class TipoSugerencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}