namespace Shark.Domain.CustomerManagement;

public readonly record struct CustomerAddress
{
    public readonly Guid AddressId = Guid.NewGuid();
    public readonly Guid CustomerId;
    public readonly Address Address;
    public CustomerAddress(Guid addressId,Guid customerId, Address address)
    {
        AddressId = addressId;
        CustomerId = customerId;
        Address = address;
    }
    public static CustomerAddress From(CustomerAddressEntity entity)
        => new (entity.AddressId,entity.CustomerId,Address.Create(entity.AddressLine1, entity.AddressLine2, entity.Number,entity.District, entity.City, entity.State, entity.PostalCode));
    public static CustomerAddress From(AddressDto entity)
    {        
        var addr = new CustomerAddress(entity.AddressId,entity.CustomerId,Address.Create(entity.AddressLine1, entity.AddressLine2, entity.Number, entity.District, entity.City, entity.State, entity.PostalCode));
        return addr;
    }
    public static CustomerAddressEntity To(CustomerAddress customerAddress)
    {
        CustomerAddressEntity r = new (customerAddress.AddressId,customerAddress.CustomerId,customerAddress.Address);                
        return r;
    }
}
