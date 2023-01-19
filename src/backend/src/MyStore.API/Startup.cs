namespace MyStore.API
{
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using MyStore.API.Commons;
    using MyStore.Common.API;
    using MyStore.Common.API.Mvc;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System.Globalization;

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddHttpContextAccessor();

            var allowedHosts = Configuration.GetSection("AllowedHosts").Get<string[]>();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                    builder => builder
                                .AllowAnyHeader()
                                .WithMethods("POST", "PUT", "GET", "DELETE", "Options")
                                .WithOrigins(allowedHosts)));

            services.AddJwt(Configuration);

            services.AddControllersWithViews(options =>
            {
                options.ModelValidatorProviders.Clear();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-CA");

            if (Env.IsDevelopment())
            {
                services.AddSwaggerGen("MyStore API", "v1");
            }

            services.AddCacheManagerConfiguration(Configuration);
            services.AddCacheManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.MyStoreUseExceptionHandler();

            if (Env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Membership API V1");
                    c.DisplayRequestDuration();
                    c.DocExpansion(DocExpansion.None);
                    c.ShowExtensions();
                    c.EnableValidator();
                    c.EnableFilter();
                });
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
