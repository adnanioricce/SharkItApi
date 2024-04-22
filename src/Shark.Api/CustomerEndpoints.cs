using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Shark.Infra.DAL;

namespace Shark.API.CustomerManagement;
public record CustomerHandlers(ApplicationDbContext ctx)
    : IRequestHandler<InsertCustomerCommand>
    ,IRequestHandler<UpdateCustomerCommand>
    ,IRequestHandler<GetCustomerByIdQuery,CustomerDto>
    ,IRequestHandler<GetCustomersQuery,IEnumerable<CustomerDto>>
{    
    public async Task Handle(InsertCustomerCommand request, CancellationToken cancellationToken)
    {
        var handler = new InsertCustomerCommandHandler(StandardRepository.Insert<CustomerEntity>(ctx));
        await handler.HandleAsync(request);
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var handler = new UpdateCustomerCommandHandler(StandardRepository.Update<Guid,CustomerEntity>(ctx));
        await handler.HandleAsync(request);
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var handler = new GetCustomerByIdQueryHandler(StandardRepository.GetByIdAsync<CustomerEntity>(ctx));
        return await handler.HandleAsync(request);
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var handler = new GetCustomersQueryHandler(StandardRepository.GetAsync<CustomerEntity>(ctx));
        return await handler.HandleAsync(request);
    }
}

public static class CustomerEndpoints
{
    public static void RegisterCustomersResource(this WebApplication app){
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
    }
}