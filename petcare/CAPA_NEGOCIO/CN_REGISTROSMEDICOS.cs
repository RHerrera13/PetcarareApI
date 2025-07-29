using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPA_DATOS;
using CAPA_ENTIDADES;
using Microsoft.Extensions.Configuration;

namespace CAPA_NEGOCIO
{
    public class CN_REGISTROSMEDICOS
    {
        private readonly CD_REGISTROSMEDICOS _CDRegistros;

        public CN_REGISTROSMEDICOS(IConfiguration configuration)
        {
            _CDRegistros = new CD_REGISTROSMEDICOS(configuration);
        }

        public void CN_InsertarRegistro(CE_REGISTROSMEDICOS registro)
        {
            if (registro == null)
                throw new ArgumentNullException(nameof(registro), "El registro médico no puede ser nulo.");

            _CDRegistros.INSERTAR_REGISTRO(registro);
        }

        public void CN_ActualizarRegistro(CE_REGISTROSMEDICOS registro, out int num, out string msg)
        {
            if (registro == null)
                throw new ArgumentNullException(nameof(registro), "El registro médico no puede ser nulo.");

            _CDRegistros.ACTUALIZAR_REGISTRO(registro, out num, out msg);
        }

        public void CN_EliminarRegistro(int id)
        {
            CE_REGISTROSMEDICOS registro = new CE_REGISTROSMEDICOS { RegistroID = id };
            _CDRegistros.ELIMINAR_REGISTRO(registro);
        }

        public List<CE_REGISTROSMEDICOS> CN_ListarRegistros()
        {
            return _CDRegistros.LISTAR_REGISTROS();
        }
    }
}
