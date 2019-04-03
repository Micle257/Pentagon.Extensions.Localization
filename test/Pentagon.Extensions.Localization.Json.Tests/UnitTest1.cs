using System;
using Xunit;

namespace Pentagon.Extensions.Localization.Json.Tests
{
    using EntityFramework.Persistence;
    using Microsoft.Extensions.Logging.Abstractions;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var store = new CultureStore(NullLogger<CultureStore>.Instance, null);

            var s = store.GetAllResourcesAsync("cs-CZ");
        }
    }
}
