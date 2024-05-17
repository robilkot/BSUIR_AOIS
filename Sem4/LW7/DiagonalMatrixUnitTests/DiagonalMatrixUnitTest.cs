using DiagonalMatrixClassLibrary;
using System.Diagnostics.CodeAnalysis;

namespace DiagonalMatrixUnitTests
{
    [ExcludeFromCodeCoverage]
    public class DiagonalMatrixUnitTest
    {
        private readonly bool[] word2 = [false, true, false, true, true, true, false, false, true, true, true, true, true, true, true, true];
        private readonly bool[] word3 = [true, true, false, true, true, true, false, false, true, true, true, true, true, true, true, false];
        private readonly bool[] word9 = [false, true, false, false, true, false, true, false, false, true, false, true, false, true, false, true];

        [Fact]
        void GetWord()
        {
            var matrix = new DiagonalMatrix();
            matrix.SetWord(9, word9);

            Assert.Equal(word9, matrix.GetWord(9));
        }

        [Fact]
        void ApplyFunction()
        {
            var matrix = new DiagonalMatrix();
            matrix.SetWord(2, word2);
            matrix.SetWord(3, word3);

            Assert.Equal("0 0 1 0 0 0 1 1 0 0 0 0 0 0 0 0 ", matrix.ApplyFunction(2, 3, 15, Functions.KVPairs[FunctionsSet.F8]).ToRowString());
        }

        [Fact]
        void SumAB()
        {
            var matrix = new DiagonalMatrix();
            matrix.SetWord(2, word2);
            matrix.SetWord(3, word3);
            matrix.SetWord(9, word9);

            List<int> replaceResult = matrix.SumAB([false, true, false]);

            var word = matrix.GetWord(replaceResult[0]);

            Assert.Equal("0 1 0 1 1 1 0 0 1 1 1 1 0 1 0 1 ", word.ToRowString());
        }

        [Fact]
        void FindInRange()
        {
            var matrix = new DiagonalMatrix();
            matrix.SetWord(2, word2);
            matrix.SetWord(3, word3);
            matrix.SetWord(9, word9);

            bool[] a = [false, true, false, false, true, false, true, false, true, true, false, true, false, true, false, true];
            bool[] b = [true, true, false, true, true, true, false, false, true, true, true, true, true, true, true, false];

            var wordsInRange = matrix.FindInRange(a, b);

            Assert.Equal(wordsInRange[0], word2);
        }

        [Fact]
        void ToString_DoesNotThrow()
        {
            var matrix = new DiagonalMatrix();

            _ = matrix.ToString();
        }
    }
}