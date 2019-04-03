// -----------------------------------------------------------------------
//  <copyright file="CultureStore.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
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

        public IEnumerable<string> GetFilePaths()
        {
            var baseDirectory = AppDomain.CurrentDomain;

            var dir = Path.Combine(baseDirectory.BaseDirectory, _options.ResourceFolder);

            return Directory.EnumerateFileSystemEntries(dir);
        }

        public IReadOnlyCollection<RootObjectJson> GetDeserializedJsonObjects()
        {
            var result = new List<RootObjectJson>();

            var files = GetFilePaths().ToList();

            foreach (var file in files)
            {
                var text = File.ReadAllText(file);

                var json = JsonHelpers.Deserialize<RootObjectJson>(text);

                result.Add(json);
            }

            return result;
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
            var res = Jsons.Select(a => a.Culture);

            return Task.FromResult<IReadOnlyList<CultureInfo>>(res.Select(a => CultureInfo.GetCultureInfo(a)).ToList());
        }

        /// <inheritdoc />
        public Task<CultureInfo> GetCultureAsync(string name)
        {
            var res = Jsons.Select(a => a.Culture);

            return Task.FromResult(res.Select(a => CultureInfo.GetCultureInfo(a)).FirstOrDefault());
        }
    }
}