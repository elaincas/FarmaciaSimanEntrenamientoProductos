using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class CredencialesDto
    {
        public int AplicacionId { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }
}
