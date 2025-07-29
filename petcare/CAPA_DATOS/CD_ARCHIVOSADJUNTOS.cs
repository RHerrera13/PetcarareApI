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
    public class CD_ARCHIVOSADJUNTOS
    {
        private readonly CD_CONEXION _conexion;
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public CD_ARCHIVOSADJUNTOS(IConfiguration configuration)
        {
            _conexion = new CD_CONEXION(configuration);
        }

        #region Insertar archivo
        public void INSERTAR_ARCHIVO(CE_ARCHIVOSADJUNTOS archivo)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_ArchivosAdjuntos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "AGREGAR";
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = archivo.RegistroID;
            cmd.Parameters.Add("@NombreArchivo", SqlDbType.NVarChar, 255).Value = archivo.NombreArchivo;
            cmd.Parameters.Add("@TipoArchivo", SqlDbType.NVarChar, 50).Value = archivo.TipoArchivo ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaSubida", SqlDbType.DateTime).Value = archivo.FechaSubida;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = archivo.EstadoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el archivo adjunto: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Actualizar archivo
        public void ACTUALIZAR_ARCHIVO(CE_ARCHIVOSADJUNTOS archivo, out int num, out string msg)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_ArchivosAdjuntos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ACTUALIZAR";
            cmd.Parameters.Add("@ArchivoID", SqlDbType.Int).Value = archivo.ArchivoID;
            cmd.Parameters.Add("@RegistroID", SqlDbType.Int).Value = archivo.RegistroID;
            cmd.Parameters.Add("@NombreArchivo", SqlDbType.NVarChar, 255).Value = archivo.NombreArchivo;
            cmd.Parameters.Add("@TipoArchivo", SqlDbType.NVarChar, 50).Value = archivo.TipoArchivo ?? (object)DBNull.Value;
            cmd.Parameters.Add("@FechaSubida", SqlDbType.DateTime).Value = archivo.FechaSubida;
            cmd.Parameters.Add("@EstadoID", SqlDbType.Int).Value = archivo.EstadoID;
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
                throw new Exception("Error al actualizar el archivo adjunto: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Eliminar archivo
        public void ELIMINAR_ARCHIVO(CE_ARCHIVOSADJUNTOS archivo)
        {
            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_ArchivosAdjuntos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "ELIMINAR";
            cmd.Parameters.Add("@ArchivoID", SqlDbType.Int).Value = archivo.ArchivoID;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el archivo adjunto: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }
        }
        #endregion

        #region Listar archivos
        public List<CE_ARCHIVOSADJUNTOS> LISTAR_ARCHIVOS()
        {
            List<CE_ARCHIVOSADJUNTOS> lista = new List<CE_ARCHIVOSADJUNTOS>();
            DataTable tabla = new DataTable();

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_ArchivosAdjuntos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "LISTAR";
            da.SelectCommand = cmd;

            try
            {
                da.Fill(tabla);
                foreach (DataRow dr in tabla.Rows)
                {
                    CE_ARCHIVOSADJUNTOS fila = new CE_ARCHIVOSADJUNTOS
                    {
                        ArchivoID = Convert.ToInt32(dr["ArchivoID"]),
                        RegistroID = Convert.ToInt32(dr["RegistroID"]),
                        NombreArchivo = Convert.ToString(dr["NombreArchivo"]),
                        TipoArchivo = dr["TipoArchivo"] is DBNull ? string.Empty : Convert.ToString(dr["TipoArchivo"]),
                        FechaSubida = Convert.ToDateTime(dr["FechaSubida"]),
                        EstadoID = Convert.ToInt32(dr["EstadoID"])
                    };

                    lista.Add(fila);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los archivos adjuntos: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return lista;
        }
        #endregion

        #region Buscar archivo por ID
        public CE_ARCHIVOSADJUNTOS BUSCAR_ARCHIVO_POR_ID(int archivoId)
        {
            CE_ARCHIVOSADJUNTOS archivo = null;

            cmd.Connection = _conexion.AbrirConexion();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();

            cmd.CommandText = "sp_ArchivosAdjuntos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Modo", SqlDbType.NVarChar, 20).Value = "BUSCAR";
            cmd.Parameters.Add("@ArchivoID", SqlDbType.Int).Value = archivoId;

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        archivo = new CE_ARCHIVOSADJUNTOS
                        {
                            ArchivoID = Convert.ToInt32(reader["ArchivoID"]),
                            RegistroID = Convert.ToInt32(reader["RegistroID"]),
                            NombreArchivo = Convert.ToString(reader["NombreArchivo"]),
                            TipoArchivo = reader["TipoArchivo"] is DBNull ? string.Empty : Convert.ToString(reader["TipoArchivo"]),
                            FechaSubida = Convert.ToDateTime(reader["FechaSubida"]),
                            EstadoID = Convert.ToInt32(reader["EstadoID"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el archivo adjunto: " + ex.Message);
            }
            finally
            {
                _conexion.CerrarConexion();
                cmd.Parameters.Clear();
            }

            return archivo;
        }
        #endregion
    }
}
