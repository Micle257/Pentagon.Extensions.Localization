// -----------------------------------------------------------------------
//  <copyright file="LocalizationDefinitionConvention.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Collections.Tree;
    using Helpers;
    using Interfaces;
    using JetBrains.Annotations;

    public class LocalizationInstanceDefinition : IEquatable<LocalizationInstanceDefinition>
    {
        /// <inheritdoc />
        public bool Equals(LocalizationInstanceDefinition other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(Type, other.Type);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((LocalizationInstanceDefinition)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => (Type != null ? Type.GetHashCode() : 0);

        public static bool operator ==(LocalizationInstanceDefinition left, LocalizationInstanceDefinition right) => Equals(left, right);

        public static bool operator !=(LocalizationInstanceDefinition left, LocalizationInstanceDefinition right) => !Equals(left, right);

        public Type Type { get; }

        public LocalizationInstanceDefinition(PropertyInfo propertyInfo, ILocalizationFormattedValueDefinition definition)
        {
            PropertyInfo = propertyInfo;
            Definition = definition;
            Type = propertyInfo.PropertyType;
        }

        public LocalizationInstanceDefinition(Type type)
        {
            Type = type;
        }

        public PropertyInfo PropertyInfo { get; }

        public ILocalizationFormattedValueDefinition Definition { get; }

        /// <inheritdoc />
        public override string ToString() => $"{(Definition != null ? $"Keyed: {Definition.Key}" : "")} | {(Type != null ? $"Typed: {Type}" : "")}";
    }

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

                    var value = LocalizationValueDefinitionBuilder.FromConvention(key, fieldInfo.FieldType);

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

        static IDictionary<Type, IEnumerable<PropertyInfo>> GetInstanceProperties([NotNull] Type type)
        {
            var map = new Dictionary<Type, IEnumerable<PropertyInfo>>();

            Initialize(type, map);

            return map;

            static void Initialize(Type type, Dictionary<Type, IEnumerable<PropertyInfo>> map)
            {
                var properties = new List<PropertyInfo>();

                // process leaf definition
                foreach (var fieldInfo in type.GetRuntimeProperties().Where(a => typeof(ILocalizationValue).IsAssignableFrom(a.PropertyType)))
                {
                    properties.Add(fieldInfo);
                }

                // process nested definition
                foreach (var fieldInfo in type.GetRuntimeProperties().Where(a => type.GetNestedTypes(BindingFlags.Public).Any(nt => nt == a.PropertyType)))
                {
                    properties.Add(fieldInfo);

                    Initialize(fieldInfo.PropertyType, map);
                }

                map.Add(type, properties);
            }
        }

        public static HierarchyList<LocalizationInstanceDefinition> GetInstanceHierarchy(Type type)
        {
            var map = GetInstanceProperties(type);

            var hier = map.ToDictionary(a => new LocalizationInstanceDefinition(a.Key), a => a.Value.Select(b =>
                                                                                                            {
                                                                                                                var value = LocalizationValueDefinitionBuilder.FromConvention(GetKey(b), b.PropertyType);

                                                                                                                return new LocalizationInstanceDefinition(b, value);
                                                                                                            }));

            var hierarchy = HierarchyList<LocalizationInstanceDefinition>.FromDictionaryFreely(new ReadOnlyDictionary<LocalizationInstanceDefinition, IEnumerable<LocalizationInstanceDefinition>>(hier));

            return hierarchy.SingleOrDefault();
        }

        public static T CreateLocalizationInstance<T>(Func<string, string> localization)
                where T : class, new() =>
                (T) CreateLocalizationInstance(typeof(T), localization);

        public static object CreateLocalizationInstance(Type type, Func<string, string> localization)
        {
            var list = GetInstanceHierarchy(type);

            if (list.Root.Value.Type != type)
                throw new ArgumentException("Invalid hierarchy list.");

            return Process(type, list.Root, localization);

            static object Process(Type type, HierarchyListNode<LocalizationInstanceDefinition> parentNode, Func<string, string> localization)
            {
                var instance = Activator.CreateInstance(type);

                foreach (var node in parentNode.Children)
                {
                    if (node.IsLeafNode())
                    {
                        var definition = node.Value.Definition;

                        // TODO change default value
                        var rawValue =  (localization?.Invoke(definition.Key) ?? "...");

                        var value = LocalizationValueBuilder.FromConvention(rawValue, definition);

                        SetPropertyValue(instance, node.Value.PropertyInfo, value);
                    }
                    else
                    {
                        var nestedType = node.Value.Type;

                        var nestedInstance = Process(nestedType, node, localization);

                        var nestedProp = type.GetRuntimeProperties().SingleOrDefault(a => a.PropertyType == nestedType);

                        SetPropertyValue(instance, nestedProp, nestedInstance);
                    }
                }

                return instance;
            }
        }

        public static ValueTask<object> CreateLocalizationInstanceAsync(Type type, Func<string, ValueTask<string>> localization)
        {
            var list = GetInstanceHierarchy(type);

            if (list.Root.Value.Type != type)
                throw new ArgumentException("Invalid hierarchy list.");

            return Process(type, list.Root, localization);

            static async ValueTask<object> Process(Type type, HierarchyListNode<LocalizationInstanceDefinition> parentNode, Func<string, ValueTask<string>> localization)
            {
                var instance = Activator.CreateInstance(type);

                foreach (var node in parentNode.Children)
                {
                    if (node.IsLeafNode())
                    {
                        var definition = node.Value.Definition;

                        // TODO change default value
                        var rawValue = await (localization?.Invoke(definition.Key) ?? new ValueTask<string>("...")).ConfigureAwait(false);

                        var value = LocalizationValueBuilder.FromConvention(rawValue, definition);

                        SetPropertyValue(instance, node.Value.PropertyInfo, value);
                    }
                    else
                    {
                        var nestedType = node.Value.Type;

                        var nestedInstance = await Process(nestedType, node, localization).ConfigureAwait(false);

                        var nestedProp = type.GetRuntimeProperties().SingleOrDefault(a => a.PropertyType == nestedType);

                        SetPropertyValue(instance, nestedProp, nestedInstance);
                    }
                }

                return instance;
            }
        }

        public static IEnumerable<ILocalizationFormattedValueDefinition> GetDefinitions(Type type = null)
        {
            foreach (var definitionType in GetLocalizationDefinitionTypes(type))
            {
                foreach (var fieldInfo in GetFields(definitionType))
                {
                    var key = GetKey(fieldInfo);

                    var value = LocalizationValueDefinitionBuilder.FromConvention(key, fieldInfo.FieldType);

                    yield return (ILocalizationFormattedValueDefinition)value;
                }
            }
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

                    if (type.Name.EndsWith("Localization"))
                        builder.Append(type.Name[..^12] + ".");
                    else
                        builder.Append(type.Name + ".");
                }

                return builder;
            }
        }

        static void SetPropertyValue(object instance, PropertyInfo info, object value)
        {
            if (info.CanWrite)
            {
                info.SetValue(instance, value);
                return;
            }

            var back = instance.GetType().GetField($"<{info.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

            back.SetValue(instance, value);
        }
    }
}