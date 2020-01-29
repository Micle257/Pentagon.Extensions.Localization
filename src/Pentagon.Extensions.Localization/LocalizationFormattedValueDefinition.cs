﻿namespace Pentagon.Extensions.Localization {
    using System;
    using JetBrains.Annotations;

    public class LocalizationValueDefinition : ILocalizationFormattedValueDefinition
    {
        public LocalizationValueDefinition([NotNull] string key, params Type[] formatTypes)
        {
            Key         = key ?? throw new ArgumentNullException(nameof(key));
            FormatTypes = formatTypes ?? Array.Empty<Type>();
        }

        [NotNull]
        public string Key { get; }

        [NotNull]
        public Type[] FormatTypes { get; }
    }

    public class LocalizationFormattedValueDefinition<T> : LocalizationValueDefinition
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2> : LocalizationValueDefinition
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3> : LocalizationValueDefinition
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4> : LocalizationValueDefinition
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4)) { }
    }
}