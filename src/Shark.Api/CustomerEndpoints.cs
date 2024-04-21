using MediatR;
using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

namespace Shark.API.CustomerManagement;
// public record struct CreateCustomerRequest(CustomerDto dto) : IRequest;
public static class CustomerEndpoints
{
    public static async Task<IResult> CreateCustomer(
        IMediator mediator
        ,InsertCustomerCommand cmd)
    {        
        await mediator.Send(cmd);
        return Results.Ok();
    }
    public static async Task<IResult> GetCustomerById(
        IMediator mediator
        ,GetCustomerByIdQuery cmd)
    {        
        var response = await mediator.Send(cmd);
        if(response is null)
            return Results.NoContent();
        return Results.Ok(response);
    }
}