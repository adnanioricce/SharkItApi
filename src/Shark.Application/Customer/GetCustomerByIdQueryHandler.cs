
using Shark.Domain.Base;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

public record struct GetCustomerByIdQuery(Guid CustomerId);
public record GetCustomerByIdQueryHandler(GetByIdAsync<CustomerEntity> GetByIdAsync) : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    public async Task<CustomerDto> HandleAsync(GetCustomerByIdQuery query)
    {
        var entity = await GetByIdAsync(query.CustomerId);        
        if(entity is null)
            return null;
        return CustomerDto.From(entity);
    }
}