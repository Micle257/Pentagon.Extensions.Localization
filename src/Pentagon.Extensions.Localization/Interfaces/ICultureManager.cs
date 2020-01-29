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
    using JetBrains.Annotations;

    public interface ICultureManager
    {
        [NotNull]
        Task<IReadOnlyDictionary<string, string>> GetResourcesAsync([NotNull] CultureInfo culture, bool includeParentResources = true);

        [NotNull]
        Task<CultureObject> GetCultureAsync([NotNull] CultureInfo culture);
    }
}