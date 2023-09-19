using Contacts.Domain.Models;

namespace Contacts.Application.Abstractions.Repositories;

public interface IContactsRepository : IRepository<Contact, Guid>
{
}