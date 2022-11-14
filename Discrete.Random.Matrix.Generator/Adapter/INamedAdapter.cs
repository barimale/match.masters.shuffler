using Discrete.Random.Matrix.Generator.DiscreteMatrixDistribution;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    public interface INamedAdapter: IDiscreteUniformDistribution
    {
        string GetUniqueName();
    }
}
