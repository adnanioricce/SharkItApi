using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Extensions;
using Shark.Domain.Interfaces;

namespace Shark.Infra.DAL;

public static class StandardRepository {
    public static InsertAsync<TEntity> Insert<TEntity>(ApplicationDbContext ctx)
        where TEntity : class
    {
        return async (entity) => {
            var set = ctx.Set<TEntity>();
            set.Add(entity);
            return await ctx.SaveChangesAsync();
        };
    }
    public static UpdateAsync<TId,TEntity> Update<TId,TEntity>(ApplicationDbContext ctx)
        where TEntity : class
    {
        return async (id,entity) => {
            var set = ctx.Set<TEntity>();            
            var existingEntity = await set.FindAsync(id);
            if(existingEntity == default){
                throw new InvalidOperationException("A request to update a entity that doesn't exists on the database occurred");
            }
            // set.(entity);
            entity.CopyDirtyPropsToDestination(existingEntity);
            return await ctx.SaveChangesAsync();
        };
    }
    public static GetByIdAsync<TEntity> GetByIdAsync<TEntity>(ApplicationDbContext ctx) where TEntity : class
        => async (id) => await ctx.Set<TEntity>().FindAsync(id);
    public static GetAsync<TEntity> GetAsync<TEntity>(ApplicationDbContext ctx) where TEntity : class
    {
        return async (query) => {
            return await ctx.Set<TEntity>()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        };
    }
    public static Query<TEntity> Query<TEntity>(ApplicationDbContext ctx) where TEntity : class
    {
        return () => ctx.Set<TEntity>().AsQueryable();
    }
}
    