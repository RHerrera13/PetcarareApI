using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_USUARIOS
    {
        public int? UsuarioID { get; set; }
        public string ? NombreCompleto { get; set; }
        public string ?Email { get; set; }
        public string ? Telefono { get; set; }
        public string? Contrasena { get; set; }
        public DateTime?  FechaRegistro { get; set; }
        public int? EstadoID { get; set; }
    }
}
