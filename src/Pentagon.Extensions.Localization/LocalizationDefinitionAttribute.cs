// -----------------------------------------------------------------------
//  <copyright file="LocalizationDefinitionAttribute.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using JetBrains.Annotations;

    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class LocalizationDefinitionAttribute : Attribute { }
}