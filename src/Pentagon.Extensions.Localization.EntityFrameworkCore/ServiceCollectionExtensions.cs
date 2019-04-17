// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework
{
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFCultureLocalization(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
                services.AddCultureCache()
                        .AddEFCultureStore(serviceLifetime);

        public static IServiceCollection AddEFCultureStore(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureRepository), typeof(CultureStore), serviceLifetime));
            services.Add(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureStore), serviceLifetime));

            return services;
        }
    }
}