using Disqord;
using Disqord.Bot.Commands.Application;
using Disqord.Bot.Commands.Interaction;
using Game.GrandArchive;

namespace Game.Commands;

[SlashGroup("ga")]
public class GrandArchive : DiscordApplicationModuleBase
{
    [SlashCommand("search")]
    public async Task<DiscordInteractionResponseCommandResult> SearchAsync(string name)
    {
        var response = await Context.Services.GetRequiredService<IGrandArchiveApi>().Search(name);
        if (response.Data.Count == 0)
            return Response("No cards found.");

        var card = response.Data.FirstOrDefault();
        if (card is null)
        {
            return Response("No cards found.");
        }

        var embed = new LocalEmbed()
            .WithTitle(card.Name ?? "Unknown Card")
            .WithDescription(card.Flavor ?? string.Empty + "\n\n" + card.EffectRaw)
            .WithImageUrl("https://api.gatcg.com" + card.Editions.FirstOrDefault()?.Image)
            .WithFooter("Data provided by the Grand Archive Index API");

        return Response(embed);
    }
}