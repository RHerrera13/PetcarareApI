using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPA_ENTIDADES
{
    public class CE_MASCOTAS
    {
        public int? MascotaID { get; set; }
        public int? UsuarioID { get; set; }

        public string? Nombre { get; set; }
        public  string? Especie { get; set; }
        public string ? Raza { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Edad { get; set; } // se creo el calculo de edad en la base de datos
        public string? Sexo { get; set; }
        public string? Color { get; set; }
        public decimal? Peso { get; set; }
        public string ?FotoURL { get; set; }
        public string? Notas { get; set; }
        public int EstadoID { get; set; }
    }
}
