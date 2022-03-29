using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creations.Entities
{
    public class ZonasEncargados
    {
        [Key]
        public int ZonaEncargadoID { get; set; }
        public string Descripcion { get; set; }
        public int ZonaID { get; set; }
        public int UsuarioEncargadoID { get; set; }
        public bool Activo { get; set; }
    }
}
