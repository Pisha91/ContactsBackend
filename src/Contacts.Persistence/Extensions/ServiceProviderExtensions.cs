using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Persistence.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider RunMigration(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ContactsDbContext>();
    
        dbContext.Database.Migrate();

        return serviceProvider;
    }
}