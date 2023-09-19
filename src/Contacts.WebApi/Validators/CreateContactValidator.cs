using System.Text.RegularExpressions;
using Contacts.Domain.Limitations;
using Contacts.WebApi.RequestsDtos;
using FluentValidation;

namespace Contacts.WebApi.Validators;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    // I used common approach for any phone world wide as there is no requirement
    private static readonly Regex PhoneRegex = new(@"^\d{10,15}$");
    // I used BG iban validation as there is no requirement 
    private static readonly Regex IbanRegex = new(@"^BG\d{2}[A-Z]{4}\d{6}[0-9A-Z]{8}$");
    public CreateContactValidator()
    {
        // If this rules going to be reused in different models they should be moved to separate validators for field into Domain assembly and used here with SetValidator
        // Also it is a nice practice to use custom error messages from the resource file
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ContactsLimitations.FirstNameMaxLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(ContactsLimitations.LastNameMaxLength);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(ContactsLimitations.AddressMaxLength);
        RuleFor(x => x.DateOfBirth).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Phone).Must(x => PhoneRegex.IsMatch(x)).When(x => !string.IsNullOrEmpty(x.Phone)).WithMessage("Phone should contains only numbers");
        RuleFor(x => x.IBAN).NotEmpty();
        RuleFor(x => x.IBAN).Must(x => IbanRegex.IsMatch(x)).When(x => !string.IsNullOrEmpty(x.IBAN)).WithMessage("IBAN is no valid. Example: BG51AGTH115244DWJHKRPM");
    }
}