using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rush_Rh.Models
{
    public class Formularios
    {
        public class Sexos{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class EstadosCiviles{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Nacionalidades{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Generos{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        //Pagina avisos
        public class MediosEnvio
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Departamentos
        {
            public int? Id { get; set; }
            public string? Nombre { get; set; }
        }

        public class Puestos
        {
            public int? Id { get; set; }
            public string? Nombre { get; set; }
            
            public string? DescripcionPuesto { get; set; }
            public int IdDepartamento { get; set;}
            public Documento DescripcionPuestoDocumento { get; set; }
        }

        public class Oficinas
        {
            public int Id { get; set;}
            public string Nombre { get; set;}
        }


        /*Usado en pagina info complementaria*/
        public class Pais
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Estado
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int PaisId { get; set; }
        }

        public class Municipio
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int EstadoId { get; set; }
        }

        public class Colonia
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int MunicipioId { get; set; }
        }

        public class TipoSangre
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Parentesco
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class NivelesAcademicos{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class EstatusEstudio{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class TiposExperiencias{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        public class TiposLicencias{
            public int Id { get; set; }
            public string Licencia { get; set; }
        }

        public class TiposUsuarios{
            public int Id { get; set;}
            public string Nombre { get; set;}
            
        }

        public class Meses{
            public int Id { get; set; }
            public string Nombre { get; set; }
        }
    }
}