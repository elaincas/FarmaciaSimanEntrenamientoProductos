
using FarmaciaSimanEntrenamientoProductos.Application;
using FarmaciaSimanEntrenamientoProductos.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FarmaciaSimanEntrenamientoProductos.Controllers
{
    [Produces("application/json")]
    [Route("api/Productos")]
    public class ProductosController : Controller
    {
        private readonly ProductosService _productoService;
        private readonly CommonsService _commonsService;
        public ProductosController(ProductosService productoService,CommonsService commonsService)
        {
            _productoService = productoService;
            _commonsService = commonsService;
        }

        [HttpGet, Route("estados")]
        public IActionResult ObtenerSucursales()
        {
            var resultado = _productoService.ObtenesEstados();
            return Ok(resultado);
        }

        [HttpGet, Route("obtenerProductos")]
        public IActionResult ObtenerProductos()
        {
            var resultado = _productoService.ObtenerTodosLosProductos();
            //return Ok(JsonConvert.SerializeObject(resultado));
            return Ok(resultado);

        }

        [HttpPost]
        [Route("ingresarProductos")]
        public IActionResult ObtenerPlanimetrias([FromBody] ProductoDto producto)
        {
            var resultado = _productoService.IngresarProductos(producto);

            if (resultado.RespuestaTipo != RespuestaTipo.Ok) { return BadRequest(resultado.Respuesta); }

            return Ok(resultado);
        }
        [HttpGet, Route("obtenerColaborador/{colaboradorID}/{colaboradorDespedido}")]
        public IActionResult ObtenerInformacionColaborador(int colaboradorID,bool colaboradorDespedido) {
            var resutlado = _commonsService.ObtenerInformacionColaborador(colaboradorID, colaboradorDespedido);

            if (resutlado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resutlado.Respuesta);

            return Ok(resutlado);
        }
        [HttpPost, Route("asignar")]
        public IActionResult AsignarProductoAColaborador([FromBody]ProductoPrestamoDto prestamo) {
            var resultado = _commonsService.AsignarProductoAColaborador(prestamo);

            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpPost, Route("recuperar/{esDataColaboradorDespedido}")]
        public IActionResult RecuperarProducto([FromBody]ProductoPrestamoDto prestamo,bool esDataColaboradorDespedido)
        {
            var resultado = _commonsService.RecuperarProducto(prestamo,esDataColaboradorDespedido);

            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpGet, Route("productoasignadocolaborador/{fechaDesde}/{fechaHasta}/{estadoID}")]
        public IActionResult ObtenerProductosAsignadosAcolaboradorOrdenadosPorEstado(string fechaDesde, string fechaHasta,int estadoID) {
            var filtro = new FiltroProductosDto() { FechaHasta = fechaHasta, FechaDesde = fechaDesde, FiltroID = estadoID };
            var resultado  = _commonsService.ObteberProductoAsignadosAColaboradorOrdenadosPorEstado(filtro);
            return Ok(resultado);
        }

        [HttpGet, Route("productosdisponibles/{usuario}")]
        public IActionResult ObtenerProductosDisponiblesParaAsignar(int usuario) {
            var resultado = _commonsService.ObtenerProductosDisponiblesParaAsignar(usuario);
            return Ok(resultado);
        }

        [HttpGet, Route("obtenerzonas")]
        public IActionResult ObtenerZonas()
        {
            var resultado = _commonsService.ObtenerZonas();
            return Ok(resultado);
        }

        [HttpPost, Route("asignarzona")]
        public IActionResult AsignarZonaAEncargado([FromBody]ZonasEncargadosDto zona)
        {
            var resultado = _commonsService.AsignarEncargadoDeZona(zona);
            if (resultado.RespuestaTipo != RespuestaTipo.Ok) 
                return BadRequest(resultado.Respuesta);
            return Ok(resultado);
        }
        [HttpPost, Route("trasladarproducto")]
        public IActionResult TrasladarProducto([FromBody] List<TrasladosDto> traslado)
        {
            var resultado = _commonsService.RealizarTraslado(traslado);
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);
            return Ok(resultado);
        }
        [HttpPost, Route("aceptardenegartraslado")]
        public IActionResult AceptarDenegarTraslado([FromBody]TrasladosDto traslado)
        {
            var resultado = _commonsService.AceptarDenegarTraslado(traslado);
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);
            return Ok(resultado);
        }
        [HttpGet, Route("obtenerTraslados/{usuarioID}")]
        public IActionResult ObtenerTraslados(int UsuarioID)
        {
            var resultado = _commonsService.ObtenerTrasladosPorUsuarioEncargadoDeZona(UsuarioID);
            return Ok(resultado);
        }
        [HttpGet, Route("obtenerzonasporusuario/{usuarioID}")]
        public IActionResult ObtenerZonas(int usuarioID)
        {
            var resultado = _commonsService.ObtenerZonasAsignadasAlUsuario(usuarioID);
            return Ok(resultado);
        }

        [HttpGet, Route("obtenerAuxiliar/{fechaDesde}/{fechaHasta}/{filtroID}")]
        public IActionResult ObtenerZonas(string fechaDesde, string fechaHasta, int filtroID)
        {
            var filtro = new FiltroProductosDto() { FechaHasta = fechaHasta, FechaDesde = fechaDesde, FiltroID = filtroID };
            var resultado = _productoService.ObtenerAuxiliarDeProductos(filtro);
            if (resultado.RespuestaTipo != RespuestaTipo.Ok) 
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpGet, Route("obtenerColaboradores")]
        public IActionResult obtenerColaboradores()
        {
            var resultado = _commonsService.ObtenerColaboradores();
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpGet, Route("productosmalestados")]
        public IActionResult obtenerProductosMalEstado()
        {
            var resultado = _productoService.ObtenerProdutosEnMalEstados_Perdidos();
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }

        [HttpPost, Route("actualizarvale")]
        public IActionResult ActualizarValeColaborador([FromBody] ProductoPrestamoDto productosColaborador)
        {
            var resultado = _productoService.ActualizarDatoValeColaborador(productosColaborador);
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpGet, Route("obtenerColaboradoresDespedidos")]
        public IActionResult obtenerColaboradoresInactivos()
        {
            var resultado = _commonsService.ObtenerColaboradoresDespedidos();
            if (resultado.RespuestaTipo != RespuestaTipo.Ok)
                return BadRequest(resultado.Respuesta);

            return Ok(resultado);
        }
        [HttpGet, Route("productounidadesdisponible/{productoId}/{usuarioId}")]
        public IActionResult ProductoUnidadesdisponible(int productoId,int usuarioId)
        {
            
            var resultado = _productoService.ProductoUnidadesdisponible(productoId, usuarioId);
            return Ok(resultado);
        }
        [HttpGet, Route("anularasignacion/{productoTransaccionId}")]
        public IActionResult AnularAsignacion(int productoTransaccionId)
        {

            var resultado = _productoService.AnularAsignacion(productoTransaccionId);
            return Ok(resultado);
        }
    }
}