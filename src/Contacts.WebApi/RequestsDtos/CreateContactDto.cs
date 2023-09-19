namespace Contacts.WebApi.RequestsDtos;

public class CreateContactDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string IBAN { get; set; } = null!;
}