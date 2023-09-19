using System.Net;
using System.Net.Http.Json;
using Contacts.Domain.Models;
using Contacts.Fakers.RequestDtos;
using Contacts.WebApi.Infrastructure.ErrorResponse;
using Contacts.WebApi.RequestsDtos;
using FluentAssertions;
using Xunit;

namespace Contacts.WebApi.IntegrationTests.Controllers.Contacts;

public class CreateContactTests : ControllerBaseTests
{
    public CreateContactTests(WebApiFixture webApiFixture) : base(webApiFixture)
    {
    }
    
    [Fact]
    public async Task CreateContact_ShouldReturnBadRequest_WhenDtoIsNotValid()
    {
        // Arrange
        var contactRequestDto = new CreateContactDto();
        
        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/contacts", contactRequestDto);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var items = await response.Content.ReadFromJsonAsync<ApiErrors>();
        items.Should().NotBeNull();
        items!.DetailedErrors.Should().NotBeNull();
        items.DetailedErrors!.Count.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task CreateContact_ShouldCreateContact_WhenDtoIsValid()
    {
        // Arrange
        var contactRequestDto = CreateContactFaker.Fake();
        
        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/v1/contacts", contactRequestDto);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        Contact? contact = await DbContext.Set<Contact>().FindAsync(id);
        contact.Should().NotBeNull();
        contact.Should().BeEquivalentTo(contactRequestDto);
        contact!.CreatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(2));
        contact.UpdatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(2));
        contact.IsDeleted.Should().BeFalse();
    }
}