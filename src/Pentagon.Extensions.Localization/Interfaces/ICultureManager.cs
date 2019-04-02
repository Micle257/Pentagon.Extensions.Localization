// -----------------------------------------------------------------------
//  <copyright file="ICultureManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    public interface ICultureManager
    {
        Task<IDictionary<string, string>> GetResourcesAsync(CultureInfo culture);

        Task<CultureObject> GetCultureAsync(CultureInfo culture);
    }
}