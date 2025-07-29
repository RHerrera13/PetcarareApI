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
    public class CD_MASCOTAS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_MASCOTAS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar mascota
        public void INSERTAR_MASCOTA(CE_MASCOTAS mascota)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Mascotas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = mascota.UsuarioID;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = mascota.Nombre;
            cmd.Parameters.Add("@Especie", SqlDbType.NVarChar, 50).Value = mascota.Especie;
            cmd.Parameters.Add("@Raza", SqlDbType.NVarChar, 50).Value = mascota.Raza ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = mascota.FechaNacimiento;
            cmd.Parameters.Add("@Sexo", SqlDbType.NVarChar, 10).Value = mascota.Sexo ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Color", SqlDbType.NVarChar, 50).Value = mascota.Color ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Peso", SqlDbType.Decimal).Value = mascota.Peso;
            cmd.Parameters.Add("@FotoURL", SqlDbType.NVarChar, 255).Value = mascota.FotoURL ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = mascota.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = mascota.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar mascota
        public void ACTUALIZAR_MASCOTA(CE_MASCOTAS mascota, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Mascotas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = mascota.MascotaID;
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = mascota.UsuarioID;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = mascota.Nombre;
            cmd.Parameters.Add("@Especie", SqlDbType.NVarChar, 50).Value = mascota.Especie;
            cmd.Parameters.Add("@Raza", SqlDbType.NVarChar, 50).Value = mascota.Raza ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = mascota.FechaNacimiento;
            cmd.Parameters.Add("@Sexo", SqlDbType.NVarChar, 10).Value = mascota.Sexo ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Color", SqlDbType.NVarChar, 50).Value = mascota.Color ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Peso", SqlDbType.Decimal).Value = mascota.Peso;
            cmd.Parameters.Add("@FotoURL", SqlDbType.NVarChar, 255).Value = mascota.FotoURL ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = mascota.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = mascota.EstadoID;
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
                throw new Exception("Error al actualizar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar mascota
        public void ELIMINAR_MASCOTA(CE_MASCOTAS mascota)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Mascotas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = mascota.MascotaID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar mascotas
        public List<CE_MASCOTAS> LISTAR_MASCOTAS()
        {
            List<CE_MASCOTAS> lista = new List<CE_MASCOTAS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Mascotas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_MASCOTAS fila = new CE_MASCOTAS
                    {
                        MascotaID = Convert.ToInt32(dr["MascotaID"]),
                        UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                        Nombre = Convert.ToString(dr["Nombre"]),
                        Especie = Convert.ToString(dr["Especie"]),
                        Raza = dr["Raza"] is DBNull ? string.Empty : Convert.ToString(dr["Raza"]),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Sexo = dr["Sexo"] is DBNull ? string.Empty : Convert.ToString(dr["Sexo"]),
                        Color = dr["Color"] is DBNull ? string.Empty : Convert.ToString(dr["Color"]),
                        Peso = Convert.ToDecimal(dr["Peso"]),
                        FotoURL = dr["FotoURL"] is DBNull ? string.Empty : Convert.ToString(dr["FotoURL"]),
                        Notas = dr["Notas"] is DBNull ? string.Empty : Convert.ToString(dr["Notas"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar mascotas: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar mascota por ID
        public CE_MASCOTAS BUSCAR_MASCOTA_POR_ID(int mascotaId)
        {
            CE_MASCOTAS mascota = null;
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Mascotas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = mascotaId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        mascota = new CE_MASCOTAS
                        {
                            MascotaID = Convert.ToInt32(reader["MascotaID"]),
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            Especie = Convert.ToString(reader["Especie"]),
                            Raza = reader["Raza"] is DBNull ? string.Empty : Convert.ToString(reader["Raza"]),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            Sexo = reader["Sexo"] is DBNull ? string.Empty : Convert.ToString(reader["Sexo"]),
                            Color = reader["Color"] is DBNull ? string.Empty : Convert.ToString(reader["Color"]),
                            Peso = Convert.ToDecimal(reader["Peso"]),
                            FotoURL = reader["FotoURL"] is DBNull ? string.Empty : Convert.ToString(reader["FotoURL"]),
                            Notas = reader["Notas"] is DBNull ? string.Empty : Convert.ToString(reader["Notas"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar la mascota: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return mascota;
        }
        #endregion
    }
}
