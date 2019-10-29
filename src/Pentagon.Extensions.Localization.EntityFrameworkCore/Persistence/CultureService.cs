// -----------------------------------------------------------------------
//  <copyright file="CultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using EntityFrameworkCore.Interfaces.Stores;
    using EntityFrameworkCore.Repositories;
    using EntityFrameworkCore.Specifications;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CultureService : ICultureRetriever, ICultureStore
    {
        [NotNull]
        readonly ILogger<CultureService> _logger;

        [NotNull]
        readonly IStore<CultureEntity> _store;

        [NotNull]
        readonly IStore<CultureResourceEntity> _resourceStore;

        public CultureService([NotNull] ILogger<CultureService> logger,
                              [NotNull] IStore<CultureEntity> store,
                              [NotNull] IStore<CultureResourceEntity> resourceStore)
        {
            _logger = logger;
            _store = store;
            _resourceStore = resourceStore;
        }

        /// <inheritdoc />
        public async Task<CultureResourceEntity> GetOneAsync(string culture, string key, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Retrieving culture resource for culture={culture} and key={key}.");

            try
            {
                var spec = new GetOneSpecification<CultureResourceEntity>();

                spec.AddConfiguration(q => q.Include(a => a.Culture));
                spec.AddFilter(r => r.Key == key && r.Culture.Name == culture);

                var resource = await _resourceStore.GetOneAsync(spec, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (resource != null)
                    return resource;

                _logger.LogWarning($"No resource found for culture={culture} and key={key}.");

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: "Exception occured while reading database.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<CultureResourceEntity>> GetAllAsync(string culture, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Retrieving all culture resources for culture={culture}.");

            try
            {
                var spec = new GetManySpecification<CultureResourceEntity>();

                spec.AddConfiguration(q => q.Include(a => a.Culture));
                spec.AddFilter(r => r.Culture.Name == null || r.Culture.Name == culture);

                var resource = await _resourceStore.GetManyAsync(spec, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (resource?.Any(a => a.Culture.Name != null) == true)
                    return resource;

                _logger.LogWarning($"No resources found for culture={culture}.");

                return Array.Empty<CultureResourceEntity>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: "Exception occured while reading database.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<CultureEntity>> GetCulturesAsync(CancellationToken cancellationToken)
        {
            var spec = new GetManySpecification<CultureEntity>();

            spec.AddConfiguration(q => q.Include(a => a.Resources));
            spec.AddFilter(a => a.ActiveFlag);

            var cultures = await _store.GetManyAsync(spec, cancellationToken: cancellationToken).ConfigureAwait(false);

            return cultures;
        }

        public async Task<CultureEntity> GetCultureAsync(string name, CancellationToken cancellationToken)
        {
            var spec = new GetOneSpecification<CultureEntity>();

            spec.AddConfiguration(q => q.Include(a => a.Resources));
            spec.AddFilter(a => a.Name == name && a.ActiveFlag);

            var culture = await _store.GetOneAsync(spec, cancellationToken: cancellationToken).ConfigureAwait(false);

            return culture;
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<string, string>> GetResourceAsync(string cultureName, string key, CancellationToken cancellationToken)
        {
            var res = await GetOneAsync(cultureName, key, cancellationToken);

            return new KeyValuePair<string, string>(res.Key, res.Value);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<string, string>> GetAllResourcesAsync(string cultureName, CancellationToken cancellationToken)
        {
            var cultureResourceEntities = await GetAllAsync(cultureName, cancellationToken);

            return cultureResourceEntities.ToDictionary(a => a.Key, a => a.Value);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<CultureInfo>> GetAvailableCulturesAsync(CancellationToken cancellationToken)
        {
            var cultures = await GetCulturesAsync(cancellationToken);

            return cultures.Select(a => CultureInfo.GetCultureInfo(a.Name)).ToList();
        }

        /// <inheritdoc />
        async Task<CultureInfo> ICultureStore.GetCultureAsync(string name, CancellationToken cancellationToken)
        {
            var culture = await GetCultureAsync(name, cancellationToken);

            var info = CultureInfo.GetCultureInfo(culture.Name);

            return info;
        }
    }
}