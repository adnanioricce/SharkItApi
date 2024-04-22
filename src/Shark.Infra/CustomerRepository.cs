using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;

public sealed class CustomerRepository : IRepository<CustomerEntity>
{
    private readonly ApplicationDbContext _ctx;
    private readonly DbSet<CustomerEntity> _set;    
    public async Task<IEnumerable<CustomerEntity>> Get(Pagination query)
    {
        return await _set
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
    }    

    public async Task<CustomerEntity> GetById(Guid id)
    {
        return await _set.FindAsync(id);
    }
    public CustomerRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
        _set = _ctx.Set<CustomerEntity>();
    }    
    public void Add(CustomerEntity entity)
    {
        _set.Add(entity);
    }
    
    public void Update(CustomerEntity entity)
    {                                
        _set.Attach(entity);
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