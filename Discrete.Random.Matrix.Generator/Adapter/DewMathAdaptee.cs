using Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    internal class DewMathAdaptee: INamedAdapter
    {
        private const string NAME = "Dew.Math.Core";
        private readonly IDiscreteUniformDistribution _engine;

        public DewMathAdaptee()
        {
            _engine = new DiscreteUniformDewMath();
        }

        public string GetUniqueName()
        {
            return NAME;
        }

        public Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper, int n, int m, double[] masses, bool isStrict)
        {
            return _engine.ExecuteCategoricalAsync(lower, upper, n, m, masses,isStrict);
        }

        public Task<Tuple<double, double, string, int[]>> ExecuteUniformAsync(int lower, int upper, int n, int m, bool isStrict)
        {
            return _engine.ExecuteUniformAsync(lower, upper, n, m, isStrict);
        }
    }
}
