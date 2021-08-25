using BankSystem.Models;
using BankSystem.Service;
using System;
using Xunit;

namespace BankSystem.Tests
{
    public class ExchangeTests
    {
        [Fact]
        public void GetConvertSum_10_USD_RUB_Eq_800 ()
        {
            //Arrange
          
            RUB rubl = new RUB() { Rate = 0.2 };
            USD dol = new USD() { Rate = 16 };

            Exchange exchange = new Exchange();

            //Act
            var result = exchange.ConverterCurrency<USD, RUB>(10, dol, rubl);

            //Assert
            Assert.Equal(800, result);

        }


    }
}
