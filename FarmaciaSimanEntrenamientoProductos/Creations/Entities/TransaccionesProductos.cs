using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class TransaccionesProductos
    {
        [Key]
        public int TransaccionesProductosID { get; set; }
        public int ProductoDetalleID { get; set; }
        public  ProductosDetalles ProductoDetalle { get; set; }
        public int? ProductoPrestamoID { get; set; }
        public  ProductosPrestamos ProductoPrestamo { get; set; }
        public DateTime FechaCambioEstado { get; set; }
        public int UsuarioID { get; set; }
        public int EstadoID { get; set; }
        public  EstadosProductos Estado { get; set; }
        public Boolean Activo { get; set; }
        public int? ZonaID { get; set; }
    }
}
