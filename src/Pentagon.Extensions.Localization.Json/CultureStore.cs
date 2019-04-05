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

        public IEnumerable<(string Name, string Path)> GetFilePaths()
        {
            var baseDirectory = AppDomain.CurrentDomain;

            var dir = Path.Combine(baseDirectory.BaseDirectory, _options.ResourceFolder);

            var ss = Directory.GetFiles(dir);

            foreach (var s in ss)
            {
                var name = s.Split(Path.DirectorySeparatorChar).LastOrDefault();

                if (!name.EndsWith(".json"))
                    continue;

                var pureName = name.Remove(name.IndexOf(".json", StringComparison.Ordinal));

                yield return (pureName, s);
            }
        }

        public IReadOnlyCollection<RootObjectJson> GetDeserializedJsonObjects()
        {
            var result = new List<RootObjectJson>();

            var files = GetFilePaths().ToList();

            foreach (var file in files)
            {
                var text = File.ReadAllText(file.Path);

                var json = JsonHelpers.Deserialize<RootObjectJson>(text);

                if (json.Culture == null && CultureHelpers.Exists(file.Name))
                    json.Culture = file.Name;

                if (!CultureHelpers.Exists(json.Culture))
                {
                    _logger.LogWarning($"Unknown culture name: {json.Culture}.");
                    continue;
                }

                if (result.Any(a => a.Culture == json.Culture))
                {
                    _logger.LogWarning($"Culture with the same name already exists: {json.Culture}.");
                    continue;
                }

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
            var result = new List<CultureInfo>();

            var res = Jsons.Select(a => a.Culture);

            foreach (var re in res)
            {
                if (re == null)
                {
                    result.Add(CultureInfo.InvariantCulture);
                    continue;
                }

                if (CultureHelpers.Exists(re))
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
            if (name ==null)
                return CultureInfo.InvariantCulture;

            return CultureInfo.GetCultureInfo(name);
        }
    }
}