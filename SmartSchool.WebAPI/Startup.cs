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

namespace SmartSchool.WebAPI
{
    public class Startup
    {
        // A configuração que é injetada é a possibilidade de acessar 
        // o appsettings.json
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Contexto que vai gerenciar a conexão com o banco de dados
            // O SmartContext está recebendo em seu construtor um a connection string
            services.AddDbContext<SmartContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
            );

            // O auto mapper busca dentro dos assemblies, ou seja, dentro das dll,
            // quem está herdando de profile 
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            //services.AddTransient<IRepository, Repository>();
            // Cria uma nova instância para cada requisição
            services.AddScoped<IRepository, Repository>();

            // Versionamento
            services.AddVersionedApiExplorer(options => 
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options => 
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            var apiProviderDescription = services.BuildServiceProvider()
                                                 .GetService<IApiVersionDescriptionProvider>();

            // Adicionar o swagger
            services.AddSwaggerGen(options => 
            {
                foreach(var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    // Documentação open Id
                    options.SwaggerDoc(
                        description.GroupName, 
                        new Microsoft.OpenApi.Models.OpenApiInfo(){
                            Title = "SmartSchool API",
                            Version = description.ApiVersion.ToString(),
                            TermsOfService = new Uri("http://SeusTermosDeUso.com"), // Termos
                            Description = "A descrição da WebAPI do SmartSchool", // Descrição
                            License = new Microsoft.OpenApi.Models.OpenApiLicense
                            {
                                Name = "SmartSchool License",
                                Url = new Uri("http://smart.com")
                            },
                            Contact = new Microsoft.OpenApi.Models.OpenApiContact
                            {
                                Name = "Lindomar",
                                Email = "lindomardias21@gmail.com"
                            }
                        });
                }
                
                // Configuração do XML
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFileFullPath);
            });

            services.AddControllers()
                    .AddNewtonsoftJson(
                        opt => opt.SerializerSettings.ReferenceLoopHandling = 
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                                IApplicationBuilder app, 
                                IWebHostEnvironment env,
                                IApiVersionDescriptionProvider apiDescription
                                )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Swagger
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach(var description in apiDescription.ApiVersionDescriptions)
                    {
                        // Sempre que eu estiver na raiz redirecionar para esta url
                        options.SwaggerEndpoint($"swagger/{description.GroupName}/swagger.json", 
                                                description.GroupName.ToUpperInvariant());
                    }
                    options.RoutePrefix = ""; // Sempre que o prefix for vazio redireciona para a raiz
                });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
