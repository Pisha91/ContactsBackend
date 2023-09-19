using Contacts.Application.Abstractions;
using Contacts.Application.Abstractions.Repositories;
using Contacts.Domain.Attributes;
using Contacts.Domain.Models;
using Contacts.Domain.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Contacts.Application.Contacts.Commands;

[WithTransaction]
public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand>
{
    private readonly IContactsRepository _contactsRepository;
    private readonly ILogger<DeleteContactCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteContactCommandHandler(
        IContactsRepository contactsRepository,
        ILogger<DeleteContactCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(contactsRepository);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(unitOfWork);
        
        _contactsRepository = contactsRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        Contact? contact = await _contactsRepository.GetByIdAsync(request.Id);
        Guard.ValidateIfExist(contact, $"Contact with id {request.Id} not found");

        bool result = contact.Delete();
        if (result)
        {
            await _contactsRepository.UpdateAsync(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("contact with id {ContactId} deleted", request.Id);
        } 
    }
}