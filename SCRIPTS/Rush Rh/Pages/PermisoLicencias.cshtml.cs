using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using static Rush_Rh.Models.Formularios;
using System.IO;
using System.Text.Json;
using iText.IO.Source;




namespace Rush_Rh.Pages
{
    public class PermisoLicencias : PageModel
    {
        private readonly ILogger<PermisoLicencias> _logger;

        [BindProperty]
        public LicenciaRegistro infoLicencia { get; set; }

        [BindProperty]
        public VacacionRegistro infoVacacion { get; set; }

        
        public LicenciaDetalle Licencia { get; set; }

        
        public LicenciaDetalleJefe LicenciaJefe { get; set; }

        
        public LicenciaDetalleRhGerente LicenciaRHGerente { get; set; }

        
        public VacacionDetalle Vacacion { get; set; }

        
        public VacacionDetalleJefe VacacionJefe { get; set; }

        
        public VacacionDetalleRhGerente VacacionRhGerente { get; set; }

        
        public int DiasDisponiblesVacaciones { get; set; }

        
        public string TipoPermiso { get; set; }

        
        public string TipoAccion { get; set; }
        public string Permiso { get; set;  }
        public int Año  { get; set; }
        public int Mes { get; set; }

        
        public bool TengoSolicitudesLicencias { get; set; }
        public bool TengoSolicitudesVacaciones { get; set; }

        string conexion = "";

        //para variable de sesion
        int idUsuario { get; set; }

        public int idPuesto { get; set; }

        

        public List<TiposLicencias> tiposLicencias = new List<TiposLicencias>();
        public List<UsuarioNombre> usuarios = new List<UsuarioNombre>(); 
        public List<Licencias> licencias = new List<Licencias>();
        public List<Vacaciones> vacaciones = new List<Vacaciones>();
        public List<DiasFestivos> diasFestivos = new List<DiasFestivos>();
        public List<FechasVacaciones> fechasVacaciones = new List<FechasVacaciones>();
        public List<FechasLicencias> fechasLicencias = new List<FechasLicencias>();

        public List<Licencias> Licencias { get; set; } = new List<Licencias>();
        public List<Vacaciones> Vacaciones { get; set; } = new List<Vacaciones>();


        public PermisoLicencias(ILogger<PermisoLicencias> logger, IConfiguration configuration)

        {
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");
            _logger = logger;
        }

        public IActionResult OnGet(string? accion, int? año, int? mes)
        {

            idUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            if(idUsuario == 0){
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "Antes inicie sesión para ingresar.";
                TempData["MessageType"] = "info"; // Puede ser 'error', 'info', etc.
                return RedirectToPage("/Index");
            }

            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

            int Year = año ?? DateTime.Now.Year;
            int Month = mes ?? DateTime.Now.Month;
            //director general
            if(idPuesto == 5 )
            {
                // Si accion es null, asignarle un valor por defecto
                accion ??= "confirmacionLicencia"; 

                if(accion == "confirmacionLicencia")
                { 
                    GetLicenciasParaConfirmarRHandGerente(Year, Month);
                    GetTiposLicencias();
                    TipoPermiso = "licencia";
                    TipoAccion = "confirmarLicencia";

                }
                else if(accion == "confirmacionVacacion")
                {
                    GetVacacionesParaConfirmarRHandGerente(Year, Month);
                    GetDiasVacaciones(idUsuario);
                    TipoPermiso = "vacacion";
                    TipoAccion = "confirmarVacacion";
                }
            }
            //RH
            else if (idPuesto == 3)
            {
                // Si accion es null, asignarle un valor por defecto
                accion ??= "solicitarLicencia"; 

                if(accion == "solicitarLicencia")
                {
                    GetLicencias(Year, Month);
                    GetTiposLicencias();
                    TipoPermiso = "licencia";
                    TipoAccion = "solicitarLicencia";
                }
                else if(accion == "solicitarVacacion")
                {
                    GetVacaciones(Year, Month);
                    GetDiasVacaciones(idUsuario);
                    TipoPermiso = "vacacion";
                    TipoAccion = "solicitarVacacion";
                }      

                else if(accion == "confirmacionLicencia")
                { 
                    GetLicenciasParaConfirmarRHandGerente(Year, Month);
                    GetTiposLicencias();
                    TipoPermiso = "licencia";
                    TipoAccion = "confirmarLicencia";
                }
                else if(accion == "confirmacionVacacion")
                {
                    GetVacacionesParaConfirmarRHandGerente(Year, Month);
                    GetDiasVacaciones(idUsuario);
                    TipoPermiso = "vacacion";
                    TipoAccion = "confirmarVacacion";
                }
                GetUsuarios();
            }
            //usuaios normales y jefes directos cuando les manden solicitudes
            else
            {
                // Si tipo es null, asignarle un valor por defecto
                accion ??= "solicitarLicencia";

                if(accion == "solicitarLicencia")
                {
                    GetNumeroSolicitudesParaJefe();
                    GetLicencias(Year, Month);
                    GetTiposLicencias();
                    TipoPermiso = "licencia";
                    TipoAccion = "solicitarLicencia";
                }
                else if(accion == "solicitarVacacion")
                {
                    GetNumeroSolicitudesParaJefe();
                    GetVacaciones(Year, Month);
                    GetDiasVacaciones(idUsuario);
                    TipoPermiso = "vacacion";
                    TipoAccion = "solicitarVacacion";
                }   
                else if(accion == "confirmacionLicenciaJefe")
                {
                    GetLicenciasParaConfirmarJefeDirecto();
                    GetTiposLicencias();
                    TipoPermiso = "licencia";
                    TipoAccion = "confirmarLicenciaJefe";
                }
                else if(accion == "confirmacionVacacionJefe")
                {
                    GetVacacionesParaConfirmarJefeDirecto();
                    GetDiasVacaciones(idUsuario);
                    TipoPermiso = "vacacion";
                    TipoAccion = "confirmarVacacionJefe";
                }            
                GetUsuarios();
            }
            return Page();

        }

