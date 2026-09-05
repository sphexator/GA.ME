using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Game.GrandArchive;
using Qmmands;

namespace Game.Commands;

public partial class GrandArchive
{
    [SlashGroup("event")]
    public class EventCommands(IGrandArchiveApi api, ILogger<EventCommands> logger) : DiscordApplicationGuildModuleBase
    {
        [SlashCommand("info")]
        [Description("Gets information about an Omnidex event.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> InfoAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                return Response("Event not found.");
            }

            return Response(GrandArchivePresentation.BuildEventEmbed(response.Content));
        }

        [SlashCommand("players")]
        [Description("Lists the players participating in an Omnidex event.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> PlayersAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventPlayersAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
            {
                return Response("No players found for that event.");
            }

            logger.LogInformation("Returned {Count} players for event {EventId}", response.Content.Count, eventId);
            return Pages(GrandArchivePresentation.BuildPlayerPages(response.Content));
        }

        [SlashCommand("teams")]
        [Description("Lists the teams participating in an Omnidex event (if applicable).")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> TeamsAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventTeamsAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
            {
                return Response("No teams found for that event.");
            }

            return Pages(GrandArchivePresentation.BuildTeamPages(response.Content));
        }

        [SlashCommand("judges")]
        [Description("Lists the judges in an Omnidex event.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> JudgesAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventJudgesAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
            {
                return Response("No judges found for that event.");
            }

            return Pages(GrandArchivePresentation.BuildJudgePages(response.Content));
        }

        [SlashCommand("standings")]
        [Description("Gets the Swiss stage standings for an Omnidex event.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> StandingsAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventStandingsAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                return Response("No standings found for that event.");
            }

            return Pages(GrandArchivePresentation.BuildStandingsPages(response.Content));
        }

        [SlashCommand("pairings")]
        [Description("Gets pairings for an Omnidex event (latest stage and round by default).")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> PairingsAsync(
            [Description("The Omnidex event ID.")] long eventId,
            [Description("The round number.")] [Minimum(1)]
            long? round = null,
            [Description("The stage number.")] [Minimum(1)]
            long? stage = null)
        {
            var response = await api.GetEventPairingsAsync(eventId, round, stage);
            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                return Response("No pairings found for that event.");
            }

            return Pages(GrandArchivePresentation.BuildPairingsPages(response.Content));
        }

        [SlashCommand("decklists")]
        [Description("Gets the public decklists for an Omnidex event (if available).")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> DecklistsAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventDecklistsAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
            {
                return Response("No public decklists found for that event.");
            }

            var visible = response.Content.Where(d => d.Visible).ToArray();
            if (visible.Length == 0)
            {
                return Response("No public decklists found for that event.");
            }

            logger.LogInformation("Returned {Count} decklists for event {EventId}", visible.Length, eventId);
            return Pages(GrandArchivePresentation.BuildDecklistPages(visible));
        }

        [SlashCommand("statistics")]
        [Description("Gets the public statistics for an Omnidex event (if available).")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> StatisticsAsync(
            [Description("The Omnidex event ID.")] long eventId)
        {
            var response = await api.GetEventStatisticsAsync(eventId);
            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                return Response("No statistics found for that event.");
            }

            return Response(GrandArchivePresentation.BuildStatisticsEmbed(response.Content));
        }
    }
}