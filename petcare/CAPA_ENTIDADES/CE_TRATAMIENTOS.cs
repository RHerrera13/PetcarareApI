using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_TRATAMIENTOS
    {
        public int TratamientoID { get; set; }
        public int MascotaID { get; set; }
        public int EventoID { get; set; }
        public string? TipoTratamiento { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime? ProximaDosis { get; set; }  // Opcional
        public string? Notas { get; set; }            // Opcional
        public int EstadoID { get; set; }
    }
}
