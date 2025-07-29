using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CAPA_ENTIDADES;
using Azure.Core.Pipeline;

namespace CAPA_DATOS
{
    public class CD_ESTADOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_ESTADOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar estados
        public void INSERTAR_ESTADO(CE_ESTADOS obj)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Estados";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@NombreEstado", SqlDbType.VarChar, 20).Value = obj.NombreEstado;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 100).Value = obj.Descripcion;
            cmd.Parameters.Add("@CodigoEstado", SqlDbType.VarChar, 10).Value = obj.CodigoEstado;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el estado: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar estados
        public void ACTUALIZAR_ESTADO(CE_ESTADOS obj, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Estados";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = obj.EstadoID;
            cmd.Parameters.Add("@NombreEstado", SqlDbType.VarChar, 20).Value = obj.NombreEstado;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 100).Value = obj.Descripcion;
            cmd.Parameters.Add("@CodigoEstado", SqlDbType.VarChar, 10).Value = obj.CodigoEstado;
            cmd.Parameters.Add("@O_Numero", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@O_Mensage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                num = Convert.ToInt32(cmd.Parameters["@O_Numero"].Value);
                msg = Convert.ToString(cmd.Parameters["@O_Mensage"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar estado
        public void ELIMINAR_ESTADO(CE_ESTADOS obj)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Estados";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = obj.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el estado: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar estados
        public List<CE_ESTADOS> Listar_ESTADO()
        {
            List<CE_ESTADOS> lista_Estado = new List<CE_ESTADOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Estados";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_ESTADOS fila = new CE_ESTADOS
                    {
                        EstadoID = dr["EstadoID"] is DBNull ? 0 : Convert.ToInt32(dr["EstadoID"]),
                        NombreEstado = dr["NombreEstado"] is DBNull ? string.Empty : Convert.ToString(dr["NombreEstado"]),
                        Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]),
                        CodigoEstado = dr["CodigoEstado"] is DBNull ? string.Empty : Convert.ToString(dr["CodigoEstado"])
                    };

                    lista_Estado.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar el estado: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista_Estado;
        }
        #endregion

        #region Buscar estado por ID
        public CE_ESTADOS BUSCAR_ESTADO_POR_ID(int estadoId)
        {
            CE_ESTADOS estado = null;
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Estados";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = estadoId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estado = new CE_ESTADOS
                        {
                            EstadoID = reader["EstadoID"] is DBNull ? 0 : Convert.ToInt32(reader["EstadoID"]),
                            NombreEstado = reader["NombreEstado"] is DBNull ? string.Empty : Convert.ToString(reader["NombreEstado"]),
                            Descripcion = reader["Descripcion"] is DBNull ? string.Empty : Convert.ToString(reader["Descripcion"]),
                            CodigoEstado = reader["CodigoEstado"] is DBNull ? string.Empty : Convert.ToString(reader["CodigoEstado"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el estado: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return estado;
        }
        #endregion
    }
}
