using NUnit.Framework;
using System;
using ArithmeticAPI.Core;

namespace ArithmeticAPI.Tests
{
    [TestFixture]
    public class ArithmeticServiceTests
    {
        private ArithmeticService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new ArithmeticService();
        }

        [TestCase(1, 2, 3)]
        [TestCase(-1, -2, -3)]
        [TestCase(0, 5, 5)]
        public void Add_ReturnsSum(double a, double b, double expected)
        {
            Assert.That(_service.Add(a, b), Is.EqualTo(expected).Within(0.0001));
        }

        [TestCase(5, 3, 2)]
        [TestCase(-1, -2, 1)]
        [TestCase(0, 5, -5)]
        public void Subtract_ReturnsDifference(double a, double b, double expected)
        {
            Assert.That(_service.Subtract(a, b), Is.EqualTo(expected).Within(0.0001));
        }

        [TestCase(2, 3, 6)]
        [TestCase(-2, 3, -6)]
        [TestCase(0, 5, 0)]
        public void Multiply_ReturnsProduct(double a, double b, double expected)
        {
            Assert.That(_service.Multiply(a, b), Is.EqualTo(expected).Within(0.0001));
        }

        [TestCase(6, 3, 2)]
        [TestCase(-6, 3, -2)]
        [TestCase(5, 2, 2.5)]
        public void Divide_ReturnsQuotient(double a, double b, double expected)
        {
            Assert.That(_service.Divide(a, b), Is.EqualTo(expected).Within(0.0001));
        }

        [Test]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            Assert.That(() => _service.Divide(5, 0), Throws.TypeOf<DivideByZeroException>());
        }
    }
}
