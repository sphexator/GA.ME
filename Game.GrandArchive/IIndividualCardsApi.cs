using Refit;

namespace Game.GrandArchive;

/// <summary>
/// Endpoints under the "Individual cards" tag.
/// </summary>
public interface IIndividualCardsApi
{
    /// <summary>
    /// GET /cards/{slug} — returns a card matching the provided slug.
    /// </summary>
    [Get("/cards/{slug}")]
    Task<IApiResponse<Card>> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /cards/edition/{editionSlugOrUuid} — returns a card matching the provided edition slug or edition UUID.
    /// </summary>
    [Get("/cards/edition/{editionSlugOrUuid}")]
    Task<IApiResponse<Card>> GetByEditionAsync(string editionSlugOrUuid, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /cards/{setPrefix}/{collectorNumber} — returns a card matching the provided set prefix and collector number.
    /// </summary>
    [Get("/cards/{setPrefix}/{collectorNumber}")]
    Task<IApiResponse<Card>> GetBySetAndCollectorNumberAsync(string setPrefix, string collectorNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /cards/images/{filename} — returns a card edition's image. Set <paramref name="rounded"/> to round the corners.
    /// </summary>
    [Get("/cards/images/{filename}")]
    Task<HttpContent> GetImageAsync(string filename, [AliasAs("rounded")] bool? rounded = null,
        CancellationToken cancellationToken = default);
}