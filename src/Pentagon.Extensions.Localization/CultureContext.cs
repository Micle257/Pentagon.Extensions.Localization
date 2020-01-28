// -----------------------------------------------------------------------
//  <copyright file="CultureContext.cs">
//   Copyright (c) Michal Pokorn�. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Helpers;
    using Interfaces;

    public class CultureContext : ICultureContext, ICultureContextWriter
    {
        /// <inheritdoc />
        public CultureInfo UICulture { get; private set; } = CultureInfo.CurrentUICulture;

        /// <inheritdoc />
        public CultureInfo Culture { get; private set; } = CultureInfo.CurrentCulture;
        
        /// <inheritdoc />
        public void SetLanguage(string uiCultures, string cultures = null)
        {
            if (uiCultures == null)
                throw new ArgumentNullException(nameof(uiCultures));

            cultures = cultures ?? uiCultures;

            if (!CultureHelper.TryParse(uiCultures, out var ui))
                ui = CultureInfo.CurrentUICulture;

            if (!CultureHelper.TryParse(cultures, out var format))
                format = CultureInfo.CurrentCulture;

            UICulture = ui;
            Culture = format;
        }
        
        public static IEnumerable<CultureInfo> Get(IEnumerable<string> cultures)
        {
            foreach (var culture in cultures)
            {
                if (CultureHelper.TryParse(culture, out var ui))
                    yield return ui;
            }
        }
    }
}