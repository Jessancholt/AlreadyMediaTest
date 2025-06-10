using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Test.Core.Configurations;
using Test.DataAccess.Configurations;
using Test.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServer();

var app = builder.Build();

app.SwaggerConfig();

app.UseHttpsRedirection();

app.MapControllers();

app.ConfigureExceptionHandler();

app.UseCors();

app.Run();
