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
    public class CD_EVENTOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_EVENTOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar evento
        public void INSERTAR_EVENTO(CE_EVENTOS evento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Eventos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = evento.MascotaID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = evento.UsuarioID;
            cmd.Parameters.Add("@TipoEvento", SqlDbType.NVarChar, 50).Value = evento.TipoEvento;
            cmd.Parameters.Add("@Titulo", SqlDbType.NVarChar, 100).Value = evento.Titulo;
            cmd.Parameters.Add("@FechaHora", SqlDbType.DateTime).Value = evento.FechaHora;
            cmd.Parameters.Add("@DoctorAsignado", SqlDbType.NVarChar, 100).Value = evento.DoctorAsignado ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = evento.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = evento.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar evento
        public void ACTUALIZAR_EVENTO(CE_EVENTOS evento, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Eventos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = evento.EventoID;
            cmd.Parameters.Add("@MascotaID", SqlDbType.Int).Value = evento.MascotaID ?? (object)DBNull.Value;
            cmd.Parameters.Add("@UsuarioID", SqlDbType.Int).Value = evento.UsuarioID;
            cmd.Parameters.Add("@TipoEvento", SqlDbType.NVarChar, 50).Value = evento.TipoEvento;
            cmd.Parameters.Add("@Titulo", SqlDbType.NVarChar, 100).Value = evento.Titulo;
            cmd.Parameters.Add("@FechaHora", SqlDbType.DateTime).Value = evento.FechaHora;
            cmd.Parameters.Add("@DoctorAsignado", SqlDbType.NVarChar, 100).Value = evento.DoctorAsignado ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar).Value = evento.Descripcion ?? (object)DBNull.Value;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = evento.EstadoID;
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
                throw new Exception("Error al actualizar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar evento
        public void ELIMINAR_EVENTO(CE_EVENTOS evento)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Eventos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = evento.EventoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar eventos
        public List<CE_EVENTOS> LISTAR_EVENTOS()
        {
            List<CE_EVENTOS> lista = new List<CE_EVENTOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Eventos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);

                foreach (DataRow dr in tabla.Rows)
                {
                    CE_EVENTOS fila = new CE_EVENTOS
                    {
                        EventoID = Convert.ToInt32(dr["EventoID"]),
                        MascotaID = dr["MascotaID"] is DBNull ? null : Convert.ToInt32(dr["MascotaID"]),
                        UsuarioID = Convert.ToInt32(dr["UsuarioID"]),
                        TipoEvento = Convert.ToString(dr["TipoEvento"]),
                        Titulo = Convert.ToString(dr["Titulo"]),
                        FechaHora = Convert.ToDateTime(dr["FechaHora"]),
                        DoctorAsignado = dr["DoctorAsignado"] is DBNull ? string.Empty : Convert.ToString(dr["DoctorAsignado"]),
                        Descripcion = dr["Descripcion"] is DBNull ? string.Empty : Convert.ToString(dr["Descripcion"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar eventos: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar evento por ID
        public CE_EVENTOS BUSCAR_EVENTO_POR_ID(int eventoId)
        {
            CE_EVENTOS evento = null;
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_Eventos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@EventoID", SqlDbType.Int).Value = eventoId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        evento = new CE_EVENTOS
                        {
                            EventoID = Convert.ToInt32(reader["EventoID"]),
                            MascotaID = reader["MascotaID"] is DBNull ? null : Convert.ToInt32(reader["MascotaID"]),
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            TipoEvento = Convert.ToString(reader["TipoEvento"]),
                            Titulo = Convert.ToString(reader["Titulo"]),
                            FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                            DoctorAsignado = reader["DoctorAsignado"] is DBNull ? string.Empty : Convert.ToString(reader["DoctorAsignado"]),
                            Descripcion = reader["Descripcion"] is DBNull ? string.Empty : Convert.ToString(reader["Descripcion"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el evento: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return evento;
        }
        #endregion
    }
}
