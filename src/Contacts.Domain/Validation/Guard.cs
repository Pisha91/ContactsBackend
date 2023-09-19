using System.Diagnostics.CodeAnalysis;
using Contacts.Domain.Exceptions;

namespace Contacts.Domain.Validation;

public static class Guard
{
    public static void ValidateIfExist<TData>([NotNull] TData? data, string validationMessage)
    {
        if (data is null)
        {
            throw new EntityNotFoundException(validationMessage);
        }
    }
}