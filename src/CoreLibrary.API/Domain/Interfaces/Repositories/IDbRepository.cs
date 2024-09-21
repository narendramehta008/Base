using System.Linq.Expressions;

namespace CoreLibrary.API.Domain.Interfaces.Repositories;

public interface IDbRepository<TDbContext> where TDbContext : class
{
    IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    Task<int> AddRangeSaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    Task<int> AddSaveAsync<TEntity>(TEntity entity) where TEntity : class;

    Task<int> DeleteSaveAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

    Task<int> DeleteSaveAsync<TEntity>(TEntity entity) where TEntity : class;

    Task<int> UpdateDetachSaveAsync<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class;
    void Detach<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;

    Task<int> UpdateRangeSaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

    Task<int> UpdateSaveAsync<TEntity>(TEntity entity) where TEntity : class;

    Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
}
