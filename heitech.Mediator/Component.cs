using heitech.Mediator;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PerdixComponent
    {
        ///<summary>
        /// Adds a scan for all Perdix Outlets and Mediator to the serviceCollection
        ///</summary>
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.Scan
            (
                scan =>
                 scan.FromApplicationDependencies()
                        .AddClasses(classes => classes.AssignableTo<IOutlet>())
                            .AsImplementedInterfaces()
                            .WithTransientLifetime()

                        .AddClasses(classes => classes.AssignableTo(typeof(IOutlet<>)))
                            .AsImplementedInterfaces()
                            .WithTransientLifetime()
            );

            services.AddTransient<ISentinel, SentinelNullObject>();
            services.AddScoped<IMediator, Mediator2>();

            return services;
        }

        public static IServiceCollection AddSentinel<T>(this IServiceCollection services)
            where T : ISentinel
        {
            var descriptor = new ServiceDescriptor(typeof(ISentinel), typeof(T), ServiceLifetime.Transient);
            services.Replace(descriptor);

            return services;
        }
    }
}