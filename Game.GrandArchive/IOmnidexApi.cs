using System.Text.Json.Serialization;
using Refit;

namespace Game.GrandArchive;

/// <summary>
/// Endpoints under the "Omnidex" tag.
/// </summary>
public interface IOmnidexApi
{
    /// <summary>
    /// GET /omnidex/events/{eventId} — returns an Omnidex event matching the provided ID.
    /// </summary>
    [Get("/omnidex/events/{eventId}")]
    Task<IApiResponse<OmnidexEvent>> GetEventAsync(long eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/players — returns a list of players participating in the event.
    /// </summary>
    [Get("/omnidex/events/{eventId}/players")]
    Task<IApiResponse<List<OmnidexEventPlayer>>> GetEventPlayersAsync(long eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/teams — returns a list of teams participating in the event (if applicable).
    /// </summary>
    [Get("/omnidex/events/{eventId}/teams")]
    Task<IApiResponse<List<OmnidexEventTeam>>> GetEventTeamsAsync(long eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/judges — returns a list of judges in the event.
    /// </summary>
    [Get("/omnidex/events/{eventId}/judges")]
    Task<IApiResponse<List<OmnidexEventJudge>>> GetEventJudgesAsync(long eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/standings — returns Swiss stage standings for the event.
    /// </summary>
    [Get("/omnidex/events/{eventId}/standings")]
    Task<IApiResponse<OmnidexEventStandings>> GetEventStandingsAsync(long eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/pairings — returns pairings for an event.
    /// By default it returns the latest stage and round.
    /// </summary>
    [Get("/omnidex/events/{eventId}/pairings")]
    Task<IApiResponse<OmnidexEventPairingsResponse>> GetEventPairingsAsync(
        long eventId,
        [AliasAs("round")] long? round = null,
        [AliasAs("stage")] long? stage = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/decklists — returns public decklist data for the event (if available).
    /// </summary>
    [Get("/omnidex/events/{eventId}/decklists")]
    Task<IApiResponse<List<OmnidexEventPlayerDecklist>>> GetEventDecklistsAsync(long eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// GET /omnidex/events/{eventId}/statistics — returns public statistics data for an Omnidex event (if available).
    /// </summary>
    [Get("/omnidex/events/{eventId}/statistics")]
    Task<IApiResponse<OmnidexEventStatistics>> GetEventStatisticsAsync(long eventId,
        CancellationToken cancellationToken = default);
}

public sealed class OmnidexEvent
{
    /// <summary>"ascent", "nationals", "regionals", "regular", "store-championships" or "worlds".</summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("decklists")]
    public bool Decklists { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>"team-standard-3v3", "draft" or "standard".</summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("host")]
    public OmnidexEventHost? Host { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("judges")]
    public List<long> Judges { get; set; } = [];

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("players")]
    public List<long> Players { get; set; } = [];

    [JsonPropertyName("ranked")]
    public bool Ranked { get; set; }

    /// <summary>"online" or "physical".</summary>
    [JsonPropertyName("setting")]
    public string? Setting { get; set; }

    /// <summary>"bo1", "bo3" or "bo5".</summary>
    [JsonPropertyName("singleEliminationMatchConfig")]
    public string? SingleEliminationMatchConfig { get; set; }

    [JsonPropertyName("singleEliminationCutSize")]
    public int SingleEliminationCutSize { get; set; }

    [JsonPropertyName("stages")]
    public List<OmnidexEventStage> Stages { get; set; } = [];

    /// <summary>"canceled", "canceled-reset", "canceled-suspended", "completable", "complete", "rsvp", "started" or "starting".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("startAt")]
    public string? StartAt { get; set; }

    [JsonPropertyName("startedAt")]
    public string? StartedAt { get; set; }

    /// <summary>"bo1", "bo3" or "bo5".</summary>
    [JsonPropertyName("swissMatchConfig")]
    public string? SwissMatchConfig { get; set; }

    [JsonPropertyName("swissRounds")]
    public int SwissRounds { get; set; }

    /// <summary>"single-elimination" or "swiss".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("vpMultiplier")]
    public double VpMultiplier { get; set; }

    /// <summary>Only present for team formats.</summary>
    [JsonPropertyName("teamSize")]
    public int? TeamSize { get; set; }

    /// <summary>Only present for team formats.</summary>
    [JsonPropertyName("teams")]
    public List<string>? Teams { get; set; }
}

public sealed class OmnidexEventHost
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("addressCountryCode")]
    public string? AddressCountryCode { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public sealed class OmnidexEventStage
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>"completable", "complete", "started" or "waiting".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>"single-elimination" or "swiss".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class OmnidexEventPlayer
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("cp")]
    public int Cp { get; set; }

    /// <summary>"ascendant", "bronze", "demigod", "diamond", "gold", "platinum", "silver" or "unranked".</summary>
    [JsonPropertyName("emblem")]
    public string? Emblem { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }
}

public sealed class OmnidexEventJudge : OmnidexEventPlayer
{
    [JsonPropertyName("judgeExperience")]
    public int JudgeExperience { get; set; }

    [JsonPropertyName("judgeLevel")]
    public int JudgeLevel { get; set; }
}

public sealed class OmnidexEventTeam
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("players")]
    public List<OmnidexEventTeamPlayer> Players { get; set; } = [];
}

public sealed class OmnidexEventTeamPlayer
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("slot")]
    public int Slot { get; set; }
}

