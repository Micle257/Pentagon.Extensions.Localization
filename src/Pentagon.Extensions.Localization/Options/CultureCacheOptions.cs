// -----------------------------------------------------------------------
//  <copyright file="CultureCacheOptions.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Options
{
    using System;

    public class CultureCacheOptions
    {
        public int CacheLifespanInSeconds { get; set; } = 8 * 60 * 60;
    }
}