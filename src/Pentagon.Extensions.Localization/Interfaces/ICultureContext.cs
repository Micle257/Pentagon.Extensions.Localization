// -----------------------------------------------------------------------
//  <copyright file="ICultureContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.Interfaces
{
    using System.Collections.Generic;
    using System.Globalization;

    public interface ICultureContext
    {
        CultureInfo UICulture { get; }

        CultureInfo Culture { get; }
    }
}