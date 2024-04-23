using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shark.Infra.Repositories;
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
app.MapPost("customers",async ([FromBody]InsertCustomerCommand cmd,[FromServices]IMediator mediator) => {
    await mediator.Send(cmd);
    return Results.Ok();
})
.WithName("CreateCustomer");
app.MapPut("customers",async ([FromBody]UpdateCustomerCommand cmd,[FromServices]IMediator mediator) => {
    await mediator.Send(cmd);
    return Results.Ok();
})
.WithName("UpdateCustomer");
app.MapDelete("customers", async ([FromBody] DeleteCustomerCommand cmd, [FromServices] IMediator mediator) =>
{
    await mediator.Send(cmd);
    return Results.Ok();
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
