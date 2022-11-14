using static Discrete.Random.Matrix.Generator.Adapter.ServiceProvider.ProviderConfigurator;

namespace Discrete.Random.Matrix.Generator.Adapter
{
    public static class ServiceProvider
    {
        public static Provider GetProvider()
        {
            INamedAdapter accord = new AccordMathAdaptee();
            INamedAdapter mathnet = new MathNETAdaptee();

            var provider = new ProviderConfigurator()
                .WithAdaptees(accord, mathnet)
                .Configure();

            return provider;
        }

        public class ProviderConfigurator
        {
            private HashSet<INamedAdapter> _adaptees;

            internal ProviderConfigurator()
            {
                this._adaptees = new HashSet<INamedAdapter>();
            }

            public ProviderConfigurator WithAdaptees(params INamedAdapter[] adaptees)
            {
                foreach(var adaptee in adaptees)
                    _adaptees.Add(adaptee);

                return this;
            }

            public ProviderConfigurator WithAdaptee(INamedAdapter adaptee)
            {
                _adaptees.Add(adaptee);

                return this;
            }

            public Provider Configure()
            {
                return new Provider(this._adaptees.ToList().AsReadOnly());
            }

            public class Provider
            {
                private readonly IReadOnlyList<INamedAdapter> _adaptees;

                public Provider(IReadOnlyList<INamedAdapter> adaptees)
                {
                    this._adaptees = adaptees;
                } 

                public IEnumerable<string> GetRegisterAdapteesName()
                {
                    return this._adaptees.Select(p => p.GetUniqueName());
                }

                public async Task<Tuple<double, double, string, int[]>> ExecuteCategoricalByNameAsync(string name, int lower, int upper, int n, int m, double[] masses, bool isStrict)
                {
                    return await this._adaptees
                        .First(p => p.GetUniqueName() == name)
                        .ExecuteCategoricalAsync(lower, upper, n, m, masses,isStrict);
                }

                public async Task<Tuple<double, double, string, int[]>> ExecuteUniformByNameAsync(string name, int lower, int upper, int n, int m, bool isStrict)
                {
                    return await this._adaptees
                    .First(p => p.GetUniqueName() == name)
                        .ExecuteUniformAsync(lower, upper, n, m, isStrict);
                }
            }
        }
    }
}
