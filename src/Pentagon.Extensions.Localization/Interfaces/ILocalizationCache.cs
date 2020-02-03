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
    public interface ILocalizationCache  : ILocalizationAccessor
    {
        ValueTask<string> GetValueAsync([NotNull] string key, params object[] formatArguments);

        Task<IDictionary<string, string>> GetAllAsync(string cultureName = null, Func<string, bool> keyPredicate = null);

        Task<string> ForceCacheUpdateAsync([NotNull] string key);

        ILocalizationCache WithCulture([NotNull] CultureInfo culture);

        ValueTask<LocalizationContext<T>> CreateContextAsync<T>();

        LocalizationContext<T> CreateContext<T>();
    }

    [PublicAPI]
    public interface ILocalizationAccessor
    {
        string this[[NotNull] string key] { get; }

        string this[[NotNull] string key, [NotNull] params object[] formatArguments] { get; }

        bool Contains([NotNull] string key);
    }
}