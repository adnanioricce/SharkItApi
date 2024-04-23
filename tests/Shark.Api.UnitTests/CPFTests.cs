using Shark.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shark.Api.UnitTests
{
    public class CPFTests
    {
        [Theory]
        [InlineData("00000400000")]
        [InlineData("0")]
        [InlineData("00000000000000")]
        [InlineData("")]
        [InlineData(null)]
        public void Given_a_cpf_string_When_is_not_a_valid_cpf_Expected_to_throw_a_exception(string cpfStr)
        {                        
            // Act .. Assert
            Assert.Throws<ArgumentException>(() => CPF.Create(cpfStr));
        }
        [Theory]
        [InlineData("586.435.460-03")]
        [InlineData("58643546003")]
        public void CreateCpfTests(string cpfStr)
        {
            // Act .. Assert
            var cpf = CPF.Create(cpfStr);
            Assert.Equal(cpfStr,cpf.ToString());
        }
    }
}
