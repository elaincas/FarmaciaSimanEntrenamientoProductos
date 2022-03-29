using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creations.Entities
{
    public class TrasladosDetalle
    {
        [Key]
        public int TrasladoDetalleID { get; set; }
        public int TrasladoID { get; set; }
        public Traslados Traslado { get; set; }
        public int ProductoDetalleID { get; set; }
        public ProductosDetalles ProductoDetalle { get; set; }
        public int EstadoID { get; set; }
        public EstadosProductos EstadoProducto { get; set; }
        public DateTime FechaCambioEstado { get; set; }
        public int UsuarioID { get; set; }
        public string Observacion { get; set; }

    }
}
