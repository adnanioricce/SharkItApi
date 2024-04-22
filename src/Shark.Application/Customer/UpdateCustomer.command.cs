
using MediatR;
using Shark.Domain.Base;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

public record struct UpdateCustomerCommand(CustomerDto Customer) : IRequest;

public readonly record struct UpdateCustomerCommandHandler(UpdateAsync<Guid,CustomerEntity> UpdateAsync) : ICommandHandler<UpdateCustomerCommand>
{
    public async Task HandleAsync(UpdateCustomerCommand command)
    {
        var result = Customer.From(command.Customer);
        if(!result.IsSuccess())
            return;
        var entity = CustomerEntity.From(result.Value);
        await UpdateAsync(entity.CustomerId,entity);
    }
}