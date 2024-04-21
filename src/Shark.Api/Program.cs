using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;

var app = Program.Create(args);

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public partial class Program {    
    public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder){
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Customer).Assembly));
        return builder;
    }

    public static WebApplication Create(string[] args){
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        ConfigureServices(builder);
        builder.Services.AddDbContext<ApplicationDbContext>((sp,options) => {
            var connStr = builder.Configuration.GetConnectionString("Default");
            options.UseNpgsql(connStr);
            options.EnableDetailedErrors();
            if(builder.Environment.IsDevelopment()){
                options.EnableSensitiveDataLogging();
            }
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }        
        app.UseHttpsRedirection();
        app.MapPost("customer/create",async (CustomerDto dto,[FromServices]IMediator mediator) => {    
            var command = new InsertCustomerCommand(dto);
            await mediator.Send(command);
        });
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
        {
            var forecast =  Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();
        return app;
    }
};