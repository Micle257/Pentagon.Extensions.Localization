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
        public static IServiceCollection AddJsonCultureLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> configure = null)
        {
            services.AddOptions();

            services.Configure<JsonLocalizationOptions>(configure);

            return services.AddCultureCache()
                           .AddJsonCultureStore()
                           .AddJsonCultureManager();
        }

        public static IServiceCollection AddJsonCultureStore(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureStore), ServiceLifetime.Scoped));

            return services;
        }

        public static IServiceCollection AddJsonCultureManager(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureManager), typeof(CultureManager), ServiceLifetime.Scoped));

            return services;
        }
    }
}