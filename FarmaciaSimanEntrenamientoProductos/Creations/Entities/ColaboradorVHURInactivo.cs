﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Creation.Entities
{
    public class ColaboradorVHURInactivo
    {
        public int ColaboradorID { get; set; }
        public string Nombre { get; set; }
        public int PuestoID { get; set; }
        public string DescripcionPuesto { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
