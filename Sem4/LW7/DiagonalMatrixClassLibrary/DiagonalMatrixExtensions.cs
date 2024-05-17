using System.Text;

namespace DiagonalMatrixClassLibrary
{
    public static class DiagonalMatrixExtensions
    {
        public static string ToRowString(this bool[] array)
        {
            StringBuilder builder = new(array.Length);

            foreach (bool b in array)
            {
                builder.Append(b ? "1 " : "0 ");
            }

            return builder.ToString();
        }
    }
}
