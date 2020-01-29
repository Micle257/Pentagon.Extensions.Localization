// -----------------------------------------------------------------------
//  <copyright file="LocalizationDefinitionConvention.cs">
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

    public static class LocalizationDefinitionConvention
    {
        [NotNull]
        [ItemNotNull]
        static IEnumerable<Type> GetLocalizationDefinitionTypes(Type type = null)
        {
            if (type != null)
                yield return type;

            var definitionTypes = AppDomain.CurrentDomain.GetLoadedTypes()
                                           .Where(a => CustomAttributeExtensions.GetCustomAttribute<LocalizationDefinitionAttribute>((MemberInfo)a) != null);

            foreach (var definitionType in definitionTypes)
            {
                yield return definitionType;
            }
        }

        public static void InitializeLocalizationDefinition(Type type = null)
        {
            foreach (var definitionType in GetLocalizationDefinitionTypes(type))
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
            }
        }

        [NotNull]
        [ItemNotNull]
        static IEnumerable<FieldInfo> GetFields([NotNull] Type type)
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

        public static IEnumerable<ILocalizationFormattedValueDefinition> GetDefinitions(Type type = null)
        {
            foreach (var definitionType in GetLocalizationDefinitionTypes(type))
            {
                foreach (var fieldInfo in GetFields(definitionType))
                {
                    var key = GetKey(fieldInfo);

                    var value = LocalizationValueDefinitionBuilder.FromConvention(key, fieldInfo);

                    yield return (ILocalizationFormattedValueDefinition)value;
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
                {
                    Declere(declaringType, builder);

                    builder.Append(type.Name + ".");
                }

                return builder;
            }
        }
    }
}