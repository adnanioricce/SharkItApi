using System.ComponentModel.DataAnnotations;

namespace Shark.Domain.CustomerManagement;

public readonly record struct Address
{    
    public readonly string AddressLine1;
    public readonly string AddressLine2;
    public readonly int Number;        
    public readonly string District;
    public readonly string City;
    public readonly string State;
    public readonly string PostalCode;
    public Address(string addressLine1, string addressLine2, int number, string district, string city, string state, string postalCode){
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        Number = number;
        District = district;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
    public static Address From(CustomerAddress entity)
        => new (entity.AddressLine1, entity.AddressLine2, entity.Number,entity.District, entity.City, entity.State, entity.PostalCode);
    public static Address From(AddressDto entity)
    {
        //TODO: Validate
        var addr = new Address(entity.AddressLine1, entity.AddressLine2, entity.Number,entity.District, entity.City, entity.State, entity.PostalCode);
        return addr;
    }
    public static CustomerAddress To(Guid customerId,Address entity)
    {
        CustomerAddress r = new (entity.AddressLine1, entity.AddressLine2, entity.Number,entity.District, entity.City, entity.State, entity.PostalCode);
        r.CustomerId = customerId;
        return r;
    }
}
public class Customer
{
    private Customer(Guid customerId
        ,string firstName
        ,string middleName
        ,DateTime dateOfBirth
        ,string cpf
        ,IList<Address> addresses)
    {
        CustomerId = customerId;
        FirstName = firstName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        CPF = cpf;
        _addresses = addresses.ToList();
    }
    private readonly List<Address> _addresses = new List<Address>();
    public Guid CustomerId { get; private set; }
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CPF { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();
    public static IEnumerable<ValidationResult> Validate(CustomerDto dto){        
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            yield return new ValidationResult("First name is required");
            // return Result.Failure<Customer>("First name is required");
        
        if (string.IsNullOrWhiteSpace(dto.MiddleName))
            yield return new ValidationResult("MiddleName name is required");

        if (string.IsNullOrWhiteSpace(dto.CPF))
            yield return new ValidationResult("CPF is required");
            // return Result.Failure<Customer>("CPF is required");
        if (dto.DateOfBirth < DateTime.Parse("01/01/1543"))
            yield return new ValidationResult("Date of birth is too young to be registered a 'Customer'");
    }
    public static Result<Customer> From(CustomerDto dto)
    {
        var errors = Validate(dto).Select(v => v.ToError()).ToList();
        if(errors.Any())
            return Result.Fail<Customer>(errors);
        
        var addrs = dto.Addresses.Select(e => Address.From(e)).ToList();
        var customer = new Customer(dto.CustomerId == Guid.Empty ? Guid.NewGuid() : dto.CustomerId, dto.FirstName, dto.MiddleName, dto.DateOfBirth, dto.CPF,
            addrs);
        return Result.Ok(customer);
    }
}
