using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiagonalMatrixClassLibrary
{
    public class DiagonalMatrix
    {
        public const int MatrixSize = 16;

        private readonly bool[,] _matrix = new bool[MatrixSize, MatrixSize];

        public List<int> SumAB(bool[] key)
        {
            List<int> result = [];

            for (int i = 0; i < MatrixSize; i++)
            {
                var currentWord = GetWord(i);

                if (currentWord[0] == key[0] &&
                    currentWord[1] == key[1] &&
                    currentWord[2] == key[2])
                {
                    var A = currentWord[3..(6 + 1)];
                    var B = currentWord[7..(10 + 1)];
                    var S = BinarySum(A, B);

                    for (int j = 11, si = 0; j < MatrixSize; j++, si++)
                    {
                        currentWord[j] = S[si];
                    }
                    SetWord(i, currentWord);

                    result.Add(i);
                }
            }

            return result;
        }

        private static bool[] BinarySum(bool[] arr1, bool[] arr2)
        {
            var result = new bool[arr1.Length + 1];
            bool carry = false;

            for (int i = arr1.Length - 1; i >= 0; i--)
            {
                bool sum = arr1[i] ^ arr2[i] ^ carry;
                carry = (arr1[i] & arr2[i]) | (arr2[i] & carry) | (arr1[i] & carry);

                result[i + 1] = sum;
            }

            result[0] = carry;

            return result;
        }

        private void SetValue(int i, int j, bool value)
        {
            var iIndex = i % MatrixSize;
            var jIndex = j % MatrixSize;
            _matrix[iIndex, jIndex] = value;
        }

        private bool GetValue(int i, int j)
        {
            var iIndex = i % MatrixSize;
            var jIndex = j % MatrixSize;
            return _matrix[iIndex, jIndex];
        }

        public void SetWord(int position, bool[] word)
        {
            for (var i = position; i < position + MatrixSize; i++)
            {
                SetValue(i, position, word[i - position]);
            }
        }

        public bool[] GetWord(int position)
        {
            var word = new bool[MatrixSize];

            for (int i = position, j = 0; i < position + MatrixSize; i++, j++)
            {
                word[j] = GetValue(i, position);
            }

            return word;
        }

        public void SetAddressColumn(int position, bool[] address)
        {
            for (int i = position, j = 0; i < MatrixSize; i++, j++)
            {
                SetValue(i, j, address[j]);
            }
        }

        public bool[] GetAddressColumn(int position)
        {
            var address = new bool[MatrixSize];

            for (int i = position, j = 0; i < position + MatrixSize; i++, j++)
            {
                address[j] = GetValue(i, j);
            }

            return address;
        }

        public bool[] ApplyFunction(int positionWord1, int positionWord2, int resultPosition, Func<bool, bool, bool> function)
        {
            var word1 = GetWord(positionWord1);
            var word2 = GetWord(positionWord2);

            var resultWord = ApplyFunction(word1, word2, function);

            SetWord(resultPosition, resultWord);

            return resultWord;
        }

        private static bool[] ApplyFunction(bool[] word1, bool[] word2, Func<bool, bool, bool> function)
        {
            var result = new bool[MatrixSize];

            for (int i = 0; i < word1.Length; i++)
            {
                result[i] = function(word1[i], word2[i]);
            }

            return result;
        }
        public static bool GetG(bool g, bool a, bool s, bool l) => g || (!a && s && !l);
        public static bool GetL(bool g, bool a, bool s, bool l) => l || (a && !s && !g);

        public List<bool[]> FindInRange(bool[] min, bool[] max)
        {
            if (Compare(max, min) == -1)
            {
                return FindInRange(max, min);
            }

            List<bool[]> words = new(MatrixSize);

            for (int i = 0; i < MatrixSize; i++)
            {
                words.Add(GetWord(i));
            }

            var wordsLessThanMin = words.Where(word => Compare(word, min) == -1);
            var wordsGreaterThanMax = words.Where(word => Compare(word, max) == 1);

            var toRemove = wordsGreaterThanMax.Concat(wordsLessThanMin).ToList();

            foreach(var word in toRemove)
            {
                words.Remove(word);
            }

            return words;
        }

        // -1 <
        // 0  =
        // 1  >
        public static int Compare(bool[] s, bool[] a)
        {
            (var g, var l) = (false, false);

            for (int i = 0; i < s.Length; i++)
            {
                g = GetG(g, a[i], s[i], l);
                l = GetL(g, a[i], s[i], l);
            }

            return
                g && !l ? 1 :
                !g && l ? -1 : 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new(MatrixSize * MatrixSize * 2);

            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int k = 0; k < _matrix.GetLength(1); k++)
                {
                    builder.Append(_matrix[i, k] ? "1 " : "0 ");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
