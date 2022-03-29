using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ProductoPrestamoDto : BaseDto<ProductoPrestamoDto>
    {
        public int ProductoPrestamoId { get; set; }
        public int ColaboradorID { get; set; }
        public string NombreColaborador { get; set; }
        public int ProductoID { get; set; }
        public string Producto { get; set; }
        public int ProductoDetalleID { get; set; }
        public int EstadoID { get; set; }
        public string Estado { get; set; }
        public Decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public int? ValeId { get; set; }
        public List<ProductoPrestamoDto> listaProductos { get; set; }
        public int TransaccionesProductosId { get; set; }
        public string Asigna { get; set; }
        public bool esDtovalidoParaCambiarEstado(out string mensaje) {
            mensaje = "Éxito";
            if (ColaboradorID == 0)
            {
                mensaje = "Ingrese un colaborador.";
                return false;
            }
            if (ProductoDetalleID == 0)
            {
                mensaje = "Ingrese un producto para asignar.";
                return false;
            }
            if (UsuarioId == 0)
            {
                mensaje = "El usuario que está asignando el producto es inválido.";
                return false;
            }
            return true;
        }

        public bool esDtoValidoParaAsignarProductoAColaborador(out string mensaje)
        {
            mensaje = "Éxito";
            if (ColaboradorID == 0)
            {
                mensaje = "Ingrese un colaborador.";
                return false;
            }
            if (ProductoID == 0)
            {
                mensaje = "Ingrese un producto para asignar.";
                return false;
            }
            if (Cantidad == 0)
            {
                mensaje = "La cantidad no puede ser cero";
                return false;
            }
            if (Fecha == null)
            {
                mensaje = "La fecha de asignación es inválida.";
                return false;
            }
            if (UsuarioId == 0)
            {
                mensaje = "El usuario que está asignando el producto es inválido.";
                return false;
            }
            return true;
        }
    }
}
