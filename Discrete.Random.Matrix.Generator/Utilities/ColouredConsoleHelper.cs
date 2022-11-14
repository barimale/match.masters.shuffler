namespace Discrete.Random.Matrix.Generator.Utilities
{
    public static class ColouredConsoleHelper
    {
        public static Tuple<string, ConsoleColor> PrintChar(int colorNumber)
        {
            char special = 'ø';

            return Tuple.Create(
                special.ToString(),
                Convert(colorNumber));
        }

        private static ConsoleColor Convert(int colorNumber)
        {
            switch (colorNumber)
            {
                case 0:
                    return ConsoleColor.Green;
                case 1:
                    return ConsoleColor.Blue;
                case 2:
                    return ConsoleColor.Red;
                case 3:
                    return ConsoleColor.Yellow;
                case 4:
                    return ConsoleColor.Magenta;
                case 5:
                    return ConsoleColor.DarkYellow;
                case 6:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.DarkGray;
            }
        }
    }
}
