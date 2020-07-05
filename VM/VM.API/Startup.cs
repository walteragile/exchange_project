using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VM.Core.Contracts;
using VM.Core.Entities;
using VM.Infrastructure.CurrencyManagers;
using VM.Infrastructure.DataAccess;
using VM.Infrastructure.ExternalServices;

namespace VM.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ExchangeDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IExternalService, ExternalService>();
            services.AddScoped<ICurrencyManagerFactory, CurrencyManagerFactory>();
            services.AddScoped<ICurrencyManager, DollarManager>();
            services.AddScoped<IRepository<ExchangePurchases>, GenericRepository<ExchangePurchases>>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}