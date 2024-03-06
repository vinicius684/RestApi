using Asp.Versioning.ApiExplorer;
using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Configuration;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddWebApiConfig();//
builder.Services.DependenciInjectionConfig();//
builder.AddSwaggerConfig();

// Add services to the container.
builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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


app.UseSwaggerConfig(apiVersionDescriptionProvider);
app.UseWebApiConfig(app.Environment);//

app.Run();
