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
    public class CN_EVENTOS
    {
        private readonly CD_EVENTOS _CDEventos;

        public CN_EVENTOS(IConfiguration configuration)
        {
            _CDEventos = new CD_EVENTOS(configuration);
        }

        public void CN_InsertarEvento(CE_EVENTOS evento)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento), "El evento no puede ser nulo.");

            _CDEventos.INSERTAR_EVENTO(evento);
        }

        public void CN_ActualizarEvento(CE_EVENTOS evento, out int num, out string msg)
        {
            if (evento == null)
                throw new ArgumentNullException(nameof(evento), "El evento no puede ser nulo.");

            _CDEventos.ACTUALIZAR_EVENTO(evento, out num, out msg);
        }

        public void CN_EliminarEvento(int id)
        {
            CE_EVENTOS evento = new CE_EVENTOS { EventoID = id };
            _CDEventos.ELIMINAR_EVENTO(evento);
        }

        public List<CE_EVENTOS> CN_ListarEventos()
        {
            return _CDEventos.LISTAR_EVENTOS();
        }
    }
}
