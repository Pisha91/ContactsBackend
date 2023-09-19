using Contacts.Application.Abstractions.Repositories;
using Contacts.Domain.Models;

namespace Contacts.Persistence.Repositories;

public class ContactsRepository : Repository<Contact, Guid>, IContactsRepository
{
    public ContactsRepository(ContactsDbContext context) : base(context)
    {
    }
}