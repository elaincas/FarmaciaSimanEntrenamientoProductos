using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmaciaSimanEntrenamientoProductos.Application;
using FarmaciaSimanEntrenamientoProductos.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FarmaciaSimanEntrenamientoProductos.Controllers
{
    [Produces("application/json")]
    [Route("api/seguridad")]
    public class SeguridadController : Controller
    {
        private readonly CommonsService _commonsService;

        public SeguridadController(CommonsService commonService)
        {
            _commonsService = commonService;
        }

        [HttpPost, Route("Auth")]
        public IActionResult IniciarSesion([FromBody] CredencialesDto credenciales)
        {
            String mensaje = String.Empty;
            var resultado = _commonsService.IniciarSesion(credenciales, out mensaje);

            if (mensaje != "Ok")
                return BadRequest(mensaje);

            return Ok(resultado);
        }
    }
}