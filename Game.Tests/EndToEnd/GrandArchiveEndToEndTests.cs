using Game.GrandArchive;
using Refit;

namespace Game.Tests;

public class LiveGrandArchiveE2ETests
{
    [Fact]
    public async Task Search_LiveApiSmokeTest_WhenEnabled()
    {
        // Keep CI stable: only run when explicitly enabled.
        if (!string.Equals(Environment.GetEnvironmentVariable("GA_E2E_LIVE"), "1", StringComparison.Ordinal))
        {
            return;
        }

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.gatcg.com")
        };

        var api = RestService.For<IGrandArchiveApi>(httpClient);
        var result = await api.Search("Rai");

        Assert.NotNull(result.Content);
        Assert.NotEmpty(result.Content!.Data);
    }
}