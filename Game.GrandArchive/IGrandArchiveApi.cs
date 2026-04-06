using System.Text.Json.Serialization;
using Refit;

namespace Game.GrandArchive;

public interface IGrandArchiveApi
{
    [Get("/cards/search")]
    Task<IApiResponse<CardsResponse>> SearchAsync(string name);
}

public sealed class CardsResponse
{
    [JsonPropertyName("data")]
    public List<Card> Data { get; set; } = [];

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("order")]
    public string? Order { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("paginated_cards_count")]
    public int PaginatedCardsCount { get; set; }

    [JsonPropertyName("sort")]
    public string? Sort { get; set; }

    [JsonPropertyName("total_cards")]
    public int TotalCards { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
}

public sealed class Card
{
    [JsonPropertyName("classes")]
    public List<string> Classes { get; set; } = [];

    [JsonPropertyName("cost")]
    public Cost? Cost { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("durability")]
    public int? Durability { get; set; }

    [JsonPropertyName("editions")]
    public List<Edition> Editions { get; set; } = [];

    [JsonPropertyName("elements")]
    public List<string> Elements { get; set; } = [];

    [JsonPropertyName("effect")]
    public string? Effect { get; set; }

    [JsonPropertyName("effect_raw")]
    public string? EffectRaw { get; set; }

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    [JsonPropertyName("last_update")]
    public DateTimeOffset LastUpdate { get; set; }

    [JsonPropertyName("legality")]
    public object? Legality { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("life")]
    public int? Life { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("power")]
    public int? Power { get; set; }

    [JsonPropertyName("referenced_by")]
    public List<object> ReferencedBy { get; set; } = [];

    [JsonPropertyName("references")]
    public List<object> References { get; set; } = [];

    [JsonPropertyName("result_editions")]
    public List<Edition> ResultEditions { get; set; } = [];

    [JsonPropertyName("rule")]
    public List<object> Rule { get; set; } = [];

    [JsonPropertyName("speed")]
    public object? Speed { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("subtypes")]
    public List<string> Subtypes { get; set; } = [];

    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = [];

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}

public sealed class Cost
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    // JSON has "value": "1" (string), so keep it as string to avoid surprises.
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public sealed class Edition
{
    [JsonPropertyName("card_id")]
    public string? CardId { get; set; }

    [JsonPropertyName("circulationTemplates")]
    public List<CirculationTemplate> CirculationTemplates { get; set; } = [];

    [JsonPropertyName("circulations")]
    public List<object> Circulations { get; set; } = [];

    [JsonPropertyName("collaborators")]
    public List<object> Collaborators { get; set; } = [];

    [JsonPropertyName("collector_number")]
    public string? CollectorNumber { get; set; }

    [JsonPropertyName("configuration")]
    public string? Configuration { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("effect")]
    public string? Effect { get; set; }

    [JsonPropertyName("effect_html")]
    public string? EffectHtml { get; set; }

    [JsonPropertyName("effect_raw")]
    public string? EffectRaw { get; set; }

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; set; }

    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("last_update")]
    public DateTimeOffset LastUpdate { get; set; }

    [JsonPropertyName("orientation")]
    public string? Orientation { get; set; }

    [JsonPropertyName("other_orientations")]
    public List<object> OtherOrientations { get; set; } = [];

    [JsonPropertyName("rarity")]
    public int? Rarity { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("set")]
    public CardSet? Set { get; set; }

    [JsonPropertyName("thema_charm_foil")]
    public object? ThemaCharmFoil { get; set; }

    [JsonPropertyName("thema_ferocity_foil")]
    public object? ThemaFerocityFoil { get; set; }

    [JsonPropertyName("thema_foil")]
    public object? ThemaFoil { get; set; }

    [JsonPropertyName("thema_grace_foil")]
    public object? ThemaGraceFoil { get; set; }

    [JsonPropertyName("thema_mystique_foil")]
    public object? ThemaMystiqueFoil { get; set; }

    [JsonPropertyName("thema_valor_foil")]
    public object? ThemaValorFoil { get; set; }

    [JsonPropertyName("thema_charm_nonfoil")]
    public object? ThemaCharmNonfoil { get; set; }

    [JsonPropertyName("thema_ferocity_nonfoil")]
    public object? ThemaFerocityNonfoil { get; set; }

    [JsonPropertyName("thema_grace_nonfoil")]
    public object? ThemaGraceNonfoil { get; set; }

    [JsonPropertyName("thema_mystique_nonfoil")]
    public object? ThemaMystiqueNonfoil { get; set; }

    [JsonPropertyName("thema_nonfoil")]
    public object? ThemaNonfoil { get; set; }

    [JsonPropertyName("thema_valor_nonfoil")]
    public object? ThemaValorNonfoil { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}

public sealed class CirculationTemplate
{
    [JsonPropertyName("kind")]
    public string? Kind { get; set; }

    [JsonPropertyName("population")]
    public int? Population { get; set; }

    [JsonPropertyName("population_operator")]
    public string? PopulationOperator { get; set; }

    [JsonPropertyName("printing")]
    public bool? Printing { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("edition_id")]
    public string? EditionId { get; set; }

    [JsonPropertyName("foil")]
    public bool? Foil { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("variants")]
    public List<object> Variants { get; set; } = [];

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public sealed class CardSet
{
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("last_update")]
    public DateTimeOffset LastUpdate { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }
}