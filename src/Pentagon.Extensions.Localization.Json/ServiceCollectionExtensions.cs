// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework
{
    using System;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonCultureLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> configure = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddOptions();

            services.Configure<JsonLocalizationOptions>(configure);

            return services.AddCultureCache(serviceLifetime)
                           .AddJsonCultureStore(serviceLifetime)
                           .AddJsonCultureManager(serviceLifetime);
        }

        public static IServiceCollection AddJsonCultureStore(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureStore), serviceLifetime));

            return services;
        }

        public static IServiceCollection AddJsonCultureManager(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureManager), typeof(CultureManager), serviceLifetime));

            return services;
        }
    }
}