// -----------------------------------------------------------------------
//  <copyright file="CultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Helpers;
    using Interfaces;
    using IO.Json;
    using JetBrains.Annotations;
    using Json.Json;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CultureStore : ICultureStore
    {
        [NotNull]
        readonly ILogger<CultureStore> _logger;

        [NotNull]
        JsonLocalizationOptions _options;

        IReadOnlyCollection<RootObjectJson> _jsons;

        public CultureStore([NotNull] ILogger<CultureStore> logger,
                            IOptionsSnapshot<JsonLocalizationOptions> optionsSnapshot)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = optionsSnapshot?.Value ?? new JsonLocalizationOptions();
        }

        IReadOnlyCollection<RootObjectJson> Jsons => _jsons ?? (_jsons = GetDeserializedJsonObjects());

        public IReadOnlyCollection<RootObjectJson> GetDeserializedJsonObjects()
        {
            var result = new ConcurrentDictionary<string, RootObjectJson>();

            foreach (var json in JsonLocalization.LoadJsonFromDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.ResourceFolder), false))
            {
                result.AddOrUpdate(json.Culture, json, (_, __) => json);
            }

            if (_options.UseEmbedded)
            {
                foreach (var json in JsonLocalization.LoadJsonFromAssembly(null, false))
                {
                    result.AddOrUpdate(json.Culture, json, (_, __) => json);
                }
            }

            return result.Values.ToList();
        }

        /// <inheritdoc />
        public Task<KeyValuePair<string, string>> GetResourceAsync(string cultureName, string key)
        {
            var res = Jsons.Where(a => a.Culture == cultureName)
                           .SelectMany(a => a.Resources)
                           .Select(a => new KeyValuePair<string, string>(a.Key, a.Value));

            return Task.FromResult(res.FirstOrDefault());
        }

        /// <inheritdoc />
        public Task<IReadOnlyDictionary<string, string>> GetAllResourcesAsync(string cultureName)
        {
            var res = Jsons.Where(a => a.Culture == cultureName)
                           .SelectMany(a => a.Resources)
                           .Select(a => new KeyValuePair<string, string>(a.Key, a.Value))
                           .ToDictionary(a => a.Key, a => a.Value);

            return Task.FromResult<IReadOnlyDictionary<string, string>>(res);
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<CultureInfo>> GetAvailableCulturesAsync()
        {
            var result = new List<CultureInfo>();

            var res = Jsons.Select(a => a.Culture);

            foreach (var re in res)
            {
                if (re == null)
                {
                    result.Add(CultureInfo.InvariantCulture);
                    continue;
                }

                if (CultureHelper.Exists(re))
                    result.Add(CultureInfo.GetCultureInfo(re));
            }

            return Task.FromResult<IReadOnlyList<CultureInfo>>(result);
        }

        /// <inheritdoc />
        public Task<CultureInfo> GetCultureAsync(string name)
        {
            var res = Jsons.Select(a => a.Culture)
                           .Where(a => a == name);

            return Task.FromResult(res.Select(GetCultureInfo).FirstOrDefault());
        }

        CultureInfo GetCultureInfo(string name)
        {
            if (name == null)
                return CultureInfo.InvariantCulture;

            return CultureInfo.GetCultureInfo(name);
        }
    }
}