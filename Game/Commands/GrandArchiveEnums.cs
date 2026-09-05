using Disqord.Bot.Commands.Application;

namespace Game.Commands;

/// <summary>
/// How multi-value filters (class, element, type, subtype) are combined.
/// </summary>
public enum FilterLogic
{
    [ChoiceName("OR - any of the values")]
    Or,

    [ChoiceName("AND - all of the values")]
    And
}

/// <summary>
/// Card speed stat values supported by the API.
/// </summary>
public enum CardSpeed
{
    [ChoiceName("fast")]
    Fast,

    [ChoiceName("none")]
    None,

    [ChoiceName("slow")]
    Slow
}

/// <summary>
/// Card configurations supported by the API.
/// </summary>
public enum CardConfiguration
{
    [ChoiceName("default")]
    Default,

    [ChoiceName("flip")]
    Flip
}

/// <summary>
/// Result ordering.
/// </summary>
public enum ResultOrder
{
    [ChoiceName("Ascending")]
    Asc,

    [ChoiceName("Descending")]
    Desc
}

/// <summary>
/// Game formats for legality searches.
/// </summary>
public enum LegalityFormat
{
    [ChoiceName("DRAFT")]
    Draft,

    [ChoiceName("PANTHEON")]
    Pantheon,

    [ChoiceName("STANDARD")]
    Standard
}

/// <summary>
/// Legality states for legality searches.
/// </summary>
public enum LegalityState
{
    [ChoiceName("ANY")]
    Any,

    [ChoiceName("LEGAL")]
    Legal,

    [ChoiceName("RESTRICTED")]
    Restricted
}

public static class GrandArchiveEnumExtensions
{
    public static string ToApiValue(this FilterLogic value)
        => value == FilterLogic.And ? "AND" : "OR";

    public static string ToApiValue(this CardSpeed value)
        => value switch
        {
            CardSpeed.Fast => "fast",
            CardSpeed.None => "none",
            _ => "slow"
        };

    public static string ToApiValue(this CardConfiguration value)
        => value == CardConfiguration.Flip ? "flip" : "default";

    public static string ToApiValue(this ResultOrder value)
        => value == ResultOrder.Desc ? "DESC" : "ASC";

    public static string ToApiValue(this LegalityFormat value)
        => value switch
        {
            LegalityFormat.Draft => "DRAFT",
            LegalityFormat.Pantheon => "PANTHEON",
            _ => "STANDARD"
        };

    public static string ToApiValue(this LegalityState value)
        => value switch
        {
            LegalityState.Legal => "LEGAL",
            LegalityState.Restricted => "RESTRICTED",
            _ => "ANY"
        };
}