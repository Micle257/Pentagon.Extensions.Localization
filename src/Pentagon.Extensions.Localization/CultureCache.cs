// -----------------------------------------------------------------------
//  <copyright file="CultureCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;

    public class CultureCache : ICultureCache
    {
        readonly ConcurrentDictionary<string, string> _resources = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public CultureCache(CultureInfo culture)
        {
            Culture = culture;
        }

        /// <inheritdoc />
        public DateTimeOffset? CachedAt { get; private set; }

        public CultureInfo Culture { get; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Resources => _resources;

        /// <inheritdoc />
        public bool IsLoaded => CachedAt != null;

        /// <inheritdoc />
        public string this[string key]
        {
            get
            {
                if (!CachedAt.HasValue)
                {
                    var errorText = "Culture cache is not loaded.";
                    var ex = new InvalidOperationException(errorText); // TODO custom exception
                    //_logger.LogError(errorText, ex);
                    throw ex;
                }

                if (!Resources.TryGetValue(key, out var value))
                    return key;

                return value;
            }
        }

        /// <inheritdoc />
        public void SetResources(IDictionary<string, string> data)
        {
            _resources.Clear();

            foreach (var resource in data)
                _resources.TryAdd(resource.Key, resource.Value);

            CachedAt = DateTimeOffset.Now;
        }

        public bool HasResource(string key) => _resources.ContainsKey(key);
    }
}