using System.Text.Json;
using Game.GrandArchive;

namespace Game.Tests;

public class CardsResponseSerializationTests
{
    [Fact]
    public void Deserialize_MapsNestedCardAndEditionProperties()
    {
        const string json = """
                            {
                              "data": [
                                {
                                  "name": "Arisanna",
                                  "flavor": "The light answers.",
                                  "effect_raw": "On Enter: Gain 1 life.",
                                  "editions": [
                                    {
                                      "card_id": "card_123",
                                      "image": "/images/cards/arisanna.png",
                                      "set": {
                                        "id": "set_1",
                                        "name": "Dawn of Ashes",
                                        "prefix": "DOA",
                                        "release_date": "2024-01-05T00:00:00Z"
                                      }
                                    }
                                  ]
                                }
                              ],
                              "has_more": false,
                              "order": "asc",
                              "page": 1,
                              "page_size": 1,
                              "paginated_cards_count": 1,
                              "sort": "name",
                              "total_cards": 1,
                              "total_pages": 1
                            }
                            """;

        var response = JsonSerializer.Deserialize<CardsResponse>(json);

        Assert.NotNull(response);
        Assert.Single(response.Data);
        Assert.Equal("Arisanna", response.Data[0].Name);
        Assert.Equal("card_123", response.Data[0].Editions[0].CardId);
        Assert.Equal("DOA", response.Data[0].Editions[0].Set?.Prefix);
    }

    [Fact]
    public void NewModels_InitializeCollectionsToEmptyLists()
    {
        var card = new Card();
        var edition = new Edition();

        Assert.NotNull(card.Classes);
        Assert.NotNull(card.Editions);
        Assert.NotNull(card.Elements);
        Assert.NotNull(edition.CirculationTemplates);
        Assert.NotNull(edition.Circulations);
    }
}

