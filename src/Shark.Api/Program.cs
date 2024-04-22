using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shark.API.CustomerManagement;
using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;

var app = Program.Create(args);
app.RegisterCustomersResource();
app.Run();
public partial class Program {    
    public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder){
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
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
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }        
        app.UseHttpsRedirection();
        
        return app;
    }
};