using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.Application.Models;
using CoreLibrary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Infrastructure.Repositories;

public class DbRepository : DbRepository<DataContext>, IDbRepository
{
    public DbRepository(DataContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<T>> GetAll<T>(GetRequest<T>? request) where T : class
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (request != null)
        {
            if (request.Filter != null) query = query.Where(request.Filter);
            if (request.Includes != null)
                query = request.Includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (request.OrderBy != null) query = request.OrderBy(query);

            if (request.Skip.HasValue) query = query.Skip(request.Skip.Value);

            if (request.Take.HasValue) query = query.Take(request.Take.Value);
        }

        return await query.ToArrayAsync();
    }

    public int NestedAddRangeSave<TEntity, TNEntity>(IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<TNEntity>> predicate)
        where TEntity : BaseEntity
        where TNEntity : BaseParentEntity
    {
        int status = 0;
        foreach (var entity in entities)
            status = NestedAddSave(entity, predicate);
        return status;
    }

    public int NestedAddSave<TEntity, TNEntity>(TEntity entity, Func<TEntity, IEnumerable<TNEntity>> predicate) where TEntity : BaseEntity where TNEntity : BaseParentEntity
    {
        var status = 0;
        if (entity.Id == 0)
            AddSave(entity);
        foreach (var item in predicate(entity))
        {
            item.ParentId = entity.Id;
            AddSave(item);
        }
        return status;
    }

    public int NestedInAddRangeSave<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<TEntity>> predicate) where TEntity : BaseParentEntity
    {
        int status = 0;
        foreach (var entity in entities)
            status = NestedInAddSave(entity, predicate);
        return status;
    }

    public int NestedInAddSave<TEntity>(TEntity entity, Func<TEntity, IEnumerable<TEntity>> predicate) where TEntity : BaseParentEntity
    {
        var status = 0;
        if (entity.Id == 0)
            AddSave(entity);
        foreach (var item in predicate(entity))
        {
            item.ParentId = entity.Id;
            NestedInAddSave(item, predicate);
        }
        return status;
    }
}