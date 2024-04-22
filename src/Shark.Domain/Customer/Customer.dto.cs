using Shark.Domain.CustomerManagement;

public record CustomerDto
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CPF { get; set; }
    public List<AddressDto> Addresses { get; set; }

    public static CustomerDto From(CustomerEntity entity)
    {
        return new CustomerDto{
            CustomerId = entity.CustomerId,
            FirstName = entity.FirstName,
            MiddleName = entity.MiddleName,
            DateOfBirth = entity.DateOfBirth,
            CPF = entity.CPF,
            Addresses = entity.CustomerAddresses.Select(addr => new AddressDto{
                AddressLine1 = addr.AddressLine1,
                AddressLine2 = addr.AddressLine2,
                District = addr.District,
                City = addr.City,
                State = addr.State,
                PostalCode = addr.PostalCode,
                Number = addr.Number
            }).ToList()
        };
    }
}

public record AddressDto
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public int Number { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
}