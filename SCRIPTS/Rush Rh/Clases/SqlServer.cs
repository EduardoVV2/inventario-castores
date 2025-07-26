using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RushtecRH.Clases
{
    public class SqlServer:IDisposable
    {
        public string CadenaConexion { get; set; }
        public SqlConnection Conexion { get; set; }
        public bool EsReferencia { get; set; }





        public void Conectar()
        {
            this.Conexion = new SqlConnection(this.CadenaConexion);
            this.Conexion.Open();
        }

        public void Conectar(string CadenaConexion)
        {
            this.CadenaConexion = CadenaConexion;
            this.Conexion = new SqlConnection(CadenaConexion);
            this.Conexion.Open();
        }

        public void Conectar(SqlConnection Conexion)
        {
            this.Conexion = Conexion;
        }

        public void Desconectar()
        {
            this.Conexion.Close();
        }

        public void Dispose()
        {
            try
            {
                this.Conexion?.Dispose();
            }
            catch { /*do nothing*/ }
        }

        public DataSet EjecutarColeccionQuery(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            throw new NotImplementedException();
        }

        public DataSet EjecutarColeccionStoreProcedure(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            throw new NotImplementedException();
        }

        public void EjecutarNonQuery(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = this.Conexion;
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    cmd.Parameters.AddRange(Parametros.ToArray());
                }
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable EjecutarQuery(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            DataTable _tmp = new DataTable();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = this.Conexion;
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    cmd.Parameters.AddRange(Parametros.ToArray());
                }
                using (var data = new SqlDataAdapter(cmd))
                {
                    data.Fill(_tmp);
                }
            }
            return _tmp;
        }

        public DataTableReader EjecutarReaderQuery(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            DataTableReader _tmp;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = this.Conexion;
                cmd.CommandText = Query;
                if (Parametros != null)
                {
                    foreach (var par in Parametros)
                    {
                        cmd.Parameters.AddRange(Parametros.ToArray());
                    }
                }
                using (var reader = cmd.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(reader);
                        _tmp = dt.Clone().CreateDataReader();
                    }
                }
            }
            return _tmp;
        }

        public DataTableReader EjecutarReaderStoreProcedure(string Query, List<SqlParameter> Parametros, SqlTransaction? transaction = null)
        {
            DataTableReader tmp;
            using (var command = this.Conexion.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                


                // Asociar la transacción si se proporciona
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                using (var dr = command.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(dr);
                        tmp = dt.CreateDataReader();
                    }
                }
                if (Parametros != null)
                {
                    Parametros.Clear();
                    Parametros = null;
                }
            }
            return tmp;
        }


        public void EjecutarSP(string nombreSP, List<SqlParameter> parametros = null, SqlTransaction? transaction = null)
        {
            using (var command = new SqlCommand(nombreSP, Conexion))
            {
                command.CommandType = CommandType.StoredProcedure;
                
            
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                if (parametros != null)
                {
                    command.Parameters.AddRange(parametros.ToArray());
                }
                
                command.ExecuteNonQuery();
            }
        }




        public DataTable EjecutarDataStoreProcedure(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            DataTable tmp;
            using (var command = this.Conexion.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                using (var dr = command.ExecuteReader())
                {
                    using (var dt = new DataTable())
                    {
                        dt.Load(dr);
                        tmp = dt;
                    }
                }
                if (Parametros != null)
                {
                    Parametros.Clear();
                    Parametros = null;
                }
            }
            return tmp;
        }

        public void EjecutarStoreProcedure(string Query, List<System.Data.SqlClient.SqlParameter> Parametros)
        {
            using (var command = this.Conexion.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Query;
                if (Parametros != null)
                {
                    command.Parameters.AddRange(Parametros.ToArray());
                }
                command.ExecuteNonQuery();
            }
        }

        public void EstablecerCadenaConexion(string CadenaConexion)
        {
            this.CadenaConexion = CadenaConexion;
        }

        public void EstablecerConexion(SqlConnection Conexion)
        {
            this.Conexion = Conexion;
        }

        public object ObtenerConexion()
        {
            return Conexion;
        }

        public bool ObtenerEsreferencia()
        {
            return this.EsReferencia;
        }

        public void EstablecerEsreferencia(bool Valor)
        {
            this.EsReferencia = Valor;
        }
    }
}
