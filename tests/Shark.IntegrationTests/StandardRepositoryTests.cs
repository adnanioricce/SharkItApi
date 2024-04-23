using Bogus;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Shark.IntegrationTests.Tools;

public class StandardRepositoryTests
{
    IRepository<CustomerEntity> CreateRepository()
    {
        var ctx = Ioc.GetService<ApplicationDbContext>();
        var repository = new CustomerRepository(ctx);
        return repository;
    }
    [Fact]
    public async Task InsertTests()
    {
        // Given
        using var repo = CreateRepository();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();        
        var seed = FakeGenerator.GenerateCustomer();
        var entity = Customer.Create(seed).Map(CustomerEntity.From).Unwrap();
        // When
        repo.AddAsync(entity);
        var saveCount = await repo.SaveChangesAsync();
        // Then
        Assert.True(saveCount > 0);
    }
    [Fact]
    public async Task UpdateTests()
    {
        // Given
        using var repo = CreateRepository();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();        
        var seed = FakeGenerator.GenerateCustomer();
        var entity = Customer.Create(seed).Map(CustomerEntity.From).Unwrap();
        ctx.Set<CustomerEntity>().Add(entity);
        await ctx.SaveChangesAsync();
        var expectedFirst = entity.FirstName + " Updated";
        entity.FirstName += " Updated";
        // When
        repo.UpdateAsync(entity);
        var updateCount = await repo.SaveChangesAsync();
        // Then
        Assert.True(updateCount > 0);
        Assert.Equal(expectedFirst,entity.FirstName);
    }
    [Fact]
    public async Task GetTests()
    {
        // Given
        using var repo = CreateRepository();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();                
        var seed = FakeGenerator.GenerateCustomers(10);
        var entities = seed.Select(c => CustomerEntity.From(Customer.Create(c).Value));
        ctx.Set<CustomerEntity>().AddRange(entities);
        await ctx.SaveChangesAsync();        
        // When
        var customers = await repo.ListAsync(new(1,100));        
        // Then
        Assert.True(customers.Count() <= 100);
    }
    [Fact]
    public async Task GetByIdTests()
    {
        // Given
        using var repo = CreateRepository();
        var faker = new Faker();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();        
        var set = ctx.Set<CustomerEntity>();
        var sample = await set.FirstOrDefaultAsync();
        // When
        var customer = await repo.GetByIdAsync(sample.CustomerId);
        // Then
        Assert.NotNull(customer);
        Assert.NotEmpty(customer.CustomerAddresses);
    }
}