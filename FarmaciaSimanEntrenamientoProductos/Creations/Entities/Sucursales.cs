using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class Sucursales
    {
        [Key]
        public int Codigo { get; set; }
        public string Sucursal { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public Boolean Activa { get; set; }
    }
}
