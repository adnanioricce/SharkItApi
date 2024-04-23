using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shark.Domain.CustomerManagement;
public sealed class Customer
{
    private Customer(Guid customerId
        ,string firstName
        ,string middleName
        ,DateTime dateOfBirth
        ,CPF cpf
        ,IList<CustomerAddress> addresses)
    {
        CustomerId = customerId;
        FirstName = firstName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        CPF = cpf;
        _addresses = addresses.ToList();
    }
    private readonly List<CustomerAddress> _addresses = new List<CustomerAddress>();
    public Guid CustomerId { get; private set; }
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public CPF CPF { get; private set; }
    public IReadOnlyList<CustomerAddress> Addresses => _addresses.AsReadOnly();
    public static IEnumerable<ValidationResult> Validate(CustomerDto dto){
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            yield return new ValidationResult("First name is required");            
        
        if (string.IsNullOrWhiteSpace(dto.MiddleName))
            yield return new ValidationResult("MiddleName name is required");

        if (dto.DateOfBirth < DateTime.Parse("01/01/1543"))
            yield return new ValidationResult("Date of birth is too young to be registered a 'Customer'");
    }
    public static Result<Customer> Create(CustomerDto dto)
    {
        var errors = Validate(dto).Select(v => v.ToError()).ToList();
        if(errors.Any())
            return Result.Fail<Customer>(errors);
        
        var addrs = dto.Addresses.Select(e => CustomerAddress.From(e)).ToList();
        var customer = new Customer(Guid.NewGuid(), dto.FirstName, dto.MiddleName, dto.DateOfBirth, CPF.Create(dto.CPF),
            addrs);
        return Result.Ok(customer);
    }
    public static Customer From(CustomerDto dto)
    {
        if(dto.CustomerId == Guid.Empty)
        {
            throw new ArgumentException("CustomerId is required. When mapping to a Customer from a dto, is assumed that you're passing a existing customer, if you need to create one, call Customer.Create factory method");
        }
        var errors = Validate(dto).ToList();        
        if (errors.Any())
        {
            var errorMessage = string.Join("\n", errors.Select(v => $"{v.ErrorMessage} \n {string.Join("\n", v.MemberNames)}"));
            throw new ValidationException(errorMessage);
        }
        var addrs = dto.Addresses.Select(e => CustomerAddress.From(e)).ToList();
        var customer = new Customer(dto.CustomerId , dto.FirstName, dto.MiddleName, dto.DateOfBirth, CPF.Create(dto.CPF),addrs);
        return customer;
    }
}
