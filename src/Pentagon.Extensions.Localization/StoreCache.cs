// -----------------------------------------------------------------------
//  <copyright file="StoreCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class StoreCache<TEntity> : IStoreCache<TEntity>
    {
        readonly IMemoryCache _cache;
        readonly MemoryCacheEntryOptions _cacheOptions;
        readonly string _cacheKey = typeof(TEntity).Name;

        protected StoreCache(IMemoryCache cache)
        {
            _cache = cache;

            _cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TEntity>> GetCachedAsync()
        {
            var value = _cache.Get<IReadOnlyList<TEntity>>(_cacheKey) ?? await ReloadAsync();

            return value;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TEntity>> ReloadAsync()
        {
            var value = await GetDataAsync();

            _cache.Set(_cacheKey, value, _cacheOptions);

            return value;
        }

        protected abstract Task<IReadOnlyList<TEntity>> GetDataAsync();
    }
}