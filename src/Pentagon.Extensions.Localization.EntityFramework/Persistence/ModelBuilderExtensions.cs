// -----------------------------------------------------------------------
//  <copyright file="ModelBuilderExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Linq;
    using Entities;
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

        public static ModelBuilder SeedInvariantCultureResourcesKeysFromConventions(this ModelBuilder builder, Type type = null)
        {
            var definitions = LocalizationDefinitionConvention.GetDefinitions(type);

            builder.Entity<CultureEntity>()
                   .HasData(new CultureEntity
                            {
                                    Name       = LocalizationConstants.Invariant,
                                    ActiveFlag = true,
                                    Id         = 1
                            });

            builder.Entity<CultureResourceEntity>()
                   .HasData(definitions.Select(a => new CultureResourceEntity
                                                    {
                                                            CultureId = 1,
                                                            Key = a.Key,
                                                            Value = a.Key
                                                    }));

            return builder;
        }
    }
}