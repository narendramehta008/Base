using CoreLibrary.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class DataContextBase<TRepo> : DbContext where TRepo : DbContext
{
    public DataContextBase(DbContextOptions<TRepo> options)
        : base(options)
    {
    }

    //public DbSet<User> Users { get; set; }
}

