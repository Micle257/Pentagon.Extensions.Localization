// -----------------------------------------------------------------------
//  <copyright file="LocalizationHelper.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Extensions.Localization {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Threading;

    public static class LocalizationHelper
    {
        public static IReadOnlyDictionary<string, string> GetResources(CultureObject cultureEntity, bool includeParent = false)
        {
            var resources = cultureEntity.Resources;

            if (!includeParent)
                return resources;

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

        public static CultureObject GetCultureObject(CultureInfo culture, Func<string, IDictionary<string, string>> allResources)
        {
            return GetCultureObjectAsync(culture, s => Task.FromResult(allResources(s))).AwaitSynchronously();
        }

        public static async Task<CultureObject> GetCultureObjectAsync(CultureInfo culture, Func<string, Task<IDictionary<string, string>>> allResources)
        {
            var resources = await allResources(culture.Name).ConfigureAwait(false);

            if (resources == null)
            {
                var invariantResources = await allResources(LocalizationConstants.Invariant).ConfigureAwait(false);

                if (invariantResources != null)
                {
                    return new CultureObject
                                                  {
                                                          Name = LocalizationConstants.Invariant,
                                                          Resources = invariantResources.ToDictionary(a => a.Key, a => a.Value)
                                                  };
                }

                return null;
            }

            var resultCulture = new CultureObject
            {
                Name = culture.Name,
                Resources = resources.ToDictionary(a=>a.Key,a=>a.Value)
            };

            // if culture is country specific
            if (!culture.IsNeutralCulture)
            {
                var neutralCulture = culture.Parent;

#if DEBUG
                Debug.Assert(neutralCulture.IsNeutralCulture);
#endif

                var parentCultureResources = await allResources(neutralCulture.Name).ConfigureAwait(false);

                if (parentCultureResources != null)
                {
                    resultCulture.ParentCulture = new CultureObject
                    {
                        Name = neutralCulture.Name,
                        Resources = parentCultureResources.ToDictionary(a => a.Key, a => a.Value)
                    };

                    var invariantCulture = neutralCulture.Parent;

                    #if DEBUG
                    Debug.Assert(Equals(invariantCulture, invariantCulture.Parent));
#endif

                    var invariantResources = await allResources(LocalizationConstants.Invariant).ConfigureAwait(false);

                    if (invariantResources != null)
                    {
                        resultCulture.ParentCulture.ParentCulture = new CultureObject
                        {
                            Name = LocalizationConstants.Invariant,
                            Resources = invariantResources.ToDictionary(a => a.Key, a => a.Value)
                        };
                    }
                }
            }
            else
            {
                var invariantCulture = culture.Parent;

                // Assert: if (!Equals(invariantCulture, invariantCulture.Parent))

#if DEBUG
                Debug.Assert(Equals(invariantCulture, invariantCulture.Parent));
#endif

                var invariantResources = await allResources(LocalizationConstants.Invariant).ConfigureAwait(false);

                if (invariantResources != null)
                {
                    resultCulture.ParentCulture = new CultureObject
                                                                {
                                                                        Name = LocalizationConstants.Invariant,
                                                                        Resources = invariantResources.ToDictionary(a => a.Key, a => a.Value)
                                                                };
                }
            }

            return resultCulture;
        }
    }
}