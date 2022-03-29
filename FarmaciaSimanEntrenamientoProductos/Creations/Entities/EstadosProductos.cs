using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class EstadosProductos
    {
        [Key]
        public int EstadoID { get; set; }
        public string Descripcion { get; set; }
        public List<TransaccionesProductos> Transacciones { get; set; } = new List<TransaccionesProductos>();
        public List<ProductosDetalles> ProductosDetalles { get; set; } = new List<Entities.ProductosDetalles>();
        public List<TrasladosDetalle> TrasladosDetalles { get; set; } = new List<TrasladosDetalle>();
    }   
}
