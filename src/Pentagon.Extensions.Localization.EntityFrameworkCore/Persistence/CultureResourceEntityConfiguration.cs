// -----------------------------------------------------------------------
//  <copyright file="CultureResourceEntityConfiguration.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Options;

    class CultureResourceEntityConfiguration : IEntityTypeConfiguration<CultureResourceEntity>
    {
        readonly CultureEntityConfigurationOptions _options;

        public CultureResourceEntityConfiguration(CultureEntityConfigurationOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<CultureResourceEntity> b)
        {
            var options = _options ?? new CultureEntityConfigurationOptions();

            b.ToTable((options.TablePrefix ?? "") + (options.CultureResourceTableName ?? DatabaseObjectNames.CultureResource));

            b.Property(a => a.Value)
             .HasMaxLength(1024);

            b.Property(a => a.Key)
             .HasMaxLength(256)
             .IsRequired();

            b.HasIndex(a => new {a.CultureId, a.Key})
             .HasFilter($"[{nameof(CultureResourceEntity.DeletedFlag)}] = 0")
             .IsUnique();

            b.HasOne(a => a.Culture)
             .WithMany(a => a.Resources)
             .HasForeignKey(a => a.CultureId)
             .IsRequired();
        }
    }
}