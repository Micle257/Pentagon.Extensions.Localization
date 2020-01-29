// -----------------------------------------------------------------------
//  <copyright file="CultureCacheOptions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Options
{
    public class CultureCacheOptions
    {
        public bool IncludeParentResources { get; set; } = true;

        public int CacheLifespanInSeconds { get; set; } = 8 * 60 * 60;

        public LocalizationNotFoundBehavior IndicateLocalizationValueNotFound { get; set; } = LocalizationNotFoundBehavior.Null;
    }

    public enum LocalizationNotFoundBehavior
    {
        Null,
        Exception,
        Key,
        KeyWithNotFoundIndication
    }
}