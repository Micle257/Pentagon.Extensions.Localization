// -----------------------------------------------------------------------
//  <copyright file="ICultureContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;
    using JetBrains.Annotations;

    public interface ICultureContext
    {
        CultureInfo UICulture { get; }

        CultureInfo Culture { get; }
        
        void SetLanguage([NotNull] string uiCulture, string culture = null);
    }
}