// -----------------------------------------------------------------------
//  <copyright file="CultureCacheStringLocalizer.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Interfaces;
    using Microsoft.Extensions.Localization;
    
    public class CultureCacheStringLocalizer : IStringLocalizer
    {
        readonly ILocalizationCache _cultureCacheManager;

        public CultureCacheStringLocalizer(ILocalizationCache cultureCacheManager)
        {
            _cultureCacheManager = cultureCacheManager;
        }

        public CultureInfo Culture { get; private set; } = CultureInfo.CurrentUICulture;

        /// <inheritdoc />
        public virtual LocalizedString this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                var value = GetStringSafely(name, null);

                return new LocalizedString(name, value ?? name, value == null, searchedLocation: "DB");
            }
        }

        /// <inheritdoc />
        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException(nameof(name));

                var format = GetStringSafely(name, null);
                var value = string.Format(format ?? name, arguments);

                return new LocalizedString(name, value, format == null, searchedLocation: "DB");
            }
        }

        /// <summary> Creates a new <see cref="CultureCacheStringLocalizer" /> for a specific <see cref="CultureInfo" />. </summary>
        /// <param name="culture"> The <see cref="CultureInfo" /> to use. </param>
        /// <returns> A culture-specific <see cref="CultureCacheStringLocalizer" />. </returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
            => new CultureCacheStringLocalizer(_cultureCacheManager) {Culture = culture};

        /// <inheritdoc />
        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
                GetAllStrings(includeParentCultures, Culture);

        /// <summary> Returns all strings in the specified culture. </summary>
        /// <param name="includeParentCultures"> </param>
        /// <param name="culture"> The <see cref="CultureInfo" /> to get strings for. </param>
        /// <returns> The strings. </returns>
        protected IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            var cache = _cultureCacheManager.GetAllAsync(culture.Name).Result;

            var resourceNames = cache.Select(a => a.Key);

            foreach (var name in resourceNames)
            {
                var value = GetStringSafely(name, culture);

                yield return new LocalizedString(name, value ?? name, value == null, searchedLocation: "DB");
            }
        }

        string GetStringSafely(string name, CultureInfo culture)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (culture == null)
                culture = Culture;

            return _cultureCacheManager.WithCulture(culture)[name];
        }
    }
}