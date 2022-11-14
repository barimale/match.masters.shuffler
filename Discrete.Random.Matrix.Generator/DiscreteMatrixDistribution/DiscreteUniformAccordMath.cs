using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution
{
    public class DiscreteUniformAccordMath : IDiscreteUniformDistribution
    {

        private readonly System.Random _randomSource;
        public DiscreteUniformAccordMath()
        {
            _randomSource = SystemRandomSource.Default;
        }

        public async Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper, int n, int m, double[] masses, bool isStrict)
        {
            if (DiscreteUniform.IsValidParameterSet(lower, upper) == false)
                throw new Exception();

            if (upper - lower + 1 != masses.Length)
                throw new Exception("Masses length needs to be equal: upper - lower + 1");

            var cd = new GeneralDiscreteDistribution(masses); // double check if it is the same as categorical

            var length = n * m;
            int[] vector = new int[length];

            cd.Generate(length, vector, _randomSource);

            var mean = cd.Mean;
            var median = cd.Median;

            var rescaledVector = vector
                .Select(p => p + lower)
                .ToArray();

            ShufflerHelper.ShuffleWithNeighberhood(ref rescaledVector, n, m, isStrict);

            return Tuple.Create(
                mean,
                median,
                "Discrete Categorical Distribution",
                rescaledVector
            );
        }

        public async Task<Tuple<double, double, string, int[]>> ExecuteUniformAsync(int lower, int upper, int n, int m, bool isStrict)
        {
            if (DiscreteUniform.IsValidParameterSet(lower, upper) == false)
                throw new Exception();

            var c = upper - lower + 1;
            double[] masses = new double[c];
            for (int i = 0; i < masses.Length; i++)
            {
                masses[i] = (double)(1D / (double)c);
            }
            //var du = new UniformDiscreteDistribution(lower, upper+1); bug?
            var du = new GeneralDiscreteDistribution(masses);

            var length = n * m;
            int[] vector = new int[length];

            du.Generate(length, vector, _randomSource);

            var mean = du.Mean;
            var median = du.Median;

            var rescaledVector = vector
                .Select(p => p + lower)
                .ToArray();

            ShufflerHelper.ShuffleWithNeighberhood(ref rescaledVector, n, m, isStrict);

            return Tuple.Create(
                mean,
                median,
                "Discrete Uniform Distribution",
                rescaledVector
            );
        }
    }
}