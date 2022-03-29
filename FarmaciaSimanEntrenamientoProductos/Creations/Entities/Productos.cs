using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class Productos
    {
        [Key]
        public int ProductoID { get; set; }
        public string Descripcion { get; set; }
        public Boolean Activo { get; set; }
        public List<ProductosDetalles> ProductosDetalles { get; set; } = new List<ProductosDetalles>();
    }
}
