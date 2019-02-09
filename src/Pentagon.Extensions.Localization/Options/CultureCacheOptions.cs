// -----------------------------------------------------------------------
//  <copyright file="CultureCacheOptions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Options
{
    using System;

    public class CultureCacheOptions
    {
        public bool AutoReload { get; set; } = true;

        public TimeSpan? CacheLifespan { get; set; } = null;
    }
}