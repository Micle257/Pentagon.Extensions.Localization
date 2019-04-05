using System;
using Xunit;

namespace Pentagon.Extensions.Localization.Json.Tests
{
    using System.Globalization;
    using EntityFramework;
    using EntityFramework.Persistence;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
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

            var context = di.GetService<ICultureContext>();

            context.SetLanguage("cs-CZ");

            var cache = di.GetService<ILocalizationCache>();

            var mss = cache["first"];
        }
    }
}
