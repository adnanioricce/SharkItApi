using MediatR;
using Shark.Domain.Base;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Shark.Infra.DAL;

namespace Shark.Application.CustomerManagement;

public record InsertCustomerCommand(CustomerDto Customer) : IRequest;
public record InsertCustomerCommandHandler(InsertAsync<CustomerEntity> InsertDelegate) : ICommandHandler<InsertCustomerCommand>
{    
    public async Task HandleAsync(InsertCustomerCommand command)
    {
        Result<Customer> mapResult = Customer.From(command.Customer);
        if(!mapResult.IsSuccess())
            return;
        CustomerEntity entity = CustomerEntity.From(mapResult.Value);
        int result = await InsertDelegate(entity);
    }
}