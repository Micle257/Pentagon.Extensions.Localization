namespace Pentagon.Extensions.Localization {
    using System;
    using Interfaces;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class LocalizationCacheExtensions
    {
        public static string Create(this ILocalizationCache cache, LocalizationValueDefinition valueDefinition)
        {
            return cache[valueDefinition.Key, Array.Empty<object>()];
        }

        public static string Create<T>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T> valueDefinition, T formatValue)
        {
            return cache[valueDefinition.Key, new object[] { formatValue }];
        }

        public static string Create<T1, T2>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2> valueDefinition, T1 formatValue1, T2 formatValue2)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2 }];
        }

        public static string Create<T1, T2, T3>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3 }];
        }

        public static string Create<T1, T2, T3, T4>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4 }];
        }

        public static string Create<T1, T2, T3, T4, T5>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4, formatValue5 }];
        }

        public static string Create<T1, T2, T3, T4, T5, T6>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4, formatValue5, formatValue6 }];
        }

        public static string Create<T1, T2, T3, T4, T5, T6, T7>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6, T7 formatValue7)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4, formatValue5, formatValue6, formatValue7 }];
        }

        public static string Create<T1, T2, T3, T4, T5, T6, T7, T8>(this ILocalizationCache cache, LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8> valueDefinition, T1 formatValue1, T2 formatValue2, T3 formatValue3, T4 formatValue4, T5 formatValue5, T6 formatValue6, T7 formatValue7, T8 formatValue8)
        {
            return cache[valueDefinition.Key, new object[] { formatValue1, formatValue2, formatValue3, formatValue4, formatValue5, formatValue6, formatValue7, formatValue8 }];
        }
    }
}