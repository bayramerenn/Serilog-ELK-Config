using Logging;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Log.Logger = new LoggerConfiguration()
//    .Enrich.FromLogContext()
//    .Enrich.WithExceptionDetails()
//    .WriteTo.Debug()
//    .WriteTo.Console()
//    .WriteTo.Elasticsearch(
//        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://localhost:9200/"))
//        {
//            AutoRegisterTemplate= true,
//            IndexFormat = "Serilog-dev-022022"
//        }
//    )
//    .CreateLogger();

StartupConfig.ConfigureLoggin();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
