// -----------------------------------------------------------------------
//  <copyright file="JsonLocalization.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Helpers;
    using IO.Json;
    using JetBrains.Annotations;

    public static class JsonLocalization
    {
        [NotNull]
        static readonly ConcurrentDictionary<string, RootObjectJson> _cachedJsons = new ConcurrentDictionary<string, RootObjectJson>();

        public static void LoadJson(RootObjectJson json)
        {
            if (json?.Culture == null || !CultureHelper.Exists(json.Culture))
                return;

            _cachedJsons.AddOrUpdate(json.Culture, json, (jsonName, tuple) => json);
        }

        public static IReadOnlyList<RootObjectJson> LoadJsonFromAssembly(Assembly assembly = null, bool staticCache = true)
        {
            var result = new List<RootObjectJson>();

            var data = GetJsonEmbeddedFiles(assembly);

            foreach (var (name, content) in data)
            {
                var json = JsonHelpers.Deserialize<RootObjectJson>(content);

                if (json.Culture == null && CultureHelper.Exists(name))
                    json.Culture = name;
                else if (!CultureHelper.Exists(json.Culture))
                    continue;

                if (json.Culture == null)
                    json.Culture = LocalizationConstants.Invariant;

                if (staticCache)
                    _cachedJsons.AddOrUpdate(name, json, (jsonName, tuple) => json);

                result.Add(json);
            }

            return result;
        }

        public static IReadOnlyList<RootObjectJson> LoadJsonFromDirectory(string folderPath, bool staticCache = true)
        {
            var result = new List<RootObjectJson>();

            var data = GetJsonFiles(folderPath);

            foreach (var (name, path, content) in data)
            {
                var json = JsonHelpers.Deserialize<RootObjectJson>(content);

                if (json.Culture == null && CultureHelper.Exists(name))
                    json.Culture = name;
                else if (!CultureHelper.Exists(json.Culture))
                    continue;

                if (json.Culture == null)
                    json.Culture = LocalizationConstants.Invariant;

                if (staticCache)
                    _cachedJsons.AddOrUpdate(name, json, (jsonName, tuple) => json);

                result.Add(json);
            }

            return result;
        }

        public static string GetCachedResource(CultureInfo culture, string key, bool includeParentResources)
        {
            var allResources = _cachedJsons.Values
                                           .ToDictionary(a => a.Culture,
                                                         a => a.Resources
                                                               .Where(b => string.Equals(b.Key,
                                                                                         key,
                                                                                         StringComparison.OrdinalIgnoreCase)).ToDictionary(av => av.Key, av => av.Value));

            var obj = LocalizationHelper.GetCultureObject(culture, k => allResources.ContainsKey(k) ? allResources[k] : null);

            if (obj == null)
                return null;

            var resources = LocalizationHelper.GetResources(obj, includeParentResources);

            return resources.Select(a => new KeyValuePair<string, string>(a.Key, a.Value))
                            .FirstOrDefault(a => string.Equals(a.Key, key, StringComparison.OrdinalIgnoreCase))
                            .Value;
        }

        /// <inheritdoc />
        public static IReadOnlyDictionary<string, string> GetAllCachedResources(CultureInfo culture, bool includeParentResources)
        {
            var allResources = _cachedJsons.Values
                                           .ToDictionary(a => a.Culture, a => a.Resources.ToDictionary(b => b.Key, b => b.Value));

            var obj = LocalizationHelper.GetCultureObject(culture, k => allResources.ContainsKey(k) ? allResources[k] : null);

            if (obj == null)
                return new Dictionary<string, string>();

            var resources = LocalizationHelper.GetResources(obj, includeParentResources);

            return resources;
        }

        public static IEnumerable<(string Name, string Content)> GetJsonEmbeddedFiles(Assembly assembly)
        {
            assembly = assembly ?? Assembly.GetExecutingAssembly();

            var allManifest = assembly.GetManifestResourceNames();

            foreach (var s in allManifest)
            {
                if (!s.EndsWith(value: ".json"))
                    continue;

                using (var reader = new StreamReader(assembly.GetManifestResourceStream(s)))
                {
                    var pureName = s.Remove(s.IndexOf(value: ".json", StringComparison.Ordinal));

                    yield return (s, reader.ReadToEnd());
                }
            }
        }

        public static IEnumerable<(string Name, string Path, string Content)> GetJsonFiles(string folderPath)
        {
            var ss = Directory.GetFiles(folderPath);

            foreach (var s in ss)
            {
                var name = s.Split(Path.DirectorySeparatorChar).LastOrDefault();

                if (!name.EndsWith(value: ".json"))
                    continue;

                var pureName = name.Remove(name.IndexOf(value: ".json", StringComparison.Ordinal));

                var content = File.ReadAllText(s);

                yield return (pureName, s, content);
            }
        }
    }
}