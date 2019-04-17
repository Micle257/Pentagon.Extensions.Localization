// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using EntityFramework;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCultureCache(this IServiceCollection services)
        {
            services.AddMemoryCache()
                    .AddOptions();

            services.AddScoped<ICultureContext, CultureContext>();

            services.Add(ServiceDescriptor.Describe(typeof(ILocalizationCache), typeof(LocalizationCache), ServiceLifetime.Transient));
            services.Add(ServiceDescriptor.Describe(typeof(ICultureManager), typeof(CultureManager), ServiceLifetime.Scoped));

            return services;
        }
    }
}