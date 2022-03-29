using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class SucursalesDto
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public static List<Sucursales> MapToEntidadList(List<SucursalesDto> listaDto)
        {
            List<Sucursales> sucursales = new List<Sucursales>();
            foreach (var item in listaDto)
            {
                Sucursales sucursal = new Sucursales()
                {
                    Activa = true,
                    Codigo = item.Codigo,
                    Contacto = String.Empty,
                    Email = string.Empty,
                    Sucursal = item.Descripcion,
                    Telefono = string.Empty
                };
                sucursales.Add(sucursal);
            }

            return sucursales;
        }
    }
}
