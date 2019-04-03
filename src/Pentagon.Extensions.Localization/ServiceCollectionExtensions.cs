// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCultureCache(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddMemoryCache()
                    .AddOptions();

            services.AddTransient<ICultureContext, CultureContext>();

            services.Add(ServiceDescriptor.Describe(typeof(ILocalizationCache), typeof(LocalizationCache), serviceLifetime));

            return services;
        }
    }
}