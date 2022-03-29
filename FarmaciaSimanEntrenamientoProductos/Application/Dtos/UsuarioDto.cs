using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Usuario { get; set; }
    }
}
