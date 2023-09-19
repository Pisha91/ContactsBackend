using Contacts.Fakers.RequestDtos;
using Contacts.WebApi.RequestsDtos;
using Contacts.WebApi.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Contacts.WebApi.UnitTests.Validators;

public class CreateContactValidatorTests
{
    private readonly CreateContactValidator _validator = new();
    
    [Fact]
    public void ShouldHaveError_WhenAllFieldsIsNull()
    {
        // Arrange
        var model = new CreateContactDto();
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.FirstName);
        result.ShouldHaveValidationErrorFor(person => person.LastName);
        result.ShouldHaveValidationErrorFor(person => person.Phone);
        result.ShouldHaveValidationErrorFor(person => person.Address);
        result.ShouldHaveValidationErrorFor(person => person.DateOfBirth);
        result.ShouldHaveValidationErrorFor(person => person.IBAN);
    }
    
    [Fact]
    public void ShouldHaveError_WhenFirstNameExceedsMaxLength()
    {
        // Arrange
        var model = CreateContactFaker.Fake();
        model.FirstName = string.Join("", Enumerable.Range(1, 51).Select(x=> 'a'));
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.FirstName);
    }
    
    [Fact]
    public void ShouldHaveError_WhenLastNameExceedsMaxLength()
    {
        // Arrange
        var model = CreateContactFaker.Fake();
        model.LastName = string.Join("", Enumerable.Range(1, 51).Select(x=> 'a'));
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.LastName);
    }
    
    [Fact]
    public void ShouldHaveError_WheAddressExceedsMaxLength()
    {
        // Arrange
        var model = CreateContactFaker.Fake();
        model.Address = string.Join("", Enumerable.Range(1, 256).Select(x=> 'a'));
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.Address);
    }
    
    [Theory]
    [InlineData("3546")]
    [InlineData("35464564564564564654")]
    [InlineData("3546r567567567")]
    public void ShouldHaveError_WhePhoneIsNotValid(string phone)
    {
        // Arrange
        var model = CreateContactFaker.Fake();
        model.Phone = phone;
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.Phone);
    }
    
    [Theory]
    [InlineData("BG51AGTH115244DWJHKRPM")]
    [InlineData("B651AGTH115244DWJHKRPM")]
    [InlineData("BT51AGTH115244DWJHKRPM")]
    [InlineData("BG51AGTH115244DWJHKRPM8")]
    [InlineData("BG51AGTH115244DWJHKRPMM")]
    [InlineData("BG51AGTH115244DWJH")]
    [InlineData("BG5AAGTH115244DWJHKRPM")]
    [InlineData("BG51AG5H115244DWJHKRPM")]
    [InlineData("BG51AGTH115N44DWJHKRPM")]
    [InlineData("BG51AGTH115244DW6HKRPM")]
    public void ShouldHaveError_WheIbanIsNotValid(string phone)
    {
        // Arrange
        var model = CreateContactFaker.Fake();
        model.Phone = phone;
        
        // Act
        var result = _validator.TestValidate(model);
        
        //Assert
        result.ShouldHaveValidationErrorFor(person => person.Phone);
    }
}