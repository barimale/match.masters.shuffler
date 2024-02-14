using Discrete.Random.Matrix.Generator.Adapter;
using Discrete.Random.Matrix.Generator.Utilities;
using Xunit.Abstractions;

namespace Random.Matrix.Generator
{
    public class As_A_Developer
    {
        private readonly ITestOutputHelper output;

        public As_A_Developer(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(4,4,0,3, false)]
        [InlineData(2, 2, 0, 1, false)]
        [InlineData(9, 9, 0, 4, false)]
        public async Task Generate_matrix_n_x_m_with_two_colors(int n, int m, int lower, int upper, bool isStrict)
        {
            //given
            var amountOfColors = upper - lower + 1;
            var provider = ServiceProvider.GetProvider();
            var allAdaptees = provider.GetRegisterAdaptersName();
            var firstOne = allAdaptees.First();

            //when
            var result = await provider.ExecuteUniformByNameAsync(firstOne, lower, upper, n, m, isStrict);
            double[] masses = new double[upper - lower + 1];
            for (int i = 0; i < masses.Length; i++)
            {
                masses[i] = (double)(1D / (double)masses.Length);
            }

            var result2 = await provider.ExecuteCategoricalByNameAsync(firstOne, lower, upper,n, m, masses, isStrict);

            //then
            var array = result.Item4.ToMatrixString(n, m);
            var array2 = result2.Item4.ToMatrixString(n, m);

            output.WriteLine(array);
            output.WriteLine(array2);

        }
    }
}