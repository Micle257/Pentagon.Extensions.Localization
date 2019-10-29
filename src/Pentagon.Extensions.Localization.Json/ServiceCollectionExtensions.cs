// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Json
{
    using System;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IServiceCollection AddJsonCultureLocalization([NotNull] this IServiceCollection services, Action<JsonLocalizationOptions> configure = null)
        {
            services.AddOptions();

            services.Configure<JsonLocalizationOptions>(configure ?? (o => { }));

            return services.AddCulture()
                           .AddJsonCultureStore();
        }

        [NotNull]
        static IServiceCollection AddJsonCultureStore([NotNull] this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureStore), ServiceLifetime.Scoped));

            return services;
        }
    }
}