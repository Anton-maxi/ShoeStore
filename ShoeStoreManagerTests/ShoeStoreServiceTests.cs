using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoeStoreManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShoeStoreManager.Tests
{
    [TestClass]
    public class ShoeStoreServiceTests
    {
        private ShoeStoreService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ShoeStoreService();
        }

        [TestMethod]
        public void AuthenticateUser_WithCorrectCredentials_ShouldReturnTrue()
        {
            // Arrange
            string validLogin = "zborikanton@gmail.com";
            string validPassword = "0666910724";

            // Act
            bool result = _service.AuthenticateUser(validLogin, validPassword);

            // Assert
            Assert.IsTrue(result, "Користувач з коректними даними мав пройти авторизацію.");
        }

        [TestMethod]
        public void AuthenticateUser_WithIncorrectCredentials_ShouldReturnFalse()
        {
            // Arrange
            string invalidLogin = "wrong_user";
            string invalidPassword = "wrong_password";

            // Act
            bool result = _service.AuthenticateUser(invalidLogin, invalidPassword);

            // Assert
            Assert.IsFalse(result, "Користувач з неправильними даними не повинен авторизуватися.");
        }

        [TestMethod]
        public void GetAllShoes_ShouldReturnPopulatedDataTable()
        {
            // Act
            DataTable result = _service.GetAllShoes();

            // Assert
            Assert.IsNotNull(result);
            // Перевіряємо наявність необхідних колонок у схемі таблиці [cite: 23, 24]
            Assert.IsTrue(result.Columns.Contains("item_number"), "Таблиця повинна містити колонку item_number");
            Assert.IsTrue(result.Columns.Contains("name"), "Таблиця повинна містити колонку name");
        }

        [TestMethod]
        public void GetFilteredShoes_ByPriceRange_ShouldReturnOnlyMatchingShoes()
        {
            // Arrange
            int minPrice = 500;
            int maxPrice = 2000;

            // Act
            DataTable filteredData = _service.GetFilteredShoes(null, minPrice, maxPrice, 0);

            // Assert
            Assert.IsNotNull(filteredData);

            foreach (DataRow row in filteredData.Rows)
            {
                decimal price = Convert.ToDecimal(row["price_one_pair"]);
                
                Assert.IsTrue(price >= minPrice, $"Ціна {price} менша за мінімальну {minPrice}");
                Assert.IsTrue(price <= maxPrice, $"Ціна {price} більша за максимальну {maxPrice}");
            }
        }
    }
}