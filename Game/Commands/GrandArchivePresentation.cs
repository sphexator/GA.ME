using System.Text;
using Disqord;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Game.GrandArchive;

namespace Game.Commands;

/// <summary>
/// Builds Discord embeds and paged menus for Grand Archive API data.
/// </summary>
public static class GrandArchivePresentation
{
    public const string ApiCredit = "Data provided by the Grand Archive Index API";

    private const int LinesPerPage = 15;
    private const int MaxDescriptionLength = 4096;

    public static Page[] BuildCardPages(IReadOnlyList<Card> cards)
    {
        var pages = new Page[cards.Count];
        for (var i = 0; i < cards.Count; i++)
        {
            var embed = BuildCardEmbed(cards[i])
                .WithFooter($"Page {i + 1}/{cards.Count} - {ApiCredit}");
            pages[i] = new Page().WithEmbeds(embed);
        }

        return pages;
    }

    public static LocalEmbed BuildCardEmbed(Card card)
    {
        var embed = new LocalEmbed()
            .WithTitle(CardPresentationBuilder.BuildTitle(card))
            .WithDescription(CardPresentationBuilder.BuildDescription(card));

        var imageUrl = CardPresentationBuilder.BuildImageUrl(card.Editions.FirstOrDefault());
        if (imageUrl is not null)
        {
            embed = embed.WithImageUrl(imageUrl);
        }

        return embed;
    }

    public static Page[] BuildFeaturedSetPages(IReadOnlyList<FeaturedSetGroup> groups)
    {
        var pages = new Page[groups.Count];
        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var description = group.Sets.Count == 0
                ? "No sets in this group."
                : string.Join('\n', group.Sets.Select(FormatSet));

            var embed = new LocalEmbed()
                .WithTitle(group.Name ?? "Featured Sets")
                .WithDescription(Truncate(description))
                .WithFooter($"Page {i + 1}/{groups.Count} - {ApiCredit}");

            var imageUrl = BuildImageUrl(group.Image);
            if (imageUrl is not null)
            {
                embed = embed.WithImageUrl(imageUrl);
            }

            pages[i] = new Page().WithEmbeds(embed);
        }

