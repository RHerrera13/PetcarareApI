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
    public class CD_MEDICAMENTOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_MEDICAMENTOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar medicamento
        public void INSERTAR_MEDICAMENTO(CE_MEDICAMENTOS medicamento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Medicamentos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = medicamento.MascotaID;
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = medicamento.RegistroID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = medicamento.Nombre;
            cmd.Parameters.Add("@Dosis", SqlDbType.NVarChar, 100).Value = medicamento.Dosis ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Frecuencia", SqlDbType.NVarChar, 100).Value = medicamento.Frecuencia ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = medicamento.FechaInicio;
            cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = medicamento.FechaFin ?? (object)DBNull.Value;
            cmd.Parameters.Add("@DosisTomadas", SqlDbType.Int).Value = medicamento.DosisTomadas;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = medicamento.EstadoID;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = medicamento.Notas ?? (object)DBNull.Value;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el medicamento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar medicamento
        public void ACTUALIZAR_MEDICAMENTO(CE_MEDICAMENTOS medicamento, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Medicamentos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@MedicamentoID", SqlDbType.Int).Value = medicamento.MedicamentoID;
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = medicamento.MascotaID;
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = medicamento.RegistroID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = medicamento.Nombre;
            cmd.Parameters.Add("@Dosis", SqlDbType.NVarChar, 100).Value = medicamento.Dosis ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Frecuencia", SqlDbType.NVarChar, 100).Value = medicamento.Frecuencia ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = medicamento.FechaInicio;
            cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = medicamento.FechaFin ?? (object)DBNull.Value;
            cmd.Parameters.Add("@DosisTomadas", SqlDbType.Int).Value = medicamento.DosisTomadas;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = medicamento.EstadoID;
            cmd.Parameters.Add("@Notas", SqlDbType.NVarChar).Value = medicamento.Notas ?? (object)DBNull.Value;
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
                throw new Exception("Error al actualizar el medicamento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar medicamento
        public void ELIMINAR_MEDICAMENTO(CE_MEDICAMENTOS medicamento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Medicamentos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@MedicamentoID", SqlDbType.Int).Value = medicamento.MedicamentoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el medicamento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar medicamentos
        public List<CE_MEDICAMENTOS> LISTAR_MEDICAMENTOS()
        {
            List<CE_MEDICAMENTOS> lista = new List<CE_MEDICAMENTOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Medicamentos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);
                foreach (DataRow dr in tabla.Rows)
                {
                    CE_MEDICAMENTOS fila = new CE_MEDICAMENTOS
                    {
                        MedicamentoID = Convert.ToInt32(dr["MedicamentoID"]),
                        MascotaID = Convert.ToInt32(dr["MascotaID"]),
                        RegistroID = dr["RegistroID"] is DBNull ? null : Convert.ToInt32(dr["RegistroID"]),
                        Nombre = Convert.ToString(dr["Nombre"]),
                        Dosis = dr["Dosis"] is DBNull ? string.Empty : Convert.ToString(dr["Dosis"]),
                        Frecuencia = dr["Frecuencia"] is DBNull ? string.Empty : Convert.ToString(dr["Frecuencia"]),
                        FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                        FechaFin = dr["FechaFin"] is DBNull ? null : Convert.ToDateTime(dr["FechaFin"]),
                        DosisTomadas = Convert.ToInt32(dr["DosisTomadas"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"]),
                        Notas = dr["Notas"] is DBNull ? string.Empty : Convert.ToString(dr["Notas"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar medicamentos: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar medicamento por ID
        public CE_MEDICAMENTOS BUSCAR_MEDICAMENTO_POR_ID(int medicamentoId)
        {
            CE_MEDICAMENTOS medicamento = null;

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Medicamentos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@MedicamentoID", SqlDbType.Int).Value = medicamentoId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        medicamento = new CE_MEDICAMENTOS
                        {
                            MedicamentoID = Convert.ToInt32(reader["MedicamentoID"]),
                            MascotaID = Convert.ToInt32(reader["MascotaID"]),
                            RegistroID = reader["RegistroID"] is DBNull ? null : Convert.ToInt32(reader["RegistroID"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            Dosis = reader["Dosis"] is DBNull ? string.Empty : Convert.ToString(reader["Dosis"]),
                            Frecuencia = reader["Frecuencia"] is DBNull ? string.Empty : Convert.ToString(reader["Frecuencia"]),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = reader["FechaFin"] is DBNull ? null : Convert.ToDateTime(reader["FechaFin"]),
                            DosisTomadas = Convert.ToInt32(reader["DosisTomadas"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"]),
                            Notas = reader["Notas"] is DBNull ? string.Empty : Convert.ToString(reader["Notas"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el medicamento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return medicamento;
        }
        #endregion
    }
}
