using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ProductoDto:BaseDto<ProductoDto>
    {
        public int ProductoID { get; set; }
        public int ProductoDetalleID { get; set; }
        public decimal Precio { get; set; }
        public int EstadoID { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Observacion { get; set; }
        public string NombreProducto { get; set; }
        public string Estado { get; set; }
        public Decimal Cantidad { get; set; }
        public int ZonaID { get; set; }

        public bool EsDtoValidoParaIngreso(out string mensaje)
        {
            mensaje = "Éxito";

            if (Precio == 0)
            {
                mensaje = "Precio incorrecto.";
                return false;
            }
            if (ZonaID == 0)
            {
                mensaje = "Zona incorrecta.";
                return false;
            }
            if (Cantidad == 0)
            {
                mensaje = "Cantidad incorrecta.";
                return false;
            }
         
            if (FechaIngreso == null) {
                mensaje = "Fecha de ingreso incorrecta.";
                return false;
            }
            if (UsuarioId == 0 ) {
                mensaje = "Se necesita un usuario que registré la transacción.";
                return false;
            }
            return true;
        }
    }

   
}
