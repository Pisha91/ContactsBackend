using Contacts.Application.Abstractions;
using Contacts.Application.Abstractions.Repositories;
using Contacts.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, string connection)
    {
        serviceCollection.AddDbContext<ContactsDbContext>(options => options.UseSqlServer(connection));
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IContactsRepository, ContactsRepository>();
        
        return serviceCollection;
    }
}