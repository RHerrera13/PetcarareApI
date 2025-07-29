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
    public class CN_MEDICAMENTOS
    {
        private readonly CD_MEDICAMENTOS _CDMedicamentos;

        public CN_MEDICAMENTOS(IConfiguration configuration)
        {
            _CDMedicamentos = new CD_MEDICAMENTOS(configuration);
        }

        public void CN_InsertarMedicamento(CE_MEDICAMENTOS medicamento)
        {
            if (medicamento == null)
                throw new ArgumentNullException(nameof(medicamento), "El medicamento no puede ser nulo.");

            _CDMedicamentos.INSERTAR_MEDICAMENTO(medicamento);
        }

        public void CN_ActualizarMedicamento(CE_MEDICAMENTOS medicamento, out int num, out string msg)
        {
            if (medicamento == null)
                throw new ArgumentNullException(nameof(medicamento), "El medicamento no puede ser nulo.");

            _CDMedicamentos.ACTUALIZAR_MEDICAMENTO(medicamento, out num, out msg);
        }

        public void CN_EliminarMedicamento(int id)
        {
            CE_MEDICAMENTOS medicamento = new CE_MEDICAMENTOS { MedicamentoID = id };
            _CDMedicamentos.ELIMINAR_MEDICAMENTO(medicamento);
        }

        public List<CE_MEDICAMENTOS> CN_ListarMedicamentos()
        {
            return _CDMedicamentos.LISTAR_MEDICAMENTOS();
        }
    }
}
