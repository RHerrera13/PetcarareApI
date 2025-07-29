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
    public class CN_VACUNAS
    {
        private readonly CD_VACUNAS _CDVacunas;

        public CN_VACUNAS(IConfiguration configuration)
        {
            _CDVacunas = new CD_VACUNAS(configuration);
        }

        public void CN_InsertarVacuna(CE_VACUNAS vacuna)
        {
            if (vacuna == null)
                throw new ArgumentNullException(nameof(vacuna), "La vacuna no puede ser nula.");

            _CDVacunas.INSERTAR_VACUNA(vacuna);
        }

        public void CN_ActualizarVacuna(CE_VACUNAS vacuna, out int num, out string msg)
        {
            if (vacuna == null)
                throw new ArgumentNullException(nameof(vacuna), "La vacuna no puede ser nula.");

            _CDVacunas.ACTUALIZAR_VACUNA(vacuna, out num, out msg);
        }

        public void CN_EliminarVacuna(int id)
        {
            CE_VACUNAS vacuna = new CE_VACUNAS { VacunaID = id };
            _CDVacunas.ELIMINAR_VACUNA(vacuna);
        }

        public List<CE_VACUNAS> CN_ListarVacunas()
        {
            return _CDVacunas.LISTAR_VACUNAS();
        }
    }
}
