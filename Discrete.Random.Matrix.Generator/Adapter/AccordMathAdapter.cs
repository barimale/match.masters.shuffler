using Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    internal class AccordMathAdapter: INamedAdapter
    {
        private const string NAME = "Accord.Math";
        private readonly IDiscreteUniformDistribution _adaptee;

        public AccordMathAdapter()
        {
            _adaptee = new DiscreteUniformAccordMath();
        }

        public string GetUniqueName()
        {
            return NAME;
        }

        public Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper, int n, int m, double[] masses, bool isStrict)
        {
            return _adaptee.ExecuteCategoricalAsync(lower, upper, n, m, masses,isStrict);
        }

        public Task<Tuple<double, double, string, int[]>> ExecuteUniformAsync(int lower, int upper, int n, int m, bool isStrict)
        {
            return _adaptee.ExecuteUniformAsync(lower, upper, n, m, isStrict);
        }
    }
}
