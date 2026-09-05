using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Game.GrandArchive;
using Qmmands;

namespace Game.Commands;

[SlashGroup("ga")]
public partial class GrandArchive(ILogger<GrandArchive> logger, IGrandArchiveApi api)
    : DiscordApplicationGuildModuleBase
{
    [SlashCommand("random")]
    [Description("Gets random cards.")]
    public async Task<DiscordCommandResult<IDiscordCommandContext>> RandomAsync(
        [Description("The amount of random cards (1-50, default 8).")] [Range(1, 50)]
        int amount = 8)
    {
        var response = await api.GetRandomAsync(amount);
        if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
        {
            return Response("No cards found.");
        }

        logger.LogInformation("Returned {Count} random cards", response.Content.Count);
        return Pages(GrandArchivePresentation.BuildCardPages(response.Content));
    }

    [SlashCommand("featured-sets")]
    [Description("Lists the featured set groups and the sets they contain.")]
    public async Task<DiscordCommandResult<IDiscordCommandContext>> FeaturedSetsAsync()
    {
        var response = await api.GetFeaturedSetsAsync();
        if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Count == 0)
        {
            return Response("No featured sets found.");
        }

        logger.LogInformation("Returned {Count} featured set groups", response.Content.Count);
        return Pages(GrandArchivePresentation.BuildFeaturedSetPages(response.Content));
    }
}