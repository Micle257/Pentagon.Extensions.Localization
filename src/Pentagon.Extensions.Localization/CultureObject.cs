// -----------------------------------------------------------------------
//  <copyright file="CultureObject.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    [PublicAPI]
    public class CultureObject
    {
        public CultureObject(string name, IDictionary<string,string> resources, CultureObject parent = null)
        {
            Name = name;
            Resources = (IReadOnlyDictionary<string, string>) (resources ?? new Dictionary<string, string>());
            ParentCulture = parent;
        }

        public bool IsInvariant => Name == null || Name == LocalizationConstants.Invariant;

        public string Name { get;  }

        [NotNull]
        public IReadOnlyDictionary<string, string> Resources { get; } 

        public CultureObject ParentCulture { get; }

        /// <inheritdoc />
        public override string ToString() => $"{(IsInvariant ? "Invariant" : Name)} culture{(ParentCulture == null ? "" : $" (parent: {ParentCulture}")}";
    }
}