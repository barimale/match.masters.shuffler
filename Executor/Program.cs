using Discrete.Random.Matrix.Generator.Adapter;
using Discrete.Random.Matrix.Generator.Utilities;

//given
var rowsAmount = 9;
var columnsAmount = 9;
var lower = 0;
var upper = 5;
var isStrict = true;
var amountOfColors = upper - lower + 1;
double[] masses = new double[amountOfColors];

for (int i = 0; i < masses.Length; i++)
{
    masses[i] = (double)(1D/ (double)amountOfColors);
}

// Match Masters
// on perpouse two weights are modified
masses[1] = (double)(masses[1] / 3.1D);
masses[2] = (double)(masses[2] / 3.1D);
var rest = 1D - masses[1] - masses[2];
for (int i = 0; i < masses.Length; i++)
{
    if (i == 1 || i == 2)
        continue;
    masses[i] = (double)(rest / (double)amountOfColors);
}

var provider = ServiceProvider.GetProvider();
var allAdaptees = provider.GetRegisterAdapteesName();

foreach (var adapteeName in allAdaptees)
{
    //when
    var fromCategorical = await provider.ExecuteCategoricalByNameAsync(adapteeName, lower, upper, rowsAmount, columnsAmount, masses, isStrict);
    var fromUniform = await provider.ExecuteUniformByNameAsync(adapteeName, lower, upper, rowsAmount, columnsAmount, isStrict);

    //then
    // Categorical
    var array = fromCategorical.Item4.ToMatrixString(rowsAmount, columnsAmount);
    var points = fromCategorical.Item4.ToSpecialWithColor(rowsAmount, columnsAmount);
    var name = fromCategorical.Item3;
    var mean = fromCategorical.Item1;
    var median = fromCategorical.Item2;

    DisplayResultsOf(
        adapteeName,
        array,
        points,
        name,
        mean.ToString(),
        median.ToString(),
        false);

    PrintLegend(lower, upper, masses: masses);

    PrintLineBreaks(1);

    // Uniform
    var array2 = fromUniform.Item4.ToMatrixString(rowsAmount, columnsAmount);
    var points2 = fromUniform.Item4.ToSpecialWithColor(rowsAmount, columnsAmount);
    var name2 = fromUniform.Item3;
    var mean2 = fromUniform.Item1;
    var median2 = fromUniform.Item2;

    DisplayResultsOf(
        adapteeName,
        array2,
        points2,
        name2,
        mean2.ToString(),
        median2.ToString(),
        false);

    PrintLegend(lower, upper, "All masses are uniformed(1/n, where n = masses count)");

    if(allAdaptees.Last() != null && allAdaptees.Last() != adapteeName) PrintLineBreaks(3);
}

Console.ResetColor();
Console.ReadKey();

static void DisplayResultsOf(string adapteeName, string array, List<Tuple<int, int, string, ConsoleColor>> points, string name, string mean, string median, bool withNunmberArrayVisible)
{
    Console.ResetColor();
    Console.WriteLine(name.ToUpperInvariant());
    Console.WriteLine("Multicolor array:");
    Console.WriteLine("Adaptee name: " + adapteeName);
    Console.WriteLine("Mean: " + mean);
    Console.WriteLine("Median: " + median);
    Console.WriteLine();

    points.ForEach(p =>
    {
        if (p.Item1 < 0)
        {
            Console.WriteLine();
        }
        else
        {
            Console.ForegroundColor = p.Item4;
            Console.Write(p.Item3 + ' ', p.Item4);
        }
    });

    Console.WriteLine();
    if(withNunmberArrayVisible)
    {
        Console.ResetColor();
        Console.WriteLine("Array:");
        Console.WriteLine();
        Console.WriteLine(array);
    }
}

static void PrintLegend(int lower, int upper,string? additionalInfo = null, double[]? masses = null)
{
    Console.ResetColor();
    Console.WriteLine(additionalInfo != null ? $"Legend({additionalInfo}):" : "Legend");

    for (int i = lower; i < upper + 1; i++)
    {
        var (special, color) = ColouredConsoleHelper.PrintChar(i);
        var descriptionRow = masses != null ?
            special + " - " + i + " with mass: " + masses[i - lower].ToString()
            : special + " - " + i;

        Console.ForegroundColor = color;
        Console.WriteLine(descriptionRow);
    }
}

static void PrintLineBreaks(int count)
{
    if (count > 3) throw new Exception("Max count supported is equal to 3 inclusive.");

    for(int i = count; i > 0; i--)
    {
        Console.WriteLine(i % 2 == 0 ? "--------------------------------------------------" : "");
    }
}