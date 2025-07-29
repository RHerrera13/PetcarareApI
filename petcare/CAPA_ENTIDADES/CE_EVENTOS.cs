using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_EVENTOS
    {
        public int EventoID { get; set; }
        public int? MascotaID { get; set; } // Puede ser NULL
        public int UsuarioID { get; set; }
        public string? TipoEvento { get; set; }
        public string? Titulo { get; set; }
        public DateTime FechaHora { get; set; }
        public string? DoctorAsignado { get; set; } // Opcional
        public string? Descripcion { get; set; }     // Opcional
        public int EstadoID { get; set; }
    }
}
