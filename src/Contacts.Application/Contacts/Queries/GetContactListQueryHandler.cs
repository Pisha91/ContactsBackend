using Contacts.Application.Abstractions.Repositories;
using Contacts.Application.Contacts.Dto;
using MediatR;

namespace Contacts.Application.Contacts.Queries;

public class GetContactListQueryHandler : IRequestHandler<GetContactListQuery, IReadOnlyCollection<ContactDto>>
{
    private readonly IContactsRepository _contactsRepository;

    public GetContactListQueryHandler(IContactsRepository contactsRepository)
    {
        ArgumentNullException.ThrowIfNull(contactsRepository);
        
        _contactsRepository = contactsRepository;
    }

    // In real app it should be paged query with the limitation for max amount of returned items
    public Task<IReadOnlyCollection<ContactDto>> Handle(GetContactListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<ContactDto> contacts = _contactsRepository.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Phone = x.Phone,
                Address = x.Address,
                IBAN = x.IBAN
            }).ToArray();

        return Task.FromResult(contacts);
    }
}