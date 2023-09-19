namespace Contacts.Application.Abstractions.Repositories;

public interface IRepository<TEntity, in TId> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(TId id);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity);
}