using System;
using Xunit;

namespace Pentagon.Extensions.Localization.Json.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging.Abstractions;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var se = new ServiceCollection()
                    .AddLogging();

            se.AddJsonCultureLocalization();

            var di = se.BuildServiceProvider();

            var context = di.GetService<ICultureContextWriter>();

            context.SetLanguage("cs-CZ");

            var cache = di.GetService<ILocalizationCache>();

            var mss = cache["first"];
        }

        [Fact]
        public void Test2()
        {
            JsonLocalization.LoadJsonFromDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Localization"));

            var resource = JsonLocalization.GetCachedResource(CultureInfo.GetCultureInfo("sk-SK"), "ShortCut", true);


        }
    }
}
