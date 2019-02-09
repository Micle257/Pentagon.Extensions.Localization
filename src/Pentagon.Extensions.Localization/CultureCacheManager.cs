// -----------------------------------------------------------------------
//  <copyright file="CultureCacheManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Options;

    public class CultureCacheManager : ICultureCacheManager
    {
        readonly ILogger<CultureCacheManager> _logger;
        readonly ICultureManager _cultureManager;
        readonly CultureCacheOptions _options;

        readonly ConcurrentDictionary<string, ICultureCache> _caches = new ConcurrentDictionary<string, ICultureCache>(StringComparer.OrdinalIgnoreCase);

        public CultureCacheManager(ILogger<CultureCacheManager> logger,
                                   IOptionsSnapshot<CultureCacheOptions> optionsSnapshot,
                                   ICultureManager cultureManager)
        {
            _logger = logger;
            _cultureManager = cultureManager;
            _options = optionsSnapshot?.Value ?? new CultureCacheOptions();
        }

        public ICultureCache GetCache(CultureInfo culture)
        {
            if (!_caches.TryGetValue(culture.Name, out var cache))
            {
                cache = new CultureCache(culture);

                LoadCacheCoreAsync(cache).Wait();

                _caches.TryAdd(culture.Name, cache);
            }

            return cache;
        }

        public Task LoadCacheAsync(CultureInfo cultureInfo)
        {
            var cache = GetCache(cultureInfo);

            return LoadCacheCoreAsync(cache);
        }

        async Task LoadCacheCoreAsync(ICultureCache cache)
        {
            var resources = await _cultureManager.GetResourcesAsync(cache.Culture);

            cache.SetResources(resources);
        }
    }
}