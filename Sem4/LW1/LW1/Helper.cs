using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LW1
{
    [ExcludeFromCodeCoverage]
    public static class Helper
    {
        public static void ShowDetails(BinaryFixed a)
        {
            var directCodeString = a.DirectCode().ToConsoleString();
            var invertCodeString = a.InvertCode().ToConsoleString();
            var additionalCodeString = a.AdditionalCode().ToConsoleString();

            var precision = a.Precision;
            if (precision != 0)
            {
                directCodeString = directCodeString.Insert(a.BitCount - precision, ".");
                invertCodeString = invertCodeString.Insert(a.BitCount - precision, ".");
                additionalCodeString = additionalCodeString.Insert(a.BitCount - precision, ".");
            }

            Console.WriteLine(
                $"Dec:\t{a.ToDecimal()}\n" +
                $"Dir:\t{directCodeString}\n" +
                $"Inv:\t{invertCodeString}\n" +
                $"Add:\t{additionalCodeString}\n"
                );
        }
        public static void ShowDetails(BinaryFloat a)
        {
            Console.WriteLine(
                $"Dec:\t{a.ToFloat()}\n" +
                $"Bit:\t{a.ToBitArray().ToConsoleString()}\n"
                );
        }

        public static int ReadInt()
        {
            while (true)
            {
                Console.Write("Inp:\t");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var result))
                {
                    return result;
                }
            }
        }

        public static float ReadFloat()
        {
            while (true)
            {
                Console.Write("Inp:\t");
                var input = Console.ReadLine();

                if (float.TryParse(input, out var result))
                {
                    return result;
                }
            }
        }


        public static string ToConsoleString(this BitArray array)
        {
            StringBuilder result = new(array.Length);

            for(int k = 0; k < array.Length / 8; k++)
            {
            for (int i = 0; i <= 7; i++)
            {
                result.Append(array[i + k * 8] ? '1' : '0');
            }
            }

            return result.ToString();
        }
    }
}