using Shark.Application.CustomerManagement;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Infra.DAL;
using Shark.Domain.Interfaces;
public class GetCustomerByIdQueryHandlerTests
{
    [Fact]
    public async Task Given_list_request_Should_return_only_especified_count()
    {
        // Given
        var entity = new CustomerEntity();
        var command = new GetCustomersQuery(1,100);                    
        GetAsync<CustomerEntity> getAsync = (query) => Task.FromResult(Enumerable.Repeat(entity,query.PageSize));
        var handler = new GetCustomersQueryHandler(getAsync);
        // When
        var response = await handler.HandleAsync(command);
        // Then
        Assert.NotEmpty(response);
        Assert.Equal(100,response.Count());
    }
    [Fact]
    public async Task Given_list_request_When_there_is_no_data_in_range_Then_a_empty_list_is_expected()
    {
        // Given
        var entity = new CustomerEntity();
        var command = new GetCustomersQuery(1,100);                    
        GetAsync<CustomerEntity> getAsync = (query) => Task.FromResult(Enumerable.Empty<CustomerEntity>());
        var handler = new GetCustomersQueryHandler(getAsync);
        // When
        var response = await handler.HandleAsync(command);
        // Then
        Assert.Empty(response);        
    }
}