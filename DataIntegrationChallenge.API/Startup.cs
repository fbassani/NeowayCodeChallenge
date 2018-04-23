using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataIntegrationChallenge.API.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataIntegrationChallenge.API {
    public class Startup {
        public IConfigurationRoot Configuration { get; }


        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services) {
            services.AddMvc().AddControllersAsServices();
            var builder = GetContainerBuilder(services);
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        private ContainerBuilder GetContainerBuilder(IServiceCollection services) {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new MongoDbModule(Configuration));

            return builder;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}