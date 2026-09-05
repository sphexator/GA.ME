using System.Text.Json.Serialization;
using Refit;

namespace Game.GrandArchive;

/// <summary>
/// Endpoints under the "Multiple cards" tag: search, autocomplete and random.
/// </summary>
public interface IMultiCardsApi
{
    /// <summary>
    /// GET /cards/search — returns cards matching the provided query parameters.
    /// </summary>
    [Get("/cards/search")]
    Task<IApiResponse<CardsResponse>> SearchAsync([Query] CardsSearchQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /cards/autocomplete — returns a list of up to 10 cards matching the given partial name.
    /// </summary>
    [Get("/cards/autocomplete")]
    Task<IApiResponse<List<AutocompleteCard>>> AutocompleteAsync([AliasAs("name")] string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /cards/random — returns random cards (default 8, min 1, max 50).
    /// </summary>
    [Get("/cards/random")]
    Task<IApiResponse<List<Card>>> GetRandomAsync([AliasAs("amount")] int? amount = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Query parameters for GET /cards/search. All properties are optional;
/// null values are omitted from the query string.
/// </summary>
public sealed class CardsSearchQuery
{
    /// <summary>Get cards with any `class` value included in this list.</summary>
    [AliasAs("class")]
    public List<string>? Class { get; set; }

    /// <summary>How cards are targeted by <see cref="Class"/>: "AND" (all values) or "OR" (any value). Default: "OR".</summary>
    [AliasAs("class_logic")]
    public string? ClassLogic { get; set; }

    /// <summary>Include or exclude editions which have collaborators.</summary>
    [AliasAs("collab")]
    public bool? Collab { get; set; }

    /// <summary>Get a card with an edition's `collector_number` that exactly matches this string.</summary>
    [AliasAs("collector_number")]
    public string? CollectorNumber { get; set; }

    /// <summary>Find cards using a specific configuration: "default" or "flip".</summary>
    [AliasAs("configuration")]
    public string? Configuration { get; set; }

    /// <summary>Get a card with an edition's `effect_raw` similar to this string.</summary>
    [AliasAs("edition_effect")]
    public string? EditionEffect { get; set; }

    /// <summary>Get a card with an edition's `flavor` similar to this string.</summary>
    [AliasAs("edition_flavor")]
    public string? EditionFlavor { get; set; }

    /// <summary>Get a card with an edition's `last_update` date greater than this value.</summary>
    [AliasAs("edition_last_update")]
    public DateTime? EditionLastUpdate { get; set; }

    /// <summary>Get a card with an edition's `slug` that exactly matches this string.</summary>
    [AliasAs("edition_slug")]
    public string? EditionSlug { get; set; }

    /// <summary>Get a card with `effect_raw` or edition `effect_raw` similar to this string.</summary>
    [AliasAs("effect")]
    public string? Effect { get; set; }

    /// <summary>Get cards with any `elements` value included in this list. "null" matches cards with no element.</summary>
    [AliasAs("element")]
    public List<string>? Element { get; set; }

    /// <summary>How cards are targeted by <see cref="Element"/>: "AND" (all values) or "OR" (any value). Default: "OR".</summary>
    [AliasAs("element_logic")]
    public string? ElementLogic { get; set; }

    /// <summary>Get a card with a `flavor` similar to this string.</summary>
    [AliasAs("flavor")]
    public string? Flavor { get; set; }

    /// <summary>Get a card with an edition's `illustrator` that exactly matches this string. "null" matches no illustrator.</summary>
    [AliasAs("illustrator")]
    public string? Illustrator { get; set; }

    /// <summary>Get a card with an edition's set's `language` that exactly matches any value in this list.</summary>
    [AliasAs("language")]
    public List<string>? Language { get; set; }

    /// <summary>Get a card with a `last_update` date greater than this value.</summary>
    [AliasAs("last_update")]
    public DateTime? LastUpdate { get; set; }

    /// <summary>Game format to target with <see cref="LegalityLimit"/>: "DRAFT", "PANTHEON" or "STANDARD".</summary>
    [AliasAs("legality_format")]
    public string? LegalityFormat { get; set; }

    /// <summary>Exact card limit, used in parallel with <see cref="LegalityFormat"/>.</summary>
    [AliasAs("legality_limit")]
    public int? LegalityLimit { get; set; }

    /// <summary>Whether a card is legal or restricted ("ANY", "LEGAL", "RESTRICTED"). Ignored if <see cref="LegalityLimit"/> is set. Default: "ANY".</summary>
    [AliasAs("legality_state")]
    public string? LegalityState { get; set; }

    /// <summary>Get a card with a `name` similar to this string.</summary>
    [AliasAs("name")]
    public string? Name { get; set; }

    /// <summary>Order of the results: "ASC" (default) or "DESC".</summary>
    [AliasAs("order")]
    public string? Order { get; set; }

    /// <summary>Move to a specific page of results. Default: 1.</summary>
    [AliasAs("page")]
    public int? Page { get; set; }

    /// <summary>Results per page (1-50). Default: 50.</summary>
    [AliasAs("page_size")]
    public int? PageSize { get; set; }

    /// <summary>Get cards with any edition's set's `prefix` included in this list.</summary>
    [AliasAs("prefix")]
    public List<string>? Prefix { get; set; }

    /// <summary>Get a card with an edition's `rarity` that exactly matches this number.</summary>
    [AliasAs("rarity")]
    public int? Rarity { get; set; }

    /// <summary>Get a card with a `rule` whose title or description contains this case-insensitive string.</summary>
    [AliasAs("rule")]
    public string? Rule { get; set; }

    /// <summary>Get a card with a `rule` whose title exactly matches this case-insensitive string. "null" matches rules with no title.</summary>
    [AliasAs("rule_title")]
    public string? RuleTitle { get; set; }

    /// <summary>Split each `result_edition` into its own dedicated data entry instead of grouping editions by card name.</summary>
    [AliasAs("separate_editions")]
    public bool? SeparateEditions { get; set; }

    /// <summary>Get a card with a `slug` that exactly matches this string.</summary>
    [AliasAs("slug")]
    public string? Slug { get; set; }

    /// <summary>Get a card by its speed stat: "fast", "none" or "slow".</summary>
    [AliasAs("speed")]
    public List<string>? Speed { get; set; }

    /// <summary>Get a card whose numeric stats are contained within a semicolon-delimited list of values.</summary>
    [AliasAs("stats")]
    public string? Stats { get; set; }

    /// <summary>Primary card/edition field the results are sorted by. Default: "collector_number".</summary>
    [AliasAs("sort")]
    public string? Sort { get; set; }

    /// <summary>Get cards with any `subtype` value included in this list.</summary>
    [AliasAs("subtype")]
    public List<string>? Subtype { get; set; }

    /// <summary>How cards are targeted by <see cref="Subtype"/>: "AND" (all values) or "OR" (any value). Default: "OR".</summary>
    [AliasAs("subtype_logic")]
    public string? SubtypeLogic { get; set; }

    /// <summary>Get cards with any `type` value included in this list.</summary>
    [AliasAs("type")]
    public List<string>? Type { get; set; }

    /// <summary>How cards are targeted by <see cref="Type"/>: "AND" (all values) or "OR" (any value). Default: "OR".</summary>
    [AliasAs("type_logic")]
    public string? TypeLogic { get; set; }
}

/// <summary>
/// Simplified card returned by GET /cards/autocomplete.
/// </summary>
public sealed class AutocompleteCard
{
    [JsonPropertyName("classes")]
    public List<string> Classes { get; set; } = [];

    [JsonPropertyName("editions")]
    public List<AutocompleteEdition> Editions { get; set; } = [];

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("subtypes")]
    public List<string> Subtypes { get; set; } = [];

    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = [];

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}

public sealed class AutocompleteEdition
{
    [JsonPropertyName("card_id")]
    public string? CardId { get; set; }

    [JsonPropertyName("collector_number")]
    public string? CollectorNumber { get; set; }

    [JsonPropertyName("effect")]
    public string? Effect { get; set; }

    [JsonPropertyName("effect_raw")]
    public string? EffectRaw { get; set; }

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    [JsonPropertyName("illustrator")]
    public string? Illustrator { get; set; }

    [JsonPropertyName("rarity")]
    public int? Rarity { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}