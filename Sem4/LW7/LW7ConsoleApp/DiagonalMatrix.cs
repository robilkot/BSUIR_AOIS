using System.Text;

namespace LW7ConsoleApp
{
    public class DiagonalMatrix
    {
        public const int MatrixSize = 16;

        private readonly bool[,] _matrix = new bool[MatrixSize, MatrixSize];

        public bool[] GetWord(int wordNumber)
        {
            if (wordNumber >= MatrixSize)
            {
                throw new IndexOutOfRangeException();
            }

            bool[] word = new bool[MatrixSize];
            
            // todo: wrap
            for (var i = wordNumber; i < MatrixSize; i++)
            {
                word[i - wordNumber] = _matrix[i, wordNumber];
            }
            for (var i = 0; i < wordNumber; i++)
            {
                word[MatrixSize - wordNumber + i] = _matrix[i, wordNumber];
            }

            return word;
        }

        public void SetWord(int wordNumber, bool[] word)
        {
            if (wordNumber >= MatrixSize)
            {
                throw new IndexOutOfRangeException();
            }

            if (word.Length != MatrixSize)
            {
                throw new ArgumentException("Word is too long");
            }

            // todo: wrap
            for (var i = wordNumber; i < MatrixSize; i++)
            {
                _matrix[i, wordNumber] = word[i - wordNumber];
            }

            for (int i = 0; i < wordNumber; i++)
            {
                _matrix[i, wordNumber] = word[MatrixSize - wordNumber + i];
            }
        }


        public void SetDiogonalColumn(int columnNumber, bool[] column)
        {
            if (columnNumber >= MatrixSize)
            {
                throw new IndexOutOfRangeException();
            }

            for (int col = 0; col < MatrixSize - columnNumber; col++)
                _matrix[columnNumber + col, col] = column[col];

            for (int col = MatrixSize - columnNumber, i = 0; col < MatrixSize; col++, i++)
                _matrix[i, col] = column[MatrixSize - columnNumber + i];
        }

        public bool[] GetDiogonalColumn(int columnNumber)
        {
            if (columnNumber >= MatrixSize)
            {
                throw new IndexOutOfRangeException();
            }

            bool[] column = new bool[MatrixSize];

            for (int col = 0; col < MatrixSize - columnNumber; col++)
                column[col] = _matrix[columnNumber + col, col];
            for (int col = MatrixSize - columnNumber, i = 0; col < MatrixSize; col++, i++)
                column[MatrixSize - columnNumber + i] = _matrix[i, col];
            return column;
        }

        public bool[] SumAB(bool k0, bool k1, bool k2)
        {
            bool[] sumAB = new bool[MatrixSize];

            for (int i = 0; i < MatrixSize; i++)
            {
                bool[] Iword = GetWord(i);
                if (Iword[0] == k0 && Iword[1] == k1 && Iword[2] == k2)
                {
                    bool[] A = [Iword[3], Iword[4], Iword[5], Iword[6]];
                    bool[] B = [Iword[7], Iword[8], Iword[9], Iword[10]];

                    bool[] S = SumBinaryArrays(A, B);

                    int si = 0;

                    for (int j = 11; j < MatrixSize; j++)
                    {
                        Iword[j] = S[si];
                        si++;
                    }
                    SetWord(i, Iword);
                }
            }
            return sumAB;
        }

        public bool[] ExecuteFunction(int columnNumber1, int columnNumber2, int rezColumnNumber, int functionNumber)
        {
            if (columnNumber1 < 0 || columnNumber1 > 15 || columnNumber2 < 0 || columnNumber2 > 15 || rezColumnNumber < 0 || rezColumnNumber > 15)
                throw new ArgumentException("Входные параметры должны находиться в диапазоне от 0 до 15.");
            if (columnNumber1 == columnNumber2 || columnNumber1 == rezColumnNumber || columnNumber2 == rezColumnNumber)
                throw new ArgumentException("Входные параметры не могут быть равны друг другу.");
            if (functionNumber < 0 || functionNumber > 15)
                throw new ArgumentException("Номер функциидолжны находиться в диапазоне от 0 до 15.");
            bool[] result = new bool[MatrixSize];

            // todo: my functions
            Type type = this.GetType();
            string fname = "f" + functionNumber.ToString();
            var method = type.GetMethod(fname);
            if (method == null)
            {
                throw new ArgumentException("Такая функция не определена" + fname);
            }

            bool[] A = GetWord(columnNumber1);
            bool[] B = GetWord(columnNumber2);

            for (int i = 0; i < MatrixSize; i++)
            {

                result[i] = (bool)method.Invoke(this, new object[] { A[i], B[i] });
            }
            SetWord(rezColumnNumber, result);
            return result;
        }


        private static bool[] SumBinaryArrays(bool[] arr1, bool[] arr2)
        {

            bool[] result = new bool[arr1.Length + 1];
            bool carry = false;

            for (int i = arr1.Length - 1; i >= 0; i--)
            {
                bool bit1 = arr1[i];
                bool bit2 = arr2[i];

                bool sum = bit1 ^ bit2 ^ carry;
                carry = (bit1 & bit2) | (bit2 & carry) | (bit1 & carry);

                result[i + 1] = sum;
            }

            result[0] = carry;

            return result;
        }

        public List<int> FindSame(bool?[] key)
        {
            List<int> rezKeys = new List<int>();

            if (key.Length != MatrixSize)
            {
                throw new ArgumentException("Ключ долен состоять из 16 эллементов.");
            }
            for (int j = 0; j < MatrixSize; j++)
            {
                bool[] checkWord = GetWord(j);
                bool same = true;
                for (int i = 0; i < MatrixSize; i++)
                {
                    if (key[i] == null || checkWord[i] == key[i])
                    {
                        continue;
                    }
                    else
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
                    rezKeys.Add(j);
            }
            return rezKeys;
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
