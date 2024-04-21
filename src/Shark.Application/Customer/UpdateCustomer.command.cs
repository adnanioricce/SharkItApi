
using Shark.Domain.Base;

public record struct UpdateCustomerCommand(CustomerDto Customer);

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public UpdateCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(UpdateCustomerCommand command)
    {
        throw new NotImplementedException();
        // _context.Customers.Update(command.Customer);
        await _context.SaveChangesAsync();
    }
}