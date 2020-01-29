namespace Pentagon.Extensions.Localization {
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [Serializable]
    public class LocalizationNotFoundException : Exception
    {
        public LocalizationNotFoundException(string key, CultureInfo cultureInfo) : base($"Localization by key '{key}' was not found. Culture: {cultureInfo}") { }

        protected LocalizationNotFoundException(
                SerializationInfo info,
                StreamingContext context) : base(info, context) { }
    }
}