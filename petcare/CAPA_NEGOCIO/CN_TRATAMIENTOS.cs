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
    public class CN_TRATAMIENTOS
    {
        private readonly CD_TRATAMIENTOS _CDTratamientos;

        public CN_TRATAMIENTOS(IConfiguration configuration)
        {
            _CDTratamientos = new CD_TRATAMIENTOS(configuration);
        }

        public void CN_InsertarTratamiento(CE_TRATAMIENTOS tratamiento)
        {
            if (tratamiento == null)
                throw new ArgumentNullException(nameof(tratamiento), "El tratamiento no puede ser nulo.");

            _CDTratamientos.INSERTAR_TRATAMIENTO(tratamiento);
        }

        public void CN_ActualizarTratamiento(CE_TRATAMIENTOS tratamiento, out int num, out string msg)
        {
            if (tratamiento == null)
                throw new ArgumentNullException(nameof(tratamiento), "El tratamiento no puede ser nulo.");

            _CDTratamientos.ACTUALIZAR_TRATAMIENTO(tratamiento, out num, out msg);
        }

        public void CN_EliminarTratamiento(int id)
        {
            CE_TRATAMIENTOS tratamiento = new CE_TRATAMIENTOS { TratamientoID = id };
            _CDTratamientos.ELIMINAR_TRATAMIENTO(tratamiento);
        }

        public List<CE_TRATAMIENTOS> CN_ListarTratamientos()
        {
            return _CDTratamientos.LISTAR_TRATAMIENTOS();
        }
    }
}
