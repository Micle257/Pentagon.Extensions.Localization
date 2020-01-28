﻿// -----------------------------------------------------------------------
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
    using Helpers;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Options;
    using Threading;

    public class LocalizationCache : ILocalizationCache
    {
        const string CacheKeyPrefix = "LOCALIZATION_";

        [NotNull]
        readonly ICultureStore _store;

        [NotNull]
        readonly IMemoryCache _cache;

        [NotNull]
        readonly ICultureManager _manager;

        readonly MemoryCacheEntryOptions _cacheOptions;

        readonly CultureCacheOptions _options;

        [NotNull]
        CultureInfo _culture;

        public LocalizationCache([NotNull] ICultureStore store,
                                 [NotNull] IMemoryCache cache,
                                 [NotNull] ICultureContext context,
                                 [NotNull] ICultureManager manager,
                                 IOptionsSnapshot<CultureCacheOptions> optionsSnapshot)
        {
            _store   = store ?? throw new ArgumentNullException(nameof(store));
            _cache   = cache ?? throw new ArgumentNullException(nameof(cache));
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));

            _options = optionsSnapshot?.Value ?? new CultureCacheOptions();
            _cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(_options.CacheLifespanInSeconds));

            _culture = context.UICulture;
        }

        /// <inheritdoc />
        public string this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                var value = _cache.Get<string>(GetKeyName(key)) ?? ForceCacheUpdate(key);

                return value;
            }
        }

        /// <inheritdoc />
        public string this[string key, params object[] formatArguments]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (formatArguments == null)
                    throw new ArgumentNullException(nameof(formatArguments));

                var value = this[key];

                if (value != null)
                    value = string.Format(value, formatArguments);

                return value;
            }
        }

        public string ForceCacheUpdate(string key)
        {
            if (_options.IncludeParentResources)
            {
                var all = _manager.GetResourcesAsync(_culture).AwaitSynchronously();

                foreach (var pair in all)
                    _cache.Set(GetKeyName(pair.Key), pair.Value, _cacheOptions);

                return _cache.Get<string>(GetKeyName(key));
            }

            var value = _store.GetResourceAsync(_culture.Name, key)?.AwaitSynchronously().Value;

            if (value == null)
                return null;

            _cache.Set(GetKeyName(key), value, _cacheOptions);

            return value;
        }

        /// <inheritdoc />
        public async Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null)
        {
            if (!CultureHelper.TryParse(cultureName, out var culture))
                throw new FormatException($"Culture is invalid '{cultureName}'.");

            var all = await _manager.GetResourcesAsync(culture, _options != null && _options.IncludeParentResources).ConfigureAwait(false);

            var result = new Dictionary<string, string>();

            foreach (var entity in all)
            {
                if (keyPredicate?.Invoke(entity.Key) == false)
                    continue;

                var inCacheValue = _cache.Get<string>(GetKeyName(entity.Key)) ?? _cache.Set(GetKeyName(entity.Key), entity.Value, _cacheOptions);

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