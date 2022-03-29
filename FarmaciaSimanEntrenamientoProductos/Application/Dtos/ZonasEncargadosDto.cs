using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ZonasEncargadosDto: BaseDto<ZonasEncargadosDto>
    {
        public int ZonaEncargadoID { get; set; }
        public string Descripcion { get; set; }
        public int ZonaID { get; set; }
        public int UsuarioEncargadoID { get; set; }

        public bool EsDtoValidoParaIngreso(out string mensaje)
        {
            mensaje = "Éxito";

            if (ZonaID == 0)
            {
                mensaje = "Zona incorrecta.";
                return false;
            }
            if (UsuarioEncargadoID == 0)
            {
                mensaje = "Usuario incorrecto para asignar!";
                return false;
            }
            return true;
        }
    }
}
