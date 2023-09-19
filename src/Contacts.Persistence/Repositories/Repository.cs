using Contacts.Application.Abstractions.Repositories;
using Contacts.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Persistence.Repositories;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IHasIdentifier<TId>
{
    private readonly ContactsDbContext _context;

    protected Repository(ContactsDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Set<TEntity>().AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Entry(entity).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }
}