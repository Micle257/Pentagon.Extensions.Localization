// -----------------------------------------------------------------------
//  <copyright file="ILocalizationCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    public interface ILocalizationCache
    {
        string this[string key] { get; }

        Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null);

        string ForceCacheUpdate(string key);

        bool Contains(string key);

        ILocalizationCache WithCulture(CultureInfo culture);
    }
}