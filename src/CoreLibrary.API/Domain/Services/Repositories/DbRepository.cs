using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreLibrary.API.Domain.Services.Repositories;

[ExcludeFromCodeCoverage]
public class DbRepository<TDbContext> : IDbRepository<TDbContext> where TDbContext : DbContext
{
    protected readonly TDbContext _dbContext;
    public DbRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //public static DbSet<T> GetDbSet<T>(this DbContext _context) where T : class => 
    //    (DbSet<T>)_context.GetType().GetMethod("Set", types: Type.EmptyTypes).MakeGenericMethod(typeof(T)).Invoke(_context, null);

    private IQueryable<object> Set(Type t)
    {
        var entityType = _dbContext.Model.FindEntityType(t);
        var data = _dbContext.GetType().GetMethods().First(a => a.Name.Contains("get_") && a.ReturnTypeCustomAttributes.ToString().Trim(']').Split(".").Last() == t.Name)
            ?.Invoke(_dbContext, null) ?? new List<object>().AsQueryable();
        return (IQueryable<object>)data;
    }

    public object? Find(Type type, params object?[]? keyValues)
    {
        if (keyValues == null || keyValues.Length == 0) return Set(type);
        return _dbContext.Find(type, keyValues);
    }
    public int AddRangeSave<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().AddRange(entities);
        return _dbContext.SaveChanges();
    }

    public async Task<int> AddRangeSaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        return await _dbContext.SaveChangesAsync();
    }

    public int AddSave<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Add(entity);
        return _dbContext.SaveChanges();
    }

    public async Task<int> AddSaveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Add(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public int DeleteSave<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return _dbContext.SaveChanges();
    }

    public int DeleteSave<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        IQueryable<TEntity> entities = _dbContext.Set<TEntity>().Where(expression);
        _dbContext.Set<TEntity>().RemoveRange(entities);
        return _dbContext.SaveChanges();
    }

    public async Task<int> DeleteSaveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Remove(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteSaveAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        IQueryable<TEntity> entities = _dbContext.Set<TEntity>().Where(expression);
        _dbContext.Set<TEntity>().RemoveRange(entities);
        return await _dbContext.SaveChangesAsync();
    }

    public void Detach<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
    {
        TEntity val = _dbContext.Set<TEntity>().Local.FirstOrDefault(predicate);
        if (val != null)
        {
            _dbContext.Entry(val).State = EntityState.Detached;
        }
    }

    public TEntity? FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        return EntityFrameworkQueryableExtensions.AsNoTracking(_dbContext.Set<TEntity>()).FirstOrDefault(expression);
    }

    public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(EntityFrameworkQueryableExtensions.AsNoTracking(_dbContext.Set<TEntity>()), expression);
    }

    public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        return EntityFrameworkQueryableExtensions.AsNoTracking(_dbContext.Set<TEntity>()).Where(expression);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
    {
        return await EntityFrameworkQueryableExtensions.ToListAsync(EntityFrameworkQueryableExtensions.AsNoTracking(_dbContext.Set<TEntity>()));
    }

    public int UpdateDetachSave<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class
    {
        UpdateDetach(entity, predicate);
        return _dbContext.SaveChanges();
    }

    public async Task<int> UpdateDetachSaveAsync<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class
    {
        UpdateDetach(entity, predicate);
        return await _dbContext.SaveChangesAsync();
    }

    public int UpdateRangeSave<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        return _dbContext.SaveChanges();
    }

    public async Task<int> UpdateRangeSaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        return await _dbContext.SaveChangesAsync();
    }

    public int UpdateSave<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Update(entity).State = EntityState.Modified;
        return _dbContext.SaveChanges();
    }

    public async Task<int> UpdateSaveAsync<TEntity>(TEntity entity) where TEntity : class
    {
        _dbContext.Set<TEntity>().Update(entity).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
    }

    public IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
    {
        return EntityFrameworkQueryableExtensions.AsNoTracking(_dbContext.Set<TEntity>()).Where(expression);
    }

    private void UpdateDetach<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class
    {
        DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
        TEntity val = dbSet.Local.FirstOrDefault(predicate);
        if (val != null)
        {
            _dbContext.Entry(val).State = EntityState.Detached;
        }

        dbSet.Update(entity).State = EntityState.Modified;
    }
}