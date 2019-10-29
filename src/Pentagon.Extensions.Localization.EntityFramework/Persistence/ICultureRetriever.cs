// -----------------------------------------------------------------------
//  <copyright file="ICultureRepository.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    public interface ICultureRetriever
    {
        Task<CultureResourceEntity> GetOneAsync(string cultureName, string key, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CultureResourceEntity>> GetAllAsync(string cultureName, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CultureEntity>> GetCulturesAsync(CancellationToken cancellationToken = default);

        Task<CultureEntity> GetCultureAsync(string name, CancellationToken cancellationToken = default);
    }
}