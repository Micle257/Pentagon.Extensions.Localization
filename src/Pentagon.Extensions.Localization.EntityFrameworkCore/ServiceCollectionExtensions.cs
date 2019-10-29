// -----------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework
{
    using Entities;
    using EntityFrameworkCore.Extensions;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Persistence;

    public static class ServiceCollectionExtensions
    {
        [NotNull]
        public static IServiceCollection AddEFCultureLocalization([NotNull] this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddCulture();

            services.AddStoreTransient<CultureEntity>();
            services.AddStoreTransient<CultureResourceEntity>();

            services.TryAdd(ServiceDescriptor.Describe(typeof(ICultureRetriever), typeof(CultureService), serviceLifetime));
            services.TryAdd(ServiceDescriptor.Describe(typeof(ICultureStore), typeof(CultureService), serviceLifetime));

            return services;
        }
    }
}