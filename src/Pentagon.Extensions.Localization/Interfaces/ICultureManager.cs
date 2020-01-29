// -----------------------------------------------------------------------
//  <copyright file="ICultureManager.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    public interface ICultureManager
    {
        Task<IReadOnlyDictionary<string, string>> GetResourcesAsync(CultureInfo culture, bool includeParentResources = true);

        Task<CultureObject> GetCultureAsync(CultureInfo culture);
    }
}