// -----------------------------------------------------------------------
//  <copyright file="LocalizationHelper.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Pentagon.Extensions.Localization {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Threading;

    public static class LocalizationHelper
    {
        public static IReadOnlyDictionary<string, string> GetResources([NotNull] CultureObject cultureEntity, bool includeParent = false)
        {
            if (cultureEntity == null)
                throw new ArgumentNullException(nameof(cultureEntity));

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
                    return new CultureObject(LocalizationConstants.Invariant, invariantResources.ToDictionary(a => a.Key, a => a.Value));
                }

                return null;
            }

            CultureObject parentCulture = null;


            // if culture is country specific
            if (!culture.IsNeutralCulture)
            {
                var neutralCulture = culture.Parent;

                Debug.Assert(neutralCulture.IsNeutralCulture);

                var parentCultureResources = await allResources(neutralCulture.Name).ConfigureAwait(false);

                if (parentCultureResources != null)
                {
                    var invariantCulture = neutralCulture.Parent;

                    Debug.Assert(Equals(invariantCulture, invariantCulture.Parent));

                    var invariantResources = await allResources(LocalizationConstants.Invariant).ConfigureAwait(false);

                    CultureObject parentOfParentCulture = null;

                    if (invariantResources != null)
                    {
                        parentOfParentCulture = new CultureObject(LocalizationConstants.Invariant, invariantResources.ToDictionary(a => a.Key, a => a.Value));
                    };

                    parentCulture = new CultureObject(neutralCulture.Name, parentCultureResources.ToDictionary(a => a.Key, a => a.Value), parentOfParentCulture);
                }
            }
            else
            {
                var invariantCulture = culture.Parent;

                Debug.Assert(Equals(invariantCulture, invariantCulture.Parent));

                var invariantResources = await allResources(LocalizationConstants.Invariant).ConfigureAwait(false);

                if (invariantResources != null)
                {
                    parentCulture = new CultureObject(LocalizationConstants.Invariant, invariantResources.ToDictionary(a => a.Key, a => a.Value));
                }
            }

            var resultCulture = new CultureObject(culture.Name, resources.ToDictionary(a => a.Key, a => a.Value), parentCulture);

            return resultCulture;
        }
    }
}