using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creations.Entities
{
    public class LogEventos
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string VersionSistema { get; set; }
        public string Pantalla{ get; set; }

    }
}
