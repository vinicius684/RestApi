using DevIO.Api.Extensions;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace RestApi.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>//passo configuração Elamah
            {
                o.ApiKey = "2dcfd17a78f14d3e9950ada7e4bb8bc5";
                o.LogId = new Guid("c2d7342c-4660-4212-8afc-1f6cf2551619");
            });

            //services.AddLogging(builder =>//passo para o Elmah pegar meus loggings do asp.net, passar a ser um provider
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "2dcfd17a78f14d3e9950ada7e4bb8bc5";
            //        o.LogId = new Guid("c2d7342c-4660-4212-8afc-1f6cf2551619");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);//niveis de info warning pra cima
            //});

            services.AddHealthChecks()
                .AddElmahIoPublisher(options =>
                {
                    options.ApiKey = "2dcfd17a78f14d3e9950ada7e4bb8bc5";
                    options.LogId = new Guid("c2d7342c-4660-4212-8afc-1f6cf2551619");
                    options.HeartbeatId = "0ade3d165dbe45d0b8e5a1d461c0f7cf";
                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");//Heathcheck para o Banco

            services.AddHealthChecksUI()
                .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            //app.UseHealthChecks("/api/hc", new HealthCheckOptions
            //{
            //    Predicate = p => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

            //app.UseHealthChecksUI(options => {
            //    options.UIPath = "/api/hc-ui";
            //    options.ResourcesPath = $"{options.UIPath}/resources";
            //    options.UseRelativeApiPath = false;
            //    options.UseRelativeResourcesPath = false;
            //    options.UseRelativeWebhookPath = false;
            //});

            return app;
        }
    }
}