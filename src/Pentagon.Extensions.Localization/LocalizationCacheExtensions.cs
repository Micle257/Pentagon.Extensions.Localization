namespace Pentagon.Extensions.Localization {
    using System;
    using Interfaces;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class LocalizationCacheExtensions
    {
        public static string Create(this ILocalizationCache cache, LocalizationValueDefinition valueDefinition)
        {
            return cache.GetValue(valueDefinition.Key, Array.Empty<object>());
        }

        public static string Create<T>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T> valueDefinition, T formatValue)
        {
            return cache.GetValue(valueDefinition.Key, new object[] { formatValue });
        }

        public static string Create<T1, T2>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2> valueDefinition, T1 formatValue1, T2 formatValue2)
        {
            return cache.GetValue(valueDefinition.Key, new object[] { formatValue1, formatValue2 });
        }

        public static string Create<T1, T2, T3>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3)
        {
            return cache.GetValue(valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3 });
        }

        public static string Create<T1, T2, T3, T4>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4)
        {
            return cache.GetValue(valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4 });
        }
    }
}