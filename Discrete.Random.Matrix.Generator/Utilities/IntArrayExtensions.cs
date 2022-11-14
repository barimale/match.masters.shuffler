using System.Text;

namespace Discrete.Random.Matrix.Generator.Utilities
{
    public static class IntArrayExtensions
    {
        public static string ToMatrixString(this int[] vector, int rowsAmount, int columnsAmount)
        {
            var builder = new StringBuilder();

            for(int ii = 0; ii < columnsAmount; ii++)
            {
                var subvector = vector.Skip(ii * rowsAmount).Take(rowsAmount);
                foreach(var item in subvector)
                {
                    builder.Append(item);
                    builder.Append(' ');
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static List<Tuple<int, int, string, ConsoleColor>> ToSpecialWithColor(this int[] vector, int rowsAmount, int columnsAmount)
        {
            var list = new List<Tuple<int, int, string, ConsoleColor>>();

            for (int ii = 0; ii < columnsAmount; ii++)
            {
                var subvector = vector.Skip(ii * rowsAmount).Take(rowsAmount);
                var counter = 0;
                foreach (var item in subvector)
                {
                    var (special, color) = ColouredConsoleHelper.PrintChar(item);
                    list.Add(Tuple.Create(ii, counter, special, color));
                }
                list.Add(Tuple.Create(-1, -1, string.Empty, ConsoleColor.Black));
            }

            return list;
        }
    }
}
