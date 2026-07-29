using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Interfaces;
namespace Dsw2026Ej15;
using Dsw2026Ej15.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();
        builder.Services.AddDbContext<Dsw2026Ej15Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dsw2026Ej15Db")));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();
        app.MapHealthChecks("/health-check");
        app.Run();
    }
}