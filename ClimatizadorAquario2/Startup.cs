using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClimatizadorAquario2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                  builder =>
                  {
                      builder.WithOrigins("http://example.com",
                                          "http://localhost:4200",
                                          "http://localhost:4300",
                                          "http://localhost:4201",
                                          "http://localhost:51160",
                                          "http://localhost:52536",
                                          "http://srvhomolog01:8080",
                                          "http://www.homolog.ccb.policiamilitar.sp.gov.br:8080",
                                          "http://10.52.90.33:8080/",
                                          "http://10.52.90.33/",
                                          "http://10.52.89.23/",
                                          "http://intranet.ccb.policiamilitar.sp.gov.br:8080",
                                          "http://srvwdsv03",
                                          "http://srvwhml03",
                                          "*"
                                          )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains(); ;
                  });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}

            app.Use(async (context, next) =>
            {
                context.Request.EnableRewind();
                await next();
            });

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

            app.UseMvc();
        }
    }
}
