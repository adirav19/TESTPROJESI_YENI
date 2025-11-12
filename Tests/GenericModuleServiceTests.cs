using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TESTPROJESI.Core.Mapping;
using TESTPROJESI.Services.Base;
using TESTPROJESI.Services.Interfaces;
using Xunit;

namespace TESTPROJESI.Tests
{
    public class GenericModuleServiceTests
    {
        [Fact]
        public void BuildDefaultListUrl_UsesConfiguredDefaults()
        {
            var options = new ModuleServiceOptions
            {
                DefaultLimit = 25,
                DefaultSortField = "Name",
                DefaultSortDescending = true,
                DefaultQueryParameters = new Dictionary<string, string>
                {
                    ["status"] = "active"
                }
            };

            var service = new TestModuleService(options);

            var url = service.BuildUrl();

            Assert.StartsWith("/test?", url);
            Assert.Contains("limit=25", url);
            Assert.Contains("sort=Name%20DESC", url);
            Assert.Contains("status=active", url);
        }
    }

    internal sealed class TestModuleService : GenericModuleService<TestDto>
    {
        public TestModuleService(ModuleServiceOptions options)
            : base(
                  new StubApiService(),
                  new StubTokenManager(),
                  NullLogger.Instance,
                  new StubMapper(),
                  "/test",
                  options)
        {
        }

        public string BuildUrl() => BuildDefaultListUrl();
    }

    internal sealed class StubApiService : IBaseApiService
    {
        public Task<bool> DeleteAsync(string endpoint, string token = null) =>
            throw new NotImplementedException();

        public Task<T?> GetAsync<T>(string endpoint, string token = null) =>
            throw new NotImplementedException();

        public Task<T?> PostAsync<T>(string endpoint, object data, string token = null) =>
            throw new NotImplementedException();

        public Task<T?> PutAsync<T>(string endpoint, object data, string token = null) =>
            throw new NotImplementedException();
    }

    internal sealed class StubTokenManager : ITokenManager
    {
        public Task<string> GetTokenAsync() => Task.FromResult("token");

        public Task<string> RefreshTokenAsync() => Task.FromResult("token");
    }

    internal sealed class StubMapper : IMapper<JsonElement, TestDto>
    {
        public IEnumerable<TestDto> MapList(IEnumerable<JsonElement> sources) => Array.Empty<TestDto>();

        public TestDto Map(JsonElement source) => new();

        public JsonElement MapBack(TestDto destination) => default;
    }

    internal sealed class TestDto
    {
    }
}
