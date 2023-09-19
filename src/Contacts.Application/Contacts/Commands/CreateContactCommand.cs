using MediatR;

namespace Contacts.Application.Contacts.Commands;

public record CreateContactCommand(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Address,
    string Phone,
    string IBAN) : IRequest<Guid>;