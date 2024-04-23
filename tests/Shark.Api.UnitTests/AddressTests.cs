using Shark.Domain.CustomerManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shark.Api.UnitTests
{
    public class AddressTests
    {
        [Theory]
        [InlineData("","","","","","",0)]
        [InlineData("Address 1", "", "test district", "test city", "test state", "0012345667", -1)]
        [InlineData("", "", "test district", "test city", "test state", "0012345667", 0)]
        [InlineData("Address 1", "", "", "test city", "test state", "0012345667", 0)]
        [InlineData("Address 1", "", "test district", "", "test state", "0012345667", 0)]
        [InlineData("Address 1", "", "test district", "test city", "", "0012345667", 0)]
        [InlineData("Address 1", "", "test district", "test city", "", "", 0)]        
        [InlineData("Address 1", "", "test district", "test city", "test state", "asdf1234", 0)]
        public void InvalidAddresses(string addressLine1
            , string addressLine2
            , string district
            , string city
            , string state
            , string postalCode
            , int number)
        {
            Assert.Throws<ValidationException>(() => Address.Create(addressLine1, addressLine2, number, district, city, state, postalCode));
        }
        [Theory]
        [InlineData("Address 1", "", "test district", "test city", "test state", "0012345667", 1)]
        public void ValidAddresses(string addressLine1
            , string addressLine2
            , string district
            , string city
            , string state
            , string postalCode
            , int number)
        {
            var addr = Address.Create(addressLine1, addressLine2, number, district, city, state, postalCode);            
        }        
    }
}
