using System.Net;
using System.Net.Http.Json;
using Contacts.Application.Contacts.Dto;
using Contacts.Domain.Models;
using Contacts.Fakers.Models;
using FluentAssertions;
using Xunit;

namespace Contacts.WebApi.IntegrationTests.Controllers.Contacts;

public class GetListTests : ControllerBaseTests
{
    public GetListTests(WebApiFixture webApiFixture) : base(webApiFixture)
    {
    }

    [Fact]
    public async Task GetList_ShouldReturnList()
    {
        // Arrange
        var contacts = await CreateContacts();
        
        // Act
        HttpResponseMessage response = await HttpClient.GetAsync("api/v1/contacts");
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
        items.Should().NotBeNull();
        items!.Count.Should().Be(contacts.Count);
        items.Should().BeEquivalentTo(contacts,
            options => options
                .Excluding(x => x.IsDeleted)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.Version));
    }

    private async Task<IReadOnlyCollection<Contact>> CreateContacts()
    {
        var contacts = Enumerable.Range(1, 5).Select(x => ContactFaker.Fake()).ToArray();
        contacts[0].Delete();
        await DbContext.Set<Contact>().AddRangeAsync(contacts);

        await DbContext.SaveChangesAsync();

        return contacts.Where(x => x.IsDeleted == false).ToArray();
    }
}