using LeanTest.Core.ExecutionHandling;
using Microsoft.Extensions.DependencyInjection;

namespace Orders.L2Tests.TestSetup.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterStateHandler<TImplementation, TData>(this IServiceCollection container)
            where TImplementation : class, IStateHandler<TData> =>
            container.AddSingleton<TImplementation>()
                .AddSingleton<IStateHandler<TData>>(x => x.GetRequiredService<TImplementation>());

        public static IServiceCollection WithInterface<TImplementation, TInterface>(this IServiceCollection container)
            where TImplementation : class, TInterface
            where TInterface : class =>
            container.AddSingleton<TInterface>(x => x.GetRequiredService<TImplementation>());

        public static IServiceCollection WithDataForStateHandler<TImplementation, TData>(this IServiceCollection container)
            where TImplementation : class, IStateHandler<TData> =>
            container.WithInterface<TImplementation, IStateHandler<TData>>();
    }
}