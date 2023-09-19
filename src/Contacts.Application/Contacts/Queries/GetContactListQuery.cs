using Contacts.Application.Contacts.Dto;
using MediatR;

namespace Contacts.Application.Contacts.Queries;

public record GetContactListQuery() : IRequest<IReadOnlyCollection<ContactDto>>;