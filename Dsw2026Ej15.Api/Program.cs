using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Data;
namespace Dsw2026Ej15;
using Dsw2026Ej15.Api.Middlewares;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

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