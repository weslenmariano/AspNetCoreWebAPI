using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.WebAPI.Data;

/*
Caso nao esteja habilitado o autocomplite
1 - Ctrl + Shift + p
2 - Write "OmniSharp: Select Project" and press Enter.
3 - Choose the solution workspace entry.

*/
namespace SmartSchool.WebAPI
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
            services.AddDbContext<SmartContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            // services.AddSingleton<IRepository, Repository>();  Todas as instancias/chamadas serao utilizadas com o mesmo recurso (é compartilhado a mesma informaçoes com todas as requisiçoes)
            //services.AddTransient<IRepository, Repository>(); // A cada requisição será criada uma nova instancia (nunca usa a mesma instancia nas requisiçoes)
            services.AddScoped<IRepository, Repository>(); // Se em mais de uma requisição utilizar a mesma dependencia sera utilizada a mesma instancia para todas as que necessitam utiliza-la
            

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
