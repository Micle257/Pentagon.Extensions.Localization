// -----------------------------------------------------------------------
//  <copyright file="LocalizationValueDefinitionBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;

    static class LocalizationValueDefinitionBuilder
    {
        internal static ILocalizationFormattedValueDefinition FromConvention(string key, [NotNull] Type memberType)
        {
            var genericParams = memberType.GenericTypeArguments;

            switch (genericParams.Length)
            {
                case 0:
                    return new LocalizationValueDefinition(key);
                case 1:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<>).MakeGenericType(genericParams));
                case 2:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,>).MakeGenericType(genericParams));
                case 3:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,>).MakeGenericType(genericParams));
                case 4:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,,>).MakeGenericType(genericParams));
                case 5:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,,,>).MakeGenericType(genericParams));
                case 6:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,,,,>).MakeGenericType(genericParams));
                case 7:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,,,,,>).MakeGenericType(genericParams));
                case 8:
                    return GetInstance(typeof(LocalizationFormattedValueDefinition<,,,,,,,>).MakeGenericType(genericParams));
            }

            return null;

            ILocalizationFormattedValueDefinition GetInstance(Type type)
            {
                var constructor = type.GetConstructor(new[] { typeof(string) });

                var newInstance = constructor.Invoke(new object[] { key });

                return (ILocalizationFormattedValueDefinition) newInstance;
            }
        }
    }

    static class LocalizationValueBuilder
    {
        internal static ILocalizationValue FromConvention(string value, ILocalizationFormattedValueDefinition definition)
        {
            switch (definition.FormatTypes.Length)
            {
                case 0:
                    return GetInstance(definition, typeof(LocalizationValue));
                case 1:
                    return GetInstance(definition, typeof(LocalizationValue<>).MakeGenericType(definition.FormatTypes));
                case 2:
                    return GetInstance(definition, typeof(LocalizationValue<,>).MakeGenericType(definition.FormatTypes));
                case 3:
                    return GetInstance(definition, typeof(LocalizationValue<,,>).MakeGenericType(definition.FormatTypes));
                case 4:
                    return GetInstance(definition, typeof(LocalizationValue<,,,>).MakeGenericType(definition.FormatTypes));
                case 5:
                    return GetInstance(definition, typeof(LocalizationValue<,,,,>).MakeGenericType(definition.FormatTypes));
                case 6:
                    return GetInstance(definition, typeof(LocalizationValue<,,,,,>).MakeGenericType(definition.FormatTypes));
                case 7:
                    return GetInstance(definition, typeof(LocalizationValue<,,,,,,>).MakeGenericType(definition.FormatTypes));
                case 8:
                    return GetInstance(definition, typeof(LocalizationValue<,,,,,,,>).MakeGenericType(definition.FormatTypes));
            }

            return null;

            ILocalizationValue GetInstance(ILocalizationFormattedValueDefinition definition, Type type)
            {
                var constructor = type.GetConstructor(new[] { definition.GetType(), typeof(string) });

                var newInstance = constructor.Invoke(new object[] { definition, value });

                return (ILocalizationValue)newInstance;
            }
        }
    }

}