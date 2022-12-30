using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
            // injeção de dependencia para o projeto...

            services.AddDbContext<SmartContext>(
                context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            );

            services.AddControllers()
                    .AddNewtonsoftJson(
                        opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // IGNORA LOOP INFINITO E PEGA SOMENTE O PRIMEIRO NIVEL.. add pelo csproj (o erro causado no browser era: System.Text.Json.JsonException: A possible object cycle was detected which is not supported)

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // services.AddSingleton<IRepository, Repository>();  Todas as instancias/chamadas serao utilizadas com o mesmo recurso (é compartilhado a mesma informaçoes com todas as requisiçoes)
            //services.AddTransient<IRepository, Repository>(); // A cada requisição será criada uma nova instancia (nunca usa a mesma instancia nas requisiçoes)
            services.AddScoped<IRepository, Repository>(); // Se em mais de uma requisição utilizar a mesma dependencia sera utilizada a mesma instancia para todas as que necessitam utiliza-la

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var apiProviderDescription = services.BuildServiceProvider()
                                                 .GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options => 
            {
                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                                    //"smartschoolapi", 
                                    description.GroupName,
                                    new Microsoft.OpenApi.Models.OpenApiInfo(){
                                        Title = "SmartSchool API",
                                        Version = description.ApiVersion.ToString(),
                                        //TermsOfService = new Uri("http://Termosdeuso.com")
                                        Description = "Descrição da WebAPI do SmartSchool",
                                        License = new Microsoft.OpenApi.Models.OpenApiLicense{
                                            Name = "SmartSchool Licence",
                                            Url = new Uri("http://mit.com")
                                        },
                                        Contact = new Microsoft.OpenApi.Models.OpenApiContact{
                                            Name = "Weslen Mariano",
                                            Email = "",
                                            Url = new Uri("http://weslenmariano.com")

                                        }
                                    }
                                    );
                
                }
                
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory,xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProviderDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger()
               .UseSwaggerUI(options => {

                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                            description.GroupName.ToUpperInvariant());    
                }
                
                options.RoutePrefix = "";
               });

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
