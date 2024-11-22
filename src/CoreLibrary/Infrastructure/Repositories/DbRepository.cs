using CoreLibrary.API.Domain.Entities.Base;
using CoreLibrary.API.Domain.Services.Repositories;
using CoreLibrary.Application.Interfaces;
using CoreLibrary.Application.Models;
using CoreLibrary.Domain.Entities;
using CoreLibrary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Infrastructure.Repositories;

public class DbRepository : DbRepository<DataContext>, IDbRepository
{
    public DbRepository(DataContext dbContext) : base(dbContext)
    {
        //dbContext.Find(typeof(DataContext), 1);
    }

    public async Task<IEnumerable<T>> GetAll<T>(GetRequest<T>? request) where T : class
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (request != null)
        {
            if (request.Filter != null) query = query.Where(request.Filter);

            if (request.OrderBy != null) query = request.OrderBy(query);

            if (request.Skip.HasValue) query = query.Skip(request.Skip.Value);

            if (request.Take.HasValue) query = query.Take(request.Take.Value);
        }

        return await query.ToArrayAsync();
    }

}