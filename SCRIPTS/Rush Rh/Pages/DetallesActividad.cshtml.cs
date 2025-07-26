using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using static Rush_Rh.Models.Formularios;
using Rush_Rh.Models;
using RushtecRH.Clases;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
namespace Rush_Rh.Pages
{
    public class DetallesActividad : PageModel
    {
        private readonly ILogger<DetallesActividad> _logger;
        string conexion = "";


        // Para enlazar automáticamente desde la URL

        public long idIntegrante { get; set; }
        public long idActividad { get; set; }
        public int AnioActual { get; set; }
    
        public DateTime FechaMinima { get; set; }

        public DateTime FechaMaxima { get; set; }



        [BindProperty]
        public ActividadesPlaneacion actividades { get; set; }

        [BindProperty]
        public List<long> Alumnos { get; set; }

        [BindProperty]
        public long idActividadIntegrante { get; set; }


        public ActividadesPlaneacion actividadRecuperada = new ActividadesPlaneacion();

        public List<IntegrantesActividad> integrantesActividad = new List<IntegrantesActividad>();

        public List<UsuarioNombre> usuarios = new List<UsuarioNombre>(); 

        



        public DetallesActividad(ILogger<DetallesActividad> logger,  IConfiguration configuration)
        {
            _logger = logger;
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");

        }

        public void OnGet(long idActividad, int AnioActual, DateTime FechaMinima, DateTime FechaMaxima)
        {
            _logger.LogInformation($"idActividad: {idActividad}, AnioActual: {AnioActual}, FechaMinima: {FechaMinima}, FechaMaxima: {FechaMaxima}");
            this.idActividad = idActividad;
            this.AnioActual = AnioActual;
            this.FechaMinima = FechaMinima;
            this.FechaMaxima = FechaMaxima;

            // Aquí se recuperarán los detalles de la actividad
            ObtenerDetallesActividad(idActividad);  
            GetUsuarios();
            ObtenerIntegrantes();
        }


        public void messageAlert(string MessageTitle, string message, string type)
        {
            TempData["MessageTitle"] = MessageTitle;
            TempData["Message"] = message;
            TempData["MessageType"] = type; // Puede ser 'error', 'info', etc.
        }


