// -----------------------------------------------------------------------
//  <copyright file="ICultureContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Globalization;
    using JetBrains.Annotations;

    public interface ICultureContext
    {
        [NotNull]
        CultureInfo UICulture { get; }

        [NotNull]
        CultureInfo Culture { get; }
    }
}