namespace Contacts.Domain.Models;

public interface IHasIdentifier<out TId>
{
    TId Id { get; }
}