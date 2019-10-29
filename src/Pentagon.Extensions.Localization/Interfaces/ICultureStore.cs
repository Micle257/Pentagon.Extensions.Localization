// -----------------------------------------------------------------------
//  <copyright file="ICultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICultureStore
    {
        Task<KeyValuePair<string, string>> GetResourceAsync(string cultureName, string key, CancellationToken cancellationToken = default);

        Task<IReadOnlyDictionary<string, string>> GetAllResourcesAsync(string cultureName, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CultureInfo>> GetAvailableCulturesAsync(CancellationToken cancellationToken = default);

        Task<CultureInfo> GetCultureAsync(string name, CancellationToken cancellationToken = default);
    }
}