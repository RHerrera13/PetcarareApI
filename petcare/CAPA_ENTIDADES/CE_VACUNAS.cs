using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_VACUNAS
    {
        public int VacunaID { get; set; }
        public int MascotaID { get; set; }
        public int? RegistroID { get; set; }         // NULL
        public string Nombre { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime? ProximaDosis { get; set; }  // NULL
        public string Notas { get; set; }            // NULL
        public int EstadoID { get; set; }
    }
}
