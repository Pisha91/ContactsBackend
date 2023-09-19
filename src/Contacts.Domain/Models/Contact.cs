namespace Contacts.Domain.Models;

public class Contact : IHasIdentifier<Guid>
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTime DateOfBirth { get; private set; }
    public string Address { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string IBAN { get; private set; } = null!;
    public bool IsDeleted { get; private set; }
    // In real application would be required to track users as well
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    
    public byte[] Version { get; set; } = null!;

    public void Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string address,
        string phone,
        string iban)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Address = address;
        Phone = phone;
        IBAN = iban;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        IsDeleted = false;
    }

    public bool Delete()
    {
        if (IsDeleted)
        {
            return false;
        }
        
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;

        return true;
    }
}