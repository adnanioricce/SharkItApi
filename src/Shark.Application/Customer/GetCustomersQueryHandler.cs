using MediatR;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.Base;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

public record struct GetCustomersQuery(int PageNumber,int PageSize) : IRequest<IEnumerable<CustomerDto>>;
public record GetCustomersQueryHandler(GetAsync<CustomerEntity> GetAsync) : IQueryHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetCustomersQuery query)
    {        
        var entities = await GetAsync(new Pagination(query.PageNumber,query.PageSize));
        return entities
            .Select(CustomerDto.From);   
    }
}