public sealed class OmnidexEventStandings
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("rounds")]
    public OmnidexEventStandingsRounds? Rounds { get; set; }

    /// <summary>Contains player entries for 1v1 formats and team entries for team formats.</summary>
    [JsonPropertyName("standings")]
    public List<OmnidexEventStandingEntry> Standings { get; set; } = [];

    /// <summary>"completable", "complete", "started" or "waiting".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>Always "swiss".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public sealed class OmnidexEventStandingsRounds
{
    [JsonPropertyName("latest")]
    public int Latest { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

/// <summary>
/// A standings entry. Player entries have <see cref="Id"/>; team entries have <see cref="Name"/>.
/// Team-format player entries also have <see cref="Team"/> and <see cref="TeamSlot"/>.
/// </summary>
public sealed class OmnidexEventStandingEntry
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("team")]
    public string? Team { get; set; }

    [JsonPropertyName("teamSlot")]
    public int? TeamSlot { get; set; }

    [JsonPropertyName("tiebreaker")]
    public double Tiebreaker { get; set; }

    /// <summary>"active", "awaiting-decklist", "eliminated" or "winner".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("statsByes")]
    public int StatsByes { get; set; }

    [JsonPropertyName("statsDroppedRound")]
    public int? StatsDroppedRound { get; set; }

    [JsonPropertyName("statsDroppedStage")]
    public int? StatsDroppedStage { get; set; }

    [JsonPropertyName("statsGamesPlayed")]
    public int StatsGamesPlayed { get; set; }

    [JsonPropertyName("statsGamesWon")]
    public int StatsGamesWon { get; set; }

    [JsonPropertyName("statsLosses")]
    public int StatsLosses { get; set; }

    [JsonPropertyName("statsPercentGW")]
    public double StatsPercentGW { get; set; }

    [JsonPropertyName("statsPercentMW")]
    public double StatsPercentMW { get; set; }

    [JsonPropertyName("statsPercentOGW")]
    public double StatsPercentOGW { get; set; }

    [JsonPropertyName("statsPercentOMW")]
    public double StatsPercentOMW { get; set; }

    [JsonPropertyName("statsScore")]
    public double StatsScore { get; set; }

    [JsonPropertyName("statsTies")]
    public int StatsTies { get; set; }

    [JsonPropertyName("statsWins")]
    public int StatsWins { get; set; }
}

public sealed class OmnidexEventPairingsResponse
{
    [JsonPropertyName("pairings")]
    public List<OmnidexEventMatch> Pairings { get; set; } = [];

