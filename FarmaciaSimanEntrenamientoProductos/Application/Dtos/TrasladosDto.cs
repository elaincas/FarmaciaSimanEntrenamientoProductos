using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class TrasladosDto: BaseDto<TrasladosDto>
    {
        public int TrasladoID { get; set; }
        public int ZonaIDEnvia { get; set; }
        public int ZonaIDRecibe { get; set; }
        public ZonasEncargadosDto ZonaEncargado { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioID { get; set; }
        public Boolean Activo { get; set; }
        public int EstadoID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public virtual TrasladosDetalleDto TrasladoDetalle { get; set; }
        public virtual List<TrasladosDetalleDto> TrasladosDetalles { get; set; }

        public  virtual List<TrasladosDto> ProductosTraslado { get; set; }

        public bool EsDtoValidoParaIngreso(out string mensaje)
        {
            mensaje = "Éxito";

            if (ZonaIDRecibe == 0)
            {
                mensaje = "Zona incorrecta.";
                return false;
            }
            if (ZonaIDEnvia == 0)
            {
                mensaje = "Usuario incorrecto para asignar!";
                return false;
            }
            if (UsuarioID == 0)
            {
                mensaje = "Usuario incorrecto!";
                return false;
            }
            if (Cantidad == 0)
            {
                mensaje = "Cantidad incorrecta!";
                return false;
            }
            if (ProductoID == 0)
            {
                mensaje = "Producto incorrecto!";
                return false;
            }
            return true;
        }
    }
}
