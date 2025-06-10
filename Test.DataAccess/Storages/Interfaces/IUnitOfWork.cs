namespace Test.DataAccess.Storages.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    T Get<T>() where T : IDataAccessObject;
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
