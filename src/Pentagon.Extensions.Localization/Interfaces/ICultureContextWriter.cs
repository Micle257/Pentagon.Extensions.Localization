namespace Pentagon.Extensions.Localization.Interfaces {
    using JetBrains.Annotations;

    public interface ICultureContextWriter
    {
        void SetLanguage([NotNull] string uiCulture, string culture = null);
    }
}