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
    public class CD_TRATAMIENTOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_TRATAMIENTOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar tratamiento
        public void INSERTAR_TRATAMIENTO(CE_TRATAMIENTOS tratamiento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Tratamientos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = tratamiento.MascotaID;
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = tratamiento.EventoID;
            cmd.Parameters.Add("@TipoTratamiento", SqlDbType.NVarChar, 50).Value = tratamiento.TipoTratamiento;
            cmd.Parameters.Add("@FechaAplicacion", SqlDbType.Date).Value = tratamiento.FechaAplicacion;
            cmd.Parameters.Add("@ProximaDosis", SqlDbType.Date).Value = tratamiento.ProximaDosis.HasValue ? tratamiento.ProximaDosis.Value : (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = tratamiento.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = tratamiento.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar tratamiento
        public void ACTUALIZAR_TRATAMIENTO(CE_TRATAMIENTOS tratamiento, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Tratamientos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@TratamientoID", SqlDbType.Int).Value = tratamiento.TratamientoID;
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = tratamiento.MascotaID;
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = tratamiento.EventoID;
            cmd.Parameters.Add("@TipoTratamiento", SqlDbType.NVarChar, 50).Value = tratamiento.TipoTratamiento;
            cmd.Parameters.Add("@FechaAplicacion", SqlDbType.Date).Value = tratamiento.FechaAplicacion;
            cmd.Parameters.Add("@ProximaDosis", SqlDbType.Date).Value = tratamiento.ProximaDosis.HasValue ? tratamiento.ProximaDosis.Value : (object)DBNull.Value;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = tratamiento.Notas ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = tratamiento.EstadoID;
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
                throw new Exception("Error al actualizar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar tratamiento
        public void ELIMINAR_TRATAMIENTO(CE_TRATAMIENTOS tratamiento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Tratamientos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@TratamientoID", SqlDbType.Int).Value = tratamiento.TratamientoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar tratamientos
        public List<CE_TRATAMIENTOS> LISTAR_TRATAMIENTOS()
        {
            List<CE_TRATAMIENTOS> lista = new List<CE_TRATAMIENTOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Tratamientos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_TRATAMIENTOS tratamiento = new CE_TRATAMIENTOS
                    {
                        TratamientoID = Convert.ToInt32(dr["TratamientoID"]),
                        MascotaID = Convert.ToInt32(dr["MascotaID"]),
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        TipoTratamiento = Convert.ToString(dr["TipoTratamiento"]),
                        FechaAplicacion = Convert.ToDateTime(dr["FechaAplicacion"]),
                        ProximaDosis = dr["ProximaDosis"] is DBNull ? null : Convert.ToDateTime(dr["ProximaDosis"]),
                        Notas = dr["Notas"] is DBNull ? string.Empty : Convert.ToString(dr["Notas"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(tratamiento);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tratamientos: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar tratamiento por ID
        public CE_TRATAMIENTOS BUSCAR_TRATAMIENTO_POR_ID(int tratamientoId)
        {
            CE_TRATAMIENTOS tratamiento = null;

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Tratamientos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@TratamientoID", SqlDbType.Int).Value = tratamientoId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tratamiento = new CE_TRATAMIENTOS
                        {
                            TratamientoID = Convert.ToInt32(reader["TratamientoID"]),
                            MascotaID = Convert.ToInt32(reader["MascotaID"]),
                            EventoID = Convert.ToInt32(reader["EventoID"]),
                            TipoTratamiento = Convert.ToString(reader["TipoTratamiento"]),
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
                throw new Exception("Error al buscar el tratamiento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return tratamiento;
        }
        #endregion
    }
}
