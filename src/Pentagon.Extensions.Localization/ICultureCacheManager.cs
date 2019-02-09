// -----------------------------------------------------------------------
//  <copyright file="ICultureCacheManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System.Globalization;
    using System.Threading.Tasks;

    public interface ICultureCacheManager
    {
        ICultureCache GetCache(CultureInfo culture);
        Task LoadCacheAsync(CultureInfo cultureInfo);
    }
}