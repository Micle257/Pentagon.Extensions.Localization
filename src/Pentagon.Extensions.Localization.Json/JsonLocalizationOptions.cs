// -----------------------------------------------------------------------
//  <copyright file="JsonLocalizationOptions.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Persistence
{
    using System.IO;

    public class JsonLocalizationOptions
    {
        public string ResourceFolder { get; set; } = $"Resources{Path.DirectorySeparatorChar}Localization";

        public bool UseEmbedded { get; set; } = false;
    }
}