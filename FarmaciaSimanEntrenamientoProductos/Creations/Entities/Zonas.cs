using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creations.Entities
{
    public class Zonas
    {
        [Key]
        public int ZonaID { get; set; }
        public string Zona { get; set; }
    }
}
