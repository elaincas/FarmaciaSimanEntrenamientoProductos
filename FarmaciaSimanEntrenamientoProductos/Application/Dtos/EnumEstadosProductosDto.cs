using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application.Dtos
{
    public enum EnumEstadosProductosDto
    {
        BuenEstadoInicial = 1,
        RecuperadoBuenEstado = 2,
        RecuperadoMalEstado =3,
        Perdido = 4 ,
        MalEstado = 5,
        Asignado = 6,
        TrasladoEnProceso = 7,
        Aceptado = 8,
        Rechazado = 9,
        Standby = 10

    }
}
