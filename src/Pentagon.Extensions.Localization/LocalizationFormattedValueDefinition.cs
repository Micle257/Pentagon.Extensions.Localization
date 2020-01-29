namespace Pentagon.Extensions.Localization {
    using System;
    using JetBrains.Annotations;

    public abstract class LocalizationValueDefinitionBase : ILocalizationFormattedValueDefinition
    {
        protected LocalizationValueDefinitionBase([NotNull] string key, params Type[] formatTypes)
        {
            Key         = key ?? throw new ArgumentNullException(nameof(key));
            FormatTypes = formatTypes ?? Array.Empty<Type>();
        }

        [NotNull]
        public string Key { get; }

        [NotNull]
        public Type[] FormatTypes { get; }
    }

    public class LocalizationValueDefinition : LocalizationValueDefinitionBase
    {
        public LocalizationValueDefinition([NotNull] string key) : base(key) { }
    }

    public class LocalizationFormattedValueDefinition<T> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)) { }
    }

    public class LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8> : LocalizationValueDefinitionBase
    {
        public LocalizationFormattedValueDefinition([NotNull] string key) : base(key, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)) { }
    }
}