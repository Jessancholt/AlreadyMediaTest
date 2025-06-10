using System.Linq.Expressions;

namespace Test.DataAccess.Storages.Interfaces;

public interface IFinder<T> : IDataAccessObject where T : class
{
    IAsyncEnumerable<T> GetAsync(bool asNoTracking = false);
    IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> condition, bool asNoTracking = false);
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);
}
