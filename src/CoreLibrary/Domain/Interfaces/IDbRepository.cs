using CoreLibrary.API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Domain.Interfaces;

public interface IDbRepository : IDbRepository<DbContext>
{
    int UpdateDetachSave<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class;

    int AddSave<TEntity>(TEntity entity) where TEntity : class;
}
