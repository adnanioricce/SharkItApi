using Alternative.Shark.Api;
using Bogus;
using Bogus.Extensions.Brazil;
namespace Shark.IntegrationTests.CustomerManagement.Commands;

public class CustomerManagementCommandTests {
    [Fact]
    public void InsertCustomerCommandTests()
    {
        // Given
        var addressFaker = new Faker<AddressDto>();
        addressFaker.RuleFor(x => x.AddressLine1,f => f.Person.Address.Street);
        addressFaker.RuleFor(x => x.AddressLine2,f => f.Person.Address.Suite);
        addressFaker.RuleFor(x => x.City,f => f.Person.Address.City);
        addressFaker.RuleFor(x => x.State,f => f.Person.Address.State);
        addressFaker.RuleFor(x => x.District,f => f.Person.Address.Street);
        addressFaker.RuleFor(x => x.PostalCode,f => f.Person.Address.ZipCode);
        addressFaker.RuleFor(x => x.Number,f => f.Random.Int(1,512));
        var faker = new Faker<CustomerDto>();
        faker.RuleFor(c => c.FirstName,f => f.Person.FirstName);
        faker.RuleFor(c => c.MiddleName,f => f.Person.LastName);
        faker.RuleFor(c => c.DateOfBirth,f => f.Person.DateOfBirth);
        faker.RuleFor(c => c.CPF,f => f.Person.Cpf());
        faker.RuleFor(c => c.Addresses,f => addressFaker.GenerateBetween(0,2));
        var seed = faker.Generate();
        var cmd = new InsertCustomerCommand(seed);
        // var handler = 
        // When
        
        // Then
    }
}