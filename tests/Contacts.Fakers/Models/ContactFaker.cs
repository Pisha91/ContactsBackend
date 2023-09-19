using Bogus;
using Contacts.Domain.Models;
using Contacts.Fakers.Common;

namespace Contacts.Fakers.Models;

public static class ContactFaker
{
    public static Contact Fake()
    {
        return new Faker<Contact>().CustomInstantiator(f =>
        {
            var contact = new Contact();
            contact.Create(
                f.Person.FirstName,
                f.Person.LastName,
                f.Person.DateOfBirth.Date,
                f.Address.FullAddress(),
                f.Phone.PhoneNumber("##############"),
                IbanFaker.Fake());

            return contact;
        }).Generate();
    }
}