        private void GetUsuarios(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAllNombre", prmtrs))
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


        //Obtener los integrantes de la actividad
        private void ObtenerIntegrantes(){
            using (SqlServer sqlConexion = new SqlServer()){
                try{
                    sqlConexion.Conectar(conexion);
                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdActividad", idActividad));
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.ActividadIntegrantesGetAll", prmtrs))
                    {
                        while (dtr.Read())
                        {
                            integrantesActividad.Add(construirIntegrante(dtr));
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

        private IntegrantesActividad construirIntegrante(DataTableReader integrante){
            return new IntegrantesActividad()
            {
                Id = int.Parse(integrante["IdIntegrante"].ToString()),
                IdActividad = int.Parse(integrante["IdActividad"].ToString()),
                IdUsuario = int.Parse(integrante["IdUsuario"].ToString()),
                Nombre = integrante["Nombre"].ToString(),
                RFC = integrante["RFC"].ToString(),
                NombrePuesto = integrante["NombrePuesto"].ToString(),
            };
        }



        protected ActividadesPlaneacion ObtenerDetallesActividad(long idActividad)
        {


            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<SqlParameter> parametros = new List<SqlParameter>
                    {
                        new SqlParameter("@idActividad", idActividad)
                    };

                    using (DataTableReader reader = sqlConexion.EjecutarReaderStoreProcedure("dbo.ActividadGetWithId", parametros))
                    {
                        while (reader.Read())
                        {
                            actividadRecuperada = ConstruirActividadDetalle(reader);
                          
                        }
                    }
                }
                catch (Exception ex){
                    _logger.LogError($"Error al obtener los detalles de la actividad: {ex.Message}");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }

            return actividadRecuperada;
        }       

        private ActividadesPlaneacion ConstruirActividadDetalle(DataTableReader reader)
        {
            ActividadesPlaneacion actividad = new ActividadesPlaneacion();
            actividad.Id = Convert.ToInt64(reader["Id"]);
            actividad.NombreActividad = reader["NombreActividad"].ToString();
            actividad.FechaHoraInicio = Convert.ToDateTime(reader["FechaHoraInicio"]);
            actividad.FechaHoraFin = Convert.ToDateTime(reader["FechaHoraFin"]);
            actividad.DC3 = Convert.ToBoolean(reader["DC3"]);
            actividad.IdPlaneacion = Convert.ToInt64(reader["IdPlaneacion"]);
            return actividad;
        }



        public IActionResult OnPostEditarActividad(long idActividad, int AnioActual, DateTime FechaMinima, DateTime FechaMaxima)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Id", actividades.Id));
                    prmtrs.Add(new SqlParameter("@NombreActividad", actividades.NombreActividad));
                    prmtrs.Add(new SqlParameter("@FechaHoraInicio", actividades.FechaHoraInicio));
                    prmtrs.Add(new SqlParameter("@FechaHoraFin", actividades.FechaHoraFin));
                    prmtrs.Add(new SqlParameter("@DC3", actividades.DC3));

                    sqlConexion.EjecutarStoreProcedure("dbo.ActividadesPlaneacionEditar", prmtrs);

                    messageAlert("¡Éxito!", "Actividad editada exitosamente!", "success");
                    return RedirectToPage("/DetallesActividad", new { idActividad = actividades.Id, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al editar la actividad");
                    messageAlert("¡Error!", "¡Ocurrió un error al editar la actividad!", "error");
                    return RedirectToPage("/DetallesActividad", new { idActividad = actividades.Id, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public IActionResult OnPostEliminarActividad(int AnioActual)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@Id", actividades.Id));


                    sqlConexion.EjecutarStoreProcedure("dbo.ActividadesPlaneacionEliminar", prmtrs);

                    messageAlert("¡Éxito!", "Actividad eliminada exitosamente!", "success");
                    return RedirectToPage("/PlaneacionActividades", new { anio = AnioActual, idPlaneacion = actividades.IdPlaneacion });
                }
                catch (Exception ex)
                { 
                    _logger.LogError(ex, "Error al eliminar la actividad");
                    messageAlert("¡Error!", "¡Ocurrió un error al eliminar la actividad!", "error");
                    return RedirectToPage("/PlaneacionActividades");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostAgregarIntegrante(long idActividad, int AnioActual, DateTime FechaMinima, DateTime FechaMaxima)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    foreach (var integranteId in Alumnos) {
                        List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                        prmtrs.Add(new SqlParameter("@IdUsuario", integranteId));
                        prmtrs.Add(new SqlParameter("@IdActividad", idActividadIntegrante));

                        sqlConexion.EjecutarStoreProcedure("dbo.ActividadIntegrantesCrear", prmtrs);
                    }

                    messageAlert("¡Éxito!", "Integrantes agregados exitosamente!", "success");
                    return RedirectToPage("/DetallesActividad", new { idActividad = idActividadIntegrante, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al agregar el integrante");
                    messageAlert("¡Error!", "¡Ocurrió un error al agregar el integrante!", "error");
                    return RedirectToPage("/DetallesActividad", new { idActividad = idActividadIntegrante, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostEliminarIntegrante(long idIntegrante, long idActividad, int AnioActual, DateTime FechaMinima, DateTime FechaMaxima)
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdIntegrante", idIntegrante));

                    sqlConexion.EjecutarStoreProcedure("dbo.ActividadIntegrantesEliminar", prmtrs);

                    messageAlert("¡Éxito!", "Integrante eliminado exitosamente!", "success");
                    return RedirectToPage("/DetallesActividad", new { idActividad = idActividad, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al eliminar el integrante");
                    messageAlert("¡Error!", "¡Ocurrió un error al eliminar el integrante!", "error");
                    return RedirectToPage("/DetallesActividad", new { idActividad = idActividadIntegrante, AnioActual = AnioActual, FechaMinima = FechaMinima, FechaMaxima = FechaMaxima});
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }





    }
}