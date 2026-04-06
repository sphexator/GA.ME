using System.Net;
using System.Text;
using Game.GrandArchive;
using Refit;

namespace Game.Tests;

public class GrandArchiveApiIntegrationTests
{
    [Fact]
    public async Task Search_SendsExpectedRequest_AndBindsResponse()
    {
        var handler = new RecordingHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    {
                      "data": [
                        {
                          "name": "Rai, Storm Seer",
                          "effect_raw": "On Enter: Draw a card.",
                          "editions": [
                            {
                              "card_id": "rai_storm_seer",
                              "image": "/images/cards/rai_storm_seer.png"
                            }
                          ]
                        }
                      ],
                      "has_more": false,
                      "page": 1,
                      "page_size": 1,
                      "paginated_cards_count": 1,
                      "total_cards": 1,
                      "total_pages": 1
                    }
                    """,
                    Encoding.UTF8,
                    "application/json")
            });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.gatcg.com")
        };

        var api = RestService.For<IGrandArchiveApi>(httpClient);

        var result = await api.SearchAsync("Rai Storm");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal("/cards/search", handler.LastRequest!.RequestUri!.AbsolutePath);
        Assert.Equal("name=Rai%20Storm", handler.LastRequest.RequestUri!.Query.TrimStart('?'));

        Assert.NotNull(result.Content);
        Assert.Single(result.Content!.Data);
        Assert.Equal("Rai, Storm Seer", result.Content.Data[0].Name);
        Assert.Equal("rai_storm_seer", result.Content.Data[0].Editions[0].CardId);
    }

    private sealed class RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(responder(request));
        }
    }
}

