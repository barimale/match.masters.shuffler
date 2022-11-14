using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using System;

namespace Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution
{
    public class DiscreteUniformDistributionMathNet : IDiscreteUniformDistribution
    {

        private readonly System.Random _randomSource;
        public DiscreteUniformDistributionMathNet()
        {
            _randomSource = SystemRandomSource.Default;
        }

        public async Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper , int n, int m, double[] masses, bool isStrict)
        {
            if (DiscreteUniform.IsValidParameterSet(lower, upper) == false)
                throw new Exception();

            if (upper - lower + 1 != masses.Length)
                throw new Exception("Masses length needs to be equal: upper - lower + 1");

            var cd = new Categorical(masses, _randomSource);

            var length = n * m;
            int[] vector = new int[length];

            cd.Samples(vector);

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

            var du = new DiscreteUniform(lower, upper, _randomSource);

            var length = n * m;
            int[] vector = new int[length];

            du.Samples(vector);

            var mean = du.Mean;
            var median = du.Median;

            ShufflerHelper.ShuffleWithNeighberhood(ref vector, n, m, isStrict);

            return Tuple.Create(
                mean,
                median,
                "Discrete Uniform Distribution",
                vector);
        }
    }
}