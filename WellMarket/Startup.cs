using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WellMarket.Helpers;
using WellMarket.Repository;
using WellMarket.Services;

namespace WellMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region SERVICIOS

            services.AddSingleton<IRolEmpresa, RolEmpresaRepository>();
            services.AddSingleton<IConnection, ConnectionString>();
            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<IZona, ZonaRepository>();
            services.AddSingleton<IMunicipio, MunicipioRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IDisponibleP, DisponiblePRepository>();
            services.AddSingleton<ICategoriaP, CategoriaPRepository>();
            services.AddSingleton<IProducto, ProductoRepository>();
            services.AddSingleton<IVentas, VentasRepository>();
            services.AddSingleton<IEstatusT, EstatusTicketRepository>();
            services.AddSingleton<IEmpresa, EmpresaRepository>();
            services.AddSingleton<IMesa, MesaRepository>();
            services.AddSingleton<IGasto, GastoRepository>();
            services.AddSingleton<IIngreso, IngresoRepository>();
            services.AddSingleton<IReporte, ReporteRepository>();

            #endregion

            services.AddScoped<IAuthService, AuthService>();

            services.AddCors();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
