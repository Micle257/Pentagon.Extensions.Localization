namespace Pentagon.Extensions.Localization {
    using System;

    public interface ILocalizationFormattedValueDefinition {
        string Key { get; }
        Type[] FormatTypes { get; }
    }
}