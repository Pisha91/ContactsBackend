using MediatR;

namespace Contacts.Application.Contacts.Commands;

public record DeleteContactCommand(Guid Id) : IRequest;