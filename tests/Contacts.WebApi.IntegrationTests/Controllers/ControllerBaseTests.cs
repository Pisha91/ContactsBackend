using Contacts.Domain.Models;
using Contacts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Contacts.WebApi.IntegrationTests.Controllers;

[Collection(TestFixtureCollection.TestCollectionName)]
public abstract class ControllerBaseTests : IAsyncLifetime
{
    protected readonly HttpClient HttpClient;
    protected readonly ContactsDbContext DbContext;
    
    private readonly WebApiFixture _webApiFixture;
    private readonly IServiceScope _scope;
    
    protected ControllerBaseTests(WebApiFixture webApiFixture)
    {
        ArgumentNullException.ThrowIfNull(webApiFixture);
        
        _webApiFixture = webApiFixture;
        HttpClient = _webApiFixture.Server.CreateClient();
        _scope = _webApiFixture.Services.CreateScope();
        // DB context usage better to replace by some abstraction to easily substitute it if necessary
        DbContext = _scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
    }

    public Task InitializeAsync()
    {
        return RemoveAllContacts();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        HttpClient.Dispose();
        _scope.Dispose();
    }

    private Task RemoveAllContacts()
    {
        var context = _scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
        var items = context.Set<Contact>().ToArray();
        context.RemoveRange(items);
        
        return context.SaveChangesAsync();
    }
}