    [JsonPropertyName("round")]
    public OmnidexEventRoundWithTimer? Round { get; set; }

    [JsonPropertyName("stage")]
    public OmnidexEventStage? Stage { get; set; }
}

public sealed class OmnidexEventMatch
{
    [JsonPropertyName("completedAt")]
    public long? CompletedAt { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("pairing")]
    public List<OmnidexEventMatchPairing> Pairing { get; set; } = [];

    /// <summary>"byed", "complete", "started" or "waiting".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

public sealed class OmnidexEventMatchPairing
{
    [JsonPropertyName("dropped")]
    public bool Dropped { get; set; }

    [JsonPropertyName("eloChange")]
    public double EloChange { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("score")]
    public double Score { get; set; }

    /// <summary>"byed", "loser", "tied", "waiting" or "winner".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

public sealed class OmnidexEventRoundWithTimer
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>"complete", "started" or "waiting".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("timer")]
    public OmnidexEventRoundTimer? Timer { get; set; }
}

public sealed class OmnidexEventRoundTimer
{
    [JsonPropertyName("serverTime")]
    public string? ServerTime { get; set; }

    [JsonPropertyName("timerEnd")]
    public string? TimerEnd { get; set; }

    [JsonPropertyName("timerPaused")]
    public string? TimerPaused { get; set; }

    [JsonPropertyName("timerStart")]
    public string? TimerStart { get; set; }
}

public sealed class OmnidexEventPlayerDecklist
{
    [JsonPropertyName("decklist")]
    public OmnidexEventDecklist? Decklist { get; set; }

    [JsonPropertyName("player")]
    public long Player { get; set; }

    /// <summary>Always true.</summary>
    [JsonPropertyName("visible")]
    public bool Visible { get; set; }
}

public sealed class OmnidexEventDecklist
{
    [JsonPropertyName("main")]
    public List<OmnidexEventDecklistCard> Main { get; set; } = [];

    [JsonPropertyName("material")]
    public List<OmnidexEventDecklistCard> Material { get; set; } = [];

    [JsonPropertyName("sideboard")]
    public List<OmnidexEventDecklistCard> Sideboard { get; set; } = [];
}

public sealed class OmnidexEventDecklistCard
{
    [JsonPropertyName("card")]
    public string? Card { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}

/// <summary>
/// Event statistics. Which breakdowns are populated depends on
/// <see cref="DecklistsStatsRevealed"/> and <see cref="DecklistsTopNStatsRevealed"/>.
/// </summary>
public sealed class OmnidexEventStatistics
{
    [JsonPropertyName("decklistsAdvancedElementBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsAdvancedElementBreakdown { get; set; }

    [JsonPropertyName("decklistsBasicElementBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsBasicElementBreakdown { get; set; }

    [JsonPropertyName("decklistsChampionBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsChampionBreakdown { get; set; }

    [JsonPropertyName("decklistsStatsRevealed")]
    public bool DecklistsStatsRevealed { get; set; }

    [JsonPropertyName("decklistsTopNAdvancedElementBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsTopNAdvancedElementBreakdown { get; set; }

    [JsonPropertyName("decklistsTopNBasicElementBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsTopNBasicElementBreakdown { get; set; }

    [JsonPropertyName("decklistsTopNChampionBreakdown")]
    public OmnidexEventDecklistStatsNumericBreakdown? DecklistsTopNChampionBreakdown { get; set; }

    [JsonPropertyName("decklistsTopNRound")]
    public int? DecklistsTopNRound { get; set; }

    [JsonPropertyName("decklistsTopNStage")]
    public int? DecklistsTopNStage { get; set; }

    [JsonPropertyName("decklistsTopNStatsRevealed")]
    public bool DecklistsTopNStatsRevealed { get; set; }

    [JsonPropertyName("decklistsTopNValue")]
    public int? DecklistsTopNValue { get; set; }
}

public sealed class OmnidexEventDecklistStatsNumericBreakdown
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
}