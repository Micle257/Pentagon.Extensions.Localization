// -----------------------------------------------------------------------
//  <copyright file="CultureHelpers.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Extensions.Localization {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using JetBrains.Annotations;

    // TODO will be in Common
    public static class CultureHelpers
    {
        [NotNull]
        static readonly HashSet<string> CultureNames = CreateCultureNames();

        public static bool Exists(string name) => CultureNames.Contains(name);

        [Pure]
        [NotNull]
        [ItemNotNull]
        static HashSet<string> CreateCultureNames()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
                                          .Where(x => !string.IsNullOrEmpty(x?.Name))
                                          .ToArray();

            var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            allNames.UnionWith(cultureInfos.Select(x => x?.TwoLetterISOLanguageName));
            allNames.UnionWith(cultureInfos.Select(x => x?.Name));

            return allNames;
        }

        public static bool TryParse(string culture, out CultureInfo cultureInfo)
        {
            if (Exists(culture))
            {
                cultureInfo = CultureInfo.GetCultureInfo(culture);
                return true;
            }

            cultureInfo = null;
            return false;
        }
    }
}