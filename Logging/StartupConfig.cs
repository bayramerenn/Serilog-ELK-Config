using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Logging
{
    public static class StartupConfig
    {
        private const string APPSETTING_FILE_NAME = "appsettings";

        public static void ConfigureLoggin()
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"{APPSETTING_FILE_NAME}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{APPSETTING_FILE_NAME}.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string? environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
