using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using static Rush_Rh.Models.Usuarios;
using static Rush_Rh.Models.Formularios;

using CsvHelper;
using System.Globalization;
using System.Text;

using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Rush_Rh.Pages
{
    public class ListaUsuarios : PageModel
    {
        string conexion = "";

        public bool admin { get; private set; } = false;



        public List<UsuarioGeneral> usuariosGenerales = new List<UsuarioGeneral>();
        private List<DiasFestivos> diasFestivos = new List<DiasFestivos>();
        public List<FechasVacaciones> fechasVacaciones = new List<FechasVacaciones>();
        public List<FechasLicencias> fechasLicencias = new List<FechasLicencias>();
        public List<TiposPaseLista> tiposPaseLista = new List<TiposPaseLista>();

        public List<Departamentos> departamentos = new List<Departamentos>();
        public List<HistorialPaseLista> historial = new List<HistorialPaseLista>();





        private readonly ILogger<ListaUsuarios> _logger;

        public ListaUsuarios(IConfiguration configuration, ILogger<ListaUsuarios> logger)
        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }



        private IActionResult autorizar()
        {
            es_admin();
            if (!admin)
            {
                return Unauthorized();
            }

            return null;

        }



        private void es_admin()
        {
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            admin = (puestoUsuario == 3 || puestoUsuario == 5);
        }



        private bool esta_logeado()
        {
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            int usuario = idusuario ?? 0;
            if (usuario == 0)
            {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return false;
            }
            //Salimos de la funcion si el usuario ya esta logeado
            return true;
        }



        public IActionResult OnGet()
        {

            var resultado = esta_logeado();
            if (!resultado) return RedirectToPage("/Index"); // Si no está logeado, redirige inmediatamente.

            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes los permisos para acceder a esta página.";
                 TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                 return RedirectToPage("/Home");
            }


            OnPostGetUsuarios(0);
            OnGetHistorialPasesLista();
            return Page();
        }

        public void OnPostGetUsuarios(int estatus)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Estatus", estatus));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAll", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            usuariosGenerales.Add(construirUsuario(dtr));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

        }

        private UsuarioGeneral construirUsuario(DataTableReader usuarioGeneral)
        {
            return new UsuarioGeneral()
            {
                Id = int.Parse(usuarioGeneral["Id"].ToString()),
                Nombre = usuarioGeneral["Nombre"].ToString(),
                CURP = usuarioGeneral["CURP"].ToString(),
                Estatus = usuarioGeneral["Estatus"].ToString(),
                Nick = usuarioGeneral["Nick"].ToString(),
                CorreoElectronico = usuarioGeneral["CorreoElectronico"]?.ToString() ?? "",
            };
        }




        private void OnGetDiasFestivos()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DiasFestivosGetCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            diasFestivos.Add(construirDiasFestivos(dtr));
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        private DiasFestivos construirDiasFestivos(DataTableReader dia)
        {
            return new DiasFestivos()
            {
                Fecha = Convert.ToDateTime(dia["Fecha"]),
            };
        }


        private void OnGetTiposPaseLista()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetTiposPaseListaCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposPaseLista.Add(construirTiposPaseLista(dtr));
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        private TiposPaseLista construirTiposPaseLista(DataTableReader tipo)
        {
            return new TiposPaseLista()
            {
                Id = int.Parse(tipo["id"].ToString()),
                Nombre = tipo["nombre"].ToString(),
            };
        }


        private void OnGetFechasLicenciasConfirmadas(long usuarioId)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    // busco si tiene licencias en las fechas que solicito
                    prmtrs.Add(new SqlParameter("@UsuarioId", usuarioId));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetFechasLicenciasConfirmadasByUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            fechasLicencias.Add(construirFechasLicencias(dtr));
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        private FechasLicencias construirFechasLicencias(DataTableReader fechaLicencia)
        {
            return new FechasLicencias()
            {
                FechaInicio = Convert.ToDateTime(fechaLicencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(fechaLicencia["FechaFin"]),
                TipoLicencia = fechaLicencia["TipoLicencia"].ToString(),
            };
        }



        private void OnGetFechasVacacionesConfirmadas(long usuarioId)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    // busco si tiene vacaciones en las fechas que solicito
                    prmtrs.Add(new SqlParameter("@UsuarioId", usuarioId));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetFechasVacacionesConfirmadasByUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            fechasVacaciones.Add(construirFechasVacaciones(dtr));
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }
        private FechasVacaciones construirFechasVacaciones(DataTableReader fechaVacacion)
        {
            return new FechasVacaciones()
            {
                FechaInicio = Convert.ToDateTime(fechaVacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(fechaVacacion["FechaFin"]),
            };
        }





        public JsonResult OnGetDepartamentos()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();


                    // Llamar al procedimiento almacenado para obtener los datos de los departamentos
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetDepartamentoCollection", parametros))
                    {
                        while (dtr.Read())
                        {
                            Departamentos departamento = new Departamentos
                            {
                                Id = dtr.GetInt32(dtr.GetOrdinal("id")),
                                Nombre = dtr.GetString(dtr.GetOrdinal("nombre")),
                            };
                            departamentos.Add(departamento);
                        }
                    }
                    return new JsonResult(new
                    {
                        departamentos = departamentos
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los departamentos");
                    return new JsonResult(new { success = false, message = "Error al obtener los departamentos" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }




        private Dictionary<string, int> ObtenerTodosLosIdsUsuarios(List<string> empleadosUnicos)
        {
            var usuariosDict = new Dictionary<string, int>();

            // Extraer nombres y departamentos únicos
            var nombresDepartamentos = empleadosUnicos
                .Select(e => e.Split(" - "))
                .Select(parts => new
                {
                    Nombre = parts[0].Trim(),
                    Departamento = parts[1].Trim()
                })
                .Distinct()
                .ToList();

            // Serializar a JSON
            var jsonData = JsonSerializer.Serialize(nombresDepartamentos);

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    var parametros = new List<SqlParameter> {
                        new SqlParameter("@NombresDepartamentos", jsonData)
                    };

                    using (var dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetUsuariosIdsPorListaJSON", parametros))
                    {
                        while (dtr.Read())
                        {
                            string clave = $"{dtr["NombreCompleto"]} - {dtr["Departamento"]}";
                            usuariosDict[clave] = Convert.ToInt32(dtr["UsuarioId"]);
                        }
                    }
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

            return usuariosDict;
        }





        public IActionResult OnPostAnalizarDocumentoAsistenciasChecador([FromForm] IFormFile? DocumentoAsistenciasChecador)
        {
            // Verificar autorización
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;

            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction();


                    // Validación del archivo
                    if (DocumentoAsistenciasChecador == null || DocumentoAsistenciasChecador.Length == 0)
                    {
                        return new JsonResult(new { success = false, message = "No se ha subido ningún archivo." });
                    }

                    if (Path.GetExtension(DocumentoAsistenciasChecador.FileName) != ".csv")
                    {
                        return new JsonResult(new { success = false, message = "El archivo no es un archivo CSV válido." });
                    }

                    var resumenAsistencias = new Dictionary<string, (int asistencias, int retardos, int faltas, int licencias, int diasFestivos, int vacaciones, int departamentoId, long idUsuario, List<(DateTime fecha, string tipo, int tipoId, string? motivo)> detalles)>();


                    using (var reader = new StreamReader(DocumentoAsistenciasChecador.OpenReadStream(), Encoding.UTF8))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<RegistroAsistenciaMap>();
                        var registros = csv.GetRecords<RegistroAsistencia>().ToList();

                        // 1. Procesamiento de registros del archivo
                        var registrosProcesados = registros.Select(r =>
                        {
                            string nombre = r.Name?.Trim() ?? "";
                            string depto = r.Department?.Trim() ?? "";
                            string estado = r.AttendanceStatus?.Trim() ?? "";
                            DateTime fechaHora = DateTime.Parse(r.Time);

                            string empleadoId = $"{nombre} - {depto}";
                            var fecha = fechaHora.Date;
                            var hora = fechaHora.TimeOfDay;
                            var diaSemana = fecha.DayOfWeek;

                            return new { empleadoId, fecha, hora, estado, diaSemana };
                        })
                        .Where(r => r.estado == "Check-in" || r.estado == "Check-out")
                        .GroupBy(r => new { r.empleadoId, r.fecha })
                        .ToList();

                        Console.WriteLine($"\nTotal de registros procesados: {registrosProcesados.Count}");
                        foreach (var registro in registrosProcesados)
                        {
                            Console.WriteLine($"Empleado: {registro.Key.empleadoId}, Fecha: {registro.Key.fecha}, Registros: {registro.Count()}");
                            foreach (var r in registro)
                            {
                                Console.WriteLine($"  Estado: {r.estado}, Hora: {r.hora}");
                            }
                        }

                        // 2. Obtener lista de empleados y fechas únicas
                        var empleados = registrosProcesados.Select(g => g.Key.empleadoId).Distinct().ToList();
                        var fechasEnArchivo = registrosProcesados.Select(g => g.Key.fecha).Distinct().ToList();

                        Console.WriteLine($"\nEmpleados únicos: {string.Join(", ", empleados)}");
                        Console.WriteLine($"Fechas únicas: {string.Join(", ", fechasEnArchivo.Select(f => f.ToString("yyyy-MM-dd")))}");

                        // 3. Determinar rango de fechas a evaluar (1-15 o 16-fin de mes)
                        DateTime fechaInicio, fechaFin;
                        if (fechasEnArchivo.All(f => f.Day <= 15))
                        {
                            fechaInicio = new DateTime(fechasEnArchivo.First().Year, fechasEnArchivo.First().Month, 1);
                            fechaFin = new DateTime(fechasEnArchivo.First().Year, fechasEnArchivo.First().Month, 15);
                        }
                        else
                        {
                            fechaInicio = new DateTime(fechasEnArchivo.First().Year, fechasEnArchivo.First().Month, 16);
                            fechaFin = new DateTime(fechasEnArchivo.First().Year, fechasEnArchivo.First().Month,
                            DateTime.DaysInMonth(fechasEnArchivo.First().Year, fechasEnArchivo.First().Month));
                        }

                        // 4. Generar lista de días laborales en el período
                        var diasLaboralesEnPeriodo = Enumerable.Range(0, (fechaFin - fechaInicio).Days + 1)
                            .Select(offset => fechaInicio.AddDays(offset))
                            .Where(f => f.DayOfWeek != DayOfWeek.Sunday)
                            .ToList();

                        Console.WriteLine($"\nDías laborales en el período: {string.Join(", ", diasLaboralesEnPeriodo.Select(d => d.ToString("yyyy-MM-dd")))}");


                        //Obtener listas de dias festivos
                        OnGetDiasFestivos();

                        //Obtener listas de tipos de asistencia
                        OnGetTiposPaseLista();

                        //Obtener listas de departamentos (despues busco el ID)
                        OnGetDepartamentos();


                        // Obtener todos los IDs de una sola vez
                        var idsUsuarios = ObtenerTodosLosIdsUsuarios(empleados);


                        // 5. Procesar cada día laboral para cada empleado
                        foreach (var empleado in empleados)
                        {

                            if (!idsUsuarios.TryGetValue(empleado, out int usuarioId))
                            {
                                // Manejar caso cuando no se encuentra el usuario
                                Console.WriteLine($"No se encontró el usuario para: {empleado}");
                                return new JsonResult(new { success = false, message = $"Accion cancelada. No se encontró el usuario: {empleado}" });
                            }


                            //Obtener listas de vaciones o licencias del empleado
                            OnGetFechasLicenciasConfirmadas(usuarioId);
                            OnGetFechasVacacionesConfirmadas(usuarioId);

                            Console.WriteLine($"\nProcesando empleado: {empleado}");


                            foreach (var fecha in diasLaboralesEnPeriodo)
                            {
                                string motivo = "";
                                var tipo = "";
                                var tipoAsistenciaId = 0;

                                var diaSemana = fecha.DayOfWeek;

                                // Ignorar domingos (ya filtrado, pero por si acaso)
                                if (diaSemana == DayOfWeek.Sunday) continue;

                                // Verificar si es día festivo (implementar EsDiaFestivo según necesidad)
                                if (EsDiaFestivo(fecha))
                                {
                                    tipo = "Día festivo";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    motivo = "Sin detalles.";
                                    ActualizarResumen(empleado, 0, 0, 0, 0, 1, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }


                                motivo = ObtenerTipoLicencia(fecha);
                                if (motivo != null)
                                {
                                    tipo = "Licencia";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    ActualizarResumen(empleado, 0, 0, 0, 1, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }


                                if (tieneVacaciones(fecha))
                                {
                                    motivo = "Sin detalles.";
                                    tipo = "Vacaciones";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    ActualizarResumen(empleado, 0, 0, 0, 0, 0, 1, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }


                                // Para sábados: asistencia automática
                                if (diaSemana == DayOfWeek.Saturday)
                                {
                                    motivo = "Asistencia automática.";
                                    tipo = "Sabado";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    ActualizarResumen(empleado, 1, 0, 0, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }

                                // Buscar registros para este empleado en esta fecha
                                var registroDia = registrosProcesados
                                    .FirstOrDefault(g => g.Key.empleadoId == empleado && g.Key.fecha == fecha);

                                // Si no hay ningún registro para este día laboral - falta
                                if (registroDia == null)
                                {
                                    motivo = "No hay registros en este día.";
                                    tipo = "Falta";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    ActualizarResumen(empleado, 0, 0, 1, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }

                                // Procesar entradas y salidas
                                var entradas = registroDia.Where(r => r.estado == "Check-in").OrderBy(r => r.hora).ToList();
                                var salidas = registroDia.Where(r => r.estado == "Check-out").OrderBy(r => r.hora).ToList();

                                // Validar si hay al menos una entrada
                                if (entradas.Count == 0)
                                {
                                    // No hay entrada - falta
                                    motivo = "No hay entrada registrada.";
                                    tipo = "Falta";
                                    tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                    ActualizarResumen(empleado, 0, 0, 1, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                    continue;
                                }

                                TimeSpan primeraEntrada = entradas.First().hora;
                                bool tieneSalida = salidas.Count > 0;
                                TimeSpan ultimaSalida = tieneSalida ? salidas.Last().hora : TimeSpan.Zero;


                                // Variable para determinar el resultado final del día
                                tipo = "Asistencia"; // Valor por defecto

                                // 1. Validar hora de entrada
                                if (primeraEntrada >= new TimeSpan(9, 16, 0))
                                {
                                    motivo += "Entrada tardía: " + primeraEntrada.ToString(@"hh\:mm") + ". ";
                                    tipo = "Falta";
                                }
                                else if (primeraEntrada >= new TimeSpan(9, 1, 0) && primeraEntrada < new TimeSpan(9, 16, 0))
                                {
                                    motivo += "Entrada tardía: " + primeraEntrada.ToString(@"hh\:mm") + ". ";
                                    tipo = "Retardo";
                                }

                                // 2. Validar salida (puede sobrescribir el resultado de la entrada)
                                if (!tieneSalida || ultimaSalida < new TimeSpan(17, 0, 0))
                                {
                                    motivo += (tieneSalida ? "Salida temprana: " + ultimaSalida.ToString(@"hh\:mm") : "Salida no registrada") + ". ";
                                    tipo = "Falta"; // Salida temprana o no registrada prevalece sobre todo
                                }

                                // 3. Aplicar el resultado final
                                switch (tipo)
                                {
                                    case "Falta":
                                        tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                        ActualizarResumen(empleado, 0, 0, 1, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                        break;
                                    case "Retardo":
                                        tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                        ActualizarResumen(empleado, 0, 1, 0, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                        break;
                                    case "Asistencia":
                                        motivo = "Sin detalles.";
                                        tipoAsistenciaId = ObtenerTipoPaseListaIdDesdeNombre(tipo);
                                        ActualizarResumen(empleado, 1, 0, 0, 0, 0, 0, usuarioId, fecha, tipo, tipoAsistenciaId, motivo);
                                        break;
                                }
                            }
                        }

                        void ActualizarResumen(string empleado, int asis, int ret, int fal, int lic, int fes, int vac, long idUsuario, DateTime fecha, string tipo, int tipoId, string? motivo)
                        {
                            if (resumenAsistencias.ContainsKey(empleado))
                            {
                                var current = resumenAsistencias[empleado];
                                var nuevaListaDetalles = current.detalles;
                                nuevaListaDetalles.Add((fecha, tipo, tipoId, motivo));

                                resumenAsistencias[empleado] = (
                                    current.asistencias + asis,
                                    current.retardos + ret,
                                    current.faltas + fal,
                                    current.licencias + lic,
                                    current.diasFestivos + fes,
                                    current.vacaciones + vac,
                                    current.departamentoId,
                                    current.idUsuario,
                                    nuevaListaDetalles
                                );
                            }
                            else
                            {

                                var partes = empleado.Split(" - ");
                                var departamentoId = departamentos.FirstOrDefault(d => d.Nombre == partes[1].Trim())?.Id ?? 0;

                                resumenAsistencias[empleado] = (
                                    asis, ret, fal, lic, fes, vac, departamentoId, idUsuario,
                                    new List<(DateTime fecha, string tipo, int tipoId, string? motivo)> { (fecha, tipo, tipoId, motivo) }
                                );
                            }

                            Console.WriteLine($"Empleado: {empleado}, Asistencias: {resumenAsistencias[empleado].asistencias}, Retardos: {resumenAsistencias[empleado].retardos}, Faltas: {resumenAsistencias[empleado].faltas}, Licencias: {resumenAsistencias[empleado].licencias}, Días Festivos: {resumenAsistencias[empleado].diasFestivos}, Vacaciones: {resumenAsistencias[empleado].vacaciones}, idUsuario: {resumenAsistencias[empleado].idUsuario}, Detalles: {resumenAsistencias[empleado].detalles.Count}");
                        }


                        // Convertir a formato para frontend
                        var datos = resumenAsistencias.Select(kvp => new
                        {
                            Nombre = kvp.Key,
                            numAsistencias = kvp.Value.asistencias,
                            numRetardos = kvp.Value.retardos,
                            numFaltas = kvp.Value.faltas,
                            licencias = kvp.Value.licencias,
                            diasFestivos = kvp.Value.diasFestivos,
                            vacaciones = kvp.Value.vacaciones,
                            departamentoId = kvp.Value.departamentoId,
                            IdUsuario = kvp.Value.idUsuario,
                            detalles = kvp.Value.detalles.Select(d => new { fecha = d.fecha.ToString("yyyy-MM-dd"), tipo = d.tipo, tipoId = d.tipoId, motivo = d.motivo }).ToList()
                        }).ToList();


                        var options = new JsonSerializerOptions
                        {
                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };

                        string jsonData = JsonSerializer.Serialize(datos, options);
                        HttpContext.Session.SetString("DatosAsistencias", jsonData);


                        transaction.Commit();
                        return new JsonResult(new
                        {
                            success = true,
                            message = "Documento leído correctamente",
                            datos = datos,
                        });
                    }
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    _logger.LogError(ex, "Error al analizar las asistencias");
                    return new JsonResult(new { success = false, message = "Error al analizar las asistencias" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        private int ObtenerTipoPaseListaIdDesdeNombre(string nombre)
        {
            var tipo = tiposPaseLista.FirstOrDefault(t => t.Nombre == nombre);
            return tipo != null ? tipo.Id : 0; // Devuelve 0 si no se encuentra el tipo
        }



        private bool EsDiaFestivo(DateTime fecha)
        {
            // Verificar si la fecha está en la lista de días festivos
            foreach (var diaFestivo in diasFestivos)
            {
                if (diaFestivo.Fecha.Date == fecha.Date)
                {
                    return true;
                }
            }

            return false;
        }


        private bool tieneVacaciones(DateTime fecha)
        {
            foreach (var vacacion in fechasVacaciones)
            {
                if (vacacion.FechaInicio.Date <= fecha.Date && vacacion.FechaFin.Date >= fecha.Date)
                {
                    return true;
                }
            }

            return false;
        }


        private string ObtenerTipoLicencia(DateTime fecha)
        {
            foreach (var licencia in fechasLicencias)
            {
                if (licencia.FechaInicio.Date <= fecha.Date && licencia.FechaFin.Date >= fecha.Date)
                {
                    return licencia.TipoLicencia; // Devuelve el tipo si hay licencia
                }
            }

            return null; // No hay licencia
        }






        public IActionResult OnPostConfirmarAsistencias()
        {
            // Verificar autorización
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;

            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                int? idPaseLista = null;


                try
                {
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción


                    // Obtener los datos de la sesión
                    string jsonData = HttpContext.Session.GetString("DatosAsistencias");
                    if (string.IsNullOrEmpty(jsonData))
                    {
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "No se encontraron datos para confirmar.";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                        return RedirectToPage("/ListaUsuarios");
                    }


                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Fecha", DateTime.Now));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PasesListaCrear", prmtrs, transaction))
                    {
                        if (dtr.Read())
                        {
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["IdPaseLista"]?.ToString(), out int result))
                            {
                                idPaseLista = result;
                            }
                        }
                        else
                        {
                            throw new Exception("No se pudo obtener el ID del documento.");
                        }
                    }




                    // 2. Transformar datos al formato esperado por el SP
                    var datosAsistencias = JsonSerializer.Deserialize<List<Asistencia>>(jsonData);
                    var asistenciasParaBD = datosAsistencias.SelectMany(a =>
                    {
                        var partes = a.Nombre.Split(" - ");
                        return a.detalles.Select(d => new
                        {
                            IdUsuario = a.IdUsuario,
                            IdDepartamento = a.departamentoId,
                            Fecha = DateTime.Parse(d.fecha),
                            IdTipoPaseLista = d.tipoId,
                            Motivo = d.motivo ?? ""
                        });
                    }).ToList();



                    // Calcular rango de fechas
                    var fechaInicio = datosAsistencias.Min(a => a.detalles.Min(d => DateTime.Parse(d.fecha))).Date;
                    var fechaFin = datosAsistencias.Max(a => a.detalles.Max(d => DateTime.Parse(d.fecha))).Date;

                    // Verificar solapamiento (usando tu método existente)
                    if (VerificarPaseExistente(fechaInicio, fechaFin))
                    {
                        TempData["Message"] = $"Ya existe un pase de lista registrado que se solapa con el período: {fechaInicio:yyyy-MM-dd} - {fechaFin:yyyy-MM-dd}";
                        TempData["MessageType"] = "warning";
                        return RedirectToPage("/ListaUsuarios");
                    }


                    var options = new JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };


                    // 3. Serializar a JSON
                    string jsonParaBD = JsonSerializer.Serialize(asistenciasParaBD, options);


                    /* 4. Ejecutar en BD      */
                    var parametrosDetalle = new List<SqlParameter>
                    {
                        new SqlParameter("@IdPaseLista", idPaseLista),
                        new SqlParameter("@DetallesJSON", jsonParaBD)
                    };

                    // Usamos ExecuteNonQuery ya que no necesitamos resultados
                    using (var dtr = sqlConexion.EjecutarReaderStoreProcedure("DetallesPaseListaCrear", parametrosDetalle, transaction))
                    {
                        // Verificar resultado si el SP devuelve algo
                        if (dtr.Read())
                        {
                            var resultado = dtr["Resultado"].ToString();
                            if (resultado != "1")
                            {
                                throw new Exception(dtr["Mensaje"].ToString());
                            }
                        }
                    }


                    // 5. Limpiar y mostrar éxito
                    HttpContext.Session.Remove("DatosAsistencias");

                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Asistencias confirmadas correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.

                    transaction.Commit();


                    return RedirectToPage("/ListaUsuarios");

                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error

                    _logger.LogError(ex, "Error al confirmar asistencias");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al confirmar las asistencias.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage("/ListaUsuarios");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }


        }



        private bool VerificarPaseExistente(DateTime fechaInicio, DateTime fechaFin)
        {
            using (var sqlConexion = new SqlServer())
            {
                sqlConexion.Conectar(conexion);

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@FechaInicio", fechaInicio),
                    new SqlParameter("@FechaFin", fechaFin)
                };

                using (var dtr = sqlConexion.EjecutarReaderStoreProcedure(
                    "dbo.PasesListaVerificarExistente",
                    parametros))
                {
                    return dtr.HasRows; // True si existe, False si no
                }
            }
        }


        private void OnGetHistorialPasesLista()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                sqlConexion.Conectar(conexion);

                List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                using (var dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PasesListaGetHistorial", prmtrs))
                {
                    while (dtr.Read())
                    {
                        historial.Add(new HistorialPaseLista
                        {
                            IdPaseLista = Convert.ToInt32(dtr["IdPaseLista"]),
                            FechaEjecucion = Convert.ToDateTime(dtr["FechaEjecucion"]),
                            TotalRegistros = Convert.ToInt32(dtr["TotalRegistros"]),
                            FechaInicio = Convert.ToDateTime(dtr["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(dtr["FechaFin"]),
                        });
                    }
                }
            }

        }



        public IActionResult OnPostDetallesPasesLista(int idPaseLista)
        {
            // Verificar autorización
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    var prmtrs = new List<SqlParameter>
                    {
                        new SqlParameter("@IdPaseLista", idPaseLista)
                    };

                    var datos = new List<Asistencia>();

                    using (var dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PasesListaGetDetalles", prmtrs))
                    {
                        // Diccionario para agrupar por usuario
                        var asistenciasPorUsuario = new Dictionary<long, Asistencia>();

                        while (dtr.Read())
                        {
                            var idUsuario = Convert.ToInt64(dtr["IdUsuario"]);

                            // Si el usuario no está en el diccionario, lo agregamos
                            if (!asistenciasPorUsuario.ContainsKey(idUsuario))
                            {
                                asistenciasPorUsuario[idUsuario] = new Asistencia
                                {
                                    Nombre = $"{dtr["NombreCompleto"]} - {dtr["Departamento"]}",
                                    numAsistencias = 0,
                                    numRetardos = 0,
                                    numFaltas = 0,
                                    licencias = 0,
                                    diasFestivos = 0,
                                    vacaciones = 0,
                                    departamentoId = Convert.ToInt32(dtr["IdDepartamento"]),
                                    IdUsuario = idUsuario,
                                    detalles = new List<DetalleAsistencia>()
                                };
                            }

                            // Agregar el detalle
                            var detalle = new DetalleAsistencia
                            {
                                fecha = dtr["Fecha"].ToString(),
                                tipo = dtr["Tipo"].ToString(),
                                tipoId = Convert.ToInt32(dtr["IdTipoPaseLista"]),
                                motivo = dtr["Motivo"]?.ToString() ?? string.Empty
                            };

                            asistenciasPorUsuario[idUsuario].detalles.Add(detalle);

                            // Actualizar contadores según el tipo
                            switch (detalle.tipoId)
                            {
                                case 1:
                                    asistenciasPorUsuario[idUsuario].diasFestivos++;
                                    break;
                                case 2:
                                    asistenciasPorUsuario[idUsuario].licencias++;
                                    break;
                                case 3:
                                    asistenciasPorUsuario[idUsuario].vacaciones++;
                                    break;
                                case 4: //Sabado
                                    asistenciasPorUsuario[idUsuario].numAsistencias++;
                                    break;
                                case 5: //Falta
                                    asistenciasPorUsuario[idUsuario].numFaltas++;
                                    break;
                                case 6: //Asistencia
                                    asistenciasPorUsuario[idUsuario].numAsistencias++;
                                    break;
                                case 7: //Retardo
                                    asistenciasPorUsuario[idUsuario].numRetardos++;
                                    break;
                                default:
                                    // Manejar otros tipos si es necesario
                                    break;

                            }
                        }

                        // Convertir el diccionario a lista
                        datos = asistenciasPorUsuario.Values.ToList();
                    }

                    return new JsonResult(new { success = true, datos = datos });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener detalles de los pases de lista");
                    return new JsonResult(new { success = false, message = "Error al obtener detalles de los pases de lista" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }




        public IActionResult OnPostEliminarPaseLista(int idPaseLista)
        {
            // Verificar autorización
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    var prmtrs = new List<SqlParameter>
                    {
                        new SqlParameter("@IdPaseLista", idPaseLista)
                    };

                    using (var dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PasesListaEliminar", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            var resultado = dtr["Resultado"].ToString();
                            if (resultado != "1")
                            {
                                throw new Exception(dtr["Mensaje"].ToString());
                            }
                        }
                    }

                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "Pase de lista eliminado correctamente.";
                    TempData["MessageType"] = "success"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage("/ListaUsuarios");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al eliminar el pase de lista");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "Ocurrió un error al eliminar el pase de lista.";
                    TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    return RedirectToPage("/ListaUsuarios");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }







    }
}