using System.Linq.Expressions;

namespace CoreLibrary.Application.Models;

public class GetRequest<T> where T : class
{
    public Expression<Func<T, bool>>? Filter { get; set; } = null;
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; } = null;
    public IEnumerable<Expression<Func<T, object>>>? Includes { get; set; } //= [a => a.id, b => b.id];
    public int? Skip { get; set; } = null;
    public int? Take { get; set; } = null;
}
