using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Shark.Application.CustomerManagement;
using Shark.Domain.CustomerManagement;
using Shark.Infra.DAL;
using Shark.IntegrationTests.Tools;
namespace Shark.IntegrationTests.CustomerManagement.Commands;

public class CustomerManagementCommandTests {
    
    [Fact]
    public async Task InsertCustomerCommandTests()
    {
        // Given        
        var seed = FakeGenerator.GenerateCustomer();
        var cmd = new InsertCustomerCommand(seed);        
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        
        var customers = ctx.Set<CustomerEntity>();
        var customerCount = customers.Count();
        var handler = new InsertCustomerCommandHandler(StandardRepository.Insert<CustomerEntity>(ctx));
        // When
        await handler.HandleAsync(cmd);
        // Then
        Assert.NotEqual(customerCount, customers.Count());
        
    }
    [Fact]
    public async Task UpdateCustomerCommandTests(){
        // Given  
        var seed = FakeGenerator.GenerateCustomers(10);
        async Task SeedDb(ApplicationDbContext context){            
            var entities = seed.Select(dto => Customer.From(dto).Value).Select(CustomerEntity.From).ToList();        
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
        var queryHandler = new UpdateCustomerCommandHandler(StandardRepository.Update<Guid,CustomerEntity>(ctx));
        // When
        await queryHandler.HandleAsync(query);
        // Then        
        //ChangeTracking is not enabled here, if you're asking yourself why I am searching the database.
        var updatedEntity = customers.Where(c => c.CustomerId == sample.CustomerId).FirstOrDefault();        
        Assert.Equal(sample.FirstName,updatedEntity.FirstName);
    }
    [Fact]
    public async Task GetCustomerQueryTests()
    {
        // Given        
        using var ctx = Ioc.GetService<ApplicationDbContext>();
        using var transaction = ctx.Database.BeginTransaction();
        var seed = FakeGenerator.GenerateCustomers(100);
        var customers = ctx.Set<CustomerEntity>();
        var insert = StandardRepository.Insert<CustomerEntity>(ctx);
        var handler = new InsertCustomerCommandHandler(insert);
        foreach (var customer in seed)
        {
            var cmd = new InsertCustomerCommand(customer);            
            await handler.HandleAsync(cmd);
        }
        var query = new GetCustomersQuery(1,100);
        var queryHandler = new GetCustomersQueryHandler(StandardRepository.GetAsync<CustomerEntity>(ctx));
        // When
        var response = await queryHandler.HandleAsync(query);
        // Then
        Assert.Equal(100,response.Count());
    }
    [Fact]
    public async Task GetCustomerByIdQueryTests(){
        // Given  
        var seed = FakeGenerator.GenerateCustomers(10);
        async Task SeedDb(ApplicationDbContext context){
            
            var entities = seed.Select(dto => Customer.From(dto).Value).Select(CustomerEntity.From).ToList();        
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
        var queryHandler = new GetCustomerByIdQueryHandler(StandardRepository.GetByIdAsync<CustomerEntity>(ctx));
        // When
        var response = await queryHandler.HandleAsync(query);
        // Then
        Assert.Equal(sample.CustomerId,response.CustomerId);
    }
       
}