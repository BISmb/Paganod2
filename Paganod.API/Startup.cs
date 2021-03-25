using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Routing.Template;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using Paganod.Data;
using Paganod.Data.Context;
using Paganod.Shared.Type;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Paganod.API
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
            services.AddPaganodDatabase();

            services.AddControllers(config =>
            {

            });

            services.AddMediatR(typeof(Paganod.Application.Mapping.RecordProfile).Assembly);

            services.AddOData();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Paganod.API", Version = "v2" });
            //    c.DocInclusionPredicate((name, api) => api.HttpMethod != null);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Paganod.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();


            var builder = new ODataConventionModelBuilder(app.ApplicationServices);

            builder.EntitySet<WeatherForecast>(nameof(PaganodContext.Forecasts)).EntityType.HasKey(entity => entity.Id);
            //builder.EntitySet<Record>("Transactions").EntityType.HasKey(entity => entity.Id);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Enable full OData queries, you might want to consider which would be actually enabled in production scenaries
                endpoints.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

                // Create the default collection of built-in conventions.
                var conventions = ODataRoutingConventions.CreateDefault();

                // Insert the custom convention at the start of the collection.
                //conventions.Insert(0, new NavigationIndexRoutingConvention());

                endpoints.MapODataRoute("ODataRoute", "odata", GetEdmModel(), new MyODataParser(), conventions);

                // Work-around for #1175
                endpoints.EnableDependencyInjection();

                //endpoints.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                //endpoints.MapODataRoute("odata", "odata", GetEdmModel());

            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            var entitySet = builder.EntitySet<Record>("Records");
            entitySet.EntityType.HasKey(entity => entity.Id);

            var entitySet2 = builder.EntitySet<Record>("Transactions");
            entitySet2.EntityType.HasKey(entity => entity.Id);

            return builder.GetEdmModel();
        }
    }

    public class MyODataParser : DefaultODataPathHandler
    {
        public override Microsoft.AspNet.OData.Routing.ODataPath Parse(string serviceRoot, string odataPath, IServiceProvider requestContainer)
        {
            string[] entityToReplace = new string[]
            {

            };

            if (odataPath.Contains("schemamodels")) odataPath = odataPath.Replace("schemamodels", "records");

            var response =  base.Parse(serviceRoot, odataPath, requestContainer);

            return response;
        }
    }
}
