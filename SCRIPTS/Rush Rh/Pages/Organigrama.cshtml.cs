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
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace Rush_Rh.Pages
{
    public class Organigrama : PageModel
    {
        string conexion = "";

        private readonly ILogger<Organigrama> _logger;

        public bool admin { get; private set; } = false;

        
        //Envio de datos
        [BindProperty]
        public Departamentos departamentoRecibido { get; set; }




        //Llenar listas
        public List<Departamentos> departamentos = new List<Departamentos>();

        public List<Puestos> puestos = new List<Puestos>();



        public List<UsuarioNombre> usuarios = new List<UsuarioNombre>();

        public Organigrama(ILogger<Organigrama> logger, IConfiguration configuration)
        {
            _logger = logger;
            conexion = configuration.GetValue<string>("ConnectionStrings:Database");

        }


        private void es_admin() {
            int? puestoUsuario = HttpContext.Session.GetInt32("idPuesto");
            admin = (puestoUsuario == 3 || puestoUsuario == 5);
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


        private bool esta_logeado(){
            int? idusuario = HttpContext.Session.GetInt32("idUsuario");
            int usuario = idusuario ?? 0;
            if(usuario == 0){
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
            es_admin();
            
            var resultado = esta_logeado();
            if (!resultado) return RedirectToPage("/Index"); // Si no está logeado, redirige inmediatamente.

            return Page();
        }




        public JsonResult OnGetOrganigrama()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();
                    List<object> organigramaRecuperado = new List<object>(); // Lista con formato correcto

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetOrganigramaCollection", parametros))
                    {
                        while (dtr.Read())
                        {
                            var nodo = new
                            {
                                id = dtr.GetInt32(dtr.GetOrdinal("Id")),  
                                idPadre = dtr.IsDBNull(dtr.GetOrdinal("IdPadre")) ? (int?)null : dtr.GetInt32(dtr.GetOrdinal("IdPadre")),  
                                idUsuario = dtr.IsDBNull(dtr.GetOrdinal("IdUsuario")) ? (long?)null : dtr.GetInt64(dtr.GetOrdinal("IdUsuario")),  
                                nombre = dtr.IsDBNull(dtr.GetOrdinal("Nombre")) ? null : dtr.GetString(dtr.GetOrdinal("Nombre")),  
                                puesto = dtr.IsDBNull(dtr.GetOrdinal("NombrePuesto")) ? null : dtr.GetString(dtr.GetOrdinal("NombrePuesto")),  
                                idPuesto = dtr.IsDBNull(dtr.GetOrdinal("IdPuesto")) ? (int?)null : dtr.GetInt32(dtr.GetOrdinal("IdPuesto")),  
                                descripcionPuesto = dtr.IsDBNull(dtr.GetOrdinal("DescripcionPuesto")) ? null : dtr.GetString(dtr.GetOrdinal("DescripcionPuesto")),  
                                departamento = dtr.IsDBNull(dtr.GetOrdinal("NombreDepartamento")) ? null : dtr.GetString(dtr.GetOrdinal("NombreDepartamento")),  
                                idDepartamento = dtr.IsDBNull(dtr.GetOrdinal("IdDepartamento")) ? (int?)null : dtr.GetInt32(dtr.GetOrdinal("IdDepartamento")),  
                                img = dtr.IsDBNull(dtr.GetOrdinal("DocumentoURLFotoPerfil"))
                                    ? (dtr.GetString(dtr.GetOrdinal("Nombre")) == "Vacante"
                                        ? "../assets/Icon_ASK.png"
                                        : (!dtr.IsDBNull(dtr.GetOrdinal("Tipo")) && dtr.GetString(dtr.GetOrdinal("Tipo")) == "departamento"
                                            ? "../assets/departamentoDefault.png"
                                            : (dtr.GetString(dtr.GetOrdinal("Tipo")) == "empleado"
                                                ? "../assets/Icon-Perfil-Usuario1.png"
                                                : null
                                            )
                                        )
                                    )
                                    : dtr.GetString(dtr.GetOrdinal("DocumentoURLFotoPerfil")),
                                idImg = dtr.IsDBNull(dtr.GetOrdinal("IdDocumentoFotoPerfil")) ? (long?)null : dtr.GetInt64(dtr.GetOrdinal("IdDocumentoFotoPerfil")),  
                                tipo = dtr.GetString(dtr.GetOrdinal("Tipo")),  
                                URLDocumentoDescripcionPuesto = dtr.IsDBNull(dtr.GetOrdinal("DocumentoURLPuesto")) ? null : dtr.GetString(dtr.GetOrdinal("DocumentoURLPuesto")),  
                                idDocumentoDescripcionPuesto = dtr.IsDBNull(dtr.GetOrdinal("IdDocumentoPuesto")) ? (long?)null : dtr.GetInt64(dtr.GetOrdinal("IdDocumentoPuesto")),  
                            };

                            organigramaRecuperado.Add(nodo);
                        }
                    }

                    return new JsonResult(new
                    {
                        success = true,
                        nodes = organigramaRecuperado
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener el organigrama");
                    return new JsonResult(new { success = false, message = "Error al obtener el organigrama" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



        public IActionResult OnGetExportarOrganigrama()
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;

            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();
                    List<dynamic> organigramaRecuperado = new List<dynamic>();

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetOrganigramaCollection", parametros))
                    {
                        while (dtr.Read())
                        {
                            var tipo = dtr.IsDBNull(dtr.GetOrdinal("Tipo")) ? "" : dtr.GetString(dtr.GetOrdinal("Tipo"));
                            if (tipo == "empleado")
                            {
                                organigramaRecuperado.Add(new
                                {
                                    id = dtr.GetInt32(dtr.GetOrdinal("Id")),
                                    idPadre = dtr.IsDBNull(dtr.GetOrdinal("IdPadre")) ? (int?)null : dtr.GetInt32(dtr.GetOrdinal("IdPadre")),
                                    nombre = dtr.IsDBNull(dtr.GetOrdinal("Nombre")) ? "" : dtr.GetString(dtr.GetOrdinal("Nombre")),
                                    puesto = dtr.IsDBNull(dtr.GetOrdinal("NombrePuesto")) ? "" : dtr.GetString(dtr.GetOrdinal("NombrePuesto")),
                                    descripcionPuesto = dtr.IsDBNull(dtr.GetOrdinal("DescripcionPuesto")) ? "" : dtr.GetString(dtr.GetOrdinal("DescripcionPuesto")),
                                    departamento = dtr.IsDBNull(dtr.GetOrdinal("NombreDepartamento")) ? "" : dtr.GetString(dtr.GetOrdinal("NombreDepartamento")),
                                    tipo = tipo
                                });
                            }
                        }
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Organigrama");

                        // Agregar encabezados
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Nombre";
                        worksheet.Cell(1, 3).Value = "Puesto";
                        worksheet.Cell(1, 4).Value = "Descripción Puesto";
                        worksheet.Cell(1, 5).Value = "Departamento";
                        worksheet.Cell(1, 6).Value = "Tipo";
                        worksheet.Cell(1, 7).Value = "Jefe Directo";

                        // Agregar datos al Excel
                        int fila = 2;
                        foreach (var nodo in organigramaRecuperado)
                        {
                            worksheet.Cell(fila, 1).Value = fila - 1;
                            worksheet.Cell(fila, 2).Value = nodo.nombre;
                            worksheet.Cell(fila, 3).Value = nodo.puesto;
                            worksheet.Cell(fila, 4).Value = nodo.descripcionPuesto;
                            worksheet.Cell(fila, 5).Value = nodo.departamento;
                            worksheet.Cell(fila, 6).Value = nodo.tipo;

                            // Buscar el jefe directo, solo si tiene idPadre
                            if (nodo.idPadre != null)
                            {
                                var currentNode = organigramaRecuperado.FirstOrDefault(n => n.id == nodo.idPadre);
                                
                                // Si el jefe es un departamento, buscar hasta encontrar un jefe que sea empleado
                                while (currentNode != null && currentNode.tipo == "departamento")
                                {
                                    currentNode = organigramaRecuperado.FirstOrDefault(n => n.id == currentNode.idPadre);
                                }

                                // Si encontramos un jefe, agregarlo al Excel
                                worksheet.Cell(fila, 7).Value = currentNode?.nombre ?? "Sin jefe";
                            }
                            else
                            {
                                worksheet.Cell(fila, 7).Value = "Sin jefe"; // Nodo raíz sin jefe directo
                            }

                            fila++;
                        }


                        // Ajustar columnas automáticamente
                        worksheet.Columns().AdjustToContents();

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Position = 0; // Asegurar que la posición esté en el inicio

                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Organigrama.xlsx");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al exportar el organigrama a Excel");
                    return BadRequest("Error al generar el archivo Excel");
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



        //UNo que solo devuelva departamentos
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




         public JsonResult OnGetPuestos(){
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();


                    // Llamar al procedimiento almacenado para obtener los datos de los puestos
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.GetPuestosCollection", parametros))
                    {
                        while (dtr.Read())
                        {
                            Puestos puesto = new Puestos
                            {
                                Id = dtr.GetInt32(dtr.GetOrdinal("id")),
                                Nombre = dtr.GetString(dtr.GetOrdinal("nombre")),
                                IdDepartamento = dtr.GetInt32(dtr.GetOrdinal("idDepartamento")),
                                DescripcionPuesto = dtr["DescripcionPuesto"] != DBNull.Value ? dtr["DescripcionPuesto"].ToString() : null,
                                DescripcionPuestoDocumento =  dtr["IdDocumento"] != DBNull.Value ? new Documento { Id = Convert.ToInt32(dtr["IdDocumento"]), URL = dtr["DocumentoURLPuesto"]?.ToString() } : null,
                            };
                            puestos.Add(puesto);
                        }
                    }
                    return new JsonResult(new
                    {
                        puestos = puestos
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los puestos");
                    return new JsonResult(new { success = false, message = "Error al obtener los puestos" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
         }


        public JsonResult OnGetUsuarios()
        {
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();


                    // Llamar al procedimiento almacenado para obtener los datos de los usuarios
                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.UsuariosGetAllNombre", parametros))
                    {
                        while (dtr.Read())
                        {
                            UsuarioNombre usuario = new UsuarioNombre
                            {
                                Id = dtr.GetInt64(dtr.GetOrdinal("id")),
                                Nombre = dtr.GetString(dtr.GetOrdinal("nombre")),
                            };
                            usuarios.Add(usuario);
                        }
                    }
                    return new JsonResult(new
                    {
                        usuarios = usuarios
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los usuarios");
                    return new JsonResult(new { success = false, message = "Error al obtener los usuarios" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        //--------------------------------------------------------------------------------FUnciones propias

        private int subirDocumento(IFormFile archivo, int idTipoDocumento, long? idUsuario, string nombreCarpetaGuardar, SqlServer sqlConexion, SqlTransaction transaction)
        {
            
            var filePath = "";
            int? idDocumento = null;

            try
            {
                if (archivo != null)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/uploads/{nombreCarpetaGuardar}");
                    Directory.CreateDirectory(uploadsPath); // Asegura que la ruta existe

                    // Usa FileName para obtener el nombre del archivo
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(archivo.FileName)}";
                    filePath = Path.Combine(uploadsPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        archivo.CopyTo(fileStream); // Copia el archivo al destino
                    }

                    List<System.Data.SqlClient.SqlParameter> prmtrs = new List<System.Data.SqlClient.SqlParameter>();
                    prmtrs.Add(new SqlParameter("@IdTipoDocumento", idTipoDocumento));
                    prmtrs.Add(new SqlParameter("@DireccionDocumento", $"/uploads/{nombreCarpetaGuardar}/{uniqueFileName}"));
                    prmtrs.Add(new SqlParameter("@IdUsuario", idUsuario));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoCrear", prmtrs, transaction))
                    {
                        if (dtr.Read())
                        { 
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                            {
                                idDocumento = result;
                            }
                        }
                        else
                        {
                            throw new Exception("No se pudo obtener el ID del documento.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Eliminar el archivo guardado
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _logger.LogError(ex, "Error al subir documento");
            }

            return idDocumento ?? 0;
        }





        //--------------------------------------------------------------------------------ENVIO DE DATOS


        
        public IActionResult OnPostGuardarDepartamento([FromBody] Departamentos departamentoRecibido)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@NombreDepartamento", departamentoRecibido.Nombre));

                    sqlConexion.EjecutarStoreProcedure("dbo.DepartamentoCrear", parametros);

                    return new JsonResult(new { success = true, message = "Departamento guardado correctamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el departamento");
                    return new JsonResult(new { success = false, message = "Error al guardar el departamento" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }

        public IActionResult OnPostEditarDepartamento([FromBody] Departamentos departamentoRecibido)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@Id", departamentoRecibido.Id));
                    parametros.Add(new SqlParameter("@NombreDepartamento", departamentoRecibido.Nombre));

                    sqlConexion.EjecutarStoreProcedure("dbo.DepartamentoEditar", parametros);

                    return new JsonResult(new { success = true, message = "Departamento guardado correctamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el departamento");
                    return new JsonResult(new { success = false, message = "Error al guardar el departamento" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostEliminarDepartamento([FromBody] Departamentos departamentoRecibido)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    if(departamentoRecibido.Id == 2 || departamentoRecibido.Id == 3)
                        return new JsonResult(new { success = false, message = "No se puede eliminar el departamento" });

                    parametros.Add(new SqlParameter("@Id", departamentoRecibido.Id));

                    sqlConexion.EjecutarStoreProcedure("dbo.DepartamentoEliminar", parametros);

                    return new JsonResult(new { success = true, message = "Departamento eliminado correctamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al eliminar el departamento");
                    return new JsonResult(new { success = false, message = "Error al eliminar el departamento" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostGuardarPuesto([FromForm] Puestos puesto)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {

                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción    

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@NombrePuesto", puesto.Nombre));
                    parametros.Add(new SqlParameter("@IdDepartamento", puesto.IdDepartamento));
                    parametros.Add(new SqlParameter("@DescripcionPuesto", puesto.DescripcionPuesto));


                    if (puesto.DescripcionPuestoDocumento?.Archivo != null && puesto.DescripcionPuestoDocumento.Archivo.Length > 0)
                    {
                        int idDocumento = subirDocumento(puesto.DescripcionPuestoDocumento.Archivo, puesto.DescripcionPuestoDocumento.IdTipoDocumento, null, "puestos", sqlConexion, transaction);
                        if (idDocumento == 0)
                        {
                            throw new Exception($"No se pudo guardar el documento en la ruta");
                        }
                        parametros.Add(new SqlParameter("@IdDocumento", idDocumento));
                    } 
                    else 
                    {
                        parametros.Add(new SqlParameter("@IdDocumento", DBNull.Value));
                    }

                    sqlConexion.EjecutarReaderStoreProcedure("dbo.PuestoCrear", parametros, transaction);
                    transaction.Commit();

                    return new JsonResult(new { success = true, message = "Puesto guardado correctamente" });
                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error
                    _logger.LogError(ex, "Error al guardar el puesto");
                    return new JsonResult(new { success = false, message = "Error al guardar el puesto" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostEditarPuesto([FromForm] Puestos puesto)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {

                    //NO LLEGA SI ELIMINMADO ES TRUE O FALSE, NO HE PROBADO SI GUARDA PUESTOS

                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción    


                    int idTipoDocumento = puesto.DescripcionPuestoDocumento.IdTipoDocumento;
                    long? idDocumento = null;

                   //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                    if(puesto.DescripcionPuestoDocumento.Archivo != null){
                        //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                        if (puesto.DescripcionPuestoDocumento.Id < 0 || puesto.DescripcionPuestoDocumento.Id == 0)
                        {
                            idDocumento = subirDocumento(puesto.DescripcionPuestoDocumento.Archivo, idTipoDocumento, null, "puestos", sqlConexion, transaction);
                            if (idDocumento == 0)
                            {
                                throw new Exception($"No se pudo guardar el documento en la ruta");
                            }
                            //Elimina el documento anterior si es que existia
                            if(puesto.DescripcionPuestoDocumento.Id < 0){//quiere decir que si existia
                            
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", (puesto.DescripcionPuestoDocumento.Id*-1)) //LO convertimos a positivo
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            }
                        }
                        else //Si viene en posityivo y no lo tengo que cambiar solo lo asigno
                        {
                            idDocumento = puesto.DescripcionPuestoDocumento.Id;
                        }
                    } else { //Si no se subio un archivo nuevo puede que si exista uno ya, checamos si fue eliminado o no
                        if (puesto.DescripcionPuestoDocumento.Eliminado){
                            // Eliminar el documento asociado al datosPersonales
                            var parametrosDocumento = new List<SqlParameter>
                            {
                                new SqlParameter("@IdDocumento", puesto.DescripcionPuestoDocumento.Id)
                            };

                            idDocumento = null; //Si se elimino el documento asigno null para despues que haga la actualizacion

                            sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                        } else {
                            if (puesto.DescripcionPuestoDocumento.Id > 0){ //Si recibo un id es porque si habia un archivo
                                idDocumento = puesto.DescripcionPuestoDocumento.Id; //Si no se elimino lo mantengo
                            }
                        }
                    } 

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@IdPuesto", puesto.Id));
                    parametros.Add(new SqlParameter("@NombrePuesto", puesto.Nombre));
                    parametros.Add(new SqlParameter("@IdDepartamento", puesto.IdDepartamento));
                    parametros.Add(new SqlParameter("@DescripcionPuesto", puesto.DescripcionPuesto));
                    parametros.Add(new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value)); //Si es null se envia como DBNull


                    sqlConexion.EjecutarReaderStoreProcedure("dbo.PuestoEditar", parametros, transaction);
                    transaction.Commit();

                    return new JsonResult(new { success = true, message = "Puesto guardado correctamente" });
                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error
                    _logger.LogError(ex, "Error al guardar el puesto");
                    return new JsonResult(new { success = false, message = "Error al guardar el puesto" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }




        public IActionResult OnPostEliminarPuesto([FromForm] Puestos puesto, [FromForm] bool eliminarCardex)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {
                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción    

                    List<SqlParameter> parametros = new List<SqlParameter>();

                    long? idDocumento = null;

                    if(puesto.Id == 3 || puesto.Id == 5)
                        return new JsonResult(new { success = false, message = "No se puede eliminar el puesto" });


                    parametros.Add(new SqlParameter("@Id", puesto.Id));

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PuestoEliminar", parametros, transaction))
                    {
                        if (dtr.Read()) 
                        {
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["IdDocumento"]?.ToString(), out int result))
                            {
                                idDocumento = result;
                            }

                            if (idDocumento != null)
                            {
                                // Eliminar el documento asociado al estudio
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", idDocumento)
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            }
                        }
                    }

                    if(eliminarCardex){
                        List<SqlParameter> parametrosPuesto = new List<SqlParameter>();
                        parametrosPuesto.Add(new SqlParameter("@IdPuesto", puesto.Id));
                        //Eliminar Cardex con el puesto eliminado, cambiar estatus usuario a baja temporal del que tuviera el cardex
                        sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexBajaTemporalWithPuesto", parametrosPuesto, transaction);
                    }

                    transaction.Commit();

                    return new JsonResult(new { success = true, message = "Puesto eliminado correctamente" });
                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error
                    _logger.LogError(ex, "Error al eliminar el puesto");
                    return new JsonResult(new { success = false, message = "Error al eliminar el puesto" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



        public IActionResult OnPostAgregarNodoEmpleado([FromBody] OrganigramaInfo nodo)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {

                    int? idPuesto = null;
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    //Si la opcion seleccionada fue vacante
                    if(nodo.IdUsuario == 0){
                        nodo.IdUsuario = null;
                        nodo.Nombre = "Vacante";
                    } 

                    //Recibiendo datos tal cual vienen
                    parametros.Add(new SqlParameter("@IdPadre", nodo.IdPadre ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@IdUsuario", nodo.IdUsuario ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@Nombre", nodo.Nombre ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@IdDepartamento", nodo.IdDepartamento?.Id ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@Tipo", nodo.Tipo ?? (object)DBNull.Value));

                 
                    //Si no existe el puesto lo agregamos
                    if(nodo.IdPuesto?.Nombre != null){
                        List<SqlParameter> parametrosPuestos = new List<SqlParameter>();
                        parametrosPuestos.Add(new SqlParameter("@NombrePuesto", nodo.IdPuesto.Nombre));
                        parametrosPuestos.Add(new SqlParameter("@IdDepartamento", nodo.IdDepartamento.Id));

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PuestoCrear", parametrosPuestos))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["IdPuesto"]?.ToString(), out int result))
                                {
                                    idPuesto = result;
                                }
                            }
                        }   

                    } else {
                        idPuesto = nodo.IdPuesto.Id;
                    }

                    parametros.Add(new SqlParameter("@IdPuesto", idPuesto ?? (object)DBNull.Value));


                    sqlConexion.EjecutarStoreProcedure("dbo.OrganigramaNodoCrear", parametros);

                    return new JsonResult(new { success = true, message = "Empleado guardado correctamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el empleado");
                    return new JsonResult(new { success = false, message = "Error al guardar el empleado" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }


        public IActionResult OnPostAgregarNodoDepartamento([FromBody] OrganigramaInfo nodo)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    int? idDepartamento = null;
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    //Recibiendo datos tal cual vienen
                    parametros.Add(new SqlParameter("@IdPadre", nodo.IdPadre ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@Tipo", nodo.Tipo ?? (object)DBNull.Value));


                    //Si no existe el departamento lo agregamos
                    if(nodo.IdDepartamento?.Nombre != null){
                        List<SqlParameter> parametrosDepartamento = new List<SqlParameter>();
                        parametrosDepartamento.Add(new SqlParameter("@NombreDepartamento", nodo.IdDepartamento.Nombre));

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DepartamentoCrear", parametrosDepartamento))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["IdDepartamento"]?.ToString(), out int result))
                                {
                                    idDepartamento = result;
                                }
                            }
                        }   

                    } else {
                        idDepartamento = nodo.IdDepartamento.Id;
                    }

                    parametros.Add(new SqlParameter("@IdDepartamento", idDepartamento ?? (object)DBNull.Value));


                    sqlConexion.EjecutarStoreProcedure("dbo.OrganigramaNodoCrear", parametros);


                    return new JsonResult(new { success = true, message = "Departamento guardado correctamente" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el departamento");
                    return new JsonResult(new { success = false, message = "Error al guardar el departamento" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }




        public IActionResult OnPostEliminarRama([FromBody] List<int> nodos)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using var sqlConexion = new SqlServer();
            try
            {
                sqlConexion.Conectar(conexion);

                // Crear un string de IDs separados por comas
                string ids = string.Join(",", nodos.Select(id => id.ToString())); 

                var parametros = new List<SqlParameter>
                {
                    new("@Ids", ids)
                };

                sqlConexion.EjecutarStoreProcedure("dbo.OrganigramaNodoEliminarMultiple", parametros);

                return new JsonResult(new { success = true, message = "Rama eliminada correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la rama");
                return new JsonResult(new { success = false, message = "Error al eliminar la rama", error = ex.Message });
            }
            finally
            {
                sqlConexion.Desconectar();
            }
        }



        public IActionResult OnPostEliminarNodo([FromBody] int idNodo)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using var sqlConexion = new SqlServer();
            try
            {
                sqlConexion.Conectar(conexion);

                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    new SqlParameter("@IdEliminar", idNodo)
                };

                sqlConexion.EjecutarStoreProcedure("dbo.OrganigramaNodoEliminar", parametros);

                return new JsonResult(new { success = true, message = "Nodo eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el nodo");
                return new JsonResult(new { success = false, message = "Error al eliminar el nodo" });
            }
            finally
            {
                sqlConexion.Desconectar();
            }
        }




        public IActionResult OnPostEditarNodoEmpleado([FromForm] OrganigramaInfo nodo, [FromForm] bool jefeActualizado)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {

                    //NO LLEGA SI ELIMINMADO ES TRUE O FALSE

                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción    


                    int idTipoDocumento = nodo.IdDocumento.IdTipoDocumento;
                    long? idDocumento = null;
                    int? idPuesto = null;


                    //Si la opcion seleccionada fue vacante
                    if(nodo.IdUsuario == 0){
                        idDocumento = null;
                        nodo.IdUsuario = null;
                        nodo.Nombre = "Vacante";
                    }  else {

                    //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                        if(nodo.IdDocumento.Archivo != null){
                            //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                            if (nodo.IdDocumento.Id < 0 || nodo.IdDocumento.Id == 0)
                            {
                                idDocumento = subirDocumento(nodo.IdDocumento.Archivo, idTipoDocumento, null, "nodos", sqlConexion, transaction);
                                if (idDocumento == 0)
                                {
                                    throw new Exception($"No se pudo guardar el documento en la ruta");
                                }
                                //Elimina el documento anterior si es que existia
                                if(nodo.IdDocumento.Id < 0){//quiere decir que si existia
                                
                                    var parametrosDocumento = new List<SqlParameter>
                                    {
                                        new SqlParameter("@IdDocumento", (nodo.IdDocumento.Id*-1)) //LO convertimos a positivo
                                    };

                                    sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                                }
                            }
                            else //Si viene en posityivo y no lo tengo que cambiar solo lo asigno
                            {
                                idDocumento = nodo.IdDocumento.Id;
                            }
                        } else { 
                            if (nodo.IdDocumento.Id > 0){ //Si recibo un id es porque si habia un archivo
                                idDocumento = nodo.IdDocumento.Id; //Si no se elimino lo mantengo
                            }
                            
                        } 

                    }




                    //Si no existe el puesto lo agregamos
                    if(nodo.IdPuesto?.Nombre != null){
                        List<SqlParameter> parametrosPuestos = new List<SqlParameter>();
                        parametrosPuestos.Add(new SqlParameter("@NombrePuesto", nodo.IdPuesto.Nombre));
                        parametrosPuestos.Add(new SqlParameter("@IdDepartamento", nodo.IdDepartamento.Id));

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.PuestoCrear", parametrosPuestos, transaction))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["IdPuesto"]?.ToString(), out int result))
                                {
                                    idPuesto = result;
                                }
                            }
                        }   

                    } else {
                        idPuesto = nodo.IdPuesto.Id;
                    }




                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@IdOrganigrama", nodo.Id));
                    parametros.Add(new SqlParameter("@IdUsuario", nodo.IdUsuario));
                    parametros.Add(new SqlParameter("@NombreNuevo", nodo.Nombre ?? (object)DBNull.Value));
                    parametros.Add(new SqlParameter("@IdPadre", nodo.IdPadre));
                    parametros.Add(new SqlParameter("@IdDepartamento", nodo.IdDepartamento.Id));
                    parametros.Add(new SqlParameter("@IdPuesto", idPuesto));
                    parametros.Add(new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value)); //Si es null se envia como DBNull
                    parametros.Add(new SqlParameter("@JefeActualizado", jefeActualizado));  


                    sqlConexion.EjecutarReaderStoreProcedure("dbo.OrganigramaNodoEmpleadoEditar", parametros, transaction);
                    transaction.Commit();

                    return new JsonResult(new { success = true, message = "Nodo Empleado actualizado correctamente" });
                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error
                    _logger.LogError(ex, "Error al actualizar el nodo empleado");
                    return new JsonResult(new { success = false, message = "Error al actualizar el nodo empleado" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



        public IActionResult OnPostEditarNodoDepartamento([FromForm] OrganigramaInfo nodo)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                SqlTransaction? transaction = null;
                try
                {

                    //NO LLEGA SI ELIMINMADO ES TRUE O FALSE

                    sqlConexion.Conectar(conexion);
                    transaction = sqlConexion.Conexion.BeginTransaction(); // Iniciar la transacción


                    int idTipoDocumento = nodo.IdDocumento.IdTipoDocumento;
                    long? idDocumento = null;
                    int? idDepartamento = null;


                    //Si se subio un archivo nuevo en el input agregamos el que lo sustituya
                    if(nodo.IdDocumento.Archivo != null){
                        //Si el documento que existia cambio, se sube el nuevo. Si es igual a 0 quiere decir que no habia documento (si es menor a 0 porque lo envio en negativo si cambio)
                        if (nodo.IdDocumento.Id < 0 || nodo.IdDocumento.Id == 0)
                        {
                            idDocumento = subirDocumento(nodo.IdDocumento.Archivo, idTipoDocumento, null, "nodos", sqlConexion, transaction);
                            if (idDocumento == 0)
                            {
                                throw new Exception($"No se pudo guardar el documento en la ruta");
                            }
                            //Elimina el documento anterior si es que existia
                            if(nodo.IdDocumento.Id < 0){//quiere decir que si existia
                            
                                var parametrosDocumento = new List<SqlParameter>
                                {
                                    new SqlParameter("@IdDocumento", (nodo.IdDocumento.Id*-1)) //LO convertimos a positivo
                                };

                                sqlConexion.EjecutarReaderStoreProcedure("dbo.DocumentoEliminar", parametrosDocumento, transaction);
                            }
                        }
                        else //Si viene en posityivo y no lo tengo que cambiar solo lo asigno
                        {
                            idDocumento = nodo.IdDocumento.Id;
                        }
                    } else { 
                        if (nodo.IdDocumento.Id > 0){ //Si recibo un id es porque si habia un archivo
                            idDocumento = nodo.IdDocumento.Id; //Si no se elimino lo mantengo
                        }
                        
                    } 


                    //Si no existe el departamento lo agregamos
                    if(nodo.IdDepartamento?.Nombre != null){
                        List<SqlParameter> parametrosDepartamento = new List<SqlParameter>();
                        parametrosDepartamento.Add(new SqlParameter("@NombreDepartamento", nodo.IdDepartamento.Nombre));

                        using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.DepartamentoCrear", parametrosDepartamento, transaction))
                        {
                            if (dtr.Read())
                            {
                                //INtenta convertir el valor a entero, si no se puede asigna null
                                if (int.TryParse(dtr["IdDepartamento"]?.ToString(), out int result))
                                {
                                    idDepartamento = result;
                                }
                            }
                        }   

                    } else {
                        idDepartamento = nodo.IdDepartamento.Id;
                    }


                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@IdOrganigrama", nodo.Id));
                    parametros.Add(new SqlParameter("@IdDepartamento", idDepartamento));
                    parametros.Add(new SqlParameter("@IdDocumento", idDocumento ?? (object)DBNull.Value)); //Si es null se envia como DBNull


                    sqlConexion.EjecutarReaderStoreProcedure("dbo.OrganigramaNodoDepartamentoEditar", parametros, transaction);

                    transaction.Commit();

                    return new JsonResult(new { success = true, message = "Nodo Departamento actualizado correctamente" });

                }
                catch (Exception ex)
                {
                    transaction?.Rollback(); // Revertir en caso de error
                    _logger.LogError(ex, "Error al actualizar el nodo departamento");
                    return new JsonResult(new { success = false, message = "Error al actualizar el nodo departamento" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }

            }
        }




        public IActionResult OnPostNumeroCardexPuesto([FromForm] Puestos puesto)
        {
            // Verificar autorización antes de ejecutar cualquier lógica
            var resultadoAutorizacion = autorizar();
            if (resultadoAutorizacion != null) 
                return resultadoAutorizacion;
            using (SqlServer sqlConexion = new SqlServer())
            {
                try
                {
                    sqlConexion.Conectar(conexion);
                    List<SqlParameter> parametros = new List<SqlParameter>();

                    parametros.Add(new SqlParameter("@IdPuesto", puesto.Id));
                    int numeroCardexPuesto = 0;

                    using (DataTableReader dtr = sqlConexion.EjecutarReaderStoreProcedure("dbo.CardexTotalExistenteWithPuesto", parametros))
                    {
                        if (dtr.Read())
                        {
                            //INtenta convertir el valor a entero, si no se puede asigna null
                            if (int.TryParse(dtr["numeroCardex"]?.ToString(), out int result))
                            {
                                numeroCardexPuesto = result;
                            }
                        }
                    }

                     return new JsonResult(new { success = true, message = "El puesto tiene un cardex asociado", numeroCardexPuesto = numeroCardexPuesto });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al comprobar el cardex del puesto");
                    return new JsonResult(new { success = false, message = "Error al comprobar el cardex del puesto" });
                }
                finally
                {
                    sqlConexion.Desconectar();
                }
            }
        }



                    













    }
}