using Xunit;

namespace Contacts.WebApi.IntegrationTests;

[CollectionDefinition(TestCollectionName)]
public class TestFixtureCollection : ICollectionFixture<WebApiFixture>
{
    public const string TestCollectionName = "Tests";
}
