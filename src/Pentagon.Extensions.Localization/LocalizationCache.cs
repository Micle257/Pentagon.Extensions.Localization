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
    using System.Threading;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Options;
    using Threading;

    public class LocalizationContext<T>
    {
        public LocalizationContext(T value)
        {
            Value = value;
        }
        public T Value { get;  }
    }
    
    public class LocalizationCache : ILocalizationCache
    {
        const string CacheKeyPrefix = "LOCALIZATION_";

        const string LocalizationNotFoundPrefix = "LOCALIZATION_NOT_FOUND__";

        [NotNull]
        SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);

        [NotNull]
        readonly ICultureStore _store;

        [NotNull]
        readonly IMemoryCache _cache;

        [NotNull]
        readonly ICultureManager _manager;

        [NotNull]
        readonly MemoryCacheEntryOptions _cacheOptions;

        [NotNull]
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
                           .SetSlidingExpiration(TimeSpan.FromSeconds(_options.CacheLifespanInSeconds))
                           .RegisterPostEvictionCallback((key, value, reason, state) =>
                                                         {
                                                             if (reason != EvictionReason.Replaced)
                                                             {

                                                             }
                                                         });

            _culture = context.UICulture;
        }

        string GetFromCache([NotNull] string key)
        {
            var entryKey = GetKeyName(key);

            if (!_cache.TryGetValue<string>(entryKey, out var cacheValue))
            {
                return null;
            }

            return cacheValue;
        }

        string SetToCache([NotNull] string key, string value)
        {
            var entryKey = GetKeyName(key);

            if (_cache.TryGetValue<string>(entryKey, out var cacheValue))
            {
                return cacheValue;
            }

            var setValue = _cache.Set(entryKey, value, _cacheOptions);

            return setValue;
        }

        /// <inheritdoc />
        public async ValueTask<string> GetValueAsync(string key, params object[] formatArguments)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            await _semaphore.WaitAsync().ConfigureAwait(false);

            // value task because of hot-path to cached values
            var value = GetFromCache(key) ?? await ForceCacheUpdateAsync(key).ConfigureAwait(false);                                                       

            if (value == null)
                return GetNotFoundValue(key);

            if (formatArguments == null || formatArguments.Length == 0)
                return value;

            var formattedValue = string.Format(value, formatArguments);

            return formattedValue;
        }

        /// <inheritdoc />
        public async ValueTask<LocalizationContext<T>> CreateContextAsync<T>()
        {
            var instance = await LocalizationDefinitionConvention.CreateLocalizationInstanceAsync(typeof(T), key => GetValueAsync(key)).ConfigureAwait(false);

            return new LocalizationContext<T>((T) instance);
        }

        /// <inheritdoc />
        public LocalizationContext<T> CreateContext<T>()
        {
            var instance = LocalizationDefinitionConvention.CreateLocalizationInstance(typeof(T), s =>
                                                                                                  {
                                                                                                      var fromCache= GetFromCache(s);

                                                                                                      return fromCache ?? GetNotFoundValue(s);
                                                                                                  });

            return new LocalizationContext<T>((T)instance);
        }

        string GetNotFoundValue([NotNull] string key)
        {
            switch (_options.IndicateLocalizationValueNotFound)
            {
                case LocalizationNotFoundBehavior.Exception:
                    throw new LocalizationNotFoundException(key, _culture);
                case LocalizationNotFoundBehavior.Key:
                    return key;
                case LocalizationNotFoundBehavior.KeyWithNotFoundIndication:
                    return LocalizationNotFoundPrefix + key;
                default:
                    return null;
            }
        }

        public async Task<string> ForceCacheUpdateAsync(string key)
        {
            if (_options.IncludeParentResources)
            {
                var all = await _manager.GetResourcesAsync(_culture).ConfigureAwait(false);

                foreach (var pair in all)
                    SetToCache(pair.Key, pair.Value);

                return GetFromCache(key);
            }

            var value = (await _store.GetResourceAsync(_culture.Name, key).ConfigureAwait(false)).Value;

            if (value == null)
                return GetNotFoundValue(key);

            SetToCache(key, value);

            return value;
        }

        /// <inheritdoc />
        public async Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null)
        {
            cultureName??=_culture.Name;

            if (!CultureHelper.TryParse(cultureName, out var culture))
                throw new FormatException($"Culture is invalid '{cultureName}'.");

            await _semaphore.WaitAsync().ConfigureAwait(false);

            var all = await _manager.GetResourcesAsync(culture, _options != null && _options.IncludeParentResources).ConfigureAwait(false);

            var result = new Dictionary<string, string>();

            foreach (var entity in all)
            {
                if (keyPredicate?.Invoke(entity.Key) == false)
                    continue;

                var inCacheValue = GetFromCache(entity.Key) ?? SetToCache(entity.Key, entity.Value);

                result.Add(entity.Key, inCacheValue);
            }

            return result;
        }

        /// <inheritdoc />
        public ILocalizationCache WithCulture(CultureInfo culture)
        {
            _culture = culture;

            return this;
        }

        string GetKeyName(string key) => $"{CacheKeyPrefix}{_culture.Name}_{key}";

        /// <inheritdoc />
        public string this[string key] => GetValueAsync(key).ConfigureAwait(false).GetAwaiter().GetResult();

        /// <inheritdoc />
        public bool Contains(string key) => this[key] != null;

        /// <inheritdoc />
        public string this[string key, params object[] formatArguments]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (formatArguments == null)
                    throw new ArgumentNullException(nameof(formatArguments));

                return GetValueAsync(key, formatArguments).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}