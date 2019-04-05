// -----------------------------------------------------------------------
//  <copyright file="CultureContext.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Interfaces;
    using JetBrains.Annotations;

    public class CultureContext : ICultureContext
    {
        /// <inheritdoc />
        public CultureInfo UICulture { get; private set; }

        /// <inheritdoc />
        public CultureInfo Culture { get; private set; }
        
        /// <inheritdoc />
        public void SetLanguage(string uiCultures, string cultures = null)
        {
            if (uiCultures == null)
                throw new ArgumentNullException(nameof(uiCultures));

            cultures = cultures ?? uiCultures;

            if (!CultureHelpers.TryParse(uiCultures, out var ui))
                ui = CultureInfo.CurrentUICulture;

            if (!CultureHelpers.TryParse(cultures, out var format))
                format = CultureInfo.CurrentCulture;

            UICulture = ui;
            Culture = format;
        }
        
        public static IEnumerable<CultureInfo> Get(IEnumerable<string> cultures)
        {
            foreach (var culture in cultures)
            {
                if (CultureHelpers.TryParse(culture, out var ui))
                    yield return ui;
            }
        }
    }
}