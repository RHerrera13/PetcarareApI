using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_MEDICAMENTOS
    {
        public int MedicamentoID { get; set; }
        public int MascotaID { get; set; }
        public int? RegistroID { get; set; }  // Opcional
        public string? Nombre { get; set; }
        public string? Dosis { get; set; }         // Opcional
        public string? Frecuencia { get; set; }    // Opcional
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }   // Opcional
        public int DosisTomadas { get; set; }
        public int EstadoID { get; set; }
        public string? Notas { get; set; }         // Opcional
    }
}
