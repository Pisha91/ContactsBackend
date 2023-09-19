using Bogus;

namespace Contacts.Fakers.Common;

public static class IbanFaker
{
    private static readonly Faker Faker = new();
    public static string Fake()
    {
        return $"BG{Faker.Random.Int(10, 99)}AGTH{Faker.Random.Int(100000, 999999)}{string.Join("", Faker.Random.Chars('A', 'Z', 8))}";
    }
}