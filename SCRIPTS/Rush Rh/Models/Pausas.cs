using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Rush_Rh.Models
{
    public class Pausas
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public int IdUsuario { get; set; }
        public string Nick { get; set; }
        public string RutaFoto { get; set; }
    }

    public class FechasCalendario
    {
        public int ? Mes { get; set; }
        public int ? Año { get; set; }
    }
}