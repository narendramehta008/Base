using CoreLibrary.API.Infrastructure.Persistence;
using CoreLibrary.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class DataContext : DataContextBase<DataContext>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<UserDetails> UserDetails { get; set; }
}
