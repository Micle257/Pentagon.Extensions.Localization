// -----------------------------------------------------------------------
//  <copyright file="ActualityStoreCache.cs">
//   Copyright (c) Smartdata s. r. o. All Rights Reserved.
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
        string CacheKey = typeof(TEntity).Name;
        
        readonly IMemoryCache _cache;
        readonly MemoryCacheEntryOptions _cacheOptions;

        protected StoreCache(IMemoryCache cache)
        {
            _cache = cache;

            _cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TEntity>> GetCachedAsync()
        {
            var value = _cache.Get<IReadOnlyList<TEntity>>(CacheKey) ?? await ReloadAsync();

            return value;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TEntity>> ReloadAsync()
        {
            var value = await GetDataAsync();

            _cache.Set(CacheKey, value, _cacheOptions);

            return value;
        }

        protected abstract Task<IReadOnlyList<TEntity>> GetDataAsync();
    }
}