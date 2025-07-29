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
    public class CD_USUARIOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_USUARIOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar usuario
        public void INSERTAR_USUARIO(CE_USUARIOS obj)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Usuarios";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = obj.NombreCompleto;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = obj.Email;
            cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = obj.Telefono ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 256).Value = obj.Contrasena;
            cmd.Parameters.Add("@FechaRegistro", SqlDbType.DateTime).Value = obj.FechaRegistro;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = obj.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar usuario
        public void ACTUALIZAR_USUARIO(CE_USUARIOS obj, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Usuarios";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = obj.UsuarioID;
            cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = obj.NombreCompleto;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = obj.Email;
            cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 20).Value = obj.Telefono ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 256).Value = obj.Contrasena;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = obj.EstadoID;
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
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar usuario
        public void ELIMINAR_USUARIO(CE_USUARIOS obj)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Usuarios";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = obj.UsuarioID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar usuarios
        public List<CE_USUARIOS> LISTAR_USUARIOS()
        {
            List<CE_USUARIOS> lista = new List<CE_USUARIOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Usuarios";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_USUARIOS fila = new CE_USUARIOS
                    {
                        UsuarioID = dr["UsuarioID"] is DBNull ? 0 : Convert.ToInt32(dr["UsuarioID"]),
                        NombreCompleto = dr["NombreCompleto"] is DBNull ? string.Empty : Convert.ToString(dr["NombreCompleto"]),
                        Email = dr["Email"] is DBNull ? string.Empty : Convert.ToString(dr["Email"]),
                        Telefono = dr["Telefono"] is DBNull ? string.Empty : Convert.ToString(dr["Telefono"]),
                        FechaRegistro = dr["FechaRegistro"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["FechaRegistro"]),
                        EstadoID = dr["EstadoID"] is DBNull ? 0 : Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar usuario por ID
        public CE_USUARIOS BUSCAR_USUARIO_POR_ID(int usuarioId)
        {
            CE_USUARIOS usuario = null;
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = "sp_Usuarios";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = usuarioId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new CE_USUARIOS
                        {
                            UsuarioID = reader["UsuarioID"] is DBNull ? 0 : Convert.ToInt32(reader["UsuarioID"]),
                            NombreCompleto = reader["NombreCompleto"] is DBNull ? string.Empty : Convert.ToString(reader["NombreCompleto"]),
                            Email = reader["Email"] is DBNull ? string.Empty : Convert.ToString(reader["Email"]),
                            Telefono = reader["Telefono"] is DBNull ? string.Empty : Convert.ToString(reader["Telefono"]),
                            FechaRegistro = reader["FechaRegistro"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["FechaRegistro"]),
                            EstadoID = reader["EstadoID"] is DBNull ? 0 : Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return usuario;
        }
        #endregion Buscar usuario por ID
    }
}
