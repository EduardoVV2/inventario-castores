using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rush_Rh.Models
{

    public class Aviso
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateOnly FechaEvento { get; set; }
        public int MedioEnvio { get; set; }
        //A quien se le mandará dependiendo del medioenvio
        public int? EnvioUsuario { get; set; }
        public int? EnvioGenero { get; set; }
        public int? EnvioPuesto { get; set; }
        public int? EnvioDepartamento { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaEdicion { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int? IdDocumento { get; set; }
        public bool Activo { get; set; }
    }

    public class AvisoUsuario
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEvento { get; set; }
        public string MedioEnvio { get; set; }
        public string? URL { get; set; }
    }
    
    public class ImagenCarruselAviso
    {
        public int Id { get; set; }
        public string URL { get; set; }
    }
    
}