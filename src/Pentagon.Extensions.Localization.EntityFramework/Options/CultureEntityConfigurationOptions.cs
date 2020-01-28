// -----------------------------------------------------------------------
//  <copyright file="CultureEntityConfigurationOptions.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Options
{
    public class CultureEntityConfigurationOptions
    {
        public string TablePrefix { get; set; }

        public string CultureTableName { get; set; } = DatabaseObjectNames.Culture;

        public string CultureResourceTableName { get; set; } = DatabaseObjectNames.CultureResource;
    }
}