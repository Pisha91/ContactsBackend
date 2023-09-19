using Contacts.Application.Abstractions;
using Contacts.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contacts.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContactsDbContext _context;

    public UnitOfWork(ContactsDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException concurrencyException)
        {
            throw new ConcurrencyException("State of the models is not up to date", concurrencyException);
        }
    }

    public async Task<IDatabaseTransaction> CreateTransactionAsync(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return new DatabaseTransaction(dbContextTransaction);
    }
}