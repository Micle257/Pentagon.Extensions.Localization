// -----------------------------------------------------------------------
//  <copyright file="ModelBuilderExtensions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Collections.Tree;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Options;

    public static class CultureDataSeedHelper
    {
        public static string GetInsertScript<T>(int cultureId, string table)
        {
            var hier = LocalizationDefinitionConvention.GetInstanceHierarchy(typeof(T));

            var keys = hier.Where(a => a.IsLeafNode()).Select(a => a.Value.Definition.Key);

            var commands = new List<string>();

            foreach (var key in keys)
            {
                var sql = $"INSERT INTO {table} (CultureId, Key, Value) VALUES ({cultureId}, {key}, NULL)";

                commands.Add(sql);
            }

            return string.Join(Environment.NewLine, commands);
        }
    }

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
                   .HasData(definitions.Select((a,i) => new CultureResourceEntity
                                                    {
                                                            Id = i+1,
                                                            CultureId = 1,
                                                            Key = a.Key,
                                                            Value = a.Key
                                                    }));

            return builder;
        }
    }
}