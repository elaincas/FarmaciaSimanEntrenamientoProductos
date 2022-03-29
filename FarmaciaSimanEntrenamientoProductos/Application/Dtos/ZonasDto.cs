using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ZonasDto
    {
        public int ZonaID { get; set; }
        public string Zona { get; set; }

        public static List<Zonas> MapToEntidadList(List<ZonasDto> listaDto)
        {
            var zonas = new List<Zonas>();
            foreach (var item in listaDto)
            {
                var zona = new Zonas()
                {
                    Zona = item.Zona,
                    ZonaID   = item.ZonaID
                };
                zonas.Add(zona);
            }
            return zonas;
        }
    }
}
