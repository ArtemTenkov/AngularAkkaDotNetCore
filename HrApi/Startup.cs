using System;
using System.Net;
using Akka.Actor;
using Akka.DI.StructureMap;
using Akka.Event;
using App.Actors;
using App.Providers;
using App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using Info = Swashbuckle.AspNetCore.Swagger.Info;

namespace HrApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "HR API", Version = "v1"}); });
            services.AddSingleton(_ => ActorSystem.Create("ping-system"));
            return ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hrs API V1");
            });

            app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            var correlationId = context.TraceIdentifier;
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/html";
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                
                            }
                        });
                });
        }



        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(UserService));
                    _.WithDefaultConventions();
                });
                
                config.ForSingletonOf<UsersActorProvider>().Singleton();
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
