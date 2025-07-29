using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CAPA_DATOS
{
    public class CD_CONEXION
    {
        private  readonly string _conexionString;
        private  SqlConnection _conexion;

        public CD_CONEXION(IConfiguration configuration)
        {
            _conexionString = configuration.GetConnectionString("PetCareConnection");
            _conexion = new SqlConnection(_conexionString);
        }
        public SqlConnection AbrirConexion()
        {
            if (_conexion.State == ConnectionState.Closed) 
            {
                _conexion.Open();
            }
            return _conexion;
        }
        public SqlConnection CerrarConexion()
        {
            if (_conexion.State == ConnectionState.Open)
            {
                _conexion.Close();
            }
            return _conexion;
        }

    }
}
