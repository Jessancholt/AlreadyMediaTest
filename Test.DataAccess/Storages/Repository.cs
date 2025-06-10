﻿using Test.DataAccess.Storages.Interfaces;

namespace Test.DataAccess.Repositories;

internal class Repository<T> : IRepository<T>
    where T : class
{
    private readonly TestDbContext _context;

    public Repository(TestDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        var result = await _context.AddAsync(entity, cancellationToken);
        return result.Entity;
    }

    public async Task CreateAsync(List<T> entities, CancellationToken cancellationToken)
    {
        await _context.AddRangeAsync(entities, cancellationToken);
    }

    public T Update(T entity)
    {
        return _context.Update(entity).Entity;
    }

    public void Update(List<T> entities)
    {
        _context.UpdateRange(entities);
    }

    public T Delete(T entity)
    {
        return _context.Remove(entity).Entity;
    }

    public void Delete(List<T> entities)
    {
        _context.RemoveRange(entities);
    }
}
