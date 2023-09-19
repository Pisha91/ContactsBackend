using Contacts.Application.Abstractions;
using Contacts.Application.Abstractions.Repositories;
using Contacts.Domain.Attributes;
using Contacts.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Contacts.Application.Contacts.Commands;

[WithTransaction]
public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Guid>
{
    private readonly IContactsRepository _contactsRepository;
    private readonly ILogger<CreateContactCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactCommandHandler(
        IContactsRepository contactsRepository,
        ILogger<CreateContactCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(contactsRepository);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        
        _contactsRepository = contactsRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        Contact contact = new Contact();
        contact.Create(request.FirstName, request.LastName, request.DateOfBirth, request.Address, request.Phone, request.IBAN);

        await _contactsRepository.CreateAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Contact with id {ContactId} created", contact.Id);

        return contact.Id;
    }
}