using System.Net;
using System.Net.Http.Json;
using Contacts.Domain.Models;
using Contacts.Fakers.Models;
using Contacts.WebApi.Infrastructure.ErrorResponse;
using FluentAssertions;
using Xunit;

namespace Contacts.WebApi.IntegrationTests.Controllers.Contacts;

public class DeleteContactTests : ControllerBaseTests
{
    public DeleteContactTests(WebApiFixture webApiFixture) : base(webApiFixture)
    {
    }
    
    [Fact]
    public async Task DeleteContact_ShouldNotDeleteContact_WhenNotExists()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"api/v1/contacts/{Guid.NewGuid()}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var items = await response.Content.ReadFromJsonAsync<ApiErrors>();
        items.Should().NotBeNull();
        items!.Errors.Should().NotBeNull();
        items.Errors!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task DeleteContact_ShouldDeleteContact_WhenExists()
    {
        // Arrange
        var existingContact = await CreateContact();
        
        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"api/v1/contacts/{existingContact.Id}");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await DbContext.Entry(existingContact).ReloadAsync();
        existingContact.Should().NotBeNull();
        existingContact!.UpdatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(2));
        existingContact.IsDeleted.Should().BeTrue();
    }
    
    private async Task<Contact> CreateContact()
    {
        var contacts = ContactFaker.Fake();
        await DbContext.Set<Contact>().AddAsync(contacts);

        await DbContext.SaveChangesAsync();

        return contacts;
    }
}