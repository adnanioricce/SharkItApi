using Shark.Application.CustomerManagement;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Infra.DAL;
using Shark.Domain.Interfaces;
public class InsertCustomerCommandTests
{
    [Fact]
    public async Task Given_a_customer_input_When_handler_receives_invalid_data_Then_persistance_shouldnt_happen()
    {
        // Given
        var customer = new CustomerDto(){

        };
        var command = new InsertCustomerCommand(customer);            
        bool persistanceCalled = false;
        InsertAsync<CustomerEntity> mockInsert = (CustomerEntity entity) => {
            persistanceCalled = true;
            return Task.FromResult(-1);
        };
        var handler = new InsertCustomerCommandHandler(mockInsert);
        // When
        await handler.HandleAsync(command);
        // Then
        Assert.False(persistanceCalled);
    }
    
}