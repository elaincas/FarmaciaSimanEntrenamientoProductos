using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class TrasladosDetalleDto
    {
        public int TrasladosDetalleID { get; set; }
        public int TrasladoID { get; set; }
        public TrasladosDto Traslado { get; set; }
        public int ProductoDetalleID { get; set; }
        public int EstadoID { get; set; }
        public DateTime FechaCambioEstado { get; set; }
        public int UsuarioID { get; set; }
        public string Observacion { get; set; }

        public bool EsDtoValidoParaIngreso(out string mensaje)
        {
            mensaje = "Éxito";

            if (ProductoDetalleID == 0)
            {
                mensaje = "Producto inválido!";
                return false;
            }
            if (EstadoID == 0)
            {
                mensaje = "Estado incorrecto!";
                return false;
            }
            if (UsuarioID == 0)
            {
                mensaje = "Usuario incorrecto!";
                return false;
            }
            return true;
        }

    }
}
