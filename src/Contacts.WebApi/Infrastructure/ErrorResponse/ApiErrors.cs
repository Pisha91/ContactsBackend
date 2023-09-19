namespace Contacts.WebApi.Infrastructure.ErrorResponse;

public class ApiErrors
{
    public IReadOnlyCollection<string>? Errors { get; set; }
    public IReadOnlyCollection<KeyValuePair<string, string>>? DetailedErrors { get; set; }
}