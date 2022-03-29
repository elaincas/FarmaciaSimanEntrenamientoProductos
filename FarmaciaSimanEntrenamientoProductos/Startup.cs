using FarmaciaSimanEntrenamientoProductos.Application;
using FarmaciaSimanEntrenamientoProductos.Infraestructura;
using FarmaciaSimanEntrenamientoProductos.Infraestructure;
using FarmaciaSimanEntrenamientoProductos.Insfraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace FarmaciaSimanEntrenamientoProductos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //se resuelve objetos complejos y se agrega como provider en el AddScoped de  ConfigureServices
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EntrenamientoProductosDbContext >(options => options.UseSqlServer(Configuration.GetConnectionString("EntrenamientoProducto")));
            services.AddTransient<ProductosService, ProductosService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<CommonsService, CommonsService>();



        

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            builder.WithOrigins("*" , "http://farmaciasiman.site:1200").AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());

            app.UseMvc();

        }

    }
}
