using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class ProductosPrestamos
    {
        public ProductosPrestamos()
        {
            Activo = true;
        }

        [Key]
        public int ProductoPrestamoID { get; set; }
        public int ColaboradorID { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public int UsuarioPrestaID { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int UsuarioIngresaDevolucionID { get; set; }
        public int? ValeId { get; set; }
        public int? UsuarioIngresaVale { get; set; }
        public bool Activo { get; set; }
        public List<TransaccionesProductos> Transacciones { get; set; } = new List<TransaccionesProductos>();
    }
}
