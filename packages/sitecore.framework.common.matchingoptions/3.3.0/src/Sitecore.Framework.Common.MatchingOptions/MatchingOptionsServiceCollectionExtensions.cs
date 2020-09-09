using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Framework.Common.MatchingOptions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MatchingOptionsServiceCollectionExtensions
    {
        public static IServiceCollection AddMatchingOptions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton(typeof(IMatchingOptions<>), typeof(MatchingOptionsManager<>)));
            return services;
        }

        public static IServiceCollection ConfigureMatchingOptions<TOptions, TConfigure>(this IServiceCollection services)
            where TConfigure : class, IConfigureMatchingOptions<TOptions>
        {
            services.AddTransient<IConfigureMatchingOptions<TOptions>, TConfigure>();
            return services;
        }

        public static IServiceCollection ConfigureMatchingOptions<TOptions>(this IServiceCollection services,
            IConfigureMatchingOptions<TOptions> matchingOptionsConfig)
        {
            services.AddSingleton<IConfigureMatchingOptions<TOptions>>(matchingOptionsConfig);
            return services;
        }

        public static IServiceCollection ConfigureMatching<TOptions>(this IServiceCollection services,
            string matchIdentifier,
            Predicate<string> match,
            Func<TOptions, TOptions> setup)
        {
            services.ConfigureMatchingOptions(new ConfigureMatchingOptions<TOptions>(matchIdentifier, match, setup));
            return services;
        }

        public static IServiceCollection ConfigureMatching<TOptions>(this IServiceCollection services,
            string matchIdentifier,
            Predicate<string> match,
            Action<TOptions> setup)
        {
            return services.ConfigureMatching<TOptions>(matchIdentifier, match, o => { setup(o); return o; });
        }

        // Replicates MS' original named options feature...
        public static IServiceCollection ConfigureMatching<TOptions>(this IServiceCollection services,
            Action<TOptions> setup,
            string optionsName)
        {
            return services.ConfigureMatching<TOptions>(optionsName.ToLower(), name => name.Equals(optionsName), o => { setup(o); return o; });
        }
    }
}
