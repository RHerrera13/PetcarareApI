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
    public class CN_ARCHIVOSADJUNTOS
    {
        private readonly CD_ARCHIVOSADJUNTOS _CDArchivos;

        public CN_ARCHIVOSADJUNTOS(IConfiguration configuration)
        {
            _CDArchivos = new CD_ARCHIVOSADJUNTOS(configuration);
        }

        public void CN_InsertarArchivo(CE_ARCHIVOSADJUNTOS archivo)
        {
            if (archivo == null)
                throw new ArgumentNullException(nameof(archivo), "El archivo no puede ser nulo.");

            _CDArchivos.INSERTAR_ARCHIVO(archivo);
        }

        public void CN_ActualizarArchivo(CE_ARCHIVOSADJUNTOS archivo, out int num, out string msg)
        {
            if (archivo == null)
                throw new ArgumentNullException(nameof(archivo), "El archivo no puede ser nulo.");

            _CDArchivos.ACTUALIZAR_ARCHIVO(archivo, out num, out msg);
        }

        public void CN_EliminarArchivo(int id)
        {
            CE_ARCHIVOSADJUNTOS archivo = new CE_ARCHIVOSADJUNTOS { ArchivoID = id };
            _CDArchivos.ELIMINAR_ARCHIVO(archivo);
        }

        public List<CE_ARCHIVOSADJUNTOS> CN_ListarArchivos()
        {
            return _CDArchivos.LISTAR_ARCHIVOS();
        }
    }
}
