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
    using JetBrains.Annotations;

    public interface ILocalizationCache
    {
        string this[[NotNull] string key] { get; }

        string this[[NotNull] string key, [NotNull] params object[] formatArguments] { get; }

        Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null);

        string ForceCacheUpdate(string key);

        bool Contains(string key);

        ILocalizationCache WithCulture(CultureInfo culture);
    }
}