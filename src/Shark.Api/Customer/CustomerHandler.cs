using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

namespace Shark.Domain.CustomerManagement;

public record struct InsertCustomerCommand(CustomerDto Customer) : IRequest;
public record struct UpdateCustomerCommand(CustomerDto Customer) : IRequest;
public record struct DeleteCustomerCommand(Guid CustomerId) : IRequest;
public record struct GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto>;
public record struct GetCustomersQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<CustomerDto>>;
public record CustomerHandler(IRepository<CustomerEntity> repo)
    : IRequestHandler<InsertCustomerCommand>
    , IRequestHandler<UpdateCustomerCommand>
    , IRequestHandler<DeleteCustomerCommand>
    , IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    , IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task Handle(InsertCustomerCommand request, CancellationToken cancellationToken)
    {
        var r = Customer.Create(request.Customer)
            .Map(CustomerEntity.From)
            .Map(async entity =>
            {
                await repo.AddAsync(entity);
            });
        await r.Value;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.From(request.Customer);
        // Não é exatamente assim que as mesmas são definidas no DDD, mas decidi separar pra facilitar o trabalho de mapear os dados.
        // O efeito colateral são esses métodos de mapeamento.
        // Julguei que usar algo como o AutoMapper não era necessário.
        var entity = CustomerEntity.From(customer);
        await repo.UpdateAsync(entity);
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(request.CustomerId);
        return CustomerDto.From(entity);
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var entities = await repo.ListAsync(new Pagination(request.PageNumber, request.PageSize));
        return entities.Select(CustomerDto.From);
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(request.CustomerId);
    }
}
