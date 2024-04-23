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

    public virtual ICollection<CustomerAddressEntity> CustomerAddresses { get; set; } = new List<CustomerAddressEntity>();

    public static CustomerEntity From(Customer customer)
    {        
        return new CustomerEntity(){
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            MiddleName = customer.MiddleName,
            DateOfBirth = customer.DateOfBirth,
            CPF = customer.CPF.ToString(),
            CustomerAddresses = customer.Addresses.Select(CustomerAddress.To).ToList()
        };
    }
}

public class CustomerAddressEntity
{
    public CustomerAddressEntity()
    {
        
    }
    public CustomerAddressEntity(Guid addressId,Guid customerId, Address address)
    {
        AddressId = addressId;
        CustomerId = customerId;
        AddressLine1 = address.AddressLine1;
        AddressLine2 = address.AddressLine2;
        Number = address.Number;
        District = address.District;
        City = address.City;
        State = address.State;
        PostalCode = address.PostalCode;
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

