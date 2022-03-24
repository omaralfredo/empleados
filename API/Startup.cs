using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
/*  using Microsoft.EntityFrameworkCore;
// %% using Persistence;
// %% using MediatR;
// %% using Application.Activities;
// %% using AutoMapper;
// %% using Application.Core;
// %% using API.Extensions;
using FluentValidation.AspNetCore;
using API.Middleware; 
//
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
//using Application.Interfaces;
using Application.EmailService;
using Application.Settings;*/ 

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            })
                .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Create>();
            });
            services.AddApplicationServices(_config);

            services.AddIdentityServices(_config);

        services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromMinutes(15));
       
        services.AddTransient<IMailService, MailService>();

        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

/*
        services.AddAuthorization(options =>
        options.AddPolicy("Financiera.ReporteMovimientos",
        policy => { policy.RequireClaim(ClaimTypes.AuthorizationDecision,"Reporte movimientos");}));

        services.AddAuthorization(options =>
        options.AddPolicy("Financiera.Operaciones",
        policy => { policy.RequireClaim(ClaimTypes.AuthorizationDecision,"Operaciones");}));

        services.AddAuthorization(options =>
        options.AddPolicy("Financiera.ReporteSaldos",
        policy => { policy.RequireClaim(ClaimTypes.AuthorizationDecision,"Reporte saldos");}));

        services.AddAuthorization(options =>
        options.AddPolicy("Financiera.Cuotas",
        policy => { policy.RequireClaim(ClaimTypes.AuthorizationDecision,"Cuotas");}));

        services.AddAuthorization(options =>
        options.AddPolicy("Financiera.CargoPagos",
        policy => { policy.RequireClaim(ClaimTypes.AuthorizationDecision,"Carga de pagos");}));

*/
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            /*
            app.UseStaticFiles(new  StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "staticfiles")),
                RequestPath = "/staticfiles"
            });
            */
            

            //app.UseHttpsRedirection();

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
