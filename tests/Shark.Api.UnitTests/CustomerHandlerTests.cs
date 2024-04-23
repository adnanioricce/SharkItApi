using NSubstitute;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Shark.Api;
using Bogus;
using Bogus.Extensions.Brazil;
public class CustomerHandlerTests
{
    IEnumerable<CustomerDto> GenerateCustomers(int count)
    {
        var addressFaker = new Faker<AddressDto>();
        addressFaker.RuleFor(x => x.AddressLine1, f => f.Person.Address.Street);
        addressFaker.RuleFor(x => x.AddressLine2, f => f.Person.Address.Suite);
        addressFaker.RuleFor(x => x.City, f => f.Person.Address.City);
        addressFaker.RuleFor(x => x.State, f => f.Person.Address.State);
        addressFaker.RuleFor(x => x.District, f => f.Person.Address.Street);
        addressFaker.RuleFor(x => x.PostalCode, f => new string(f.Person.Address.ZipCode.Where(c => char.IsDigit(c)).ToArray()));
        addressFaker.RuleFor(x => x.Number, f => f.Random.Int(1, 512));
        var faker = new Faker<CustomerDto>();
        faker.RuleFor(c => c.FirstName, f => f.Person.FirstName);
        faker.RuleFor(c => c.MiddleName, f => f.Person.LastName);
        faker.RuleFor(c => c.DateOfBirth, f => DateTime.UtcNow.AddYears(f.Random.Int(-80, -18)));
        faker.RuleFor(c => c.CPF, f => f.Person.Cpf());
        faker.RuleFor(c => c.Addresses, f => addressFaker.GenerateBetween(1, 3));
        return faker.Generate(count);
    }
    [Fact]
    public async Task Given_list_request_Should_return_only_especified_count()
    {
        // Given        
        var command = new GetCustomersQuery(1,100);
        var repo = Substitute.For<IRepository<CustomerEntity>>();
        var entities = Enumerable.Repeat(new CustomerEntity(),100);
        repo.ListAsync(new Pagination(1,100)).Returns(entities);
        var handler = new CustomerHandler(repo);
        // When
        var response = await handler.Handle(command,new CancellationToken());
        // Then
        Assert.NotEmpty(response);
        Assert.Equal(100,response.Count());
    }
    [Fact]
    public async Task Given_list_request_When_there_is_no_data_in_range_Then_a_empty_list_is_expected()
    {
        // Given
        var entity = new CustomerEntity();
        var command = new GetCustomersQuery(1,100);
        var repo = Substitute.For<IRepository<CustomerEntity>>();        
        var handler = new CustomerHandler(repo);
        // When
        var response = await handler.Handle(command, new CancellationToken());
        // Then
        Assert.Empty(response);        
    }
    [Fact]
    public async Task Given_create_request_When_after_validation_succedd_Then_doesnt_throw_any_error()
    {
        // Given        
        
        var repo = Substitute.For<IRepository<CustomerEntity>>();
        var dto = GenerateCustomers(1).FirstOrDefault();
        //repo.Add(entity).;
        var command = new InsertCustomerCommand(dto);
        var handler = new CustomerHandler(repo);
        // When
        await handler.Handle(command, new CancellationToken());
        // Then        
    }
    [Fact]
    public async Task Given_update_request_When_passes_validation_Then_no_exception_should_be_throwed()
    {
        // Given        
        var repo = Substitute.For<IRepository<CustomerEntity>>();
        var dto = GenerateCustomers(1).FirstOrDefault();
        dto.CustomerId = Guid.NewGuid();
                
        var command = new UpdateCustomerCommand(dto);
        var handler = new CustomerHandler(repo);
        // When
        await handler.Handle(command, new CancellationToken());
        // Then        
    }
}