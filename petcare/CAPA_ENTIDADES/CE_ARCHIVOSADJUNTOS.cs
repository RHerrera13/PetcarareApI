using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_ARCHIVOSADJUNTOS
    {
        public int ArchivoID { get; set; }
        public int RegistroID { get; set; }
        public string? NombreArchivo { get; set; }
        public string? TipoArchivo { get; set; }       // null
        public DateTime FechaSubida { get; set; }
        public int EstadoID { get; set; }
    }
}
