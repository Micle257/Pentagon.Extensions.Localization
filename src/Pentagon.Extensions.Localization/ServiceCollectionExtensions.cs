// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCultureCache(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureCache), typeof(CultureCache), serviceLifetime));

            return services;
        }
    }
}