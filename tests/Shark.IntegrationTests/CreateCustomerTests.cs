using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shark.API.CustomerManagement;
using Shark.Domain.CustomerManagement;
using Shark.Infra.DAL;
using Shark.IntegrationTests.Tools;
namespace Shark.IntegrationTests.CustomerManagement.Api;
public class CreateCustomerTests
{
    [Fact]
    public async Task Test_CustomerCreation()
    {
        // Given
        var dto = new CustomerDto(){
            FirstName = "Test"
            ,MiddleName = "User"
            ,DateOfBirth = DateTime.Now.AddYears(-18)
            ,CPF = "12345678910"
            ,Addresses = new List<AddressDto>{
                new AddressDto{
                    AddressLine1 = "Rua do teste",
                    AddressLine2 = "N 224",
                    District = "Bairro teste",
                    Number = 224,
                    State = "TS",
                    City = "CITY TEST",
                    PostalCode = "01234567"
                }
            }
        };
        var mediator = Ioc.GetService<IMediator>();        
        // When
        var result = await CustomerEndpoints.CreateCustomer(mediator,new(dto));        
        // Then
        Assert.IsType<Ok>(result);
    }
    [Fact]
    public async Task GetTest()
    {
        // Given
        var dto = new CustomerDto(){
            FirstName = "Test"
            ,MiddleName = "User"
            ,DateOfBirth = DateTime.Now.AddYears(-18)
            ,CPF = "12345678910"
            ,Addresses = new List<AddressDto>{
                new AddressDto{
                    AddressLine1 = "Rua do teste",
                    AddressLine2 = "N 224",
                    District = "Bairro teste",
                    Number = 224,
                    State = "TS",
                    City = "CITY TEST",
                    PostalCode = "01234567"
                }
            }
        };
        var ctx = Ioc.GetService<ApplicationDbContext>();
        var set = ctx.Set<CustomerEntity>();
        var customer = set.FirstOrDefault();
        var mediator = Ioc.GetService<IMediator>();        
        // When
        var result = await CustomerEndpoints.GetCustomerById(mediator,new(customer.CustomerId));                
        // Then
        Assert.IsType<Ok>(result);        
    }
}