using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class AuxiliarProductoDto:BaseDto<AuxiliarProductoDto>
    {
        public int ProductoDetalleID { get; set; }
        public int ProductoID { get; set; }
        public string Descripcion { get; set; }
        public int ProductoPrestamoID { get; set; }
        public int ColaboradorID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCambioEstado { get; set; }
        public int EstadoID { get; set; }
        public string Estado { get; set; }
        public int UsuarioHaceTransaccion { get; set; }
        public string Usuario { get; set; }
        public string ObservacionDetallesProducto { get; set; }
        public int NoTraslado { get; set; }
        public int ZonaIDEnvia { get; set; }
        public string ZonaEnvia { get; set; }
        public int ZonaIDRecibe { get; set; }
        public string ZonaRecibe { get; set; }
        public DateTime Fecha { get; set; }
        public int ZonaIdActual { get; set; }
        public string ZonaActual { get; set; }
        public virtual List<AuxiliarProductoDto> ListaAuxiliar { get; set; } = new List<AuxiliarProductoDto>();
    }
}
