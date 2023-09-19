using Bogus;
using Contacts.Fakers.Common;
using Contacts.WebApi.RequestsDtos;

namespace Contacts.Fakers.RequestDtos;

public static class CreateContactFaker
{
    public static CreateContactDto Fake()
    {
        return new Faker<CreateContactDto>()
            .RuleFor(x => x.FirstName, f => f.Person.FirstName)
            .RuleFor(x => x.LastName, f => f.Person.LastName)
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date)
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber("##############"))
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.IBAN, IbanFaker.Fake)
            .Generate();
    }
}