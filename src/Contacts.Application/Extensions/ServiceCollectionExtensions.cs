using Contacts.Application.CommandDecorators;
using Contacts.Application.Contacts.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddMediatR(serviceConfiguration => serviceConfiguration.RegisterServicesFromAssembly(typeof(CreateContactCommand).Assembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandTransactionDecorator<,>));;
    }
}