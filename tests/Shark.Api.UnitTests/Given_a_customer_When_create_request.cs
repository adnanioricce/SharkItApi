using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;
using NSubstitute;
using Shark.Domain.Base;
using MediatR;
using Shark.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
namespace Shark.Api.UnitTests;

public class Given_a_customer_When_create_request
{    
    [Fact]
    public async Task Throws_exception_a_500_error_code_should_be_returned()
    {
        // Arrange
        var customer = new CustomerDto {};
        var command = new InsertCustomerCommand(customer);
        var mockHandler = Substitute.For<ICommandHandler<InsertCustomerCommand>>();        
        mockHandler.HandleAsync(command).Returns<Task>(Task.CompletedTask);
        var mediator = Substitute.For<IMediator>();
        mediator.Send(command,default).ThrowsAsyncForAnyArgs<Microsoft.EntityFrameworkCore.DbUpdateException>();
        var mockLogger = Substitute.For<ILogger<CustomerController>>();
        var controller = new CustomerController(mediator,mockLogger);
        
        // Act
        var result = await controller.CreateCustomer(customer);

        // Assert
        Assert.IsNotType<OkResult>(result);        
    }
    [Fact]
    public async Task Has_no_error_Then_a_default_200_error_code_will_be_Returned()
    {
        // Arrange
        var customer = new CustomerDto {};
        var command = new InsertCustomerCommand(customer);
        var mockHandler = Substitute.For<ICommandHandler<InsertCustomerCommand>>();        
        mockHandler.HandleAsync(command).Returns<Task>(Task.CompletedTask);
        var mediator = Substitute.For<IMediator>();
        mediator.Send(command,default).Returns<Task>(Task.CompletedTask);
        var mockLogger = Substitute.For<ILogger<CustomerController>>();
        var controller = new CustomerController(mediator,mockLogger);
        
        // Act
        var result = await controller.CreateCustomer(customer);

        // Assert
        Assert.IsType<OkResult>(result);
    }    
}
