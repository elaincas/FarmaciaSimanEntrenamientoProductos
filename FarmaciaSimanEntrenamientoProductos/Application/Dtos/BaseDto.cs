using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class BaseDto<T> where T  : class
    {
        public int UsuarioId { get; set; }
        public RespuestaTipo RespuestaTipo { get; set; }
        public string Respuesta { get; set; }
    }
}
