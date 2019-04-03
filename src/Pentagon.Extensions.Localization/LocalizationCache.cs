// -----------------------------------------------------------------------
//  <copyright file="LocalizationCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Options;

    class LocalizationCache : ILocalizationCache
    {
        const string CacheKeyPrefix = "LOCALIZATION_";

        readonly ICultureStore _store;
        readonly IMemoryCache _cache;
        readonly ICultureContext _context;
        readonly MemoryCacheEntryOptions _cacheOptions;

        CultureInfo _culture;

        public LocalizationCache(ICultureStore store,
                                 IMemoryCache cache,
                                 ICultureContext context,
                                 IOptionsSnapshot<CultureCacheOptions> optionsSnapshot)
        {
            _store = store;
            _cache = cache;
            _context = context;

            var options = optionsSnapshot?.Value ?? new CultureCacheOptions();
            _cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(options.CacheLifespanInSeconds));

            _culture = context.UICulture;
        }

        /// <inheritdoc />
        public string this[string key]
        {
            get
            {
                var value = _cache.Get<string>(GetKeyName(key)) ?? ForceCacheUpdate(key);

                return value;
            }
        }

        public string ForceCacheUpdate(string key)
        {
            var value = _store.GetResourceAsync(_culture.Name, key)?.Result.Value;

            if (value == null)
                return null;

            _cache.Set(GetKeyName(key), _cacheOptions);

            return value;
        }

        /// <inheritdoc />
        public async Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null)
        {
            var all = await _store.GetAllResourcesAsync(cultureName);

            var result = new Dictionary<string, string>();

            foreach (var entity in all)
            {
                if (keyPredicate?.Invoke(entity.Key) == false)
                    continue;

                var inCacheValue = _cache.Get<string>(GetKeyName(entity.Key));

                if (inCacheValue == null)
                    inCacheValue = _cache.Set(GetKeyName(entity.Key), entity.Value, _cacheOptions);

                result.Add(entity.Key, inCacheValue);
            }

            return result;
        }

        /// <inheritdoc />
        public bool Contains(string key) => this[key] != null;

        /// <inheritdoc />
        public ILocalizationCache WithCulture(CultureInfo culture)
        {
            _culture = culture;

            return this;
        }

        string GetKeyName(string key) => $"{CacheKeyPrefix}{_culture.Name}_{key}";
    }
}