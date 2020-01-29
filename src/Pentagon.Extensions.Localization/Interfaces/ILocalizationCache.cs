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

    [PublicAPI]
    public interface ILocalizationCache
    {
        string this[[NotNull] string key] { get; }

        string this[[NotNull] string key, [NotNull] params object[] formatArguments] { get; }

        Task<IDictionary<string, string>> GetAllAsync(string cultureName, Func<string, bool> keyPredicate = null);

        string ForceCacheUpdate([NotNull] string key);

        bool Contains([NotNull] string key);

        ILocalizationCache WithCulture([NotNull] CultureInfo culture);

        string GetValue([NotNull] string key, params object[] formatArguments);
    }
}