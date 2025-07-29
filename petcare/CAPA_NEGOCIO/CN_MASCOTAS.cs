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
    public class CN_MASCOTAS
    {
        private readonly CD_MASCOTAS _CDMascotas;

        public CN_MASCOTAS(IConfiguration configuration)
        {
            _CDMascotas = new CD_MASCOTAS(configuration);
        }

        public void CN_InsertarMascota(CE_MASCOTAS mascota)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota), "La mascota no puede ser nula.");

            _CDMascotas.INSERTAR_MASCOTA(mascota);
        }

        public void CN_ActualizarMascota(CE_MASCOTAS mascota, out int num, out string msg)
        {
            if (mascota == null)
                throw new ArgumentNullException(nameof(mascota), "La mascota no puede ser nula.");

            _CDMascotas.ACTUALIZAR_MASCOTA(mascota, out num, out msg);
        }

        public void CN_EliminarMascota(int id)
        {
            CE_MASCOTAS mascota = new CE_MASCOTAS { MascotaID = id };
            _CDMascotas.ELIMINAR_MASCOTA(mascota);
        }

        public List<CE_MASCOTAS> CN_ListarMascotas()
        {
            return _CDMascotas.LISTAR_MASCOTAS();
        }
    }
}
