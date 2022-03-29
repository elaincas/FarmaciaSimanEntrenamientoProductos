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
using System.Text;
using System.Threading.Tasks;

namespace FarmaciaSimanEntrenamientoProductos.Application
{
    public class CommonsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IConfiguration Configuration { get; }

        public CommonsService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            Configuration = configuration;
        }

        public ProductoPrestamoDto ObtenerInformacionColaborador(int codigoColaborador, bool colaboradorDespedido = false)
        {
            try
            {
                ProductoPrestamoDto informacionColaboradorProducto = new ProductoPrestamoDto();
                if (!colaboradorDespedido)
                {

                    informacionColaboradorProducto = (from transacciones in _unitOfWork.Repository<TransaccionesProductos>().AsQueryable()
                                                      join prodDet in _unitOfWork.Repository<ProductosDetalles>().AsQueryable() on transacciones.ProductoDetalleID equals prodDet.ProductoDetalleID
                                                      join colaborador in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable() on transacciones.ProductoPrestamo.ColaboradorID equals colaborador.ColaboradorID
                                                      where transacciones.ProductoPrestamo.ColaboradorID == codigoColaborador && transacciones.ProductoPrestamo.Activo && colaborador.Activo == true
                                                      select new ProductoPrestamoDto()
                                                      {
                                                          EstadoID = transacciones.EstadoID,
                                                          ProductoID = prodDet.ProductoID,
                                                          Fecha = transacciones.ProductoPrestamo.FechaPrestamo,
                                                          Observacion = prodDet.Observacion,
                                                          ProductoDetalleID = transacciones.ProductoDetalleID,
                                                          NombreColaborador = colaborador.Nombre,
                                                          RespuestaTipo = RespuestaTipo.Ok,
                                                          Cantidad = 1,
                                                          Estado = transacciones.Estado.Descripcion,
                                                          ColaboradorID = transacciones.ProductoPrestamo.ColaboradorID
                                                      }).LastOrDefault();
                }
                else
                {
                    informacionColaboradorProducto = (from transacciones in _unitOfWork.Repository<TransaccionesProductos>().AsQueryable()
                                                      join prodDet in _unitOfWork.Repository<ProductosDetalles>().AsQueryable() on transacciones.ProductoDetalleID equals prodDet.ProductoDetalleID
                                                      join colaborador in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable() on transacciones.ProductoPrestamo.ColaboradorID equals colaborador.ColaboradorID
                                                      where transacciones.ProductoPrestamo.ColaboradorID == codigoColaborador && transacciones.ProductoPrestamo.Activo && colaborador.Activo == false
                                                      select new ProductoPrestamoDto()
                                                      {
                                                          EstadoID = transacciones.EstadoID,
                                                          ProductoID = prodDet.ProductoID,
                                                          Fecha = transacciones.ProductoPrestamo.FechaPrestamo,
                                                          Observacion = prodDet.Observacion,
                                                          ProductoDetalleID = transacciones.ProductoDetalleID,
                                                          NombreColaborador = colaborador.Nombre,
                                                          RespuestaTipo = RespuestaTipo.Ok,
                                                          Cantidad = 1,
                                                          Estado = transacciones.Estado.Descripcion,
                                                          ColaboradorID = transacciones.ProductoPrestamo.ColaboradorID
                                                      }).LastOrDefault();
                }

                if (informacionColaboradorProducto == null)
                {
                    informacionColaboradorProducto = new ProductoPrestamoDto();

                    var colaborador = (from c in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable()
                                       where c.ColaboradorID == codigoColaborador
                                       select c).FirstOrDefault();
                    if (colaborador == null)
                    {
                        return new ProductoPrestamoDto() { Respuesta = "Colaborador no encontrado.", RespuestaTipo = RespuestaTipo.Validacion };
                    }

                    informacionColaboradorProducto.NombreColaborador = colaborador.Nombre;

                }
                informacionColaboradorProducto.RespuestaTipo = RespuestaTipo.Ok;
                return informacionColaboradorProducto;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, "", "ObtenerInformacionColaborador");
                return new ProductoPrestamoDto() { Respuesta = "Ocurrió un error al obtener la información.", RespuestaTipo = RespuestaTipo.Error };
            }

        }

        public ProductoPrestamoDto RecuperarProducto(ProductoPrestamoDto asignacion, bool esDataColaboradorDespedido = false)
        {
            try
            {
                string mensaje = string.Empty;
                if (!asignacion.esDtoValidoParaAsignarProductoAColaborador(out mensaje))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }


                if (ColaboradorTieneReporteProductoRecuperado(asignacion, out mensaje))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }

                if (!ColaboradorYaTieneProductoAsignado(asignacion, out mensaje, esDataColaboradorDespedido))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }
                ProductosDetalles productoDetalle = new ProductosDetalles();
                ProductosPrestamos productoParaAsignarAColaborador = new ProductosPrestamos();
                ProductoPrestamoDto productoAsignacion = ObtenerInformacionColaborador(asignacion.ColaboradorID, esDataColaboradorDespedido);
                productoDetalle = _unitOfWork.Repository<ProductosDetalles>().Where(x => x.ProductoDetalleID == productoAsignacion.ProductoDetalleID).FirstOrDefault();

                productoDetalle.EstadoID = asignacion.EstadoID;
                productoDetalle.Observacion = asignacion.Observacion;


                TransaccionesProductos registrarTransaccion = new TransaccionesProductos()
                {
                    UsuarioID = asignacion.UsuarioId,
                    EstadoID = productoDetalle.EstadoID,
                    ProductoDetalleID = productoDetalle.ProductoDetalleID,
                    FechaCambioEstado = DateTime.Now,
                    Activo = true,
                    ZonaID = productoDetalle.ZonaID
                };


                registrarTransaccion.ProductoPrestamoID = _unitOfWork.Repository<ProductosPrestamos>().Where(x => x.ColaboradorID == asignacion.ColaboradorID).LastOrDefault().ProductoPrestamoID;
                _unitOfWork.Repository<TransaccionesProductos>().Add(registrarTransaccion);

                _unitOfWork.SaveChanges();
                asignacion.RespuestaTipo = RespuestaTipo.Ok;
                return asignacion;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, "", "RecuperarProducto");
                return new ProductoPrestamoDto() { Respuesta = "Ha ocurrido un error.", RespuestaTipo = RespuestaTipo.Error };
            }
        }


        public ProductoPrestamoDto AsignarProductoAColaborador(ProductoPrestamoDto asignacion)
        {
            try
            {
                string mensaje = string.Empty;

                if (!asignacion.esDtoValidoParaAsignarProductoAColaborador(out mensaje))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }

                if (ColaboradorTieneReporteProductoRecuperado(asignacion, out mensaje))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }

                if (ColaboradorYaTieneProductoAsignado(asignacion, out mensaje))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                }

                if (!HayProductoDisponibleParaAsignar(asignacion, out mensaje) &&
                    (asignacion.EstadoID != (int)EnumEstadosProductosDto.Perdido && asignacion.EstadoID != (int)EnumEstadosProductosDto.RecuperadoMalEstado))
                {
                    asignacion.Respuesta = mensaje;
                    asignacion.RespuestaTipo = RespuestaTipo.Validacion;
                    return asignacion;
                };


                ProductosDetalles productoDetalle = new ProductosDetalles();
                ProductosPrestamos productoParaAsignarAColaborador = new ProductosPrestamos();


                productoDetalle = _unitOfWork.Repository<ProductosDetalles>().FirstOrDefault(x => x.ProductoID == asignacion.ProductoID && x.EstadoID == asignacion.EstadoID && x.Activo == true);

                productoDetalle.EstadoID = (int)EnumEstadosProductosDto.Asignado;
                productoParaAsignarAColaborador = new ProductosPrestamos()
                {
                    FechaPrestamo = DateTime.Now,
                    UsuarioPrestaID = asignacion.UsuarioId,
                    ColaboradorID = asignacion.ColaboradorID,
                    Activo = true
                };


                productoDetalle.Observacion = asignacion.Observacion;


                TransaccionesProductos registrarTransaccion = new TransaccionesProductos()
                {
                    UsuarioID = asignacion.UsuarioId,
                    EstadoID = productoDetalle.EstadoID,
                    ProductoDetalleID = productoDetalle.ProductoDetalleID,
                    Activo = true,
                    FechaCambioEstado = DateTime.Now,
                    ZonaID = productoDetalle.ZonaID
                };

                productoParaAsignarAColaborador.Transacciones.Add(registrarTransaccion);
                _unitOfWork.Repository<ProductosPrestamos>().Add(productoParaAsignarAColaborador);

                _unitOfWork.SaveChanges();
                asignacion.RespuestaTipo = RespuestaTipo.Ok;
                return asignacion;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, "", "AsignarProductoAColaborador");
                return new ProductoPrestamoDto() { Respuesta = "Ha ocurrido un error en la asignación.", RespuestaTipo = RespuestaTipo.Error };
            }
        }

        private bool ColaboradorTieneReporteProductoRecuperado(ProductoPrestamoDto asignacion, out string mensaje)
        {
            try
            {
                mensaje = string.Empty;

                var infoColaborador = ObtenerInformacionColaborador(asignacion.ColaboradorID);

                if (infoColaborador.ProductoID != 0 && (infoColaborador.EstadoID == (int)EnumEstadosProductosDto.RecuperadoMalEstado || infoColaborador.EstadoID == (int)EnumEstadosProductosDto.RecuperadoBuenEstado))
                {
                    mensaje = "El colaborador ya tiene reporte de producto entregado.";
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, "", "ColaboradorTieneReporteProductoRecuperado"); ;
                mensaje = string.Empty;
                return false;
            }

        }

        private bool ColaboradorYaTieneProductoAsignado(ProductoPrestamoDto asignacion, out string mensaje, Boolean esDataColaboradorDespedido = false)
        {
            try
            {
                mensaje = string.Empty;
                var infoColaborador = ObtenerInformacionColaborador(asignacion.ColaboradorID, esDataColaboradorDespedido);

                if (infoColaborador.ProductoID != 0 && (infoColaborador.EstadoID == (int)EnumEstadosProductosDto.Asignado || infoColaborador.EstadoID == (int)EnumEstadosProductosDto.Standby))
                {
                    mensaje = "El colaborador ya tiene un producto asignado";
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ColaboradorYaTieneProductoAsignado");
                mensaje = string.Empty;
                return false;
            }

        }

        private bool HayProductoDisponibleParaAsignar(ProductoPrestamoDto asignacion, out string mensaje)
        {
            try
            {
                mensaje = "";
                if (asignacion.EstadoID == (int)EnumEstadosProductosDto.BuenEstadoInicial || asignacion.EstadoID == (int)EnumEstadosProductosDto.RecuperadoBuenEstado)
                {
                    var zonas = _unitOfWork.Repository<ZonasEncargados>().Where(x => x.UsuarioEncargadoID == asignacion.UsuarioId).Select(z => z.ZonaID).ToList();
                    if (zonas.Count() == 0)
                    {
                        zonas.Add(1);
                    }
                    var productosDisponibles = _unitOfWork.Repository<ProductosDetalles>().Where(x => x.ProductoID == asignacion.ProductoID && x.EstadoID == asignacion.EstadoID && x.Activo == true && zonas.Contains((int)x.ZonaID)).ToList();

                    if (productosDisponibles.Count == 0)
                    {
                        mensaje = "No hay suficiente inventario para poder asignar.";
                        return false;
                    }
                    return true;
                }
                mensaje = "No se puede asignar producto con el estado seleccionado.";
                return false;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, "", "HayProductoDisponibleParaAsignar");
                mensaje = "No se puede asignar producto con el estado seleccionado.";
                return false;
            }

        }
        public List<ProductoPrestamoDto> ObteberProductoAsignadosAColaboradorOrdenadosPorEstados(FiltroProductosDto filtro)
        {
            try
            {
                string mensaje = string.Empty;
                if (!filtro.EsValidoElFiltro(out mensaje))
                {
                    return new List<ProductoPrestamoDto>();
                }


                List<ProductoPrestamoDto> productosColaboradores = new List<ProductoPrestamoDto>();

                SqlConnection cnx = new SqlConnection(Configuration.GetConnectionString("EntrenamientoProducto"));
                SqlCommand cmd = new SqlCommand("exec spReporteProductoAsignadosAColaboradorOrdenadosPorEstados '" + filtro.FechaDesde + "','" + filtro.FechaHasta + "'," + filtro.FiltroID, cnx);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd.CommandText, cnx.ConnectionString);
                DataTable ds = new DataTable();
                adapter.Fill(ds);

                if (ds.Rows.Count == 0)
                    return productosColaboradores;

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var fecha = ds.Rows[i].ItemArray[5].ToString();
                    var fechaHoraDividida = fecha.Split(':');
                    var fechaDividida = fechaHoraDividida[0].Split('/');
                    ProductoPrestamoDto productoColaborador = new ProductoPrestamoDto()
                    {
                        ProductoID = int.Parse(ds.Rows[i].ItemArray[0].ToString()),
                        Producto = ds.Rows[i].ItemArray[1].ToString(),
                        ProductoDetalleID = int.Parse(ds.Rows[i].ItemArray[2].ToString()),
                        EstadoID = int.Parse(ds.Rows[i].ItemArray[3].ToString()),
                        Estado = ds.Rows[i].ItemArray[4].ToString(),
                        Fecha = new DateTime(int.Parse(fechaDividida[2].Split(' ')[0]), int.Parse(fechaDividida[1]), int.Parse(fechaDividida[0])),
                        ColaboradorID = int.Parse(ds.Rows[i].ItemArray[6].ToString()),
                        NombreColaborador = ds.Rows[i].ItemArray[7].ToString(),
                        TransaccionesProductosId = int.Parse(ds.Rows[i].ItemArray[8].ToString()),
                        Asigna = ds.Rows[i].ItemArray[9].ToString()
                    };
                    productosColaboradores.Add(productoColaborador);
                };
                return productosColaboradores.ToList();

            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObteberProductoAsignadosAColaboradorOrdenadosPorEstados");
                return new List<ProductoPrestamoDto>();
            }
        }


        public List<ProductoPrestamoDto> ObteberProductoAsignadosAColaboradorOrdenadosPorEstado(FiltroProductosDto filtro)
        {
            try
            {
                string mensaje = string.Empty;
                if (!filtro.EsValidoElFiltro(out mensaje))
                {
                    return new List<ProductoPrestamoDto>();
                }


                List<ProductoPrestamoDto> productoPrestamos = new List<ProductoPrestamoDto>();

                SqlConnection cnx = new SqlConnection(Configuration.GetConnectionString("EntrenamientoProducto"));
                SqlCommand cmd = new SqlCommand("exec spReporteProductoAsignadosAColaboradorOrdenadosPorEstados '" + filtro.FechaDesde + "','" + filtro.FechaHasta + "'," + filtro.FiltroID, cnx);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd.CommandText, cnx.ConnectionString);
                DataTable ds = new DataTable();
                adapter.Fill(ds);


                if (ds.Rows.Count == 0)
                    return productoPrestamos;

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var fecha = ds.Rows[i].ItemArray[5].ToString();
                    var fechaHoraDividida = fecha.Split(':');
                    var fechaDividida = fechaHoraDividida[0].Split('/');
                    ProductoPrestamoDto productoColaborador = new ProductoPrestamoDto()
                    {
                        ProductoID = int.Parse(ds.Rows[i].ItemArray[0].ToString()),
                        Producto = ds.Rows[i].ItemArray[1].ToString(),
                        ProductoDetalleID = int.Parse(ds.Rows[i].ItemArray[2].ToString()),
                        EstadoID = int.Parse(ds.Rows[i].ItemArray[3].ToString()),
                        Estado = ds.Rows[i].ItemArray[4].ToString(),
                        Fecha = new DateTime(int.Parse(fechaDividida[2].Split(' ')[0]), int.Parse(fechaDividida[0]), int.Parse(fechaDividida[1])),
                        ColaboradorID = int.Parse(ds.Rows[i].ItemArray[6].ToString()),
                        NombreColaborador = ds.Rows[i].ItemArray[7].ToString(),
                        TransaccionesProductosId = int.Parse(ds.Rows[i].ItemArray[8].ToString()),
                        Asigna = ds.Rows[i].ItemArray[9].ToString()
                    };
                    productoPrestamos.Add(productoColaborador);
                }



                return productoPrestamos;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObteberProductoAsignadosAColaboradorOrdenadosPorEstados");
                return new List<ProductoPrestamoDto>();
            }
        }

        public List<ProductoDto> ObtenerProductosDisponiblesParaAsignar(int usuario)
        {
            try
            {
                List<ProductoDto> productos = new List<ProductoDto>();

                SqlConnection cnx = new SqlConnection(Configuration.GetConnectionString("EntrenamientoProducto"));
                SqlCommand cmd = new SqlCommand("exec spReporteProductosDisponibleParaAsignarPorEstados " + usuario.ToString());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd.CommandText, cnx.ConnectionString);
                DataTable ds = new DataTable();
                adapter.Fill(ds);


                if (ds.Rows.Count == 0)
                    return productos;

                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    var producto = new ProductoDto()
                    {
                        Cantidad = Convert.ToInt32(ds.Rows[i].ItemArray[0].ToString()),
                        ProductoID = Convert.ToInt32(ds.Rows[i].ItemArray[1].ToString()),
                        NombreProducto = ds.Rows[i].ItemArray[2].ToString(),
                        EstadoID = Convert.ToInt32(ds.Rows[i].ItemArray[3].ToString()),
                        Estado = ds.Rows[i].ItemArray[4].ToString(),
                        Observacion = ds.Rows[i].ItemArray[5].ToString()
                    };

                    productos.Add(producto);
                }

                return productos.OrderBy(x => x.FechaIngreso).ToList();
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerProductosDisponiblesParaAsignar");
                return new List<ProductoDto>();
            }

        }

        public List<ZonasDto> ObtenerZonas()
        {
            try
            {
                var zonas = (from z in _unitOfWork.Repository<Zonas>().AsQueryable()
                             select new ZonasDto()
                             {
                                 Zona = z.Zona,
                                 ZonaID = z.ZonaID
                             }).ToList();


                return zonas.ToList();
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerZonas");
                return new List<ZonasDto>();
            }
        }

        public ZonasEncargadosDto AsignarEncargadoDeZona(ZonasEncargadosDto zonaEncargado)
        {
            string mensaje = string.Empty;
            if (!zonaEncargado.EsDtoValidoParaIngreso(out mensaje))
            {
                zonaEncargado.RespuestaTipo = RespuestaTipo.Validacion;
                zonaEncargado.Respuesta = mensaje;
                return zonaEncargado;
            }
            try
            {
                ZonasEncargados zona = new ZonasEncargados()
                {

                    Descripcion = zonaEncargado.Descripcion,
                    UsuarioEncargadoID = zonaEncargado.UsuarioEncargadoID,
                    ZonaID = zonaEncargado.ZonaID,
                    Activo = true
                };

                _unitOfWork.Repository<ZonasEncargados>().Add(zona);
                _unitOfWork.SaveChanges();

                zonaEncargado.RespuestaTipo = RespuestaTipo.Ok;
                return zonaEncargado;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "AsignarEncargadoDeZona");
                mensaje = "Ocurrió un error al registrar la zona!";
                zonaEncargado.RespuestaTipo = RespuestaTipo.Error;
                return zonaEncargado;
            }


        }

        public TrasladosDto RealizarTraslado(List<TrasladosDto> traslados)
        {
            try
            {
                foreach (var traslado in traslados)
                {

                    List<TrasladosDetalle> trasladosDetalles = new List<TrasladosDetalle>();
                    string mensaje = string.Empty;
                    if (!traslado.EsDtoValidoParaIngreso(out mensaje))
                    {
                        traslado.Respuesta = mensaje;
                        traslado.RespuestaTipo = RespuestaTipo.Validacion;
                        return traslado;
                    }

                    var productosDetalles = _unitOfWork.Repository<ProductosDetalles>().Where(x => x.ProductoID == traslado.ProductoID && x.Activo == true && (x.EstadoID == (int)EnumEstadosProductosDto.BuenEstadoInicial || x.EstadoID == (int)EnumEstadosProductosDto.RecuperadoBuenEstado));

                    if (productosDetalles.Count() < traslado.Cantidad)
                    {
                        traslado.Respuesta = "No hay suficiente inventario para trasladar!";
                        traslado.RespuestaTipo = RespuestaTipo.Validacion;
                        return traslado;
                    }


                    for (int i = 0; i < traslado.Cantidad; i++)
                    {

                        TrasladosDetalle trasladoDetalle = new TrasladosDetalle()
                        {
                            EstadoID = (int)EnumEstadosProductosDto.TrasladoEnProceso,
                            FechaCambioEstado = DateTime.Now,
                            Observacion = productosDetalles.ToList()[i].Observacion,
                            ProductoDetalleID = productosDetalles.ToList()[i].ProductoDetalleID,
                            UsuarioID = traslado.UsuarioID
                        };



                        productosDetalles.ToList()[i].EstadoID = (int)EnumEstadosProductosDto.TrasladoEnProceso;
                        _unitOfWork.SaveChanges();
                        trasladosDetalles.Add(trasladoDetalle);
                    }



                    Traslados trasladoAGuardar = new Traslados()
                    {
                        Activo = true,
                        Fecha = DateTime.Now,
                        TrasladosDetalles = trasladosDetalles,
                        UsuarioID = traslado.UsuarioID,
                        ZonaIDEnvia = traslado.ZonaIDEnvia,
                        ZonaIDRecibe = traslado.ZonaIDRecibe
                    };
                    _unitOfWork.Repository<Traslados>().Add(trasladoAGuardar);
                    _unitOfWork.SaveChanges();

                }
                return new TrasladosDto()
                {
                    RespuestaTipo = RespuestaTipo.Ok
                };

            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "RealizarTraslado");
                return new TrasladosDto()
                {
                    Respuesta = "Ocurrió un error en el traslado!",
                    RespuestaTipo = RespuestaTipo.Error

                };

            }

        }

        public TrasladosDto AceptarDenegarTraslado(TrasladosDto trasladoDto)
        {
            try
            {
                var traslado = _unitOfWork.Repository<Traslados>().FirstOrDefault(x => x.TrasladoID == trasladoDto.TrasladoID);

                if (traslado == null)
                {
                    trasladoDto.Respuesta = "No se encontró el traslado!";
                    trasladoDto.RespuestaTipo = RespuestaTipo.Validacion;
                    return trasladoDto;
                }
                traslado.TrasladosDetalles = _unitOfWork.Repository<TrasladosDetalle>().Where(x => x.TrasladoID == traslado.TrasladoID).ToList();

                traslado.TrasladosDetalles.ForEach(data =>
                {
                    data.EstadoID = trasladoDto.EstadoID;
                    data.FechaCambioEstado = DateTime.Now;

                    var productoDetalle = _unitOfWork.Repository<ProductosDetalles>().FirstOrDefault(x => x.ProductoDetalleID == data.ProductoDetalleID);

                    productoDetalle.EstadoID = (int)EnumEstadosProductosDto.BuenEstadoInicial;
                    productoDetalle.ZonaID = traslado.ZonaIDRecibe;

                    TransaccionesProductos transaccion = new TransaccionesProductos()
                    {
                        Activo = true,
                        ZonaID = traslado.ZonaIDRecibe,
                        EstadoID = (int)EnumEstadosProductosDto.BuenEstadoInicial,
                        FechaCambioEstado = DateTime.Now,
                        ProductoDetalleID = data.ProductoDetalleID,
                        UsuarioID = traslado.UsuarioID,
                    };

                    _unitOfWork.Repository<TransaccionesProductos>().Add(transaccion);
                    _unitOfWork.SaveChanges();
                });

                trasladoDto.RespuestaTipo = RespuestaTipo.Ok;
                return trasladoDto;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "AceptarDenegarTraslado");
                trasladoDto.RespuestaTipo = RespuestaTipo.Error;
                trasladoDto.Respuesta = "Ocurrió un error en el traslado!";
                return trasladoDto;
            }
        }

        public List<TrasladosDto> ObtenerTrasladosPorUsuarioEncargadoDeZona(int usuario)
        {
            try
            {
                var trasladosPorEncargado = (from traslado in _unitOfWork.Repository<Traslados>().AsQueryable()
                                             join trasladoDetalle in _unitOfWork.Repository<TrasladosDetalle>().AsQueryable() on traslado.TrasladoID equals trasladoDetalle.TrasladoID
                                             join zona in _unitOfWork.Repository<ZonasEncargados>().AsQueryable() on traslado.ZonaIDRecibe equals zona.ZonaID
                                             where zona.UsuarioEncargadoID == usuario && trasladoDetalle.EstadoID == (int)EnumEstadosProductosDto.TrasladoEnProceso
                                             select new TrasladosDto()
                                             {
                                                 Cantidad = traslado.TrasladosDetalles.Count(),
                                                 ProductoID = trasladoDetalle.ProductoDetalle.ProductoID,
                                                 ZonaIDEnvia = traslado.ZonaIDEnvia,
                                                 ZonaIDRecibe = traslado.ZonaIDRecibe,
                                                 UsuarioID = traslado.UsuarioID,
                                                 TrasladoID = traslado.TrasladoID,
                                                 Fecha = traslado.Fecha,
                                                 ProductosTraslado =  new List<TrasladosDto>()
                                             }).ToList();

                foreach (var item in trasladosPorEncargado)
                {
                    var traslado = new TrasladosDto()
                    {
                        ProductoID = item.ProductoID,
                        Cantidad = item.Cantidad
                    };
                    item.ProductosTraslado.Add(traslado);
                }

                return trasladosPorEncargado;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerTrasladosPorUsuarioEncargadoDeZona");
                return new List<TrasladosDto>();
            }
        }

        public List<ZonasDto> ObtenerZonasAsignadasAlUsuario(int usuario)
        {
            try
            {
                var zonas = (from zonaEncargado in _unitOfWork.Repository<ZonasEncargados>().AsQueryable()
                             join zona in _unitOfWork.Repository<Zonas>().AsQueryable() on zonaEncargado.ZonaID equals zona.ZonaID
                             where zonaEncargado.UsuarioEncargadoID == usuario
                             select new ZonasDto()
                             {
                                 ZonaID = zonaEncargado.ZonaID,
                                 Zona = zona.Zona
                             }).ToList();
                return zonas;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerZonasAsignadasAlUsuario");
                return new List<ZonasDto>();
            }
        }
        public ColaboradorDto ObtenerColaboradores()
        {
            try
            {
                var colaboradores = (from c in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable()
                                     select new ColaboradorDto()
                                     {
                                         ColaboradorID = c.ColaboradorID,
                                         Nombre = c.Nombre,
                                     }).ToList();
                ColaboradorDto colaborador = new ColaboradorDto();
                colaborador.listaColaboradores = colaboradores;
                colaborador.Respuesta = "Éxito";
                colaborador.RespuestaTipo = RespuestaTipo.Ok;
                return colaborador;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerColaboradores");
                return new ColaboradorDto()
                {
                    Respuesta = "No se logró obtener los colaboradores",
                    RespuestaTipo = RespuestaTipo.Error,
                    listaColaboradores = new List<ColaboradorDto>()
                };
            }
        }
        public ColaboradorDto ObtenerColaboradoresDespedidos()
        {
            try
            {
                var colaboradores = (from c in _unitOfWork.Repository<ColaboradorVHUR>().AsQueryable()
                                     where c.Activo == true
                                     select new ColaboradorDto()
                                     {
                                         ColaboradorID = c.ColaboradorID,
                                         Nombre = c.Nombre,
                                     }).ToList();
                ColaboradorDto colaborador = new ColaboradorDto();
                colaborador.listaColaboradores = colaboradores;
                colaborador.Respuesta = "Éxito";
                colaborador.RespuestaTipo = RespuestaTipo.Ok;
                return colaborador;
            }
            catch (Exception ex)
            {
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "ObtenerColaboradores");
                return new ColaboradorDto()
                {
                    Respuesta = "No se logró obtener los colaboradores",
                    RespuestaTipo = RespuestaTipo.Error,
                    listaColaboradores = new List<ColaboradorDto>()
                };
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
            //_context.Add(log);
            _unitOfWork.Repository<LogEventos>().Add(log);
            _unitOfWork.SaveChanges();
        }


        public UsuarioDto IniciarSesion(CredencialesDto credenciales, out string Mensaje)
        {
            try
            {
                Mensaje = "Ok";
                var usuarioDto = new UsuarioDto();

                if (String.IsNullOrEmpty(credenciales.Usuario))
                {
                    Mensaje = "Favor ingresar un usuario válido";
                    return new UsuarioDto();

                }
                if (String.IsNullOrEmpty(credenciales.Clave))
                {

                    Mensaje = "Favor ingresar una clave válida";
                    return new UsuarioDto();
                }


                var usuario = _unitOfWork.Repository<UsuarioInformacionGeneral>().Where(x => x.Usuario == credenciales.Usuario && x.Activo == true
                            //&& x.Clave.ToString().Contains(claveMD5)   
                            ).FirstOrDefault();

                if (usuario == null)
                {
                    Mensaje = "No se encontró el usuario ingresado o posiblemente esté inaáctivo, Favor contactarse con soporte técnico";
                    return new UsuarioDto();
                }

                string claveMD5 = GenerateMD5(credenciales.Clave);
                bool esClaveValida = EsClaveValida(usuario.Clave, claveMD5);
                if (esClaveValida == false)
                {
                    Mensaje = "Verifique la contraseña o contactarse con soporte técnico";
                    return new UsuarioDto();
                }
                usuarioDto.Usuario = usuario.Usuario;
                usuarioDto.Id = usuario.ID;
                usuarioDto.Token = "";

                return usuarioDto;

            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                InsertarLogEvento(ex.Message, ex.InnerException.Message, "IniciarSesion");
                return new UsuarioDto();
            }
        }


        private string GenerateMD5(string input)
        {

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        private bool EsClaveValida(byte[] claveUsuarioEntidad, string claveIngresada)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < claveUsuarioEntidad.Length; i++)
            {
                sb.Append(claveUsuarioEntidad[i].ToString("X2"));
            }

            if (sb.ToString() != claveIngresada)
            {
                return false;
            }

            return true;
        }

    }
}
