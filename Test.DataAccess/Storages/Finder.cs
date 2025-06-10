using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Storages;

internal sealed class Finder<T> : IFinder<T>
    where T : class
{
    private readonly TestDbContext _context;

    public Finder(TestDbContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<T> GetAsync(bool asNoTracking = false)
    {
        return Find(asNoTracking).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> condition, bool asNoTracking = false)
    {
        return Find(asNoTracking).Where(condition).AsAsyncEnumerable();
    }

    public Task<T> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return Find(id, cancellationToken);
    }

    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        return Find().FirstOrDefaultAsync(condition, cancellationToken);
    }

    private IQueryable<T> Find(bool asNoTracking = false)
    {
        var set = _context.Set<T>();
        if (asNoTracking)
        {
            return set.AsNoTracking();
        }

        return set;
    }

    private Task<T> Find(Guid id, CancellationToken cancellationToken)
    {
        return _context.FindAsync<T>(id, cancellationToken).AsTask();
    }
}
