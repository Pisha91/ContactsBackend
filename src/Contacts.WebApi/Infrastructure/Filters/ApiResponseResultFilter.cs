using Contacts.WebApi.Infrastructure.ErrorResponse;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Contacts.WebApi.Infrastructure.Filters;

public class ApiResponseResultFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var response = new ApiErrors { DetailedErrors = GetDetailedErrors(context.ModelState)};

            context.Result = new ApiResponseObjectResult(response, StatusCodes.Status400BadRequest);
        }
    }

    private IReadOnlyCollection<KeyValuePair<string, string>>? GetDetailedErrors(ModelStateDictionary modelState)
    {
        ArgumentNullException.ThrowIfNull(modelState);

        if (modelState.IsValid)
        {
            return null;
        }

        return modelState
            .Select(
                kvp => new
                {
                    kvp.Key,
                    PropertyNamePosition = kvp.Key.IndexOf('.', StringComparison.Ordinal),
                    Entry = kvp.Value
                })
            .Select(
                d => new
                {
                    Key = d.PropertyNamePosition != -1 ? d.Key[(d.PropertyNamePosition + 1)..] : d.Key,
                    d.Entry
                })
            .SelectMany(d => d.Entry!.Errors.Select(e => new KeyValuePair<string, string>(d.Key, e.ErrorMessage)))
            .ToArray();
    }
}