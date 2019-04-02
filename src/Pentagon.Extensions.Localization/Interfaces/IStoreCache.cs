// -----------------------------------------------------------------------
//  <copyright file="IStoreCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStoreCache<TEntity>
    {
        Task<IReadOnlyList<TEntity>> GetCachedAsync();

        Task<IReadOnlyList<TEntity>> ReloadAsync();
    }
}