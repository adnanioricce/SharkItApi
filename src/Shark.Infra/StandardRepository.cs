using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
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
    public static UpdateAsync<TEntity> Update<TEntity>(ApplicationDbContext ctx)
        where TEntity : class
    {
        return async (entity) => {
            var set = ctx.Set<TEntity>();
            set.Add(entity);
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
}