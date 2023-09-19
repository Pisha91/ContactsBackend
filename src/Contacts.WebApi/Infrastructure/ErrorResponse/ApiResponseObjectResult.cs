using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebApi.Infrastructure.ErrorResponse;

public class ApiResponseObjectResult : ObjectResult
{
    private readonly ObjectResult? _baseObjectResult;

    public ApiResponseObjectResult(ApiErrors response)
        : base(response)
    {
    }

    public ApiResponseObjectResult(ApiErrors response, ObjectResult baseObjectResult)
        : base(response)
    {
        _baseObjectResult = baseObjectResult ?? throw new ArgumentNullException(nameof(baseObjectResult));
    }

    public ApiResponseObjectResult(ApiErrors response, int statusCode)
        : base(response)
    {
        StatusCode = statusCode;
    }

    public override void OnFormatting(ActionContext context)
    {
        if (_baseObjectResult is null)
        {
            base.OnFormatting(context);
        }
        else
        {
            _baseObjectResult.OnFormatting(context);
        }
    }
}