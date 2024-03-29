﻿using Asp.Versioning;
using DevIO.Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DevIO.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddWebApiConfig(this IServiceCollection services)
        {
           // services.AddControllers();

            services.
            AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;//passar no header do response falando se a api está obsoleta ou não

            })
            .AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });

            services.AddCors(options =>
            {
                //    options.AddPolicy("Development",
                //        builder => builder
                //            .AllowAnyOrigin()
                //            .AllowAnyMethod()
                //            .AllowAnyHeader()
                //            .AllowCredentials());

                options.AddDefaultPolicy(
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                //        .AllowCredentials());

                //options.AddPolicy("Production",
                //   builder =>
                //       builder
                //           .WithMethods("GET")//apenas get
                //           .WithOrigins("http://desenvolvedor.io")//apenas esse site
                //           .SetIsOriginAllowedToAllowWildcardSubdomains()//Permissão para subdomínios, se a app estiver no mesmo domínio da minha api tb permito que ela converse com a api
                //          //.WithHeaders(HeaderNames.ContentType, "x-custom-header") apenas esse tipo de Header pode fazer requisição, Edu não utiliza
                //           .AllowAnyHeader());
            });

            return services;
        }

        public static IApplicationBuilder UseWebApiConfig(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseCors("Development"); // Usar apenas nas demos => Configuração Ideal: Production
                app.UseCors("Production");
                app.UseHsts();//Header(chave) que pasa da app pro Client, fazendo entende-ló que  minha api só conversa HTTPS
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();//Se receber alguma chamada HTTP, redireciona para HTTPS e estabelecendo o HSTS           
          
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = "/api/hc-ui";
                    options.ResourcesPath = "/api/hc-ui-resources";

                    options.UseRelativeApiPath = false;
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeWebhookPath = false;
                });
            });

            return app;
        }
    }
}