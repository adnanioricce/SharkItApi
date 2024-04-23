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
    private Address(
        string AddressLine1
        , string AddressLine2
        , int Number
        , string District
        , string City
        , string State
        , string PostalCode)
    {
        this.AddressLine1 = AddressLine1;
        this.AddressLine2 = AddressLine2;
        this.Number = Number;
        this.District = District;
        this.City = City;
        this.State = State;
        this.PostalCode = PostalCode;
    }
    public static Address Create(
        string AddressLine1
        , string AddressLine2
        , int Number
        , string District
        , string City
        , string State
        , string PostalCode)
    {
        var addr = new Address(AddressLine1, AddressLine2,Number,District,City,State,PostalCode);
        //confesso que fiz dessa forma só para economizar no tempo do código da validação,
        //Não acho que lançar uma exceção se uma validação falhar é uma boa ideia,
        //porque isso é uma situação esperada que pode ser tratada devidamente.
        //Isso no entanto toma tempo e a exceção é algumas linhas
        var validationResult = Validate(addr).ToList();        
        if (validationResult.Any())
        {
            var errorMessage = string.Join("\n", validationResult.Select(v => $"{v.ErrorMessage} \n {string.Join("\n", v.MemberNames)}"));
            throw new ValidationException(errorMessage);
        }
        return addr;
    }
    public static IEnumerable<ValidationResult> Validate(Address address)
    {                
        if (string.IsNullOrWhiteSpace(address.AddressLine1))
            yield return new ValidationResult("AddressLine1 is required");
        if (string.IsNullOrWhiteSpace(address.District))
            yield return new ValidationResult("District is required");
        if (string.IsNullOrWhiteSpace(address.City))
            yield return new ValidationResult("City is required");
        if (string.IsNullOrWhiteSpace(address.State))
            yield return new ValidationResult("State is required");
        if (address.Number < 0)
            yield return new ValidationResult("address number can't be negative");
        if (string.IsNullOrWhiteSpace(address.PostalCode))
            yield return new ValidationResult("PostalCode is required");        
        if (address.PostalCode.Any(p => !int.TryParse(p.ToString(),out int r)))
            yield return new ValidationResult("PostalCode only accepts digits");
    }    
    public override string ToString()
    {
        return $"{this.AddressLine1} , {this.AddressLine2} {this.Number} - {this.City},{this.State} - {this.PostalCode}";
    }
}
