// -----------------------------------------------------------------------
//  <copyright file="ICultureApplicationContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using Entities;
    using EntityFrameworkCore.Abstractions.Repositories;

    public interface ICultureApplicationContext
    {
        IRepository<CultureEntity> Cultures { get; }

        IRepository<CultureResourceEntity> CultureResources { get; }
    }
}