using ElectricityApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using ElectricityApp.Models;
using Microsoft.Extensions.DependencyInjection;
using ElectricityApp.Services.Contracts;
using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(app => app.AddJsonFile("appsettings.json"));

//Log4net 
XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Host.ConfigureServices((host, services) =>
{
    var config = host.Configuration;

    services.AddDbContext<WebDatabaseContext>();
    
    //Background service DI
    services.AddHostedService<CSVReaderService>();
    services.AddSingleton<CSVReaderService>();

    services.AddScoped<IDataAgregationService, DataAgregationService>();
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");
    c.RoutePrefix = "WebServices";
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();


