using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Extensions;
using Shark.Domain.Interfaces;
namespace Shark.Infra.Repositories;
public sealed class CustomerRepository : IRepository<CustomerEntity>
{
    private readonly ApplicationDbContext _ctx;
    private readonly DbSet<CustomerEntity> _set;
    private readonly DbSet<CustomerAddressEntity> _customerAddressSet;

    public CustomerRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
        _set = _ctx.Set<CustomerEntity>();
        _customerAddressSet = _ctx.Set<CustomerAddressEntity>();
    }
    public async Task<IEnumerable<CustomerEntity>> ListAsync(Pagination query)
    {
        return await _set
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
    }    

    public async Task<CustomerEntity> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }
    public async Task AddAsync(CustomerEntity entity)
    {
        _set.Add(entity);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(CustomerEntity entity)
    {        
        _set.Attach(entity).State = EntityState.Modified;        
        foreach (var address in entity.CustomerAddresses)
        {
            var existingAddress = await _customerAddressSet.Where(addr => address.AddressId == addr.AddressId).FirstOrDefaultAsync();
            if (address.CopyDirtyPropsToDestination(existingAddress))
            {
                _customerAddressSet.Attach(address).State = EntityState.Modified;
            }
        }
        await _ctx.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var existingEntity = await _set.FindAsync(id);
        if (existingEntity is null)
            return;
        _set.Remove(existingEntity);
        await _ctx.SaveChangesAsync();
    }
    public Task<int> SaveChangesAsync()
    {
        return _ctx.SaveChangesAsync();
    }
    public void Dispose()
    {            
        _ctx.Dispose();
    }

    
}