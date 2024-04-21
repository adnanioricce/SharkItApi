using MediatR;
using Shark.Domain.Base;
using Shark.Domain.CustomerManagement;
using Shark.Infra.DAL;

namespace Shark.Application.CustomerManagement;
public record InsertCustomerEndpoint(ApplicationDbContext _context) 
    : IRequestHandler<InsertCustomerCommand>     
{
    public async Task Handle(InsertCustomerCommand request, CancellationToken cancellationToken)
    {
        var insertAsyncDelegate = StandardRepository.Insert<CustomerEntity>(_context);
        var cmdHandler = new InsertCustomerCommandHandler(insertAsyncDelegate);
        await cmdHandler.HandleAsync(request);
    }    
}
