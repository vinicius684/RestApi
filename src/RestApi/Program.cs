using Asp.Versioning.ApiExplorer;
using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Configuration;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddWebApiConfig();//
builder.Services.DependenciInjectionConfig();//
builder.AddSwaggerConfig();
builder.Services.AddLoggingConfig(builder.Configuration);

// Add services to the container.

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString; });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;//Suprimindo a forma da validação da ViewModel Automática
});



var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.

app.UseWebApiConfig(app.Environment);//
app.UseSwaggerConfig(apiVersionDescriptionProvider);
app.UseLoggingConfiguration();


app.Run();
