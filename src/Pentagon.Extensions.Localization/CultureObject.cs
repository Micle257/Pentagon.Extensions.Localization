// -----------------------------------------------------------------------
//  <copyright file="CultureObject.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System.Collections.Generic;

    public class CultureObject
    {
        public bool IsInvariant => Name == null;
        public string Name { get; set; }

        public IDictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();

        public CultureObject ParentCulture { get; set; }
    }
}