namespace Pentagon.Extensions.Localization {
    using System;
    using JetBrains.Annotations;

    public interface ILocalizationFormattedValueDefinition {
        [NotNull]
        string Key { get; }
        [NotNull]
        Type[] FormatTypes { get; }
    }
}