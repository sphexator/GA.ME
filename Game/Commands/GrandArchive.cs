using Disqord;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Game.GrandArchive;

namespace Game.Commands;

[SlashGroup("ga")]
public class GrandArchive(ILogger<GrandArchive> logger) : DiscordApplicationModuleBase
{
    [SlashCommand("search")]
    public async Task<DiscordCommandResult<IDiscordCommandContext>> SearchAsync(string name)
    {
        var response = await Context.Services.GetRequiredService<IGrandArchiveApi>().Search(name);
        if (response.Content is null || response.Content.Data.Count == 0)
        {
            return Response("No cards found.");
        }

        var cards = response.Content.Data;
        var pages = new Page[cards.Count];

        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var edition = card.Editions.FirstOrDefault();
            var pageNumber = i + 1;

            logger.LogInformation("Card {Index}: {Name} (ID: {Id})", pageNumber, card.Name, edition?.CardId);

            var descriptionParts = new[] { card.Flavor, card.EffectRaw }
                .Where(part => !string.IsNullOrWhiteSpace(part));
            var description = string.Join("\n\n", descriptionParts);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = "No description available.";
            }

            var embed = new LocalEmbed()
                .WithTitle(card.Name ?? "Unknown Card")
                .WithDescription(description)
                .WithFooter($"Page {pageNumber}/{cards.Count} - Data provided by the Grand Archive Index API");

            if (!string.IsNullOrWhiteSpace(edition?.Image))
            {
                embed = embed.WithImageUrl($"https://api.gatcg.com{edition.Image}");
            }

            pages[i] = new Page().WithEmbeds(embed);
        }

        return Pages(pages);
    }
}