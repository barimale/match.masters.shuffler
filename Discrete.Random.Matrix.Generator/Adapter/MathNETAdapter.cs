using Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    internal class MathNETAdapter : INamedAdapter
    {
        private const string NAME = "MathNet.Numerics";
        private readonly IDiscreteUniformDistribution _adaptee;

        public MathNETAdapter()
        {
            _adaptee = new DiscreteUniformDistributionMathNet();
        }

        public string GetUniqueName()
        {
            return NAME;
        }

        public async Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper, int n, int m, double[] masses, bool isStrict)
        {
            return await _adaptee.ExecuteCategoricalAsync(lower, upper, n, m, masses,isStrict);
        }

        public async Task<Tuple<double, double, string, int[]>> ExecuteUniformAsync(int lower, int upper, int n, int m, bool isStrict)
        {
            return await _adaptee.ExecuteUniformAsync(lower, upper, n, m, isStrict);
        }
    }
}
