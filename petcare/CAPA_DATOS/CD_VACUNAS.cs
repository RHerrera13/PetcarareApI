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
    public class CD_VACUNAS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_VACUNAS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar vacuna
        public void INSERTAR_VACUNA(CE_VACUNAS vacuna)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Vacunas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = vacuna.MascotaID;
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = vacuna.RegistroID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = vacuna.Nombre;
            cmd.Parameters.Add("@FechaAplicacion", SqlDbType.Date).Value = vacuna.FechaAplicacion;
            cmd.Parameters.Add("@ProximaDosis", SqlDbType.Date).Value = vacuna.ProximaDosis ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = vacuna.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = vacuna.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la vacuna: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar vacuna
        public void ACTUALIZAR_VACUNA(CE_VACUNAS vacuna, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Vacunas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@VacunaID", SqlDbType.Int).Value = vacuna.VacunaID;
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = vacuna.MascotaID;
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = vacuna.RegistroID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = vacuna.Nombre;
            cmd.Parameters.Add("@FechaAplicacion", SqlDbType.Date).Value = vacuna.FechaAplicacion;
            cmd.Parameters.Add("@ProximaDosis", SqlDbType.Date).Value = vacuna.ProximaDosis ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = vacuna.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = vacuna.EstadoID;
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
                throw new Exception("Error al actualizar la vacuna: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar vacuna
        public void ELIMINAR_VACUNA(CE_VACUNAS vacuna)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Vacunas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@VacunaID", SqlDbType.Int).Value = vacuna.VacunaID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la vacuna: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar vacunas
        public List<CE_VACUNAS> LISTAR_VACUNAS()
        {
            List<CE_VACUNAS> lista = new List<CE_VACUNAS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Vacunas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);
                foreach (DataRow dr in tabla.Rows)
                {
                    CE_VACUNAS fila = new CE_VACUNAS
                    {
                        VacunaID = Convert.ToInt32(dr["VacunaID"]),
                        MascotaID = Convert.ToInt32(dr["MascotaID"]),
                        RegistroID = dr["RegistroID"] is DBNull ? null : Convert.ToInt32(dr["RegistroID"]),
                        Nombre = Convert.ToString(dr["Nombre"]),
                        FechaAplicacion = Convert.ToDateTime(dr["FechaAplicacion"]),
                        ProximaDosis = dr["ProximaDosis"] is DBNull ? null : Convert.ToDateTime(dr["ProximaDosis"]),
                        Notas = dr["Notas"] is DBNull ? string.Empty : Convert.ToString(dr["Notas"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las vacunas: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar vacuna por ID
        public CE_VACUNAS BUSCAR_VACUNA_POR_ID(int vacunaId)
        {
            CE_VACUNAS vacuna = null;

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Vacunas";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@VacunaID", SqlDbType.Int).Value = vacunaId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vacuna = new CE_VACUNAS
                        {
                            VacunaID = Convert.ToInt32(reader["VacunaID"]),
                            MascotaID = Convert.ToInt32(reader["MascotaID"]),
                            RegistroID = reader["RegistroID"] is DBNull ? null : Convert.ToInt32(reader["RegistroID"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            FechaAplicacion = Convert.ToDateTime(reader["FechaAplicacion"]),
                            ProximaDosis = reader["ProximaDosis"] is DBNull ? null : Convert.ToDateTime(reader["ProximaDosis"]),
                            Notas = reader["Notas"] is DBNull ? string.Empty : Convert.ToString(reader["Notas"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar la vacuna: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return vacuna;
        }
        #endregion
    }
}
