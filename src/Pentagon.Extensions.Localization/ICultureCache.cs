// -----------------------------------------------------------------------
//  <copyright file="ICultureCache.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System.Collections.Generic;
    using System.Globalization;

    public interface ICultureCache
    {
        CultureInfo Culture { get; }

        IReadOnlyDictionary<string, string> Resources { get; }

        bool IsLoaded { get; }

        void SetResources(IDictionary<string, string> data);

        bool HasResource(string key);
    }
}