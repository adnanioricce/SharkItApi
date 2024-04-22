using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Shark.Domain;

namespace Shark.Domain.CustomerManagement;

public class CustomerEntity
{
    public Guid CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string CPF { get; set; } = null!;

    public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

    public static CustomerEntity From(Customer customer)
    {
        return new CustomerEntity(){
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            DateOfBirth = customer.DateOfBirth,
            CPF = customer.CPF,
            CustomerAddresses = customer.Addresses.Select(addr => Address.To(customer.CustomerId,addr)).ToList()
        };
    }
}

public class CustomerAddress
{
    public CustomerAddress()
    {
        
    }
    public CustomerAddress(string addressLine1, string addressLine2, int number, string district, string city, string state, string postalCode){
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        Number = number;
        District = district;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
    public Guid AddressId { get; set; } = Guid.NewGuid();

    public Guid CustomerId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public int Number { get; set; }

    public string? District { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public virtual CustomerEntity Customer { get; set; } = null!;
}

