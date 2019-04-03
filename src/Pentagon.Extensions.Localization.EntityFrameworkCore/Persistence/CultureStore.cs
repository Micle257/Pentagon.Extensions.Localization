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
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CultureStore : ICultureStore, ICultureRepository
    {
        readonly ILogger<CultureStore> _logger;
        readonly ICultureApplicationContext _applicationContext;

        public CultureStore(ILogger<CultureStore> logger,
                            ICultureApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        /// <inheritdoc />
        public async Task<CultureResourceEntity> GetOneAsync(string culture, string key)
        {
            _logger.LogDebug($"Retrieving culture resource for culture={culture} and key={key}.");

            try
            {
                var resource = await _applicationContext.CultureResources
                                                        .Include(a => a.Culture)
                                                        .FirstOrDefaultAsync(r => r.Key == key && r.Culture.Name == culture);

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
        public async Task<IReadOnlyList<CultureResourceEntity>> GetAllAsync(string culture)
        {
            _logger.LogDebug($"Retrieving all culture resources for culture={culture}.");

            try
            {
                var resource = await _applicationContext.CultureResources
                                                        .Include(a => a.Culture)
                                                        .Where(r => r.Culture.Name == null || r.Culture.Name == culture)
                                                        .ToListAsync();

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
        public async Task<IReadOnlyList<CultureEntity>> GetCulturesAsync()
        {
            return await _applicationContext.Cultures.Include(a => a.Resources).Where(a => a.ActiveFlag).ToListAsync();
        }

        public Task<CultureEntity> GetCultureAsync(string name)
        {
            return _applicationContext.Cultures.Include(a => a.Resources)
                                      .FirstOrDefaultAsync(a => a.Name == name && a.ActiveFlag);
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<string, string>> GetResourceAsync(string cultureName, string key)
        {
            var res = await GetOneAsync(cultureName, key);

            return new KeyValuePair<string, string>(res.Key, res.Value);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<string, string>> GetAllResourcesAsync(string cultureName)
        {
            var cultureResourceEntities = await GetAllAsync(cultureName);

            return cultureResourceEntities.ToDictionary(a => a.Key, a => a.Value);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<CultureInfo>> GetAvailableCulturesAsync()
        {
            var cultures = await GetCulturesAsync();

            return cultures.Select(a => CultureInfo.GetCultureInfo(a.Name)).ToList();
        }

        /// <inheritdoc />
        async Task<CultureInfo> ICultureStore.GetCultureAsync(string name)
        {
            var culture = await GetCultureAsync(name);

            var info = CultureInfo.GetCultureInfo(culture.Name);

            return info;
        }
    }
}