namespace Pentagon.Extensions.Localization.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Xunit;

    [LocalizationDefinition]
    public static class LocalMorePure
    {
        public static class Nest1
        {
            public static class Nest2
            {
                public static LocalizationValueDefinition NoParam;
                public static LocalizationFormattedValueDefinition<int> OneParam;
                public static LocalizationFormattedValueDefinition<int, string, double, char> FourParam;
            }

            public static LocalizationFormattedValueDefinition<int, string, double> ThreeParam;
        }

        public static LocalizationFormattedValueDefinition<int, string> TwoParam;
    }

    [LocalizationDefinition]
    public static class LocalMorePure2
    {
        public static LocalizationValueDefinition TwoParam;
    }

    public class Localization
    {
        public LocalizationValue AppName { get; set; }

        public UILocalization UI { get; set; }

        public class UILocalization
        {
            public LocalizationValue<int, string, double> Password { get; set; }

            public WebLocalization Web { get; set; }

            public class WebLocalization
            {
                public LocalizationValue NoParam { get; set; }
                public LocalizationValue<int> OneParam { get; set; }
            }
        }
    }

    public class LocalizationValueDefinitionBuilderTests
    {
        IServiceProvider _di;

        public LocalizationValueDefinitionBuilderTests()
        {
            var services = new ServiceCollection().AddCulture();

            _di = services.BuildServiceProvider();
        }

        [Fact]
        public void FactMethodName_Scenario_ExpectedBehavior()
        {
            var key = LocalizationDefinitionConvention.GetKey(typeof(Localization.UILocalization).GetProperty(nameof(Localization.UILocalization.Password)));

            var defs = LocalizationDefinitionConvention.CreateLocalizationInstanceAsync(typeof(Localization),c => new ValueTask<string>("TEST")).GetAwaiter().GetResult();

            var localizationCache = _di.GetService<ILocalizationCache>();

            var value = new Localization
                        {
                                AppName = new LocalizationValue(new LocalizationValueDefinition("AppName"), localizationCache["AppName"]),
                                UI = new Localization.UILocalization
                                     {
                                             Password = new LocalizationValue<int, string, double>(new LocalizationFormattedValueDefinition<int, string, double>("UI.Password"), localizationCache["UI.Password"]),
                                             Web = new Localization.UILocalization.WebLocalization
                                                   {
                                                           NoParam = new LocalizationValue(new LocalizationValueDefinition("UI.Web.NoParam"), localizationCache["UI.Web.NoParam"]),
                                                           OneParam = new LocalizationValue<int>(new LocalizationFormattedValueDefinition<int>("UI.Web.OneParam"), localizationCache["UI.Web.OneParam"] )
                                                   }
                                     }
                        };
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForNotFormattedValue_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.Nest1.Nest2.NoParam;

            Assert.Equal("Nest1.Nest2.NoParam", definition.Key);
            Assert.Equal(0, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted1Value_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.Nest1.Nest2.OneParam;

            Assert.Equal("Nest1.Nest2.OneParam", definition.Key);
            Assert.Equal(1, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted2Value_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.TwoParam;

            Assert.Equal("TwoParam", definition.Key);
            Assert.Equal(2, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted3Value_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.Nest1.ThreeParam;

            Assert.Equal("Nest1.ThreeParam", definition.Key);
            Assert.Equal(3, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted4Value_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.Nest1.Nest2.FourParam;

            Assert.Equal("Nest1.Nest2.FourParam", definition.Key);
            Assert.Equal(4, definition.FormatTypes.Length);
        }
    }
}
