using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class FiltroProductosColaboradoresDto
    {
        public int EstadoID { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }

        public bool EsValidoElFiltro(out string mensaje)
        {
            mensaje = string.Empty;

            if (EstadoID == 0 || EstadoID == 1)
            {
                mensaje = "Estado inválido!";
                return false;
            }
            if (string.IsNullOrEmpty(FechaDesde))
            {
                mensaje = "Fecha Desde inválida!";
                return false;
            }
            if (string.IsNullOrEmpty(FechaHasta))
            {
                mensaje = "Fecha Hasta inválida!";
                return false;
            }
            return true;
        }
    }
  
}
