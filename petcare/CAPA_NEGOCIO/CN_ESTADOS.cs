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
    
    public class CN_ESTADOS
    {
        private readonly CD_ESTADOS _CDEstados;
        // Constructor que recibe IConfiguration para la conexión a la base de datos
        public CN_ESTADOS(IConfiguration configuration)
        {
            _CDEstados = new CD_ESTADOS(configuration);
        }

        public void CN_InsertarEstado(CE_ESTADOS estado)
        {
            if (estado == null)
            {
                throw new ArgumentNullException(nameof(estado), "El estado no puede ser nulo.");
            }
            _CDEstados.INSERTAR_ESTADO(estado);
        }

        public void CN_ActualizarEstado(CE_ESTADOS estado, out int num, out string msg)
        {
            _CDEstados.ACTUALIZAR_ESTADO(estado, out num, out msg);
        }

        public void CN_EliminarEstado(int estado)
        {

            throw new NotImplementedException("El método CN_EliminarEstado no está implementado.");
        }

        public List<CE_ESTADOS> CN_ListarEstados()
        {
           return _CDEstados.Listar_ESTADO();
        }
    }
}
    