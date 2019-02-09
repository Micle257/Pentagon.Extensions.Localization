// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework
{
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFCultureStore(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureStore), serviceLifetime));

            return services;
        }

        public static IServiceCollection AddEFCultureManager(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureManager), typeof(CultureManager), serviceLifetime));

            return services;
        }
    }
}