using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_REGISTROSMEDICOS
    {
        public int RegistroID { get; set; }
        public int EventoID { get; set; }
        public string? TipoTratamiento { get; set; }
        public string? VeterinarioNombre { get; set; }  // Opcional
        public string? Descripcion { get; set; }        // Opcional
        public int EstadoID { get; set; }
    }
}
