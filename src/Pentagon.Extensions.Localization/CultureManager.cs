// -----------------------------------------------------------------------
//  <copyright file="CultureManager.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;

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

            var cultureEntity = await GetCultureAsync(culture).ConfigureAwait(false);

            return LocalizationHelper.GetResources(cultureEntity, includeParentResources);
        }

        /// <inheritdoc />
        public async Task<CultureObject> GetCultureAsync(CultureInfo culture)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture;

            var cultureEntity = (await _cultureStore.GetAvailableCulturesAsync().ConfigureAwait(false)).FirstOrDefault(a => Equals(a, culture));

            if (cultureEntity == null)
                return (null);

            return await LocalizationHelper.GetCultureObjectAsync(culture, async key => (await _cultureStore.GetAllResourcesAsync(key).ConfigureAwait(false)).ToDictionary(a => a.Key, a => a.Value)).ConfigureAwait(false);
        }
    }
}