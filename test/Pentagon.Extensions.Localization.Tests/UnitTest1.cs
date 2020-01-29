namespace Pentagon.Extensions.Localization.Tests
{
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public static class Local
    {
        public static class Nest1
        {
            public static class Nest2
            {
                public static LocalizationValueDefinition NoParam => LocalizationValueDefinitionBuilder.FromConvention(() => NoParam);

                public static LocalizationFormattedValueDefinition<int> OneParam = LocalizationValueDefinitionBuilder.FromConvention(() => OneParam);

                public static LocalizationFormattedValueDefinition<int, string, double, char> FourParam = LocalizationValueDefinitionBuilder.FromConvention(() => FourParam);
            }

            public static LocalizationFormattedValueDefinition<int, string, double> ThreeParam = LocalizationValueDefinitionBuilder.FromConvention(() => ThreeParam);
        }

        public static LocalizationFormattedValueDefinition<int, string> TwoParam => LocalizationValueDefinitionBuilder.FromConvention(() => TwoParam);
    }

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

    public class LocalizationValueDefinitionBuilderTests
    {
        public LocalizationValueDefinitionBuilderTests()
        {
            var services = new ServiceCollection().AddCulture();
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForNotFormattedValue_ReturnsCorrectDefinition()
        {
            var definition = Local.Nest1.Nest2.NoParam;

            Assert.Equal("Local.Nest1.Nest2.NoParam", definition.Key);
            Assert.Equal(0, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted1Value_ReturnsCorrectDefinition()
        {
            var definition = Local.Nest1.Nest2.OneParam;

            Assert.Equal("Local.Nest1.Nest2.OneParam", definition.Key);
            Assert.Equal(1, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted2Value_ReturnsCorrectDefinition()
        {
            var definition = Local.TwoParam;

            Assert.Equal("Local.TwoParam", definition.Key);
            Assert.Equal(2, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted3Value_ReturnsCorrectDefinition()
        {
            var definition = Local.Nest1.ThreeParam;

            Assert.Equal("Local.Nest1.ThreeParam", definition.Key);
            Assert.Equal(3, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConvention_ForFormatted4Value_ReturnsCorrectDefinition()
        {
            var definition = Local.Nest1.Nest2.FourParam;

            Assert.Equal("Local.Nest1.Nest2.FourParam", definition.Key);
            Assert.Equal(4, definition.FormatTypes.Length);
        }

        [Fact]
        public void LocalizationKeyValueConventionFromAttribute_ForFormatted4Value_ReturnsCorrectDefinition()
        {
            var definition = LocalMorePure.Nest1.Nest2.FourParam;

            Assert.Equal("LocalMorePure.Nest1.Nest2.FourParam", definition.Key);
            Assert.Equal(4, definition.FormatTypes.Length);
        }
    }
}
