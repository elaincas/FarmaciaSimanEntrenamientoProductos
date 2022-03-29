using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ColaboradorDto
    {
        public int ColaboradorID { get; set; }
        public string Nombre { get; set; }
        public string Respuesta { get; set; }
        public RespuestaTipo RespuestaTipo { get; set; }
        public List<ColaboradorDto> listaColaboradores { get; set; } = new List<ColaboradorDto>();
    }
}
