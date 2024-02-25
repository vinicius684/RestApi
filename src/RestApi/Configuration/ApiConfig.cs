using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DevIO.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddWebApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());

                //options.AddDefaultPolicy(
                //    builder => builder
                //        .AllowAnyOrigin()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowCredentials());

                options.AddPolicy("Production",
                   builder =>
                       builder
                           .WithMethods("GET")//apenas get
                           .WithOrigins("http://desenvolvedor.io")//apenas esse site
                           .SetIsOriginAllowedToAllowWildcardSubdomains()//Permissão para subdomínios, se a app estiver no mesmo domínio da minha api tb permito que ela converse com a api
                           //.WithHeaders(HeaderNames.ContentType, "x-custom-header") apenas esse tipo de Header pode fazer requisição, Edu não utiliza
                           .AllowAnyHeader());
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

            app.UseHttpsRedirection();//Se receber alguma chamada HTTP, redireciona para HTTPS e estabelecendo o HSTS           
          
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.MapControllers();

            return app;
        }
    }
}