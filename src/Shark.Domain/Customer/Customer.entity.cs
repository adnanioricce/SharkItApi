using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Shark.Domain;

namespace Shark.Domain.CustomerManagement;

public class CustomerEntity
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CPF { get; set; }

    [NotMapped]
    public List<AddressEntity> Addresses { get; set; }

    public CustomerEntity()
    {
        Addresses = new List<AddressEntity>();
    }

    public static CustomerEntity From(Customer customer)
    {
        return new CustomerEntity(){
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            DateOfBirth = customer.DateOfBirth,
            CPF = customer.CPF,
            Addresses = customer.Addresses.Select(Address.To).ToList()
        };
    }
}

public class AddressEntity
{
    public AddressEntity(string addressLine1, string addressLine2, int number, string district, string city, string state, string postalCode){
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        Number = number;
        District = district;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public int Number { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
}