        return pages;
    }

    public static LocalEmbed BuildEventEmbed(OmnidexEvent ev)
    {
        var embed = new LocalEmbed()
            .WithTitle(ev.Name ?? $"Event {ev.Id}");

        if (!string.IsNullOrWhiteSpace(ev.Description))
        {
            embed = embed.WithDescription(Truncate(ev.Description, 2000));
        }

        if (!string.IsNullOrWhiteSpace(ev.Url) && Uri.TryCreate(ev.Url, UriKind.Absolute, out _))
        {
            embed = embed.WithUrl(ev.Url);
        }

        var fields = new List<LocalEmbedField>();
        AddField(fields, "ID", ev.Id.ToString());
        AddField(fields, "Category", ev.Category);
        AddField(fields, "Format", ev.Format);
        AddField(fields, "Status", ev.Status);
        AddField(fields, "Type", ev.Type);
        AddField(fields, "Setting", ev.Setting);
        AddField(fields, "Ranked", ev.Ranked ? "Yes" : "No");
        AddField(fields, "Decklists", ev.Decklists ? "Public" : "Hidden");

        if (ev.SwissRounds > 0)
        {
            AddField(fields, "Swiss Rounds", $"{ev.SwissRounds} ({ev.SwissMatchConfig ?? "bo?"})");
        }

        if (ev.SingleEliminationCutSize > 0)
        {
            AddField(fields, "Top Cut", $"{ev.SingleEliminationCutSize} ({ev.SingleEliminationMatchConfig ?? "bo?"})");
        }

        if (ev.TeamSize is not null)
        {
            AddField(fields, "Team Size", ev.TeamSize.Value.ToString());
        }

        AddField(fields, "Starts At", ev.StartAt);

        if (ev.Host is not null)
        {
            var host = ev.Host.Name ?? $"Host {ev.Host.Id}";
            if (!string.IsNullOrWhiteSpace(ev.Host.Address))
            {
                host += $" ({ev.Host.Address})";
            }

            AddField(fields, "Host", host);
        }

        AddField(fields, "Players", ev.Players.Count.ToString());

        return embed
            .WithFields(fields)
            .WithFooter(ApiCredit);
    }

    public static Page[] BuildPlayerPages(IReadOnlyList<OmnidexEventPlayer> players)
    {
        var lines = players
            .Select(p =>
                $"#{p.Rank} {p.Username ?? $"Player {p.Id}"} ({p.Country ?? "??"}) - {p.Emblem ?? "unranked"}, {p.Cp} CP")
            .ToArray();
        return BuildTextPages("Players", lines);
    }

    public static Page[] BuildJudgePages(IReadOnlyList<OmnidexEventJudge> judges)
    {
        var lines = judges
            .Select(j => $"{j.Username ?? $"Judge {j.Id}"} - Level {j.JudgeLevel}, {j.JudgeExperience} XP")
            .ToArray();
        return BuildTextPages("Judges", lines);
    }

    public static Page[] BuildTeamPages(IReadOnlyList<OmnidexEventTeam> teams)
    {
        var lines = teams
            .Select(t =>
                $"**{t.Name ?? "Unnamed team"}**\n{string.Join(" · ", t.Players.Select(p => $"Slot {p.Slot}: {p.Id}"))}")
            .ToArray();
        return BuildTextPages("Teams", lines);
    }

    public static Page[] BuildStandingsPages(OmnidexEventStandings standings)
    {
        var title = $"Standings ({standings.Status ?? "unknown"})";
        if (standings.Rounds is not null)
        {
            title += $" - Round {standings.Rounds.Latest}/{standings.Rounds.Total}";
        }

        var lines = new List<string>(standings.Standings.Count);
        for (var i = 0; i < standings.Standings.Count; i++)
        {
            var entry = standings.Standings[i];
            var name = entry.Name ?? (entry.Id is not null ? $"Player {entry.Id}" : "Unknown");
            if (entry.Team is not null)
            {
                name += $" ({entry.Team}, slot {entry.TeamSlot})";
            }

            var record = $"{entry.StatsWins}-{entry.StatsLosses}-{entry.StatsTies}";
            var line =
                $"#{i + 1} {name} - {record}, {entry.StatsScore:0.#} pts, MW {entry.StatsPercentMW:0.0}%, OMW {entry.StatsPercentOMW:0.0}%";
            if (!string.IsNullOrWhiteSpace(entry.Status))
            {
                line += $" [{entry.Status}]";
            }

            lines.Add(line);
        }

        return BuildTextPages(title, lines);
    }

    public static Page[] BuildPairingsPages(OmnidexEventPairingsResponse response)
    {
        var title = "Pairings";
        if (response.Stage is not null)
        {
            title += $" - Stage {response.Stage.Id} ({response.Stage.Type ?? "unknown"})";
        }

        if (response.Round is not null)
        {
            title += $", Round {response.Round.Id} ({response.Round.Status ?? "unknown"})";
        }

        var lines = response.Pairings
            .Select(FormatMatch)
            .ToArray();
        return BuildTextPages(title, lines);
    }

    public static Page[] BuildDecklistPages(IReadOnlyList<OmnidexEventPlayerDecklist> decklists)
    {
        var pages = new Page[decklists.Count];
        for (var i = 0; i < decklists.Count; i++)
        {
            var decklist = decklists[i].Decklist;
            var builder = new StringBuilder();
            AppendDecklistSection(builder, "Material", decklist?.Material);
            AppendDecklistSection(builder, "Main", decklist?.Main);
            AppendDecklistSection(builder, "Sideboard", decklist?.Sideboard);

            var description = builder.Length == 0 ? "No decklist available." : builder.ToString();
            var embed = new LocalEmbed()
                .WithTitle($"Decklist - Player {decklists[i].Player}")
                .WithDescription(Truncate(description))
                .WithFooter($"Page {i + 1}/{decklists.Count} - {ApiCredit}");
            pages[i] = new Page().WithEmbeds(embed);
        }

        return pages;
    }

    public static LocalEmbed BuildStatisticsEmbed(OmnidexEventStatistics stats)
    {
        var fields = new List<LocalEmbedField>();

        if (stats.DecklistsStatsRevealed)
        {
            AddField(fields, "Decklists", FormatBreakdowns(
                ("Basic Elements", stats.DecklistsBasicElementBreakdown),
                ("Advanced Elements", stats.DecklistsAdvancedElementBreakdown),
                ("Champions", stats.DecklistsChampionBreakdown)));
        }

        if (stats.DecklistsTopNStatsRevealed)
        {
            var topN = $"Top {stats.DecklistsTopNValue?.ToString() ?? "N"}";
            if (stats.DecklistsTopNStage is not null || stats.DecklistsTopNRound is not null)
            {
                topN +=
                    $" (stage {stats.DecklistsTopNStage?.ToString() ?? "?"}, round {stats.DecklistsTopNRound?.ToString() ?? "?"})";
            }

            AddField(fields, topN, FormatBreakdowns(
                ("Basic Elements", stats.DecklistsTopNBasicElementBreakdown),
                ("Advanced Elements", stats.DecklistsTopNAdvancedElementBreakdown),
                ("Champions", stats.DecklistsTopNChampionBreakdown)));
        }

        var embed = new LocalEmbed()
            .WithTitle("Event Statistics")
            .WithFooter(ApiCredit);

        return fields.Count == 0
            ? embed.WithDescription("Statistics have not been revealed yet.")
            : embed.WithFields(fields);
    }

    public static string? BuildImageUrl(string? imagePath)
        => string.IsNullOrWhiteSpace(imagePath) ? null : $"https://api.gatcg.com{imagePath}";

    private static Page[] BuildTextPages(string title, IReadOnlyList<string> lines)
    {
        if (lines.Count == 0)
        {
            return
            [
                new Page().WithEmbeds(new LocalEmbed()
                    .WithTitle(title)
                    .WithDescription("No data available.")
                    .WithFooter(ApiCredit))
            ];
        }

        var chunks = lines.Chunk(LinesPerPage).ToArray();
        var pages = new Page[chunks.Length];
        for (var i = 0; i < chunks.Length; i++)
        {
            var embed = new LocalEmbed()
                .WithTitle(title)
                .WithDescription(Truncate(string.Join('\n', chunks[i])))
                .WithFooter($"Page {i + 1}/{chunks.Length} - {ApiCredit}");
            pages[i] = new Page().WithEmbeds(embed);
        }

        return pages;
    }

    private static string FormatSet(CardSet set)
    {
        var prefix = string.IsNullOrWhiteSpace(set.Prefix) ? "?" : set.Prefix;
        return $"{set.Name ?? "Unknown set"} ({prefix}) - {set.ReleaseDate:yyyy-MM-dd}";
    }

    private static string FormatMatch(OmnidexEventMatch match)
    {
        var players = match.Pairing.Count == 0
            ? "TBD"
            : string.Join(" vs ", match.Pairing.Select(p => $"{p.Id} ({p.Score:0.#})"));
        return $"{match.Label ?? $"Match {match.Id}"}: {players} [{match.Status ?? "unknown"}]";
    }

    private static void AppendDecklistSection(StringBuilder builder, string title,
        IReadOnlyList<OmnidexEventDecklistCard>? cards)
    {
        if (cards is null || cards.Count == 0)
        {
            return;
        }

        builder.AppendLine($"**{title}**");
        foreach (var card in cards)
        {
            builder.AppendLine($"{card.Quantity}x {card.Card}");
        }

        builder.AppendLine();
    }

    private static string FormatBreakdowns(
        params (string Name, OmnidexEventDecklistStatsNumericBreakdown? Breakdown)[] breakdowns)
    {
        var lines = breakdowns
            .Where(b => b.Breakdown is not null)
            .Select(b => $"{b.Name}: {b.Breakdown!.Total} decklists")
            .ToArray();
        return lines.Length == 0 ? "No breakdown available." : string.Join('\n', lines);
    }

    private static void AddField(List<LocalEmbedField> fields, string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        fields.Add(new LocalEmbedField()
            .WithName(name)
            .WithValue(value)
            .WithIsInline(true));
    }

    private static string Truncate(string value, int maxLength = MaxDescriptionLength)
        => value.Length <= maxLength ? value : string.Concat(value.AsSpan(0, maxLength - 3), "...");
}