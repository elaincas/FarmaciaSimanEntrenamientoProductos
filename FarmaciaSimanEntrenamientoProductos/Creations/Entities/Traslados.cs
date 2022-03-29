using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creations.Entities
{
    public class Traslados
    {
        [Key]
        public int TrasladoID { get; set; }
        public int ZonaIDEnvia { get; set; }
        public int ZonaIDRecibe { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Boolean Activo { get; set; }

        public virtual List<TrasladosDetalle> TrasladosDetalles { get; set; } = new List<TrasladosDetalle>();
    }
}
