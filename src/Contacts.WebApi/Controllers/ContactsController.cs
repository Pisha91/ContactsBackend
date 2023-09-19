using Contacts.Application.Contacts.Commands;
using Contacts.Application.Contacts.Dto;
using Contacts.Application.Contacts.Queries;
using Contacts.WebApi.Infrastructure.ErrorResponse;
using Contacts.WebApi.RequestsDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContactDto>), StatusCodes.Status200OK)]
    public Task<IReadOnlyCollection<ContactDto>> GetList()
    {
        return _mediator.Send(new GetContactListQuery());
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status400BadRequest)]
    public Task<Guid> Create([FromBody] CreateContactDto dto)
    {
        return _mediator.Send(new CreateContactCommand(
            dto.FirstName,
            dto.LastName,
            dto.DateOfBirth!.Value,
            dto.Address,
            dto.Phone,
            dto.IBAN));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrors), StatusCodes.Status409Conflict)]
    public Task Delete([FromRoute] Guid id)
    {
        return _mediator.Send(new DeleteContactCommand(id));
    }
}