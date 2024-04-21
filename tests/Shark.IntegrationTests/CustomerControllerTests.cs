using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Shark.API;
using Shark.API.Testing.Tools;
using Shark.IntegrationTests.Tools;
namespace Shark.API.Controllers.IntegrationTests;

public class CustomerControllerTests    
{    

    public CustomerControllerTests()
    {        
    }

    [Fact]    
    public async Task Create_customer_with_valid_data()
    {
        return;
        // Arrange        
        //talvez eu esteja complicando isso mais do que deveria...
        var controller = Ioc.CreateInstanceWith<CustomerController>();

        // Act
        var response = await controller.CreateCustomer(new CustomerDto(){
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
        });
        // Assert
        Assert.IsType<OkResult>(response);
    }
}
