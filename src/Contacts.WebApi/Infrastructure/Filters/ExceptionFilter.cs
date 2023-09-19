using Contacts.Domain.Exceptions;
using Contacts.WebApi.Infrastructure.ErrorResponse;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contacts.WebApi.Infrastructure.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = GetResponse(context.Exception);
        context.Result = new ApiResponseObjectResult(response.Item2);
        context.HttpContext.Response.StatusCode = response.Item1;

        context.ExceptionHandled = true;
    }
    
    private static (int, ApiErrors) GetResponse(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException ex => (StatusCodes.Status404NotFound, new ApiErrors { Errors = new [] { ex.Message }}),
            FluentValidation.ValidationException ex => (StatusCodes.Status400BadRequest, new ApiErrors { DetailedErrors = ex.Errors
                .Select(failure => new KeyValuePair<string, string>(failure.PropertyName, failure.ErrorMessage))
                .ToArray()}),
            ConcurrencyException ex => (StatusCodes.Status409Conflict, new ApiErrors { Errors = new [] { ex.Message }}),
            // Should be replaces with error from the resource file
            _ => (StatusCodes.Status500InternalServerError, new ApiErrors { Errors = new [] { "Unexpected error occured" }})
        };
    }
}