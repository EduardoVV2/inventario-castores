using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Rush_Rh.Models
{
    public class RegistroAsistenciaMap : ClassMap<RegistroAsistencia>
    {
        public RegistroAsistenciaMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Department).Name("Department");
            Map(m => m.AttendanceStatus).Name("Attendance Status");
            Map(m => m.Time).Name("Time");
        }
    }
}