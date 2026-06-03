using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoeStoreManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManager.Tests
{
    [TestClass]
    public class ShoeTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_ShouldCreateShoeSuccessfully()
        {
            // Arrange & Act
            var shoe = new Shoe("Ч001", "Кросівки білі", 10, 1500.50m);

            // Assert
            Assert.AreEqual("Ч001", shoe.Article);
            Assert.AreEqual("Кросівки білі", shoe.Name);
            Assert.AreEqual(10, shoe.Count);
            Assert.AreEqual(1500.50m, shoe.Price);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow(null)]
        public void Article_WhenNullOrWhitespace_ShouldThrowArgumentException(string invalidArticle)
        {
            string validName = "Взуття";
            int validCount = 5;
            decimal validPrice = 100m;

            Action makeInvalidShoe = () => new Shoe(invalidArticle, validName, validCount, validPrice);

            ArgumentException thrownException = Assert.ThrowsException<ArgumentException>(makeInvalidShoe);

            Assert.IsNotNull(thrownException, "Помилка ArgumentException не була викликана!");


            Assert.AreEqual("Артикул не може бути порожнім.", thrownException.Message);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow(null)]
        public void Name_WhenNullOrWhitespace_ShouldThrowArgumentException(string invalidName)
        {
            string validArticle = "П002";
            int validCount = 5;
            decimal validPrice = 100m;

            Action makeShoeWithInvalidName = () => new Shoe(validArticle, invalidName, validCount, validPrice);

            ArgumentException thrownException = Assert.ThrowsException<ArgumentException>(makeShoeWithInvalidName);

            Assert.IsNotNull(thrownException, "Помилка ArgumentException не була викликана для некоректної назви!");

            Assert.AreEqual("Назва не може бути порожньою.", thrownException.Message);
        }

        [TestMethod]
        public void Count_WhenNegative_ShouldThrowArgumentException()
        {
            string validArticle = "Д001";
            string validName = "Туфлі";
            int negativeCount = -1;
            decimal validPrice = 850m;

            Action makeShoeWithNegativeCount = () => new Shoe(validArticle, validName, negativeCount, validPrice);

            ArgumentException thrownException = Assert.ThrowsException<ArgumentException>(makeShoeWithNegativeCount);

            Assert.IsNotNull(thrownException, "Помилка ArgumentException не була викликана при від'ємній кількості!");

            Assert.AreEqual("Кількість не може бути від'ємною.", thrownException.Message);
        }

        [TestMethod]
        public void Price_WhenNegative_ShouldThrowArgumentException()
        {
            string validArticle = "Д001";
            string validName = "Туфлі";
            int validCount = 10;
            decimal negativePrice = -50.25m; // Спеціально беремо некоректну ціну

            Action makeShoeWithNegativePrice = () => new Shoe(validArticle, validName, validCount, negativePrice);

            ArgumentException thrownException = Assert.ThrowsException<ArgumentException>(makeShoeWithNegativePrice);

            Assert.IsNotNull(thrownException, "Помилка ArgumentException не була викликана при від'ємній ціні!");

            Assert.AreEqual("Ціна не може бути від'ємною.", thrownException.Message);
        }
    }
}