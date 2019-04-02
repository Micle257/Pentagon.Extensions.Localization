// -----------------------------------------------------------------------
//  <copyright file="CultureCacheOptions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Options
{
    public class CultureCacheOptions
    {
        public int CacheLifespanInSeconds { get; set; } = 8 * 60 * 60;
    }
}