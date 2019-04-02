// -----------------------------------------------------------------------
//  <copyright file="ICultureStore.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    public interface ICultureStore
    {
        Task<KeyValuePair<string, string>> GetResourceAsync(string cultureName, string key);

        Task<IReadOnlyDictionary<string, string>> GetAllResourcesAsync(string cultureName);

        Task<IReadOnlyList<CultureInfo>> GetAvailableCulturesAsync();
    }
}