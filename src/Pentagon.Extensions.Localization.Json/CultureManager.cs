// -----------------------------------------------------------------------
//  <copyright file="CultureManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class CultureManager : ICultureManager
    {
        readonly ILogger<CultureManager> _logger;
        readonly ICultureStore _cultureStore;

        public CultureManager(ILogger<CultureManager> logger,
                              ICultureStore cultureStore)
        {
            _logger = logger;
            _cultureStore = cultureStore;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<string, string>> GetResourcesAsync(CultureInfo culture, bool includeParentResources)
        {
            _logger.LogDebug($"Retrieving all culture resources for culture={culture}.");

            var cultureEntity = await GetCultureAsync(culture);

            if (includeParentResources)
                return GetResources(cultureEntity);

            return cultureEntity.Resources;
        }

        /// <inheritdoc />
        public async Task<CultureObject> GetCultureAsync(CultureInfo culture)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture;

            var cultureEntity = (await _cultureStore.GetAvailableCulturesAsync()).FirstOrDefault(a => Equals(a, culture));

            if (cultureEntity == null)
                return null;

            var resources = await _cultureStore.GetAllResourcesAsync(cultureEntity.Name);

            var resultCulture = new CultureObject
                                {
                                        Name = cultureEntity.Name,
                                        Resources = resources
                                };

            // if culture is country specific
            if (!culture.IsNeutralCulture)
            {
                var neutralCulture = culture.Parent;
          
                // Assert: if (!neutralCulture.IsNeutralCulture)
          
                var neutralCultureEntity = await _cultureStore.GetCultureAsync(neutralCulture.Name);
          
                if (neutralCultureEntity != null)
                {
                    var parentResources = await _cultureStore.GetAllResourcesAsync(neutralCultureEntity.Name);

                    resultCulture.ParentCulture = new CultureObject
                                                  {
                                                          Name = neutralCultureEntity.Name,
                                                          Resources = parentResources
                    };
          
                    var invariantCulture = neutralCulture.Parent;
          
                    // Assert: if (!Equals(invariantCulture, invariantCulture.Parent))
          
                    var invariantCultureEntity = await _cultureStore.GetCultureAsync(null);
          
                    if (invariantCultureEntity != null)
                    {
                        var invariantResources = await _cultureStore.GetAllResourcesAsync(null);

                        resultCulture.ParentCulture.ParentCulture = new CultureObject
                                                                    {
                                                                            Name = invariantCultureEntity.Name,
                                                                            Resources = invariantResources
                        };
                    }
                }
            }
            else
            {
                var invariantCulture = culture.Parent;

                // Assert: if (!Equals(invariantCulture, invariantCulture.Parent))

                var invariantCultureEntity = await _cultureStore.GetCultureAsync(null);

                if (invariantCultureEntity != null)
                {
                    var invariantResources = await _cultureStore.GetAllResourcesAsync(null);

                    resultCulture.ParentCulture.ParentCulture = new CultureObject
                                                                {
                                                                        Name = invariantCultureEntity.Name,
                                                                        Resources = invariantResources
                                                                };
                }
            }

            return resultCulture;
        }

        IReadOnlyDictionary<string, string> GetResources(CultureObject cultureEntity)
        {
            var resources = cultureEntity.Resources;

            var keys = resources.ToDictionary(a => a.Key, a => a.Value);

            if (cultureEntity.ParentCulture != null)
            {
                var innerResources = GetResources(cultureEntity.ParentCulture);

                var missingKeys = innerResources.Select(a => a.Key).Except(resources.Select(a => a.Key));

                foreach (var missingKey in missingKeys)
                {
                    var resource = innerResources.FirstOrDefault(a => a.Key == missingKey);

                    keys.Add(resource.Key, resource.Value);
                }
            }

            return keys;
        }
    }
}