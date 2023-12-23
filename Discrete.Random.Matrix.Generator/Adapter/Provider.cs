using static Discrete.Random.Matrix.Generator.Adapter.ServiceProvider.ProviderConfigurator;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    public static class ServiceProvider
    {
        public static Provider GetProvider()
        {
            INamedAdapter accord = new AccordMathAdapter();
            INamedAdapter mathnet = new MathNETAdapter();

            var provider = new ProviderConfigurator()
                .WithAdaptees(accord, mathnet)
                .Configure();

            return provider;
        }

        public class ProviderConfigurator
        {
            private HashSet<INamedAdapter> _adapters;

            internal ProviderConfigurator()
            {
                this._adapters = new HashSet<INamedAdapter>();
            }

            public ProviderConfigurator WithAdaptees(params INamedAdapter[] adaptees)
            {
                foreach(var adaptee in adaptees)
                    _adapters.Add(adaptee);

                return this;
            }

            public ProviderConfigurator WithAdaptee(INamedAdapter adaptee)
            {
                _adapters.Add(adaptee);

                return this;
            }

            public Provider Configure()
            {
                return new Provider(this._adapters.ToList().AsReadOnly());
            }

            public class Provider
            {
                private readonly IReadOnlyList<INamedAdapter> _adapters;

                public Provider(IReadOnlyList<INamedAdapter> adapters)
                {
                    this._adapters = adapters;
                } 

                public IEnumerable<string> GetRegisterAdaptersName()
                {
                    return this._adapters.Select(p => p.GetUniqueName());
                }

                public async Task<Tuple<double, double, string, int[]>> ExecuteCategoricalByNameAsync(string name, int lower, int upper, int n, int m, double[] masses, bool isStrict)
                {
                    return await this._adapters
                        .First(p => p.GetUniqueName() == name)
                        .ExecuteCategoricalAsync(lower, upper, n, m, masses,isStrict);
                }

                public async Task<Tuple<double, double, string, int[]>> ExecuteUniformByNameAsync(string name, int lower, int upper, int n, int m, bool isStrict)
                {
                    return await this._adapters
                        .First(p => p.GetUniqueName() == name)
                        .ExecuteUniformAsync(lower, upper, n, m, isStrict);
                }
            }
        }
    }
}
