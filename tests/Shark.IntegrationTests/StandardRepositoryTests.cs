using Bogus;
using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
using Shark.Infra.DAL;
using Shark.IntegrationTests.Tools;

public class StandardRepositoryTests
{
    [Fact]
    public async Task InsertTests()
    {
        // Given
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        InsertAsync<CustomerEntity> Insert = StandardRepository.Insert<CustomerEntity>(ctx);
        var seed = FakeGenerator.GenerateCustomer();
        var entity = CustomerEntity.From(Customer.From(seed).Value);        
        // When
        var saveCount = await Insert(entity);
        // Then
        Assert.True(saveCount > 0);
    }
    [Fact]
    public async Task UpdateTests()
    {
        // Given
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        UpdateAsync<Guid,CustomerEntity> Update = StandardRepository.Update<Guid,CustomerEntity>(ctx);
        var seed = FakeGenerator.GenerateCustomer();
        var entity = CustomerEntity.From(Customer.From(seed).Value);
        ctx.Set<CustomerEntity>().Add(entity);
        await ctx.SaveChangesAsync();
        var expectedFirst = entity.FirstName + " Updated";
        entity.FirstName += " Updated";
        // When
        var updateCount = await Update(entity.CustomerId,entity);
        // Then
        Assert.True(updateCount > 0);
        Assert.Equal(expectedFirst,entity.FirstName);
    }
    [Fact]
    public async Task GetTests()
    {
        // Given
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();        
        GetAsync<CustomerEntity> Get = StandardRepository.GetAsync<CustomerEntity>(ctx);
        var seed = FakeGenerator.GenerateCustomers(10);
        var entities = seed.Select(c => CustomerEntity.From(Customer.From(c).Value));
        ctx.Set<CustomerEntity>().AddRange(entities);
        await ctx.SaveChangesAsync();        
        // When
        var customers = await Get(new(1,100));        
        // Then
        Assert.True(customers.Count() <= 100);
    }
    [Fact]
    public async Task GetByIdTests()
    {
        // Given
        var faker = new Faker();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        GetByIdAsync<CustomerEntity> GetById = StandardRepository.GetByIdAsync<CustomerEntity>(ctx);        
        var set = ctx.Set<CustomerEntity>();
        var sample = await set.FirstOrDefaultAsync();
        // When
        var customer = await GetById(sample.CustomerId);
        // Then
        Assert.NotNull(customer);
        Assert.NotEmpty(customer.CustomerAddresses);
    }
}