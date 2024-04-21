namespace Shark.Domain.Interfaces;
public record struct Pagination(int PageNumber, int PageSize);
public delegate Task<int> InsertAsync<TEntity>(TEntity entity);
public delegate Task<int> UpdateAsync<TEntity>(TEntity entity);
public delegate Task<TEntity> GetByIdAsync<TId,TEntity>(TId id);
public delegate Task<TEntity> GetByIdAsync<TEntity>(Guid id);
public delegate Task<IEnumerable<TEntity>> GetAsync<TEntity>(Pagination query);

public class IRepository<T>
{
    //TODO:
}