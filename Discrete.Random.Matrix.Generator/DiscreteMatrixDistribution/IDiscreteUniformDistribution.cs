namespace Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution
{
    public interface IDiscreteUniformDistribution
    {
        Task<Tuple<double, double, string, int[]>> ExecuteCategoricalAsync(int lower, int upper, int n, int m, double[] masses, bool isStrict);
        Task<Tuple<double, double, string, int[]>> ExecuteUniformAsync(int lower, int upper, int n, int m, bool isStrict);
    }
}