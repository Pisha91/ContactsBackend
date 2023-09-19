using Contacts.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contacts.Persistence;

public class DatabaseTransaction : IDatabaseTransaction
{
    private readonly IDbContextTransaction _dbContextTransaction;

    public DatabaseTransaction(IDbContextTransaction dbContextTransaction)
    {
        _dbContextTransaction = dbContextTransaction;
    }

    public void Dispose()
    {
        _dbContextTransaction.Dispose();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
         return _dbContextTransaction.CommitAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContextTransaction.DisposeAsync();
    }
}