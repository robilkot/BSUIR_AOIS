using LW1;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LW1_Test
{
    [ExcludeFromCodeCoverage]
    public class BinaryFixedUnitTest
    {
        [Fact]
        public void BinaryInteger_InitializeWithEmptyArray_ShouldBePositive()
        {
            BinaryFixed integer = new(new BitArray(5, false));

            var result = integer.Positive();

            Assert.True(result);
        }

        [Fact]
        public void BinaryInteger_InitializeEmpty_ShouldBePositive()
        {
            BinaryFixed integer = new();

            var result = integer.Positive();

            Assert.True(result);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(-25)]
        public void BinaryInteger_InitializeWithNumber_DecimalRepresentationShouldEqual(int input)
        {
            BinaryFixed integer = new(input);

            var result = integer.ToDecimal();

            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(0, 5, 5)]
        [InlineData(5, 0, 5)]
        [InlineData(-13, 25, 12)]
        [InlineData(25, -13, 12)]
        [InlineData(13, -25, -12)]
        [InlineData(-25, 13, -12)]
        [InlineData(13, 25, 38)]
        [InlineData(25, 13, 38)]
        [InlineData(-13, -25, -38)]
        [InlineData(-25, -13, -38)]
        [InlineData(-55, 55, 0)]
        [InlineData(55, -55, 0)]
        public void BinaryInteger_Addition_ResultShouldEqualExpected(int val1, int val2, int result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var integer3 = integer1 + integer2;

            Assert.Equal(result, integer3.ToDecimal());
        }

        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(5, 0, 0)]
        [InlineData(-13, 25, -325)]
        [InlineData(25, -13, -325)]
        [InlineData(13, -25, -325)]
        [InlineData(-25, 13, -325)]
        [InlineData(13, 25, 325)]
        [InlineData(25, 13, 325)]
        [InlineData(-13, -25, 325)]
        [InlineData(-25, -13, 325)]
        [InlineData(12, 10, 120)]
        [InlineData(10, 12, 120)]
        public void BinaryInteger_Multiplication_ResultShouldEqualExpected(int val1, int val2, int result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var integer3 = integer1 * integer2;

            Assert.Equal(result, integer3.ToDecimal());
        }

        [Theory]
        [InlineData(0, 5, false)]
        [InlineData(5, 0, false)]
        [InlineData(0, 0, true)]
        [InlineData(2, 2, true)]
        [InlineData(-2, -2, true)]
        public void BinaryInteger_Equals_ResultShouldEqualExpected(int val1, int val2, bool result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var actual = integer1 == integer2;

            Assert.Equal(result, actual);
        }

        [Theory]
        [InlineData(0, 5, false)]
        [InlineData(5, 0, true)]
        [InlineData(0, 0, false)]
        [InlineData(2, 2, false)]
        [InlineData(-2, -3, true)]
        [InlineData(-3, -2, false)]
        public void BinaryInteger_Greater_ResultShouldEqualExpected(int val1, int val2, bool result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var actual = integer1 > integer2;

            Assert.Equal(result, actual);
        }

        [Theory]
        [InlineData(0, 5, true)]
        [InlineData(5, 0, false)]
        [InlineData(0, 0, false)]
        [InlineData(2, 2, false)]
        [InlineData(-2, -3, false)]
        [InlineData(-3, -2, true)]
        public void BinaryInteger_Less_ResultShouldEqualExpected(int val1, int val2, bool result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var actual = integer1 < integer2;

            Assert.Equal(result, actual);
        }

        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(5, 2, 2.5)]
        [InlineData(20, 3, 6.66666)]
        [InlineData(6, 3, 2)]
        public void BinaryInteger_Division_ResultShouldEqualExpected(int val1, int val2, double result)
        {
            BinaryFixed integer1 = new(val1);
            BinaryFixed integer2 = new(val2);

            var integer3 = integer1 / integer2;

            Assert.Equal(result, Math.Round(integer3.ToDecimal(), 5));
        }

        [Fact]
        public void BinaryInteger_DivisionByZero_ThrowsDivisionByZeroException()
        {
            BinaryFixed integer1 = new(5);
            BinaryFixed integer2 = new(0);

            Assert.Throws<DivideByZeroException>(() =>
            {
                var integer3 = integer1 / integer2; 
            });
        }
    }
}