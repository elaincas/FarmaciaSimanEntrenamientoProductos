using EntrenamientoProductos.Insfraestructure.Map;
using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class ProductosDetalles
    {
        [Key]
        public int ProductoDetalleID { get; set; }
        public int ProductoID { get; set; }
        public Productos Producto { get; set; }
        public Decimal Precio { get; set; }
        public int EstadoID { get; set; }
        public  EstadosProductos Estado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int UsuarioIngresa { get; set; }
        public string Observacion { get; set; }
        public Boolean Activo { get; set; }
        public int? ZonaID { get; set; }

        public List<TransaccionesProductos> Transacciones { get; set; } = new List<TransaccionesProductos>();
        public List<TrasladosDetalle> TrasladosDetalles { get; set; } = new List<TrasladosDetalle>();
    }
}
