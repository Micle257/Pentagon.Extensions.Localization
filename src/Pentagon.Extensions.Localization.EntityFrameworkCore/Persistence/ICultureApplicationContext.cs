// -----------------------------------------------------------------------
//  <copyright file="ICultureApplicationContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using Entities;
    using EntityFrameworkCore.Abstractions;
    using Microsoft.EntityFrameworkCore;

    public interface ICultureApplicationContext : IApplicationContext
    {
        DbSet<CultureEntity> Cultures { get; }

        DbSet<CultureResourceEntity> CultureResources { get; }
    }
}