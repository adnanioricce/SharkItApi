using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
namespace Alternative.Shark.Api {
    public record struct InsertCustomerCommand(CustomerDto Customer) : IRequest;
    public record struct UpdateCustomerCommand(CustomerDto Customer) : IRequest;
    public record struct GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto>;
    public record struct GetCustomersQuery(int PageNumber,int PageSize) : IRequest<IEnumerable<CustomerDto>>;
    public record AlternativeCustomerHandler(ApplicationDbContext ctx)
        : IRequestHandler<InsertCustomerCommand>
        , IRequestHandler<UpdateCustomerCommand>
        , IRequestHandler<GetCustomerByIdQuery,CustomerDto>
        , IRequestHandler<GetCustomersQuery,IEnumerable<CustomerDto>>
    {
        public async Task Handle(InsertCustomerCommand request, CancellationToken cancellationToken)
        {           
            var result = Customer.From(request.Customer);
            if(!result.IsSuccess())
                return;
            var customer = result.Value;
            var entity = CustomerEntity.From(customer);
            var set = ctx.Set<CustomerEntity>();
            set.Add(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = Customer.From(request.Customer);
            if(!result.IsSuccess())
                return;
            var customer = result.Value;
            var entity = CustomerEntity.From(customer);
            var set = ctx.Set<CustomerEntity>();
            set.Update(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await ctx.Set<CustomerEntity>().FindAsync(request.CustomerId);
            return CustomerDto.From(entity);
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var entities = await ctx.Set<CustomerEntity>()
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            return entities.Select(CustomerDto.From);   
        }
    }
    
    public static class AlternativeCustomerEndpoints
    {
        public static void MapEndpoints(WebApplication app){
            app.MapPost("customers",async (InsertCustomerCommand cmd,[FromServices]IMediator mediator) => {
                await mediator.Send(cmd);
                return Results.Ok();
            });
            app.MapPut("customers",async (UpdateCustomerCommand cmd,[FromServices]IMediator mediator) => {
                await mediator.Send(cmd);
                return Results.Ok();
            });
            app.MapGet("customers/{id:Guid}",async (GetCustomerByIdQuery query,[FromServices]IMediator mediator) => {
                var response = await mediator.Send(query);
                return Results.Ok(response);
            });
            app.MapGet("customers",async (GetCustomersQuery query,[FromServices]IMediator mediator) => {
                var response = await mediator.Send(query);
                return Results.Ok(response);
            });
        }
    }
}