        private void GetTiposLicencias(){
            using (SqlServer sqlConexion = new SqlServer()){

                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.TiposLicenciasGetCollection", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            tiposLicencias.Add(construirTiposLicencias(dtr));
                        }
                    }
                }
                catch(Exception ex){

                }finally{
                    sqlConexion.Desconectar();
                }
            }    
        }

        private TiposLicencias construirTiposLicencias (DataTableReader tipoLicencia){
            return new TiposLicencias()
            {
                Id = int.Parse(tipoLicencia["Id"].ToString()),
                Licencia = tipoLicencia["Licencia"].ToString()  
            };
        }

        private void GetUsuarios(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Estatus", 1));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAll", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            usuarios.Add(construirUsuario(dtr));
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private UsuarioNombre construirUsuario(DataTableReader usuario){
            return new UsuarioNombre()
            {
                Id = int.Parse(usuario["Id"].ToString()),
                Nombre = usuario["Nombre"].ToString(),
            };
        }

        private void GetLicencias(int año, int mes){
            using (SqlServer sqlConexion = new SqlServer()){
                try
                {
                    Año = año;
                    Mes = mes;
                    Permiso = "solicitarLicencia";

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetCollectionByUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            Licencias licencia = construirLicencia(dtr);

                            // Verifica si la fecha de inicio es mayor a hoy
                            if (licencia.FechaInicio <= DateTime.Now && (licencia.IdEstatus == 1 || licencia.IdEstatus == 2 || licencia.IdEstatus == 3 || licencia.IdEstatus == 6))
                            {
                                // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                List<SqlParameter> parametros = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdLicencia", licencia.Id),
                                    new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                };
                                sqlConexion.EjecutarStoreProcedure("dbo.LicenciasChangeEstatus", parametros);
                            }

                            licencias.Add(licencia);
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

        private void GetLicenciasParaConfirmarJefeDirecto(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    Permiso = "confirmacionLicenciaJefe";

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetCollectionByJefeDirecto", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            Licencias licencia = construirLicencia(dtr);

                            // Verifica si la fecha de inicio es mayor a hoy
                            if (licencia.FechaInicio <= DateTime.Now)
                            {
                                // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                List<SqlParameter> parametros = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdLicencia", licencia.Id),
                                    new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                };
                                sqlConexion.EjecutarStoreProcedure("dbo.LicenciasChangeEstatus", parametros);
                            }

                            licencias.Add(licencia);
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }
        
        private Licencias construirLicencia(DataTableReader licencia){
            return new Licencias()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                IdJefe = int.Parse(licencia["IdJefe"].ToString()),
                TipoLicencia = licencia["TipoLicencia"].ToString(),
                IdTipoLicencia = int.Parse(licencia["IdTipoLicencia"].ToString()),
                Estatus = licencia["Estatus"].ToString(),
                IdEstatus = int.Parse(licencia["IdEstatus"].ToString()),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),
            };
        }


        private void GetLicenciasParaConfirmarRHandGerente(int año, int mes)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    Año = año;
                    Mes = mes;
                    Permiso = "confirmacionLicencia";

                    int IdUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
                    int IdPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetCollectionConfirmarRHyGerente", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            if (IdPuesto == 5)
                            {
                                Licencias licencia = construirLicenciaGerente(dtr, IdUsuario);

                                // Verifica si la fecha de inicio es mayor a hoy
                                if (licencia.FechaInicio <= DateTime.Now && (licencia.IdEstatus == 2 || licencia.IdEstatus == 3 || licencia.IdEstatus == 6))
                                {
                                    // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                    List<SqlParameter> parametros = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdLicencia", licencia.Id),
                                        new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                    };
                                    sqlConexion.EjecutarStoreProcedure("dbo.LicenciasChangeEstatus", parametros);
                                }

                                licencias.Add(licencia);
                            }
                            else if (IdPuesto == 3)
                            {
                                Licencias licencia = construirLicenciaRH(dtr, IdUsuario);

                                // Verifica si la fecha de inicio es mayor a hoy
                                if (licencia.FechaInicio <= DateTime.Now && (licencia.IdEstatus == 2 || licencia.IdEstatus == 3 || licencia.IdEstatus == 6))
                                {
                                    // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                    List<SqlParameter> parametros = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdLicencia", licencia.Id),
                                        new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                    };
                                    sqlConexion.EjecutarStoreProcedure("dbo.LicenciasChangeEstatus", parametros);
                                }

                                licencias.Add(licencia);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        private Licencias construirLicenciaRH(DataTableReader licencia, int idUsuarioSesion){
            int idJefe = int.Parse(licencia["IdJefe"].ToString());
            int idEstatus = int.Parse(licencia["IdEstatus"].ToString());

            return new Licencias()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                IdJefe = idJefe,
                TipoLicencia = licencia["TipoLicencia"].ToString(),
                IdTipoLicencia = int.Parse(licencia["IdTipoLicencia"].ToString()),
                Estatus = licencia["Estatus"].ToString(),
                IdEstatus = idEstatus,
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),

                Pendiente = (idEstatus == 3 || (idJefe == idUsuarioSesion && idEstatus == 2))
            };
        }

        private Licencias construirLicenciaGerente(DataTableReader licencia, int idUsuarioSesion)
        {
            int idJefe = int.Parse(licencia["IdJefe"].ToString());
            int idEstatus = int.Parse(licencia["IdEstatus"].ToString());

            return new Licencias()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                IdJefe = idJefe,
                TipoLicencia = licencia["TipoLicencia"].ToString(),
                IdTipoLicencia = int.Parse(licencia["IdTipoLicencia"].ToString()),
                Estatus = licencia["Estatus"].ToString(),
                IdEstatus = idEstatus,
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),

                Pendiente = ((idEstatus == 6 || (idJefe == idUsuarioSesion && idEstatus == 2)))
            };
        }

        //busco si al usuario que ingreso le mandaron una solicitud para que la apruebe
        private void GetNumeroSolicitudesParaJefe(){
            int NumeroLicencias = 0;
            int NumeroVacaciones = 0;
            //Primero busco si tiene licencias
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdJefeDirecto", idUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetNumeroLicenciasConfirmarJefe", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            if(int.TryParse(dtr["LicenciasConfirmar"]?.ToString(), out int result))
                            {
                                NumeroLicencias = result;
                            }
                        }
                    }
                    // Asigna el resultado a TengoSolicitudes
                    TengoSolicitudesLicencias = NumeroLicencias > 0;
                }
                catch (Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            //si no tiene licencias busco si tiene vacaciones
            if (TengoSolicitudesLicencias == false){
                using (SqlServer sqlConexion = new SqlServer())
                {
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdJefeDirecto", idUsuario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetNumeroVacacionesConfirmarJefe", prmtrs))
                        {
                            if (dtr.Read())
                            {
                                if(int.TryParse(dtr["VacacionesConfirmar"]?.ToString(), out int result))
                                {
                                    NumeroVacaciones = result;
                                }
                            }
                        }
                    }
                    catch (Exception ex){

                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                // Asigna el resultado a TengoSolicitudes
                TengoSolicitudesVacaciones = NumeroVacaciones > 0;
            }
            else{
                TengoSolicitudesVacaciones = false;
            }
            
            
        }
        public IActionResult OnPostRegistroLicencias(){
            idUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            //Permisos personales
            if(infoLicencia.IdTipoLicencia == 2){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        int PermisosUsados = 0;
                        int PermisosRestantes = 3;

                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetPermisosPersonalesByUsuario", prmtrs))
                        {
                            if (dtr.Read())
                            {
                                if(int.TryParse(dtr["TotalDiasPermiso"]?.ToString(), out int result))
                                {
                                    PermisosUsados = result;
                                    PermisosRestantes = 3 - PermisosUsados;
                                }
                            }
                        }
                        if (PermisosRestantes > 0){
                            int DiasTotalesPermiso = 0;

                            for (DateTime fecha  = infoLicencia.FechaInicio; fecha <= infoLicencia.FechaFin; fecha = fecha.AddDays(1))
                            {
                                // Si no es sábado ni domingo, contamos como día hábil
                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    //Si el permiso se pide un viernes se contaran dos tamnien se tomarta en cuenta el sabado
                                    if (fecha.DayOfWeek == DayOfWeek.Friday)
                                    {
                                        DiasTotalesPermiso = DiasTotalesPermiso + 2;
                                    }
                                    else
                                    {
                                        DiasTotalesPermiso = DiasTotalesPermiso + 1;
                                    }  
                                }
                            }
                            //TimeSpan diferencia = infoLicencia.FechaFin - infoLicencia.FechaInicio;
                            DateTime Reincorporacion = infoLicencia.FechaFin.AddDays(1); // Avanza un día;

                            //DiasTotalesPermiso = diferencia.Days;

                            if(DiasTotalesPermiso <= PermisosRestantes){
                                while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                                }

                                prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                                prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                                prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                                prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                                prmtrs.Add(new SqlParameter("@FechaFin", infoLicencia.FechaFin));
                                prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                                prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                                prmtrs.Add(new SqlParameter("@DiasUsados", DiasTotalesPermiso));
                                using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                                {
                                  
                                } 
                                TempData["MessageTitle"] = "¡Éxito!";
                                TempData["Message"] = "¡Solicitud generada con éxito";
                                TempData["MessageType"] = "success"; 
                                return RedirectToPage(new {accion = "solicitarLicencia"});
                            }
                            else{
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡No tienes suficientes permisos personales!";
                                TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                                return RedirectToPage(new {accion = "solicitarLicencia"});
                            }
                            
                        }
                        else{
                            TempData["MessageTitle"] = "¡Error!";
                            TempData["Message"] = "¡Tus permisos personales se agotaron!";
                            TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                            return RedirectToPage(new {accion = "solicitarLicencia"});
                        }
                        
                    }
                    catch(Exception ex){
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error"; // Puede ser 'error', 'info', etc.
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }

                return RedirectToPage(new {accion = "solicitarLicencia"});
            }

            //Permiso de matrimonio
            if(infoLicencia.IdTipoLicencia == 3){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        int PermisosUsados = 1;

                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetLicenciaMatrimonioByUsuario", prmtrs))
                        {
                            if (dtr.Read())
                            {
                                if(int.TryParse(dtr["TotalPermisos"]?.ToString(), out int result))
                                {
                                    PermisosUsados = result;
                                }
                            }
                        }
                        if(PermisosUsados == 0){

                            DateTime Reincorporacion = infoLicencia.FechaInicio.AddDays(1);

                            //Busco que el dia en el que regrese no sea sabado o domingo
                            while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                            {
                                Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                            }

                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                            prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                            prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                            prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                            prmtrs.Add(new SqlParameter("@FechaFin", infoLicencia.FechaInicio));
                            prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                            prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                            prmtrs.Add(new SqlParameter("@DiasUsados", 1));
                            using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                            {
                             
                            } 
                            TempData["MessageTitle"] = "¡Éxito!";
                            TempData["Message"] = "¡Solicitud generada con éxito";
                            TempData["MessageType"] = "success"; 
                            return RedirectToPage(new {accion = "solicitarLicencia"});
                        }
                        else{
                            TempData["MessageTitle"] = "¡Error!";
                            TempData["Message"] = "¡Ya usaste tu permiso por matrimonio!";
                            TempData["MessageType"] = "error"; 
                            return RedirectToPage(new {accion = "solicitarLicencia"});
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error"; 
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage(new {accion = "solicitarLicencia"});
            }

            //Permiso por paternidad
            if(infoLicencia.IdTipoLicencia == 6){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        int diasSumados = 0; //Inicio el contador del while
                        int diasHabiles = 4; //Dias que sumare para dar los 5 dias de permiso
                        DateTime FechaFin = infoLicencia.FechaInicio;

                        //busco la fecha en que se termina su permiso de parernidad
                        while (diasSumados < diasHabiles)
                        {
                            FechaFin = FechaFin.AddDays(1); // Avanza un día

                            if (FechaFin.DayOfWeek != DayOfWeek.Saturday && FechaFin.DayOfWeek != DayOfWeek.Sunday)
                            {
                                diasSumados++;
                            }
                        }


                        
                        DateTime Reincorporacion = FechaFin.AddDays(1); // Avanza un día
                        //Busco la fecha en la que regresara a trabajar y reviso que no sea en fin de semana
                        while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                        }

                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                        prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                        prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                        prmtrs.Add(new SqlParameter("@FechaFin", FechaFin));
                        prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                        prmtrs.Add(new SqlParameter("@DiasUsados", 5));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                        {
                          
                        } 
                        TempData["MessageTitle"] = "¡Éxito!";
                        TempData["Message"] = "¡Solicitud generada con éxito";
                        TempData["MessageType"] = "success";
                        return RedirectToPage(new {accion = "solicitarLicencia"});
                    }
                    catch(Exception ex){
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage(new {accion = "solicitarLicencia"});
            }

            //Permisos por maternidad
            else if(infoLicencia.IdTipoLicencia == 7){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        //calculo los 90 dias de licencia
                        DateTime FechaFin = infoLicencia.FechaInicio.AddDays(89);
                        DateTime Reincorporacion = FechaFin.AddDays(1);


                        //Busco que el dia en el que regrese no sea sabado o domingo
                        while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                        }

                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                        prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                        prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                        prmtrs.Add(new SqlParameter("@FechaFin", FechaFin));
                        prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                        prmtrs.Add(new SqlParameter("@DiasUsados", 90));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                        {
                           
                        } 
                        TempData["MessageTitle"] = "¡Éxito!";
                        TempData["Message"] = "¡Solicitud generada con éxito";
                        TempData["MessageType"] = "success";
                        return RedirectToPage(new {accion = "solicitarLicencia"});
                    }
                    catch(Exception ex){
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage(new {accion = "solicitarLicencia"});
            }

            //Licencias sin goce de sueldo
            if(infoLicencia.IdTipoLicencia == 9){
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        
                        int LicenciasUsadas = 1;
                        DateTime Inicio = DateTime.Now;;
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetLicenciasSinGoceSueldo", prmtrs))
                        {
                            if (dtr.Read())
                            {
                                if (int.TryParse(dtr["TotalLicencias"]?.ToString(), out int result))
                                {
                                    LicenciasUsadas = result;
                                }
                            }

                        } 

                        if(LicenciasUsadas == 0){

                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                            using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetFechaInicioKardex", prmtrs))
                            {
                                if (dtr.Read())
                                {
                                    if (DateTime.TryParse(dtr["FechaInicio"]?.ToString(), out DateTime result))
                                    {
                                        Inicio = result;
                                    }
                                }

                            } 

                            DateTime FechaActual = DateTime.Now;
                            int Dias = 0;
                            
                            // Calcular años transcurridos
                            int AniosTranscurridos = FechaActual.Year - Inicio.Year;

                            // Ajustar si el aniversario no ha llegado este año
                            if (Inicio.AddYears(AniosTranscurridos) > FechaActual)
                            {
                                AniosTranscurridos--;
                            }

                            if (AniosTranscurridos == 0){
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡Aun no puedes solicitar licencias sin goce de sueldo!";
                                TempData["MessageType"] = "error";
                                return RedirectToPage(new {accion = "solicitarLicencia"});
                            }
                            else if(AniosTranscurridos == 1){
                                Dias = 14;
                            }
                            else if(AniosTranscurridos > 1 && AniosTranscurridos < 4){
                                Dias = 29;

                            }
                            else if(AniosTranscurridos >= 4){
                                Dias = 59;
                            }
                            
                            //calculo dias de licencia
                            DateTime FechaFin = infoLicencia.FechaInicio.AddDays(Dias);
                            DateTime Reincorporacion = FechaFin.AddDays(1);

                            //Busco que el dia en el que regrese no sea sabado o domingo
                            while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                            {
                                Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                            }

                            prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                            prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                            prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                            prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                            prmtrs.Add(new SqlParameter("@FechaFin", FechaFin));
                            prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                            prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                            prmtrs.Add(new SqlParameter("@DiasUsados", Dias + 1));
                            using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                            {
                               
                            } 
                            TempData["MessageTitle"] = "¡Éxito!";
                            TempData["Message"] = "¡Solicitud generada con éxito";
                            TempData["MessageType"] = "success";
                            return RedirectToPage(new {accion = "solicitarLicencia"});
                        }else{
                            TempData["MessageTitle"] = "¡Error!";
                            TempData["Message"] = "¡Tus licencias sin goce de sueldo se agotaron!";
                            TempData["MessageType"] = "error";
                            return RedirectToPage(new {accion = "solicitarLicencia"});
                        }
                        
                    }
                    catch (Exception ex){
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage(new {accion = "solicitarLicencia"});
            }

            //Permisos por fallecimiento de familiar, enfermedad de familiar, licencia medica
            else{
                using (SqlServer sqlConexion = new SqlServer()){
                    try{
                        int DiasTotalesPermiso = 0;

                            for (DateTime fecha  = infoLicencia.FechaInicio; fecha <= infoLicencia.FechaFin; fecha = fecha.AddDays(1))
                            {
                                // Si no es sábado ni domingo, contamos como día hábil
                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    DiasTotalesPermiso = DiasTotalesPermiso + 1;
                                }
                            }

                        DateTime Reincorporacion = infoLicencia.FechaFin.AddDays(1);
                        while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                        }

                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoLicencia.IdJefeDirecto));
                        prmtrs.Add(new SqlParameter("@IdTipoLicencia", infoLicencia.IdTipoLicencia));
                        prmtrs.Add(new SqlParameter("@FechaInicio", infoLicencia.FechaInicio));
                        prmtrs.Add(new SqlParameter("@FechaFin", infoLicencia.FechaFin));
                        prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", infoLicencia.Comentarios));
                        prmtrs.Add(new SqlParameter("@DiasUsados", DiasTotalesPermiso));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciaCrear", prmtrs))
                        {
                          
                        } 
                        TempData["MessageTitle"] = "¡Éxito!";
                        TempData["Message"] = "¡Solicitud generada con éxito";
                        TempData["MessageType"] = "success";
                        return RedirectToPage(new {accion = "solicitarLicencia"});
                    }
                    catch (Exception ex){
                        _logger.LogError(ex, "Hubo un error al crear la licencia");
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                        TempData["MessageType"] = "error";
                    }
                    finally{
                        sqlConexion.Desconectar();
                    }
                }
            }
            return RedirectToPage(new {accion = "solicitarLicencia"});
        }

        public IActionResult OnPostEnviarLicencia(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", 2));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasChangeEstatus", prmtrs))
                    {
                     
                    } 
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "¡Solicitud enviada con éxito";
                    TempData["MessageType"] = "success";
                    return RedirectToPage(new {accion = "solicitarLicencia"});
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Hubo un error al enviar la licencia");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al enviar tu solicitud!";
                    TempData["MessageType"] = "error";
                }
                finally{
                    sqlConexion.Desconectar();  
                }
            }
            return RedirectToPage(new {accion = "solicitarLicencia"});
        }


        public IActionResult OnPostEliminarLicencia(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasBorrar", prmtrs))
                    {
                      
                    } 
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "¡Solicitud eliminada con éxito";
                    TempData["MessageType"] = "success";
                    return RedirectToPage(new {accion = "solicitarLicencia"});
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Hubo un error al eliminar la licencia");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al eliminar tu solicitud!";
                    TempData["MessageType"] = "error";
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage(new {accion = "solicitarLicencia"});
        }

        public IActionResult OnPostConfirmarLicenciaJefe (int id, int tipoConfirmacion, string comentario){
            string tipo = "";
            tipo = "confirmacionLicenciaJefe";
            
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", tipoConfirmacion));
                    prmtrs.Add(new SqlParameter("@Comentarios", comentario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasConfirmar", prmtrs))
                    {
                     
                    } 
                    
                    return new JsonResult(new { success = true});
                }catch(Exception ex){
                    _logger.LogError(ex, "Hubo un error al confirmar la licencia");
                   
                    return new JsonResult(new { success = false });
                }finally{
                    sqlConexion.Desconectar();  
                }
            }
            return RedirectToPage(new {accion = tipo});
        }

        public IActionResult OnPostConfirmarLicenciaRHGerente(int id, int tipoConfirmacion, string comentario)
        {
            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

            if (idPuesto == 5 || idPuesto == 3)
            {
                string tipo = "";
                tipo = "confirmacionLicencia";

                using (SqlServer sqlConexion = new SqlServer())
                {
                    try
                    {
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdLicencia", id));
                        prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", tipoConfirmacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", comentario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasConfirmar", prmtrs))
                        {

                        }

                        return new JsonResult(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Hubo un error al confirmar la licencia");

                        return new JsonResult(new { success = false });
                    }
                    finally
                    {
                        sqlConexion.Desconectar();
                    }
                }
                return RedirectToPage(new { accion = tipo });
            }
            else
            {
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes acceso a esta función.";
                TempData["MessageType"] = "error"; 
                return RedirectToPage("/Index");
            }

        }

        public IActionResult OnGetDetalleLicencia(int id)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetById", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            Licencia = construirLicenciaDetalle(dtr);
                        }

                        return new JsonResult(new
                        {
                            id = Licencia.Id,
                            idJefe = Licencia.IdJefe,
                            fechaSolicitud = Licencia.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = Licencia.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = Licencia.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = Licencia.Reincorporacion.ToString("yyyy-MM-dd"),
                            idTipoLicencia = Licencia.IdTipoLicencia,
                            comentarios = Licencia.Comentarios,
                            comentariosJefe = Licencia.ComentariosJefe,
                            comentariosRH = Licencia.ComentariosRH,
                            comentariosGerente = Licencia.ComentariosGerente,
                            estatus = Licencia.Estatus,
                            nombreUsuario = Licencia.NombreCompletoUsuario, 
                            nombreJefe = Licencia.NombreCompletoJefe,


                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hubo un error al buscar la licencia");
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        private LicenciaDetalle construirLicenciaDetalle (DataTableReader licencia){
            return new LicenciaDetalle()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                IdJefe = int.Parse(licencia["IdJefe"].ToString()),
                IdTipoLicencia = int.Parse(licencia["IdTipoLicencia"].ToString()),
                Comentarios = licencia["Comentarios"].ToString(),
                ComentariosJefe = licencia["ComentariosJefe"].ToString(),
                ComentariosRH = licencia["ComentariosRH"].ToString(),
                ComentariosGerente = licencia["ComentariosGerente"].ToString(),
                IdEstatus = int.Parse(licencia["IdEstatus"].ToString()),
                Estatus = licencia["Estatus"].ToString(),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"].ToString()),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),
                /* NombreCompletoJefe = licencia["NombreCompletoJefe"].ToString(), 
                NombreCompletoUsuario = licencia ["NombreCompletoUsuario"].ToString(), */
            };
        }

        public IActionResult OnGetDetalleLicenciaConfirmarJefe(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetByIdParaConfirmarJefe", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            LicenciaJefe = construirLicenciaDetalleJefe(dtr);
                        }

                        return new JsonResult(new
                        {
                            id = LicenciaJefe.Id,
                            usuario = LicenciaJefe.Usuario,
                            idJefe = LicenciaJefe.IdJefe,
                            tipoLicencia = LicenciaJefe.TipoPermiso, 
                            fechaSolicitud = LicenciaJefe.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = LicenciaJefe.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = LicenciaJefe.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = LicenciaJefe.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = LicenciaJefe.Comentarios,
                            comentatios = LicenciaJefe.ComentariosJefe,
                            estatus = LicenciaJefe.Estatus,

                            /* nombreUsuario = LicenciaJefe.NombreCompletoUsuario, 
                            nombreJefe = LicenciaJefe.NombreCompletoJefe, */
                        });
                    }
                }
                catch(Exception ex){
                    _logger.LogError(ex, "Hubo un error al buscar la licencia");
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private LicenciaDetalleJefe construirLicenciaDetalleJefe (DataTableReader licencia){
            return new LicenciaDetalleJefe()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                Usuario = licencia["Usuario"].ToString(),
                IdJefe = int.Parse(licencia["IdJefe"].ToString()),
                TipoPermiso = licencia["TipoLicencia"].ToString(),
                Comentarios = licencia["Comentarios"].ToString(),
                ComentariosJefe = licencia["ComentariosJefe"].ToString(),
                Estatus = licencia["Estatus"].ToString(),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"].ToString()),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),

                /* NombreCompletoJefe = licencia["NombreCompletoJefe"].ToString(), 
                NombreCompletoUsuario = licencia ["NombreCompletoUsuario"].ToString(), */
            };
        }


        public IActionResult OnGetDetalleLicenciaConfirmarRHGerente(int id){
            idUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;
            string QuienAprueba = "flujo normal";
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdLicencia", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetByIdParaConfirmarRHyGerente", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            LicenciaRHGerente = construirLicenciaDetalleGerente(dtr);
                        }

                        if (LicenciaRHGerente.IdJefe == idUsuario)
                        {
                            if (idPuesto == 5)
                            {
                                QuienAprueba = "Gerente es Jefe Directo";
                            }
                            else if (idPuesto == 3)
                            {
                                QuienAprueba = "RH es Jefe Directo";
                            }
                            
                        }
                        
                        return new JsonResult(new
                        {
                            id = LicenciaRHGerente.Id,
                            usuario = LicenciaRHGerente.Usuario,
                            jefe = LicenciaRHGerente.Jefe,
                            idJefe = LicenciaRHGerente.IdJefe,
                            tipoLicencia = LicenciaRHGerente.TipoPermiso, 
                            fechaSolicitud = LicenciaRHGerente.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = LicenciaRHGerente.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = LicenciaRHGerente.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = LicenciaRHGerente.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = LicenciaRHGerente.Comentarios,
                            comentariosJefe = LicenciaRHGerente.ComentariosJefe,
                            comentariosRH = LicenciaRHGerente.ComentariosRH,
                            comentariosGerente = LicenciaRHGerente.ComentariosGerente,
                            estatus = LicenciaRHGerente.Estatus,
                            quienAprueba = QuienAprueba,
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hubo un error al buscar la licencia");
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        private LicenciaDetalleRhGerente construirLicenciaDetalleGerente (DataTableReader licencia){
            return new LicenciaDetalleRhGerente()
            {
                Id = int.Parse(licencia["Id"].ToString()),
                Usuario = licencia["Usuario"].ToString(),
                IdJefe = int.Parse(licencia["IdJefe"].ToString()),
                Jefe = licencia["Jefe"].ToString(),
                TipoPermiso = licencia["TipoLicencia"].ToString(),
                Comentarios = licencia["Comentarios"].ToString(),
                ComentariosJefe = licencia["ComentariosJefe"].ToString(),
                ComentariosRH = licencia["ComentariosRH"].ToString(),
                ComentariosGerente = licencia["ComentariosGerente"].ToString(),
                Estatus = licencia["Estatus"].ToString(),
                FechaSolicitud = Convert.ToDateTime(licencia["FechaSolicitud"].ToString()),
                FechaInicio = Convert.ToDateTime(licencia["FechaInicio"]),
                FechaFin = Convert.ToDateTime(licencia["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(licencia["Reincorporacion"]),
            };
        }

        private void GetVacaciones(int año, int mes){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    Año = año;
                    Mes = mes;
                    Permiso = "solicitarVacacion";

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetCollectionByUsuario", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            Vacaciones vacacion = construirVacacion(dtr);

                            // Verifica si la fecha de inicio es mayor a hoy
                            if (vacacion.FechaInicio <= DateTime.Now && (vacacion.IdEstatus == 1 || vacacion.IdEstatus == 2 || vacacion.IdEstatus == 3 || vacacion.IdEstatus == 6))
                            {
                                // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                List<SqlParameter> parametros = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdVacacion", vacacion.Id),
                                    new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                };
                                sqlConexion.EjecutarStoreProcedure("dbo.VacacionesChangeEstatus", parametros);
                            }

                            vacaciones.Add(vacacion);
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }


        private void GetVacacionesParaConfirmarJefeDirecto(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    Permiso = "confirmacionVacacionJefe";

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetCollectionByJefeDirecto", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            Vacaciones vacacion = construirVacacion(dtr);

                            // Verifica si la fecha de inicio es mayor a hoy
                            if (vacacion.FechaInicio <= DateTime.Now )
                            {
                                // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                List<SqlParameter> parametros = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdVacacion", vacacion.Id),
                                    new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                };
                                sqlConexion.EjecutarStoreProcedure("dbo.VacacionesChangeEstatus", parametros);
                            }

                            vacaciones.Add(vacacion);
                        }
                    }
                }
                catch(Exception ex){

                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }
        
        private Vacaciones construirVacacion(DataTableReader vacacion){
            return new Vacaciones()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                IdJefe = int.Parse(vacacion["IdJefe"].ToString()),
                Estatus = vacacion["Estatus"].ToString(),
                IdEstatus = int.Parse(vacacion["IdEstatus"].ToString()),
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),

            };
        }

        private void GetVacacionesParaConfirmarRHandGerente(int año, int mes)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    Año = año;
                    Mes = mes;
                    Permiso = "confirmacionVacacion";

                    int IdUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
                    int IdPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Year", año));
                    prmtrs.Add(new SqlParameter("@Mes", mes));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetCollectionConfirmarRHyGerente", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            if (IdPuesto == 5)
                            {
                                Vacaciones vacacion = construirVacacionGerente(dtr, IdUsuario);

                                // Verifica si la fecha de inicio es mayor a hoy
                                if (vacacion.FechaInicio <= DateTime.Now && (vacacion.IdEstatus == 2 || vacacion.IdEstatus == 3 || vacacion.IdEstatus == 6))
                                {
                                    // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                    List<SqlParameter> parametros = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdVacacion", vacacion.Id),
                                        new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                    };
                                    sqlConexion.EjecutarStoreProcedure("dbo.VacacionesChangeEstatus", parametros);
                                }

                                vacaciones.Add(vacacion);
                            }
                            else if (IdPuesto == 3)
                            {
                                Vacaciones vacacion = construirVacacionRH(dtr, IdUsuario);

                                // Verifica si la fecha de inicio es mayor a hoy
                                if (vacacion.FechaInicio <= DateTime.Now && (vacacion.IdEstatus == 2 || vacacion.IdEstatus == 3 || vacacion.IdEstatus == 6))
                                {
                                    // Ejecuta otro procedimiento almacenado para cambiar el estatus
                                    List<SqlParameter> parametros = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdVacacion", vacacion.Id),
                                        new SqlParameter("@IdEstatusPermisoLaboral", 5)
                                    };
                                    sqlConexion.EjecutarStoreProcedure("dbo.VacacionesChangeEstatus", parametros);
                                }

                                vacaciones.Add(vacacion);
                            }
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

        private Vacaciones construirVacacionRH(DataTableReader vacacion, int idUsuarioSesion){
            int idJefe = int.Parse(vacacion["IdJefe"].ToString());
            int idEstatus = int.Parse(vacacion["IdEstatus"].ToString());

            return new Vacaciones()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                IdJefe = idJefe,
                Estatus = vacacion["Estatus"].ToString(),
                IdEstatus = idEstatus,
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),

                Pendiente = (idEstatus == 3 || (idJefe == idUsuarioSesion && idEstatus == 2))
            };
        }
        
        private Vacaciones construirVacacionGerente(DataTableReader vacacion, int idUsuarioSesion){
            int idJefe = int.Parse(vacacion["IdJefe"].ToString());
            int idEstatus = int.Parse(vacacion["IdEstatus"].ToString());

            return new Vacaciones()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                IdJefe = idJefe,
                Estatus = vacacion["Estatus"].ToString(),
                IdEstatus = idEstatus,
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),

                Pendiente = ((idEstatus == 6 || (idJefe == idUsuarioSesion && idEstatus == 2)))
            };
        }

        public IActionResult OnPostRegistroVacaciones()
        {
            idUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));

                    int DiasUsados = 0;
                    DateTime FechaInicio = DateTime.Now;
                    DateTime FechaActual = DateTime.Now;
                    int DiasDisponibles = 0;

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetFechaInicioKardex", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            if (DateTime.TryParse(dtr["FechaInicio"]?.ToString(), out DateTime resultado))
                            {
                                FechaInicio = resultado;
                            }
                        }

                    }

                    // Calcular años transcurridos
                    int AniosTranscurridos = FechaActual.Year - FechaInicio.Year;

                    // Ajustar si el aniversario no ha llegado este año
                    if (FechaInicio.AddYears(AniosTranscurridos) > FechaActual)
                    {
                        AniosTranscurridos--;
                    }

                    if (AniosTranscurridos == 0)
                    {
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡Aun no tienes vacaciones!";
                        TempData["MessageType"] = "error";
                        return RedirectToPage(new { accion = "solicitarVacacion" });
                    }

                    if (AniosTranscurridos == 1)
                    {
                        DiasDisponibles = 12;
                    }

                    if (AniosTranscurridos == 2)
                    {
                        DiasDisponibles = 14;
                    }

                    if (AniosTranscurridos == 3)
                    {
                        DiasDisponibles = 16;
                    }

                    if (AniosTranscurridos == 4)
                    {
                        DiasDisponibles = 18;
                    }

                    if (AniosTranscurridos == 5)
                    {
                        DiasDisponibles = 20;
                    }

                    if (AniosTranscurridos > 5 && AniosTranscurridos < 11)
                    {
                        DiasDisponibles = 22;
                    }

                    if (AniosTranscurridos > 10 && AniosTranscurridos < 16)
                    {
                        DiasDisponibles = 24;
                    }

                    if (AniosTranscurridos > 15 && AniosTranscurridos < 21)
                    {
                        DiasDisponibles = 26;
                    }

                    if (AniosTranscurridos > 20 && AniosTranscurridos < 26)
                    {
                        DiasDisponibles = 28;
                    }

                    if (AniosTranscurridos >= 26)
                    {
                        DiasDisponibles = 30;
                    }

                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetDiasVacacionesByUsuario", prmtrs))
                    {
                        if (dtr.Read())
                        {
                            if (int.TryParse(dtr["DiasUsados"]?.ToString(), out int result))
                            {
                                DiasUsados = result;
                            }
                        }
                    }

                    DiasDisponibles = DiasDisponibles - DiasUsados;

                    int DiasTotalesPermiso = 0;

                    for (DateTime fecha = infoVacacion.FechaInicio; fecha <= infoVacacion.FechaFin; fecha = fecha.AddDays(1))
                    {
                        // Si no es sábado ni domingo, contamos como día hábil
                        if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                        {
                            //Si el permiso se pide un viernes se contaran dos días, tambien se tomarta en cuenta el sabado
                            if (fecha.DayOfWeek == DayOfWeek.Friday)
                            {
                                DiasTotalesPermiso = DiasTotalesPermiso + 2;
                            }
                            else
                            {
                                DiasTotalesPermiso = DiasTotalesPermiso + 1;
                            }
                        }
                    }

                    if (DiasTotalesPermiso <= DiasDisponibles)
                    {

                        // busco si ya tiene vacaciones en las fechas que solicito 
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetFechasVacacionesByUsuario", prmtrs))
                        {
                            while (dtr.Read())
                            {
                                fechasVacaciones.Add(construirFechasVacaciones(dtr));
                            }
                        }

                        foreach (var fecha in fechasVacaciones)
                        {
                            if ((infoVacacion.FechaInicio >= fecha.FechaInicio && infoVacacion.FechaInicio <= fecha.FechaFin) || (infoVacacion.FechaFin >= fecha.FechaInicio && infoVacacion.FechaFin <= fecha.FechaFin))
                            {
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡Ya solicitaste vacaciones en estas fechas!";
                                TempData["MessageType"] = "error";
                                return RedirectToPage(new { accion = "solicitarVacacion" });
                            }
                        }

                        // busco si tiene licencias en las fechas que solicito
                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.LicenciasGetFechasLicenciasByUsuario", prmtrs))
                        {
                            while (dtr.Read())
                            {
                                fechasLicencias.Add(construirFechasLicencias(dtr));
                            }
                        }

                        foreach (var fecha in fechasLicencias)
                        {
                            if ((infoVacacion.FechaInicio >= fecha.FechaInicio && infoVacacion.FechaInicio <= fecha.FechaFin) || (infoVacacion.FechaFin >= fecha.FechaInicio && infoVacacion.FechaFin <= fecha.FechaFin))
                            {
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡Tienes licencias solicitadas en estas fechas!";
                                TempData["MessageType"] = "error";
                                return RedirectToPage(new { accion = "solicitarVacacion" });
                            }
                        }

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DiasFestivosGetCollection", prmtrs))
                        {
                            while (dtr.Read())
                            {
                                diasFestivos.Add(construirDiasFestivos(dtr));
                            }
                        }

                        foreach (var festivo in diasFestivos)
                        {
                            if (festivo.Fecha >= infoVacacion.FechaInicio && festivo.Fecha <= infoVacacion.FechaFin)
                            {
                                TempData["MessageTitle"] = "¡Error!";
                                TempData["Message"] = "¡No puedes solicitar vacaciones en días festivos!";
                                TempData["MessageType"] = "error";
                                return RedirectToPage(new { accion = "solicitarVacacion" });
                            }
                        }

                        DateTime Reincorporacion = infoVacacion.FechaFin.AddDays(1);
                        while (Reincorporacion.DayOfWeek == DayOfWeek.Saturday || Reincorporacion.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Reincorporacion = Reincorporacion.AddDays(1); // Avanza un día si es sábado o domingo
                        }

                        prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                        prmtrs.Add(new SqlParameter("@IdJefeDirecto", infoVacacion.IdJefeDirecto));
                        prmtrs.Add(new SqlParameter("@FechaInicio", infoVacacion.FechaInicio));
                        prmtrs.Add(new SqlParameter("@FechaFin", infoVacacion.FechaFin));
                        prmtrs.Add(new SqlParameter("@FechaReincorporacion", Reincorporacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", infoVacacion.Comentarios));
                        prmtrs.Add(new SqlParameter("@DiasUsados", DiasTotalesPermiso));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesCrear", prmtrs))
                        {

                        }
                        TempData["MessageTitle"] = "¡Éxito!";
                        TempData["Message"] = "¡Solicitud generada con éxito";
                        TempData["MessageType"] = "success";
                        return RedirectToPage(new { accion = "solicitarVacacion" });
                    }
                    else
                    {
                        TempData["MessageTitle"] = "¡Error!";
                        TempData["Message"] = "¡No tienes sufucientes días disponibles!";
                        TempData["MessageType"] = "error";
                        return RedirectToPage(new { accion = "solicitarVacacion" });
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hubo un error al crear la Vacacion");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al guardar tu solicitud!";
                    TempData["MessageType"] = "error";
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage(new { accion = "solicitarVacacion" });
        }

        private DiasFestivos construirDiasFestivos(DataTableReader dia){
            return new DiasFestivos()
            {
                Fecha = Convert.ToDateTime(dia["Fecha"]),
            };
        }

        private FechasVacaciones construirFechasVacaciones(DataTableReader fecha){
            return new FechasVacaciones()
            {
                FechaInicio = Convert.ToDateTime(fecha["FechaInicio"]),
                FechaFin = Convert.ToDateTime(fecha["FechaFin"]),
            };
        }

        private FechasLicencias construirFechasLicencias(DataTableReader fecha){
            return new FechasLicencias()
            {
                FechaInicio = Convert.ToDateTime(fecha["FechaInicio"]),
                FechaFin = Convert.ToDateTime(fecha["FechaFin"]),
            };
        }

        public IActionResult OnPostEnviarVacacion(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", 2));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesChangeEstatus", prmtrs))
                    {
                    
                    } 
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "¡Solicitud enviada con éxito";
                    TempData["MessageType"] = "success";
                    return RedirectToPage(new {accion = "solicitarVacacion"});
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Hubo un error al enviar la licencia");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al enviar tu solicitud!";
                    TempData["MessageType"] = "error";
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage(new {accion = "solicitarVacacion"});
        }

        public IActionResult OnPostEliminarVacacion(int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesBorrar", prmtrs))
                    {
                      
                    } 
                    TempData["MessageTitle"] = "¡Éxito!";
                    TempData["Message"] = "¡Solicitud eliminada con éxito";
                    TempData["MessageType"] = "success";
                    return RedirectToPage(new {accion = "solicitarVacacion"});
                }
                catch (Exception ex){
                    _logger.LogError(ex, "Hubo un error al eliminar la licencia");
                    TempData["MessageTitle"] = "¡Error!";
                    TempData["Message"] = "¡Ocurrio un error al eliminar tu solicitud!";
                    TempData["MessageType"] = "error";
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
            return RedirectToPage(new {accion = "solicitarVacacion"});
        }


        public IActionResult OnPostConfirmarVacacionJefe(int id, int tipoConfirmacion, string comentario){
            string tipo = "";
            tipo = "confirmacionVacacionJefe";

            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", tipoConfirmacion));
                    prmtrs.Add(new SqlParameter("@Comentarios", comentario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesConfirmar", prmtrs))
                    {
                     
                    } 
                    
                    return new JsonResult(new { success = true});
                }catch(Exception ex){
                    _logger.LogError(ex, "Hubo un error al confirmar la vacacion");
                    
                    return new JsonResult(new { success = false });
                }finally{
                    sqlConexion.Desconectar();  
                }
            }

            return RedirectToPage(new {accion = tipo});
        }


        public IActionResult OnPostConfirmarVacacionRHGerente(int id, int tipoConfirmacion, string comentario){
            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;

            if (idPuesto == 5 || idPuesto == 3)
            {
                string tipo = "";
                tipo = "confirmacionVacacion";

                using (SqlServer sqlConexion = new SqlServer())
                {
                    try{
                        sqlConexion.Conectar(conexion);
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdVacacion", id));
                        prmtrs.Add(new SqlParameter("@IdEstatusPermisoLaboral", tipoConfirmacion));
                        prmtrs.Add(new SqlParameter("@Comentarios", comentario));
                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesConfirmar", prmtrs))
                        {
                        
                        } 
                        
                        return new JsonResult(new { success = true});
                    }catch(Exception ex){
                        _logger.LogError(ex, "Hubo un error al confirmar la vacacion");
                        
                        return new JsonResult(new { success = false });
                    }finally{
                        sqlConexion.Desconectar();  
                    }
                }

                return RedirectToPage(new {accion = tipo});
            }else{
                TempData["MessageTitle"] = "¡Error!";
                TempData["Message"] = "No tienes acceso a esta función.";
                TempData["MessageType"] = "error"; 
                return RedirectToPage("/Index");
            } 
        }
        

        public IActionResult OnGetDetalleVacacion( int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetById", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            Vacacion = construirVacacionDetalle(dtr);
                        }

                        return new JsonResult(new
                        {
                            id = Vacacion.Id,
                            fechaSolicitud = Vacacion.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = Vacacion.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = Vacacion.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = Vacacion.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = Vacacion.Comentarios,
                            comentariosJefe = Vacacion.ComentariosJefe,
                            comentariosRH = Vacacion.ComentariosRH,
                            comentariosGerente = Vacacion.ComentariosGerente,
                            idJefeDirecto = Vacacion.IdJefe,
                            diasSolicitados = Vacacion.DiasSolicitados,
                            estatus = Vacacion.Estatus,
                            nombreUsuario = Vacacion.NombreUsuario, 
                            nombreJefe = Vacacion.NombreJefe,
                            departamento = Vacacion.Departamento,
                            puesto = Vacacion.Puesto,
                           /*  periodoVacacional = Vacacion.PeriodoVacacional, */
                
                            

                        });
                    }
                }
                catch(Exception ex){
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private VacacionDetalle construirVacacionDetalle (DataTableReader vacacion){
            return new VacacionDetalle()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                IdJefe = int.Parse(vacacion["IdJefe"].ToString()),
                Comentarios = vacacion["Comentarios"].ToString(),
                ComentariosJefe = vacacion["ComentariosJefe"].ToString(),
                ComentariosRH = vacacion["ComentariosRH"].ToString(),
                ComentariosGerente = vacacion["ComentariosGerente"].ToString(),
                Estatus = vacacion["Estatus"].ToString(),
                IdEstatus = int.Parse(vacacion["IdEstatus"].ToString()),
                DiasSolicitados = int.Parse(vacacion["DiasSolicitados"].ToString()),
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
                Departamento = vacacion["NombreDepartamento"].ToString(),
                Puesto = vacacion["NombrePuesto"].ToString(),
                /* PeriodoVacacional = vacacion["NombrePeriodoVacacional"].ToString() */
                
            };
        }


        public IActionResult OnGetDetalleVacacionConfirmarJefe( int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetByIdConfirmarJefe", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            VacacionJefe = construirVacacionDetalleJefe(dtr);
                        }

                        return new JsonResult(new
                        {
                            id = VacacionJefe.Id,
                            usuario = VacacionJefe.Usuario,
                            fechaSolicitud = VacacionJefe.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = VacacionJefe.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = VacacionJefe.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = VacacionJefe.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = VacacionJefe.Comentarios,
                            comentariosJefe = VacacionJefe.ComentariosJefe,
                            idJefeDirecto = VacacionJefe.IdJefe,
                            diasUsados = VacacionJefe.DiasUsados,
                            estatus = VacacionJefe.Estatus,
                            /* idEstatus = VacacionJefe.IdEstatus, */

                        });
                    }
                }
                catch(Exception ex){
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private VacacionDetalleJefe construirVacacionDetalleJefe (DataTableReader vacacion){
            return new VacacionDetalleJefe()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                Usuario = vacacion["Usuario"].ToString(),
                IdJefe = int.Parse(vacacion["IdJefe"].ToString()),
                Comentarios = vacacion["Comentarios"].ToString(),
                ComentariosJefe = vacacion["ComentariosJefe"].ToString(),
                Estatus = vacacion["Estatus"].ToString(),
                /* IdEstatus = int.Parse(vacacion["IdEstatus"].ToString()), */
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
            };
        }


        public IActionResult OnGetDetalleVacacionConfirmarRHGerente( int id){
            idUsuario = HttpContext.Session.GetInt32("idUsuario") ?? 0;
            idPuesto = HttpContext.Session.GetInt32("idPuesto") ?? 0;
            string QuienAprueba = "flujo normal";
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdVacacion", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetByIdConfirmarRHyGerente", prmtrs))
                    {
                        if(dtr.Read())
                        {
                            VacacionRhGerente = construirVacacionDetalleRHGerente(dtr);
                        }

                        if(VacacionRhGerente.IdJefe == idUsuario)
                        {
                            if (idPuesto == 5)
                            {
                                QuienAprueba = "Gerente es Jefe Directo";
                            }
                            else if (idPuesto == 3)
                            {
                                QuienAprueba = "RH es Jefe Directo";
                            }
                        }

                        return new JsonResult(new
                        {
                            id = VacacionRhGerente.Id,
                            usuario = VacacionRhGerente.Usuario,
                            jefe = VacacionRhGerente.Jefe,
                            fechaSolicitud = VacacionRhGerente.FechaSolicitud.ToString("yyyy-MM-dd"),
                            fechaInicio = VacacionRhGerente.FechaInicio.ToString("yyyy-MM-dd"),
                            fechaFin = VacacionRhGerente.FechaFin.ToString("yyyy-MM-dd"),
                            reincorporacion = VacacionRhGerente.Reincorporacion.ToString("yyyy-MM-dd"),
                            comentarios = VacacionRhGerente.Comentarios,
                            comentariosJefe = VacacionRhGerente.ComentariosJefe,
                            comentariosRH = VacacionRhGerente.ComentariosRH,
                            comentariosGerente = VacacionRhGerente.ComentariosGerente,
                            idJefeDirecto = VacacionRhGerente.IdJefe,
                            diasUsados = VacacionRhGerente.DiasUsados,
                            estatus = VacacionRhGerente.Estatus,
                            /* nombreUsuario = VacacionRhGerente.NombreUsuario, */
                            /* nombreJefe = VacacionRhGerente.NombreJefe, */
                            /* departamento = Vacacion.Departamento, */
                            /* puesto = Vacacion.Puesto, */
                            /* periodoVacacional = Vacacion.PeriodoVacacional, */
                            quienAprueba = QuienAprueba,
                        });
                    }
                }
                catch(Exception ex){
                    return new JsonResult(new { error = "Error al obtener los detalles: " + ex.Message });
                }
                finally{
                    sqlConexion.Desconectar();
                }
            }
        }

        private VacacionDetalleRhGerente construirVacacionDetalleRHGerente (DataTableReader vacacion){
            return new VacacionDetalleRhGerente()
            {
                Id = int.Parse(vacacion["Id"].ToString()),
                Usuario = vacacion["Usuario"].ToString(),
                Jefe = vacacion["Jefe"].ToString(),
                IdJefe = int.Parse(vacacion["IdJefe"].ToString()),
                Comentarios = vacacion["Comentarios"].ToString(),
                ComentariosJefe = vacacion["ComentariosJefe"].ToString(),
                ComentariosRH = vacacion["ComentariosRH"].ToString(),
                ComentariosGerente = vacacion["ComentariosGerente"].ToString(),
                Estatus = vacacion["Estatus"].ToString(),
                DiasUsados = int.Parse(vacacion["DiasUsados"].ToString()),
                FechaSolicitud = Convert.ToDateTime(vacacion["FechaSolicitud"]),
                FechaInicio = Convert.ToDateTime(vacacion["FechaInicio"]),
                FechaFin = Convert.ToDateTime(vacacion["FechaFin"]),
                Reincorporacion = Convert.ToDateTime(vacacion["Reincorporacion"]),
                /* NombreJefe = vacacion["NombreCompletoJefe"].ToString(),  */
                /* NombreUsuario = vacacion ["NombreCompletoUsuario"].ToString(), */
                /* Departamento = vacacion["NombreDepartamento"].ToString(), */
                /* Puesto = vacacion["NombrePuesto"].ToString(), */
                /* PeriodoVacacional = vacacion["NombrePeriodoVacacional"].ToString() */
            };
        }



        private void GetDiasVacaciones (int id){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try{
                    sqlConexion.Conectar(conexion);

                    DateTime FechaInicio = DateTime.Now;
                    DateTime FechaActual = DateTime.Now;

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdUsuario", id));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetFechaInicioKardex",prmtrs))
                    {
                        if( dtr.Read())
                        {
                            if (DateTime.TryParse(dtr["FechaInicio"]?.ToString(), out DateTime resultado))
                            {
                                FechaInicio = resultado;
                            }
                        }
                    }
                    // Calcular años transcurridos
                    int AniosTranscurridos = FechaActual.Year - FechaInicio.Year;

                    // Ajustar si el aniversario no ha llegado este año
                    if (FechaInicio.AddYears(AniosTranscurridos) > FechaActual)
                    {
                        AniosTranscurridos--;
                    }

                    if (AniosTranscurridos == 0){
                        DiasDisponiblesVacaciones = 0;
                    }

                    if (AniosTranscurridos == 1){
                        DiasDisponiblesVacaciones = 12;
                    }

                    if (AniosTranscurridos == 2){
                        DiasDisponiblesVacaciones = 14;
                    }

                    if (AniosTranscurridos == 3){
                        DiasDisponiblesVacaciones = 16;
                    }

                    if (AniosTranscurridos == 4){
                        
                        DiasDisponiblesVacaciones = 18;
                    }

                    if (AniosTranscurridos == 5){
                        DiasDisponiblesVacaciones = 20;
                    }

                    if (AniosTranscurridos > 5 && AniosTranscurridos < 11){
                        DiasDisponiblesVacaciones = 22;
                    }

                    if (AniosTranscurridos > 10 && AniosTranscurridos < 16){
                        DiasDisponiblesVacaciones = 24;
                    }

                    if (AniosTranscurridos > 15 && AniosTranscurridos < 21){
                        DiasDisponiblesVacaciones = 26;
                    }

                    if (AniosTranscurridos > 20 && AniosTranscurridos < 26){
                        DiasDisponiblesVacaciones = 28;
                    }

                    if (AniosTranscurridos >= 26){
                        DiasDisponiblesVacaciones = 30;
                    }

                    int DiasUsados = 0;
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.VacacionesGetDiasVacacionesByUsuario",prmtrs))
                    {
                        if(dtr.Read())
                        {
                            if (int.TryParse(dtr["DiasUsados"]?.ToString(), out int result))
                            {
                                DiasUsados = result;
                            }
                        }
                    }
                    DiasDisponiblesVacaciones = DiasDisponiblesVacaciones - DiasUsados;
                }
                catch(Exception ex){
                    throw new Exception("Hubo un error al obtener los dia de vacaciones del usuario", ex);
                }finally{
                    sqlConexion.Desconectar();
                }
            }
        }
         
    }
}