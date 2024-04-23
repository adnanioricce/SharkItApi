namespace Shark.Domain.Interfaces;
public record struct Pagination(int PageNumber, int PageSize);

public interface IRepository<TEntity> : IDisposable
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ListAsync(Pagination pagination);
}