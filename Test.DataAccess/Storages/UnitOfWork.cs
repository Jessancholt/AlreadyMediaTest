using Microsoft.Extensions.DependencyInjection;
using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Storages;

internal class UnitOfWork : IUnitOfWork
{
    private readonly TestDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(
        TestDbContext dbContext,
        IServiceProvider serviceProvider)
    {
        _context = dbContext;
        _serviceProvider = serviceProvider;
    }

    public T Get<T>() where T : IDataAccessObject
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        return _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}
