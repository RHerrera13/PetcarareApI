using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CAPA_ENTIDADES;

namespace CAPA_DATOS
{
    public class CD_REGISTROSMEDICOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_REGISTROSMEDICOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar registro
        public void INSERTAR_REGISTRO(CE_REGISTROSMEDICOS registro)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_RegistrosMedicos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = registro.EventoID;
            cmd.Parameters.Add("@TipoTratamiento", SqlDbType.NVarChar, 50).Value = registro.TipoTratamiento;
            cmd.Parameters.Add("@VeterinarioNombre", SqlDbType.NVarChar, 100).Value = registro.VeterinarioNombre ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = registro.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = registro.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro médico: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar registro
        public void ACTUALIZAR_REGISTRO(CE_REGISTROSMEDICOS registro, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_RegistrosMedicos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = registro.RegistroID;
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = registro.EventoID;
            cmd.Parameters.Add("@TipoTratamiento", SqlDbType.NVarChar, 50).Value = registro.TipoTratamiento;
            cmd.Parameters.Add("@VeterinarioNombre", SqlDbType.NVarChar, 100).Value = registro.VeterinarioNombre ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = registro.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = registro.EstadoID;
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
                throw new Exception("Error al actualizar el registro médico: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar registro
        public void ELIMINAR_REGISTRO(CE_REGISTROSMEDICOS registro)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_RegistrosMedicos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = registro.RegistroID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el registro médico: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar registros
        public List<CE_REGISTROSMEDICOS> LISTAR_REGISTROS()
        {
            List<CE_REGISTROSMEDICOS> lista = new List<CE_REGISTROSMEDICOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_RegistrosMedicos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_REGISTROSMEDICOS fila = new CE_REGISTROSMEDICOS
                    {
                        RegistroID = Convert.ToInt32(dr["RegistroID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        TipoTratamiento = Convert.ToString(dr["TipoTratamiento"]),
                        VeterinarioNombre = dr["VeterinarioNombre"] is DBNull ? string.Empty : Convert.ToString(dr["VeterinarioNombre"]),
                        Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los registros médicos: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar registro por ID
        public CE_REGISTROSMEDICOS BUSCAR_REGISTRO_POR_ID(int registroId)
        {
            CE_REGISTROSMEDICOS registro = null;

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_RegistrosMedicos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = registroId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        registro = new CE_REGISTROSMEDICOS
                        {
                            RegistroID = Convert.ToInt32(reader["RegistroID"]),
                            EventoID = Convert.ToInt32(reader["EventoID"]),
                            TipoTratamiento = Convert.ToString(reader["TipoTratamiento"]),
                            VeterinarioNombre = reader["VeterinarioNombre"] is DBNull ? string.Empty : Convert.ToString(reader["VeterinarioNombre"]),
                            Descripcion = reader["Descripcion"] is DBNull ? string.Empty : Convert.ToString(reader["Descripcion"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el registro médico: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return registro;
        }
        #endregion
    }
}
