using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;
using Dew.Math;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution
{
    public class DiscreteUniformDewMath : IDiscreteUniformDistribution
    {

        private readonly System.Random _randomSource;
        public DiscreteUniformDewMath()
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
            var v = new Dew.Math.VectorInt(length);
            var asMatrix = new Dew.Math.Matrix(n, m, false, false);
            var result = asMatrix.RandGauss(lower, upper);
            //Statistics.UniformDFit fitter ?
            //Statistics.LatinHyperCubeDesign
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

            var asMatrix = new Dew.Math.Matrix(n, m, false, false);
            var result = asMatrix.RandUniform(lower, upper);

            var length = n * m;
            int[] vector = new int[length];
            double[] forMedian = new double[length];

            result.CopyToArray(ref vector, TRounding.rnRound);//WIP rounding - new thing

            var rescaledVector = vector
                .Select(p => p + lower)
                .ToArray();

            for (int i = 0; i < rescaledVector.Length; i++)
            {
                forMedian[i] = (double)rescaledVector[i];
            }

            var median = forMedian.Median(type: QuantileMethod.Default);
            var mean = forMedian.Mean();
            //double mean = -1D;
            //double stdDev = -1D;

            //result.MeanAndStdDev(ref mean, ref stdDev);

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