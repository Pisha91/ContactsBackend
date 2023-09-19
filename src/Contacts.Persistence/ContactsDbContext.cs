using Contacts.Domain.Limitations;
using Contacts.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Persistence;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>().HasKey(e => e.Id);
        modelBuilder.Entity<Contact>().Property(e => e.FirstName).IsRequired().HasMaxLength(ContactsLimitations.FirstNameMaxLength);
        modelBuilder.Entity<Contact>().Property(e => e.LastName).IsRequired().HasMaxLength(ContactsLimitations.LastNameMaxLength);
        modelBuilder.Entity<Contact>().Property(e => e.Phone).IsRequired().HasMaxLength(ContactsLimitations.PhoneMaxLength);
        modelBuilder.Entity<Contact>().Property(e => e.Address).IsRequired().HasMaxLength(ContactsLimitations.AddressMaxLength);
        modelBuilder.Entity<Contact>().Property(e => e.DateOfBirth).IsRequired();
        modelBuilder.Entity<Contact>().Property(e => e.IBAN).IsRequired().HasMaxLength(ContactsLimitations.IBANMaxLength);
        modelBuilder.Entity<Contact>().Property(e => e.Version).IsRowVersion();

        base.OnModelCreating(modelBuilder);
    }
}