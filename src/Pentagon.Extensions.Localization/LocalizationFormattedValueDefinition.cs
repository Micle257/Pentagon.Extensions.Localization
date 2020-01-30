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

    public interface ILocalizationValue {
        [NotNull]
        string Value { get; }

        [NotNull]
        string Key { get;}

        [NotNull]
        ILocalizationFormattedValueDefinition Definition { get;}
    }

    public abstract class LocalizationValueBase : ILocalizationValue
    {
        /// <inheritdoc />
        public string Value { get; }

        /// <inheritdoc />
        public string Key => Definition.Key;

        /// <inheritdoc />
        public ILocalizationFormattedValueDefinition Definition { get; }

        public LocalizationValueBase(string value, ILocalizationFormattedValueDefinition definition)
        {
            Value      = value;
            Definition = definition;
        }

        public static implicit operator string(LocalizationValueBase value) => value.Value;

        /// <inheritdoc />
        public override string ToString() => Value;
    }

    public class LocalizationValue : LocalizationValueBase
    {
        public new LocalizationValueDefinition Definition { get; }

        public LocalizationValue(LocalizationValueDefinition definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format() =>  Value;
    }

    public class LocalizationValue<T> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T formatValue) => string.Format(Value, formatValue);
    }

    public class LocalizationValue<T1,T2> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2) => string.Format(Value, formatValue1, formatValue2);
    }

    public class LocalizationValue<T1,T2,T3> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3) => string.Format(Value, formatValue1, formatValue2, formatValue3);
    }

    public class LocalizationValue<T1,T2,T3,T4> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3, T4> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3, T4> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4) => string.Format(Value, formatValue1, formatValue2, formatValue3, formatValue4);
    }

    public class LocalizationValue<T1, T2, T3, T4, T5> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5) => string.Format(Value, formatValue1, formatValue2, formatValue3, formatValue4, formatValue5);
    }

    public class LocalizationValue<T1, T2, T3, T4, T5, T6> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6) => string.Format(Value, formatValue1, formatValue2, formatValue3, formatValue4, formatValue5, formatValue6);
    }

    public class LocalizationValue<T1, T2, T3, T4, T5, T6, T7> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6, T7 formatValue7) => string.Format(Value, formatValue1, formatValue2, formatValue3, formatValue4, formatValue5, formatValue6, formatValue7);
    }

    public class LocalizationValue<T1, T2, T3, T4, T5, T6, T7, T8> : LocalizationValueBase
    {
        public new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8> Definition { get; }

        public LocalizationValue(LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8> definition, string value) : base(value, definition)
        {
            Definition = definition;
        }

        public string Format(T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6, T7 formatValue7, T8 formatValue8) => string.Format(Value, formatValue1, formatValue2, formatValue3, formatValue4,formatValue5, formatValue6, formatValue7, formatValue8);
    }
}