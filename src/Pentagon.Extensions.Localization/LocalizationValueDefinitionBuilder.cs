// -----------------------------------------------------------------------
//  <copyright file="LocalizationValueDefinitionBuilder.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Helpers;
    using JetBrains.Annotations;

    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class LocalizationDefinitionAttribute : Attribute
    {
    }

    static class LocalizationDefinitionConvention
    {
        public static void InitializeLocalizationDefinition(Type type = null)
        {
            if (type != null)
                InitializeLocalizationDefinitionImpl(type);

            var definitionTypes = AppDomain.CurrentDomain.GetLoadedTypes()
                                      .Where(a => a.GetCustomAttribute<LocalizationDefinitionAttribute>() != null);

            foreach (var definitionType in definitionTypes)
            {
                InitializeLocalizationDefinitionImpl(definitionType);
            }

            static void InitializeLocalizationDefinitionImpl(Type type)
            {
                foreach (var fieldInfo in GetFields(type))
                {
                    var key = GetKey(fieldInfo);

                    var value = LocalizationValueDefinitionBuilder.FromConvention(key, fieldInfo);

                    fieldInfo.SetValue(null, value);
                }

                static IEnumerable<FieldInfo> GetFields(Type type)
                {
                    foreach (var fieldInfo in type.GetRuntimeFields())
                    {
                        yield return fieldInfo;
                    }

                    foreach (var nestedType in type.GetNestedTypes(BindingFlags.Static | BindingFlags.Public))
                    {
                        foreach (var fieldInfo in GetFields(nestedType))
                        {
                            yield return fieldInfo;
                        }
                    }
                }
            }
        }

        public static string GetKey([NotNull] Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression is MemberExpression member)
            {
                var memberInfo = member.Member;

                return GetKey(memberInfo);
            }

            throw new InvalidOperationException("Cannot create location value definition from expression.");
        }

        public static string GetKey([NotNull] MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var decl = Declere(memberInfo.DeclaringType, new StringBuilder());

            var keyName = decl + memberInfo.Name;

            return keyName;

            static StringBuilder Declere(Type type, StringBuilder builder)
            {
                var declaringType = type.DeclaringType;

                if (declaringType != null)
                    Declere(declaringType, builder);

                builder.Append(type.Name + ".");

                return builder;
            }
        }
    }

    [PublicAPI]
    public static class LocalizationValueDefinitionBuilder
    {
        public static LocalizationValueDefinition FromConvention([NotNull] Expression<Func<LocalizationValueDefinition>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationValueDefinition(key);
        }

        public static LocalizationFormattedValueDefinition<T> FromConvention<T>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2> FromConvention<T1, T2>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3> FromConvention<T1, T2, T3>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3, T4> FromConvention<T1, T2, T3, T4>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3, T4>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3, T4>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5> FromConvention<T1, T2, T3, T4, T5>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6> FromConvention<T1, T2, T3, T4, T5, T6>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7> FromConvention<T1, T2, T3, T4, T5, T6, T7>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7>(key);
        }

        public static LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8> FromConvention<T1, T2, T3, T4, T5, T6, T7, T8>([NotNull] Expression<Func<LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8>>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var key = LocalizationDefinitionConvention.GetKey(selector.Body);

            return new LocalizationFormattedValueDefinition<T1, T2, T3, T4, T5, T6, T7, T8>(key);
        }

        internal static object FromConvention(string key, [NotNull] FieldInfo fieldInfo)
        {
            var fieldInfoFieldType = fieldInfo.FieldType;

            var genericParams = fieldInfoFieldType.GenericTypeArguments;

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

            object GetInstance(Type type)
            {
                var constructor = type.GetConstructor(new[] { typeof(string) });

                var newInstance = constructor.Invoke(new object[] { key });

                return newInstance;
            }
        }
    }
}