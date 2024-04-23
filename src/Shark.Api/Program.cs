using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shark.Infra.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
async Task<IResult> Handle<T>(T req,Func<T,Task<IResult>> value,ILogger<Program> logger)
{
    try
    {
        return await value(req);
    }
    catch (ArgumentException argEx)
    {
        logger.LogError("some arguments of the request are invalid. request = {req} exception: {ex}", JsonSerializer.Serialize(req), argEx);
        return Results.BadRequest(new
        {
            argEx.Message
        });
    }
    catch (ValidationException valEx)
    {
        logger.LogError("Validation failed to request {req}: {ex}", JsonSerializer.Serialize(req), valEx);
        return Results.BadRequest(new
        {
            valEx.Message
        });
    }
    catch (Exception ex)
    {
        logger.LogError("An exception was throwed when trying to update customer: {ex}", ex);
        return Results.StatusCode(500);
    }
}
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>((sp,options) => {
    var connStr = builder.Configuration.GetConnectionString("Default");
    options.UseNpgsql(connStr);    
    options.EnableDetailedErrors();
    if(builder.Environment.IsDevelopment()){
        options.EnableSensitiveDataLogging();
    }
});
builder.Services.AddScoped<IRepository<CustomerEntity>, CustomerRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
var app = builder.Build();
CheckConnection(app);

void CheckConnection(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    using var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
    var count = ctx.Set<CustomerEntity>().Count();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPost("customers",async ([FromBody]InsertCustomerCommand cmd,[FromServices]IMediator mediator, [FromServices] ILogger<Program> logger) => {
    return await Handle(cmd, async (req) =>
    {
        await mediator.Send(req);
        return Results.Ok();
    },logger);
})
.WithName("CreateCustomer");
app.MapPut("customers",async (
    [FromBody]UpdateCustomerCommand cmd,
    [FromServices]IMediator mediator, 
    [FromServices]ILogger<Program> logger) => {
        return await Handle(cmd,async (req) =>
        {
            await mediator.Send(req);
            return Results.Ok();
        },logger);
})
.WithName("UpdateCustomer");



app.MapDelete("customers", async ([FromBody] DeleteCustomerCommand cmd, [FromServices] IMediator mediator, [FromServices] ILogger<Program> logger) =>
{
    return await Handle(cmd, async req =>
    {
        await mediator.Send(req);
        return Results.Ok();
    }, logger);
})
.WithName("DeleteCustomer");
app.MapGet("customers/{id:Guid}",async (Guid id,[FromServices]IMediator mediator) => {
    var query = new GetCustomerByIdQuery(id);
    CustomerDto response = await mediator.Send(query);
    return Results.Ok(response);
})
.WithName("GetCustomerById");
app.MapGet("customers",async (int pageNumber,int pageSize,[FromServices]IMediator mediator) => {
    var query = new GetCustomersQuery(pageNumber,pageSize);
    IEnumerable<CustomerDto> response = await mediator.Send(query);
    return Results.Ok(response);
})
.WithName("GetCustomers");
app.Run();
