namespace Shark.Domain.Interfaces;
public interface IEntity<TId>
{
    TId Id {get;}
}
public record struct Pagination(int PageNumber, int PageSize);
public delegate Task<int> InsertAsync<TEntity>(TEntity entity);
public delegate Task<int> UpdateAsync<TId,TEntity>(TId id,TEntity entity);
public delegate Task<TEntity> GetByIdAsync<TId,TEntity>(TId id);
public delegate Task<TEntity> GetByIdAsync<TEntity>(Guid id);
public delegate Task<IEnumerable<TEntity>> GetAsync<TEntity>(Pagination query);
public delegate IQueryable<TEntity> Query<TEntity>();

public interface IRepository<TEntity> : IDisposable
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    Task<TEntity> GetById(Guid id);
    Task<IEnumerable<TEntity>> Get(Pagination pagination);
    // Task<IEnumerable<TEntity>> Get(Func<IQueryable,IQueryable> query);
    Task<int> SaveChangesAsync();
}