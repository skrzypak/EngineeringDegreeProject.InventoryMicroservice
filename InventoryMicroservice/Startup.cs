using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication;
using GreenPipes;
using InventoryMicroservice.Comunication.Consumers;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Middlewares;
using InventoryMicroservice.Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace InventoryMicroservice
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
            #region Authentication
            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            services.AddScoped<IPFilterMiddleware>();
            services.AddScoped<IHeaderContextService, HeaderContextService>();
            services.AddHttpContextAccessor();
            #endregion 

            services.AddDbContext<MicroserviceContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder => {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            #region MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumer<InventoryConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    config.ReceiveEndpoint("msinve.inventory.queue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<InventoryConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            #endregion

            services.AddControllers();
            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddAutoMapper(this.GetType().Assembly);

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InventoryMicroservice", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer =ABF$Hjwt'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
            #endregion

            services.AddScoped<IAllergenService, AllergenService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMicroserviceService, MicroserviceService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryMicroservice v1"));
            }

            app.UseMiddleware<IPFilterMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
