using System.Reflection;
using Contacts.Application.Abstractions;
using Contacts.Domain.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Contacts.Application.CommandDecorators;

// In the current application it is a bit overhead, but as soon messaging system is added it will become required
public class CommandTransactionDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CommandTransactionDecorator<TRequest, TResponse>> _logger;

    public CommandTransactionDecorator(
        IUnitOfWork unitOfWork,
        ILogger<CommandTransactionDecorator<TRequest, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(next);

        if (typeof(TRequest).GetCustomAttribute<WithTransactionAttribute>() is null)
        {
            return await next();
        }

        await using var transaction = await _unitOfWork.CreateTransactionAsync(cancellationToken);
        _logger.LogDebug("Transaction for {CommandHandlerName} command handler created", typeof(TRequest).FullName);

        TResponse response = await next();

        _logger.LogDebug("Transaction for {CommandHandlerName} command handler completed", typeof(TRequest).FullName);

        return response;
    }
}