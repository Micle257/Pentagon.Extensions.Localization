// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IServiceCollection AddCulture([NotNull] this IServiceCollection services)
        {
            services.AddMemoryCache()
                    .AddOptions();

            services.AddScoped<ICultureContext, CultureContext>();
            services.AddScoped<ICultureContextWriter>(c => c.GetRequiredService<ICultureContext>() as CultureContext);

            services.Add(ServiceDescriptor.Describe(typeof(ILocalizationCache), typeof(LocalizationCache), ServiceLifetime.Transient));
            services.Add(ServiceDescriptor.Describe(typeof(ICultureManager), typeof(CultureManager), ServiceLifetime.Scoped));

            return services;
        }
    }
}