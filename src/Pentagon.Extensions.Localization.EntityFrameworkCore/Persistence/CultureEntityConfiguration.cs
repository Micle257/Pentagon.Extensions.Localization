// -----------------------------------------------------------------------
//  <copyright file="CultureEntityConfiguration.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Options;

    class CultureEntityConfiguration : IEntityTypeConfiguration<CultureEntity>
    {
        readonly CultureEntityConfigurationOptions _options;

        public CultureEntityConfiguration(CultureEntityConfigurationOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<CultureEntity> b)
        {
            var options = _options ?? new CultureEntityConfigurationOptions();

            b.ToTable((options.TablePrefix ?? "") + (options.CultureTableName ?? DatabaseObjectNames.Culture));

            b.Property(a => a.Name)
             .HasMaxLength(32);

            b.HasIndex(a => a.Name)
             .IsUnique()
             .HasFilter($"[{nameof(CultureEntity.Name)}] IS NOT NULL");
        }
    }
}