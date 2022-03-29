using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public class ProductoUnidadesdisponibleDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int UnidadesNuevas { get; set; }
        public int Recurperadas { get; set; }
    }
}
