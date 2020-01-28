// -----------------------------------------------------------------------
//  <copyright file="ModelBuilderExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Options;

    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddLocalizationConfigurations(this ModelBuilder builder, CultureEntityConfigurationOptions options = null)
        {
            builder.ApplyConfiguration(new CultureEntityConfiguration(options))
                   .ApplyConfiguration(new CultureResourceEntityConfiguration(options));

            return builder;
        }
    }
}