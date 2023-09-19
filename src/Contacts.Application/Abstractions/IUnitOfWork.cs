namespace Contacts.Application.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> CreateTransactionAsync(CancellationToken cancellationToken = default);
}