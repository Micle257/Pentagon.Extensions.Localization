// -----------------------------------------------------------------------
//  <copyright file="CultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using EntityFrameworkCore.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CultureStore : ICultureStore
    {
        readonly ILogger<CultureStore> _logger;
        readonly ICultureApplicationContext _applicationContext;

        public CultureStore(ILogger<CultureStore> logger,
                            ICultureApplicationContext applicationContext)
        {
            _logger = logger;
            _applicationContext = applicationContext;
        }

        /// <inheritdoc />
        public async Task<CultureResourceEntity> GetOneAsync(string culture, string key)
        {
            _logger.LogDebug($"Retrieving culture resource for culture={culture} and key={key}.");

            var spec = new GetOneSpecification<CultureResourceEntity>(r => r.Key == key && r.Culture.Name == culture && !r.DeletedFlag);

            spec.AddConfiguration(q => q.Include(a => a.Culture));

            try
            {
                var resource = await _applicationContext.CultureResources.GetOneAsync(spec);

                if (resource == null)
                {
                    _logger.LogWarning($"No resource found for culture={culture} and key={key}.");

                    return null;
                }

                return resource;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: "Exception occured while reading database.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CultureResourceEntity>> GetAllAsync(string culture)
        {
            _logger.LogDebug($"Retrieving all culture resources for culture={culture}.");

            var spec = new GetManySpecification<CultureResourceEntity>();

            spec.Filters.Add(r => r.Culture.Name == null || r.Culture.Name == culture);
            spec.Filters.Add(r => !r.DeletedFlag);

            spec.AddConfiguration(q => q.Include(a => a.Culture));

            try
            {
                var resource = await _applicationContext.CultureResources.GetManyAsync(spec);

                if (resource?.Any(a => a.Culture.Name != null) != true)
                {
                    _logger.LogWarning($"No resources found for culture={culture}.");

                    return Array.Empty<CultureResourceEntity>();
                }

                return resource;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: "Exception occured while reading database.");
                throw;
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<CultureEntity>> GetCulturesAsync()
        {
            var spec = new GetManySpecification<CultureEntity>();

            spec.AddFilter(a => a.ActiveFlag);

            spec.AddConfiguration(q => q.Include(a => a.Resources));

            return _applicationContext.Cultures.GetManyAsync(spec);
        }

        public Task<CultureEntity> GetCultureAsync(string name)
        {
            var spec = new GetOneSpecification<CultureEntity>();

            spec.AddFilter(a => a.Name == name);
            spec.AddFilter(a => a.ActiveFlag);

            spec.AddConfiguration(q => q.Include(a => a.Resources));

            return _applicationContext.Cultures.GetOneAsync(spec);
        }
    }
}