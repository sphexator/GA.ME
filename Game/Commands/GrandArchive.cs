using Disqord;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Game.GrandArchive;

namespace Game.Commands;

[SlashGroup("ga")]
public class GrandArchive : DiscordApplicationModuleBase
{
    private readonly ILogger<GrandArchive> _logger;

    public GrandArchive(ILogger<GrandArchive> logger)
    {
        _logger = logger;
    }

    [SlashCommand("search")]
    public async Task<DiscordCommandResult<IDiscordCommandContext>> SearchAsync(string name)
    {
        var response = await Context.Services.GetRequiredService<IGrandArchiveApi>().Search(name);
        if (response.Content is null)
        {
            return Response("No cards found.");
        }

        var pages = new Page[response.Content.Data.Count + 1];
        pages[0] = new Page().WithContent($"Found {response.Content.TotalCards} cards matching '{name}'. " +
                                          $"Displaying page 1 of {response.Content.TotalPages}.");
        for (var i = 0; i < response.Content.Data.Count; i++)
        {
            var card = response.Content.Data[i];
            
            _logger.LogInformation("Card {Index}: {Name} (ID: {Id})",
                i + 1, card.Name, card.Editions.FirstOrDefault()?.CardId);

            var embed = new LocalEmbed().WithTitle(card.Name ?? "Unknown Card")
                .WithDescription(card.Flavor ?? string.Empty + "\n\n" + card.EffectRaw)
                .WithImageUrl(card.Editions.FirstOrDefault() is null ? "https://api.gatcg.com" + card.Editions.FirstOrDefault().Image : string.Empty)
                .WithFooter("Data provided by the Grand Archive Index API");
            pages[i + 1] = new Page().WithEmbeds(embed);
        }

        return Pages(pages);
    }
}