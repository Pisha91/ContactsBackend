namespace Contacts.Application.Abstractions;

public interface IDatabaseTransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}