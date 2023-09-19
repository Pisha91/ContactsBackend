namespace Contacts.Application.Contacts.Dto;

public class ContactDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; } = null!;
    public string? IBAN { get; set; }
}