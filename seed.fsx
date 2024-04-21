#r "nuget: Bogus"
#r "nuget: Microsoft.EntityFrameworkCore, Version=3.1.3.0"
#r "src/Shark.Domain/bin/Debug/net9.0/Shark.Domain.dll"
#r "src/Shark.Infra/bin/Debug/net9.0/Shark.Infra.dll"
#r "src/Shark.Application/bin/Debug/net9.0/Shark.Application.dll"

open Bogus
open Bogus.Extensions.Brazil
open Shark.Domain.CustomerManagement
open System

let generateAddress (count:int) : AddressEntity seq =    
    let faker = Faker()        
    [0..count] 
    |> Seq.map (fun i -> 
        let addr = AddressEntity()
        addr.AddressLine1 <- faker.Address.StreetAddress()
        addr.AddressLine2 <- faker.Address.SecondaryAddress()
        addr.City <- faker.Address.City()
        addr.State <- faker.Address.State()
        addr.Number <- faker.Address.BuildingNumber() |> int
        addr.District <- faker.Address.County()
        addr.PostalCode <- faker.Address.ZipCode()    
        addr
    )
    // faker.RuleFor(fun _ -> faker.StreetAddress(), _.AddressLine1)
    // faker.RuleFor(fun _ -> faker.Address.SecondaryAddress(), _.AddressLine2)
    // faker.RuleFor(fun _ -> faker.Address.BuildingNumber(), _.Number)
    // faker.RuleFor(fun _ -> faker.Address.City(), _.City)
    // faker.RuleFor(fun _ -> faker.Address.State(), _.State)
    // faker.RuleFor(fun _ -> faker.Address.ZipCode(), _.PostalCode)

let generateDummyCustomers (count : int) : CustomerEntity seq =
    let faker = Faker()        
    [0..count]
    |> Seq.map (fun i -> 
        let c = CustomerEntity()
        c.FirstName <- faker.Person.FirstName
        c.MiddleName <- faker.Person.LastName
        c.CPF <- faker.Person.Cpf()
        c.DateOfBirth <- faker.Person.DateOfBirth
        c.CustomerId <- Guid.NewGuid()        
        c.Addresses <- System.Collections.Generic.List<AddressEntity>((generateAddress 1))
        c
    )    
let saveCustomers (customers: CustomerEntity seq) =
    use ctx = new ApplicationDbContext()    
    // ctx.
    ()
    // for customer in customers do
    //     ctx
// let dummyCustomers = 
generateDummyCustomers 10


