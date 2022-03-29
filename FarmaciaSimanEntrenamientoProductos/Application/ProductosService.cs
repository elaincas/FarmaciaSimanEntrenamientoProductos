using FarmaciaSimanEntrenamientoProductos.Application.Dtos;
using FarmaciaSimanEntrenamientoProductos.Creation.Entities;
using FarmaciaSimanEntrenamientoProductos.Creations.Entities;
using FarmaciaSimanEntrenamientoProductos.Infraestructura;
using FarmaciaSimanEntrenamientoProductos.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application
{
    public class ProductosService
    {
        private readonly IUnitOfWork _unitOfWork;
        public IConfiguration Configuration { get; }

        public ProductosService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            Configuration = configuration;
            
        }

        public List<ProductoDto> ObtenerTodosLosProductos()
        {
            try
            {
                var productos = (from prod in _unitOfWork.Repository<Productos>() .AsQueryable()
                                 where prod.Activo == true
                                 select new ProductoDto()
                                 {
                                     NombreProducto = prod.Descripcion,
                                     ProductoID = prod.ProductoID
                                 }).ToList();


                return productos.ToList();
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerTodosLosProductos");
                return new List<ProductoDto>();
            }

        }

        public List<EstadosProductos> ObtenesEstados()
        {
            try
            {
                var sucursales = (from estado in _unitOfWork.Repository<EstadosProductos>().AsQueryable()
                                  select new EstadosProductos()
                                  {
                                      EstadoID = estado.EstadoID,
                                      Descripcion = estado.Descripcion
                                  }).ToList();
                return sucursales;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message, "ObtenesEstados");
                return new List<EstadosProductos>();
            }

        }
 

        public ProductoDto IngresarProductos(ProductoDto ingreso)
        {
            try
            {
                string mensaje = string.Empty;
                if (!ingreso.EsDtoValidoParaIngreso(out mensaje))
                {
                    ingreso.RespuestaTipo = RespuestaTipo.Validacion;
                    ingreso.Respuesta = mensaje;
                    return ingreso;
                };

                Productos productoNuevo = new Productos()
                {
                    Descripcion = ingreso.NombreProducto,
                    Activo = true,
                    ProductosDetalles = new List<ProductosDetalles>()
                };

                var productoYaExiste = (from producto in _unitOfWork.Repository<Productos>().AsQueryable()
                                        where producto.Descripcion.ToLower() == ingreso.NombreProducto.ToLower()
                                        select producto).FirstOrDefault();

                if (productoYaExiste != null)
                    ingreso.ProductoID = productoYaExiste.ProductoID;

                for (int i = 0; i < ingreso.Cantidad; i++)
                {
                    ProductosDetalles productoDetalleNuevo = new ProductosDetalles()
                    {
                        ProductoID = ingreso.ProductoID,
                        EstadoID = ingreso.EstadoID,
                        FechaIngreso = DateTime.Now,
                        Observacion = ingreso.Observacion,
                        Precio = ingreso.Precio,
                        UsuarioIngresa = ingreso.UsuarioId,
                        Activo = true,
                        ZonaID = ingreso.ZonaID,
                        Transacciones = new List<TransaccionesProductos>()
                    };
                    TransaccionesProductos registrarTransaccion = new TransaccionesProductos()
                    {
                        EstadoID = ingreso.EstadoID,
                        FechaCambioEstado = DateTime.Now,
                        Activo = true,
                        ZonaID = ingreso.ZonaID,
                        UsuarioID = ingreso.UsuarioId
                    };

                    productoNuevo.ProductosDetalles.Add(productoDetalleNuevo);

                    productoDetalleNuevo.Transacciones.Add(registrarTransaccion);

                }

                if (productoYaExiste == null)
                    _unitOfWork.Repository<Productos>().Add(productoNuevo);
                else
                {
                    _unitOfWork.Repository<ProductosDetalles>().AddRange(productoNuevo.ProductosDetalles);
                }
                _unitOfWork.SaveChanges();

                ingreso.RespuestaTipo = RespuestaTipo.Ok;
                return ingreso;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "IngresarProductos");
                return new ProductoDto() { Respuesta = "Ocurrió un error al ingresar los productos.", RespuestaTipo = RespuestaTipo.Error };
            }

        }

        public List<ProductoDto> ObtenerProductosDisponiblesParaAsignar()
        {
            try
            {
                var ObtenerProductoBuenEstadoInicialYRecuperadoBuenEstado = (from prodDet in _unitOfWork.Repository<ProductosDetalles>().AsQueryable()
                                                                             join prod in _unitOfWork.Repository<Productos>().AsQueryable() on prodDet.ProductoID equals prod.ProductoID
                                                                             where prodDet.EstadoID == (int)EnumEstadosProductosDto.BuenEstadoInicial ||
                                                                             prodDet.EstadoID == (int)EnumEstadosProductosDto.RecuperadoBuenEstado &&
                                                                             prodDet.Activo == true && prod.Activo == true
                                                                             select new ProductoDto()
                                                                             {
                                                                                 ProductoID = prodDet.ProductoID,
                                                                                 ProductoDetalleID = prodDet.ProductoDetalleID,
                                                                                 NombreProducto = prod.Descripcion
                                                                             }).ToList();
                return ObtenerProductoBuenEstadoInicialYRecuperadoBuenEstado;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerProductosDisponiblesParaAsignar");
                return new List<ProductoDto>();
            }

        }


        public ProductoPrestamoDto CambiarEstadoDeProductoAsignado(ProductoPrestamoDto productoAsignado)
        {

            try
            {
                string mensaje = string.Empty;

                if (productoAsignado.esDtovalidoParaCambiarEstado(out mensaje))
                {
                    productoAsignado.Respuesta = mensaje;
                    productoAsignado.RespuestaTipo = RespuestaTipo.Validacion;
                    return productoAsignado;
                };

                var productoDetalle = _unitOfWork.Repository<ProductosDetalles>().FirstOrDefault(x => x.ProductoDetalleID == productoAsignado.ProductoDetalleID);

                productoDetalle.EstadoID = productoAsignado.EstadoID;

                TransaccionesProductos registrarTransaccion = new TransaccionesProductos()
                {
                    EstadoID = productoDetalle.EstadoID,
                    ProductoDetalleID = productoAsignado.ProductoDetalleID,
                    ProductoPrestamoID = productoAsignado.ProductoPrestamoId,
                    FechaCambioEstado = DateTime.Now,
                    UsuarioID = productoAsignado.UsuarioId,
                    Activo = true,
                    ZonaID = productoDetalle.ZonaID
                };

                _unitOfWork.Repository<TransaccionesProductos>().Add(registrarTransaccion);
                _unitOfWork.SaveChanges();

            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "CambiarEstadoDeProductoAsignado");
                return new ProductoPrestamoDto() { Respuesta = "Ocurrió un error al registrar la transacción.", RespuestaTipo = RespuestaTipo.Error };
            }

            return productoAsignado;
        }

        public AuxiliarProductoDto ObtenerAuxiliarDeProductos(FiltroProductosDto filtro)
        {
            try
            {

                string mensaje = string.Empty;
                if (!filtro.EsValidoElFiltro(out mensaje))
                {
                    return new AuxiliarProductoDto()
                    {
                        Respuesta = mensaje,
                        RespuestaTipo = RespuestaTipo.Validacion
                    };
                }
                AuxiliarProductoDto productosAuxiliar = new AuxiliarProductoDto();



                
                
                SqlConnection cnx = new SqlConnection(Configuration.GetConnectionString("EntrenamientoProducto"));
                SqlCommand cmd = new SqlCommand("exec spAuxiliarProductos '" + filtro.FechaDesde + "','" + filtro.FechaHasta + "'," + filtro.FiltroID);
                
                SqlDataAdapter adapter = new SqlDataAdapter(cmd.CommandText, cnx.ConnectionString);
                DataTable ds = new DataTable();
                adapter.Fill(ds);


                if (ds.Rows.Count == 0)
                {
                    productosAuxiliar.Respuesta = "No se encontraron datos";
                    productosAuxiliar.RespuestaTipo = RespuestaTipo.Validacion;
                    return productosAuxiliar;
                }

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var fecha = ds.Rows[i].ItemArray[6].ToString();
                    var fechaHoraDividida = fecha.Split(':');
                    var fechaDividida = fechaHoraDividida[0].Split('/');

                    var fechatraslado = ds.Rows[i].ItemArray[17].ToString();
                    var fechaTrasladoHoraDividida = fechatraslado.Split(':');
                    var fechaTrasladoDividida = fechaTrasladoHoraDividida[0].Split('/');

                    var auxiliar = new AuxiliarProductoDto()
                    {
                        ProductoDetalleID = Convert.ToInt32(ds.Rows[i].ItemArray[0].ToString()),
                        ProductoID = Convert.ToInt32(ds.Rows[i].ItemArray[1].ToString()),
                        Descripcion = ds.Rows[i].ItemArray[2].ToString(),
                        ProductoPrestamoID = Convert.ToInt32(ds.Rows[i].ItemArray[3].ToString()),
                        ColaboradorID = Convert.ToInt32(ds.Rows[i].ItemArray[4].ToString()),
                        Nombre = ds.Rows[i].ItemArray[5].ToString(),
                        FechaCambioEstado = new DateTime(int.Parse(fechaDividida[2].Split(' ')[0]), int.Parse(fechaDividida[0]), int.Parse(fechaDividida[1])),
                        EstadoID = Convert.ToInt32(ds.Rows[i].ItemArray[7].ToString()),
                        Estado = ds.Rows[i].ItemArray[8].ToString(),
                        UsuarioHaceTransaccion = Convert.ToInt32(ds.Rows[i].ItemArray[9].ToString()),
                        Usuario = ds.Rows[i].ItemArray[10].ToString(),
                        ObservacionDetallesProducto = ds.Rows[i].ItemArray[11].ToString(),
                        NoTraslado = Convert.ToInt32(ds.Rows[i].ItemArray[12].ToString()),
                        ZonaIDEnvia = Convert.ToInt32(ds.Rows[i].ItemArray[13].ToString()),
                        ZonaEnvia = ds.Rows[i].ItemArray[14].ToString(),
                        ZonaIDRecibe = Convert.ToInt32(ds.Rows[i].ItemArray[15].ToString()),
                        ZonaRecibe = ds.Rows[i].ItemArray[16].ToString(),
                        Fecha = new DateTime(int.Parse(fechaTrasladoDividida[2].Split(' ')[0]), int.Parse(fechaTrasladoDividida[0]), int.Parse(fechaTrasladoDividida[1])),
                        ZonaIdActual = Convert.ToInt32(ds.Rows[i].ItemArray[18].ToString()),
                        ZonaActual = ds.Rows[i].ItemArray[19].ToString(),
                    };
                    productosAuxiliar.ListaAuxiliar.Add(auxiliar);

                }

                productosAuxiliar.RespuestaTipo = RespuestaTipo.Ok;
                return productosAuxiliar;

            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerAuxiliarDeProductos");
                AuxiliarProductoDto productosAuxiliar = new AuxiliarProductoDto()
                {
                    Respuesta = "Ha ocurrido un error al obtener el auxiliar!",
                    RespuestaTipo = RespuestaTipo.Error
                };
                return productosAuxiliar;
            }
        }

        public ProductoPrestamoDto ObtenerProdutosEnMalEstados_Perdidos()
        {
            try
            {
                ProductoPrestamoDto productosDto = new ProductoPrestamoDto();
                List<ProductosDetalles> listaDetalleMalosPerdidos = _unitOfWork.Repository<ProductosDetalles>().Where(x => x.EstadoID == (int)EnumEstadosProductosDto.RecuperadoMalEstado || x.EstadoID == (int)EnumEstadosProductosDto.Perdido || x.EstadoID == (int)EnumEstadosProductosDto.MalEstado).ToList();

                var productos = (from p in _unitOfWork.Repository<Productos>() .AsQueryable()
                                 join t in _unitOfWork.Repository<TransaccionesProductos>().AsQueryable() on p.ProductoID equals t.ProductoDetalle.ProductoID
                                 join c in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable() on t.ProductoPrestamo.ColaboradorID equals c.ColaboradorID
                                 where t.EstadoID == (int)EnumEstadosProductosDto.RecuperadoMalEstado || t.EstadoID == (int)EnumEstadosProductosDto.Perdido || t.EstadoID == (int)EnumEstadosProductosDto.MalEstado
                                 select new ProductoPrestamoDto()
                                 {
                                     ProductoPrestamoId = t.ProductoPrestamo.ProductoPrestamoID,
                                     ProductoID = t.ProductoDetalle.ProductoID,
                                     Producto = p.Descripcion,
                                     Observacion = t.ProductoDetalle.Observacion,
                                     ColaboradorID = t.ProductoPrestamo.ColaboradorID,
                                     NombreColaborador = c.Nombre,
                                     Fecha = t.FechaCambioEstado,
                                     EstadoID = t.EstadoID,
                                     Estado = t.Estado.Descripcion,
                                     ValeId = t.ProductoPrestamo.ValeId
                                 }).ToList();

                productosDto.listaProductos = productos;

                productosDto.Respuesta = "Éxito";
                productosDto.RespuestaTipo = RespuestaTipo.Ok;
                return productosDto;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerProdutosEnMalEstados_Perdidos");
                return new ProductoPrestamoDto() { Respuesta = "No se puede obtener la información!", RespuestaTipo = RespuestaTipo.Error };
                throw;
            }
        }

        public ProductoPrestamoDto ActualizarDatoValeColaborador(ProductoPrestamoDto productoColaborador)
        {
            try
            {
                string mensaje = string.Empty;
                if (productoColaborador.esDtoValidoParaAsignarProductoAColaborador(out mensaje))
                {
                    productoColaborador.Respuesta = mensaje;
                    productoColaborador.RespuestaTipo = RespuestaTipo.Validacion;
                    return productoColaborador;
                }

                if (String.IsNullOrEmpty(productoColaborador.ValeId.ToString()))
                {
                    productoColaborador.Respuesta = "Ingrese un código de vale!";
                    productoColaborador.RespuestaTipo = RespuestaTipo.Validacion;
                    return productoColaborador;
                }


                ProductosPrestamos productoColaboradorActualizarVale = _unitOfWork.Repository<ProductosPrestamos>() .Where(x => x.ProductoPrestamoID == productoColaborador.ProductoPrestamoId).LastOrDefault();

                if (!String.IsNullOrEmpty(productoColaboradorActualizarVale.ValeId.ToString()))
                {
                    productoColaborador.Respuesta = "El colaborador con vhur " + productoColaboradorActualizarVale.ColaboradorID + " ya tiene un código de vale asignado!";
                    productoColaborador.RespuestaTipo = RespuestaTipo.Validacion;
                    return productoColaborador;
                }

                productoColaboradorActualizarVale.ValeId = productoColaborador.ValeId;
                productoColaboradorActualizarVale.UsuarioIngresaVale = productoColaborador.UsuarioId;
                _unitOfWork.SaveChanges();

                productoColaborador.Respuesta = "Éxito";
                productoColaborador.RespuestaTipo = RespuestaTipo.Ok;

                return productoColaborador;
            }
            catch (Exception ex)
            {
                productoColaborador.Respuesta = "Ocurrió un error al actualizar el código de vale!";
                productoColaborador.RespuestaTipo = RespuestaTipo.Error;
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ActualizarDatoValeColaborador");
                return productoColaborador;
            }

        }

        private void InsertarLogEvento(string mensaje, string detalles = "", string pantalla = "")
        {
            Assembly thisAssem = typeof(FarmaciaSimanEntrenamientoProductos.Startup).Assembly;
            AssemblyName thisAssemName = thisAssem.GetName();

            Version version = thisAssemName.Version;

            LogEventos log = new LogEventos()
            {
                Descripcion = "Exception: " + mensaje + " Mas Detalles Exception: " + detalles,
                Fecha = DateTime.Now,
                Pantalla = pantalla,
                VersionSistema = version.ToString()
            };
            _unitOfWork.Repository<LogEventos>().Add(log);
            _unitOfWork.SaveChanges();
        }

        public ProductoUnidadesdisponibleDto ProductoUnidadesdisponible(int productoId, int usuarioId)
        {
            ProductoUnidadesdisponibleDto producto = new ProductoUnidadesdisponibleDto() { Id = productoId };
            var zonas = _unitOfWork.Repository<ZonasEncargados>().Where(x => x.UsuarioEncargadoID == usuarioId).Select(z => z.ZonaID).ToList();
            if (zonas.Count() == 0)
            {
                zonas.Add(1);
            }
            producto.Descripcion = _unitOfWork.Repository<Productos>(). FirstOrDefault(p => p.ProductoID == productoId).Descripcion;
            var searchestados = new List<int> { 1, 2 };
            var lotes = _unitOfWork.Repository<ProductosDetalles>().Where(p => p.ProductoID == productoId && p.Activo && searchestados.Contains(p.EstadoID) && zonas.Contains((int)p.ZonaID)).ToList();

            producto.UnidadesNuevas = lotes.Count(l => l.EstadoID == 1);
            producto.Recurperadas = lotes.Count(l => l.EstadoID == 2);

            return producto;
        }

        public bool AnularAsignacion(int productoTransaccionId)
        {
            var repositoryTransaccionesProductos = _unitOfWork.Repository<TransaccionesProductos>();

            var trasnaccionprod = repositoryTransaccionesProductos.FirstOrDefault(t => t.TransaccionesProductosID == productoTransaccionId && t.EstadoID == 6);
            if (trasnaccionprod == null) { return false; }

            var prestamoprod = _unitOfWork.Repository<ProductosPrestamos>().FirstOrDefault(t => t.ProductoPrestamoID == trasnaccionprod.ProductoPrestamoID);

            trasnaccionprod.Activo = false;
            prestamoprod.Activo = false;

            int transaccionesProductosIDAnterior = repositoryTransaccionesProductos.Where(t => t.Activo && t.ProductoDetalleID == trasnaccionprod.ProductoDetalleID && t.TransaccionesProductosID < trasnaccionprod.TransaccionesProductosID).Max(t => t.TransaccionesProductosID);
            int estadoIDAnterior = repositoryTransaccionesProductos.FirstOrDefault(t => t.TransaccionesProductosID == transaccionesProductosIDAnterior).EstadoID;
            _unitOfWork.Repository<ProductosDetalles>().FirstOrDefault(p => p.ProductoDetalleID == trasnaccionprod.ProductoDetalleID).EstadoID = estadoIDAnterior;
            _unitOfWork.SaveChanges();
            return true;
        }
    }

}
