using restaurantAPI.Dummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myTest
{
    public class CalculatorTest
    {
        private Calculator _calculator;
        public CalculatorTest()
        {
            _calculator = new Calculator();
            _calculator.a = 10;
            _calculator.b = 20;
        }
        [Fact]
        public void TestAddition()
        {
            var result = _calculator.Add();
            Assert.Equal(30, result);
            Assert.Equal("Addition performed 10 + 20 = 30", _calculator.result);
        }

        [Fact]
        public void TestAdditionFail()
        {
            var result = _calculator.Add();
            Assert.NotEqual(31, result);
            Assert.NotEqual("Addition performed 10 + 20 = 31", _calculator.result);
        }

        [Theory]
        [InlineData(5, 15, 20, "Addition performed 5 + 15 = 20")]
        [InlineData(0, 0, 0, "Addition performed 0 + 0 = 0")]
        [InlineData(-5, 5, 0, "Addition performed -5 + 5 = 0")]
        public void TestAdditionWithParameters(int a, int b, int expectedSum, string expectedResult)
        {
            _calculator.a = a;
            _calculator.b = b;
            var result = _calculator.Add();
            Assert.Equal(expectedSum, result);
            Assert.Equal(expectedResult, _calculator.result);
        }
    }
}
