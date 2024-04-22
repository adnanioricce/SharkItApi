using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

public class GetCustomerByIdTests
{    
    [Fact]
    public async Task Given_a_customer_id_When_it_doesnt_exist_Then_return_a_no_content_result()
    {
        // Given
        var command = new GetCustomerByIdQuery(Guid.NewGuid());                    
        GetByIdAsync<CustomerEntity> GetByIdAsync = (Guid id) => Task.FromResult<CustomerEntity>(null);
        var handler = new GetCustomerByIdQueryHandler(GetByIdAsync);
        // When
        var response = await handler.HandleAsync(command);
        // Then
        Assert.Null(response);
    }
    [Fact]
    public async Task Given_a_customer_id_When_it_exists_Then_return_a_ok_result()
    {
        // Given
        
        var id = Guid.NewGuid();
        var command = new GetCustomerByIdQuery(id);  
        var expectedCustomerDto = new CustomerDto(){
            CustomerId = id
            ,FirstName = "Test"
            ,MiddleName = "User"
            ,DateOfBirth = DateTime.Now.AddDays(18)
            ,CPF = "12345678912"
            ,Addresses = Enumerable.Empty<AddressDto>().ToList()
        };
        GetByIdAsync<CustomerEntity> GetByIdAsync = (Guid id) => Task.FromResult(new CustomerEntity{
            CustomerId = expectedCustomerDto.CustomerId
            ,FirstName = expectedCustomerDto.FirstName
            ,MiddleName = expectedCustomerDto.MiddleName
            ,DateOfBirth = expectedCustomerDto.DateOfBirth.Date
            ,CPF = expectedCustomerDto.CPF
            ,CustomerAddresses = Enumerable.Empty<CustomerAddress>().ToList()
        });
        var handler = new GetCustomerByIdQueryHandler(GetByIdAsync);
        // When
        var response = await handler.HandleAsync(command);
        // Then
        Assert.NotNull(response);
        Assert.Equal(command.CustomerId,response.CustomerId);
        Assert.Equal(expectedCustomerDto.FirstName,response.FirstName);
        // Assert.Equal(expectedCustomerDto,response);
    }
}