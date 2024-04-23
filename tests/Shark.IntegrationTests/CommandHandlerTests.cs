using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Shark.Api;
using Shark.Domain.CustomerManagement;
using Shark.Domain.Interfaces;
//using NS
using Shark.IntegrationTests.Tools;
namespace Shark.IntegrationTests.CustomerManagement.Commands;

public class CustomerManagementCommandTests {
    
    IRepository<CustomerEntity> CreateRepository()
    {
        var ctx = Ioc.GetService<ApplicationDbContext>();
        var repository = new CustomerRepository(ctx);
        return repository;
    }
    [Fact]
    public async Task InsertCustomerCommandTests()
    {
        // Given        
        var cts = new CancellationTokenSource();
        using var repo = CreateRepository();
        
        var seed = FakeGenerator.GenerateCustomer();
        var cmd = new InsertCustomerCommand(seed);
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();

        var customers = ctx.Set<CustomerEntity>();
        var customerCount = customers.Count();
        var handler = new CustomerHandler(repo);
        // When
        await handler.Handle(cmd, cts.Token);
        // Then
        Assert.NotEqual(customerCount, customers.Count());
        
        
    }
    [Fact]
    public async Task UpdateCustomerCommandTests(){
        var cts = new CancellationTokenSource();
        using var repo = CreateRepository();
        
        // Given  
        var seed = FakeGenerator.GenerateCustomers(10);
        async Task SeedDb(ApplicationDbContext context)
        {
            var entities = seed.Select(Customer.From).Select(CustomerEntity.From).ToList();
            var customers = context.Set<CustomerEntity>();
            customers.AddRange(entities);
            var saveCount = await context.SaveChangesAsync();
        }
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        await SeedDb(ctx);
        var customers = ctx.Set<CustomerEntity>();
        var savedEntities = customers.AsNoTracking().Take(10).ToList();
        var dtos = savedEntities.Select(e => CustomerDto.From(e)).ToList();
        var sample = new Faker().PickRandom(dtos);
        sample.FirstName += " Updated!";
        var query = new UpdateCustomerCommand(sample);
        var queryHandler = new CustomerHandler(repo);
        // When
        await queryHandler.Handle(query,cts.Token);
        // Then        
        //ChangeTracking is not enabled here, if you're asking yourself why I am searching the database.
        var updatedEntity = customers.Where(c => c.CustomerId == sample.CustomerId).FirstOrDefault();
        Assert.Equal(sample.FirstName, updatedEntity.FirstName);
        
    }
    [Fact]
    public async Task GetCustomerQueryTests()
    {
        // Given        
        var cts = new CancellationTokenSource();
        using var repo = CreateRepository();
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        var seed = FakeGenerator.GenerateCustomers(100);
        var customers = ctx.Set<CustomerEntity>();        
        var handler = new CustomerHandler(repo);
        foreach (var customer in seed)
        {
            var cmd = new InsertCustomerCommand(customer);            
            await handler.Handle(cmd, cts.Token);
        }
        var query = new GetCustomersQuery(1,100);
        var queryHandler = new CustomerHandler(repo);
        // When
        var response = await queryHandler.Handle(query, cts.Token);
        // Then
        Assert.Equal(100,response.Count());
    }
    [Fact]
    public async Task GetCustomerByIdQueryTests(){
        // Given  
        var cts = new CancellationTokenSource();
        using var repo = CreateRepository();
        var seed = FakeGenerator.GenerateCustomers(10);
        async Task SeedDb(ApplicationDbContext context){
            
            var entities = seed.Select(Customer.From).Select(CustomerEntity.From).ToList();        
            var customers = context.Set<CustomerEntity>();        
            customers.AddRange(entities);
            var saveCount = await context.SaveChangesAsync();
        }
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        await SeedDb(ctx);
        var savedEntities = ctx.Set<CustomerEntity>().Take(10).ToList();
        var sample = new Faker().PickRandom(savedEntities);
        var query = new GetCustomerByIdQuery(sample.CustomerId);
        var queryHandler = new CustomerHandler(repo);
        // When
        var response = await queryHandler.Handle(query, cts.Token);
        // Then
        Assert.Equal(sample.CustomerId,response.CustomerId);
    }
       
}