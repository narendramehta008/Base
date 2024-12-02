using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.API.Domain.Interfaces.Repositories;
using CoreLibrary.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Application.Interfaces;

public interface IDbRepository : IDbRepository<DbContext>
{
    int UpdateDetachSave<TEntity>(TEntity entity, Func<TEntity, bool> predicate) where TEntity : class;
    int AddSave<TEntity>(TEntity entity) where TEntity : class;
    Task<IEnumerable<TEntity>> GetAll<TEntity>(GetRequest<TEntity>? request) where TEntity : class;
    int NestedAddSave<TEntity, TNEntity>(TEntity entity, Func<TEntity, IEnumerable<TNEntity>> predicate) where TEntity : BaseEntity where TNEntity : BaseParentEntity;
    int NestedAddRangeSave<TEntity, TNEntity>(IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<TNEntity>> predicate) where TEntity : BaseEntity where TNEntity : BaseParentEntity;
    int NestedInAddSave<TEntity>(TEntity entity, Func<TEntity, IEnumerable<TEntity>> predicate) where TEntity : BaseParentEntity;
    int NestedInAddRangeSave<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, IEnumerable<TEntity>> predicate) where TEntity : BaseParentEntity;

}
