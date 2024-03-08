using DevIO.Api.Extensions;
using Elmah.Io.Extensions.Logging;

namespace RestApi.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services)
        {
            services.AddElmahIo(o =>//passo configuração Elamah
            {
                o.ApiKey = "2dcfd17a78f14d3e9950ada7e4bb8bc5";
                o.LogId = new Guid("c2d7342c-4660-4212-8afc-1f6cf2551619");
            });

            //services.AddLogging(builder =>//passo para o Elmah pegar meus logging ou do asp.net, passar a ser um provider
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "2dcfd17a78f14d3e9950ada7e4bb8bc5";
            //        o.LogId = new Guid("c2d7342c-4660-4212-8afc-1f6cf2551619");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);//niveis de info warning pra cima
            //});

            //services.AddHealthChecks()
            //    .AddElmahIoPublisher(options =>
            //    {
            //        options.ApiKey = "388dd3a277cb44c4aa128b5c899a3106";
            //        options.LogId = new Guid("c468b2b8-b35d-4f1a-849d-f47b60eef096");
            //        options.HeartbeatId = "API Fornecedores";

            //    })
            //    .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            //    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            //services.AddHealthChecksUI()
            //    .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}