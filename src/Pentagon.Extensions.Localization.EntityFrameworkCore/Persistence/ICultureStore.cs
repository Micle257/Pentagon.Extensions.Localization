// -----------------------------------------------------------------------
//  <copyright file="ICultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;

    public interface ICultureRepository
    {
        Task<CultureResourceEntity> GetOneAsync(string cultureName, string key);

        Task<IReadOnlyList<CultureResourceEntity>> GetAllAsync(string cultureName);

        Task<IReadOnlyList<CultureEntity>> GetCulturesAsync();

        Task<CultureEntity> GetCultureAsync(string name);
    }
}