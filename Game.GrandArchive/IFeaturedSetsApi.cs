using System.Text.Json.Serialization;
using Refit;

namespace Game.GrandArchive;

/// <summary>
/// Endpoints under the "Featured set groups" tag.
/// </summary>
public interface IFeaturedSetsApi
{
    /// <summary>
    /// GET /featured-sets — returns a list of featured set groups and the sets they contain.
    /// </summary>
    [Get("/featured-sets")]
    Task<IApiResponse<List<FeaturedSetGroup>>> GetFeaturedSetsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /featured-sets/images/{filename} — returns a featured set image.
    /// </summary>
    [Get("/featured-sets/images/{filename}")]
    Task<HttpContent> GetImageAsync(string filename, CancellationToken cancellationToken = default);
}

public sealed class FeaturedSetGroup
{
    [JsonPropertyName("image")]
    public string? Image { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("sets")]
    public List<CardSet> Sets { get; set; } = [];

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }
}