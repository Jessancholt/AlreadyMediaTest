﻿namespace Test.DataAccess.Storages.Interfaces;

public interface IRepository<T> : IDataAccessObject where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
    Task CreateAsync(List<T> entities, CancellationToken cancellationToken);

    T Update(T entity);
    void Update(List<T> entities);

    T Delete(T entity);
    void Delete(List<T> entities);
}
