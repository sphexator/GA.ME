using System.ComponentModel.DataAnnotations;
using Disqord;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Game.GrandArchive;

namespace Game.Commands;

public enum EditionType
{
    [Display(Name = "None-Foil")]
    NoneFoil,

    [Display(Name = "Foil")]
    Foil,

    [Display(Name = "CSR")]
    Csr
}

[SlashGroup("ga")]
public class GrandArchive(ILogger<GrandArchive> logger) : DiscordApplicationGuildModuleBase
{
    [SlashCommand("search")]
    public async Task<DiscordCommandResult<IDiscordCommandContext>> SearchAsync(string name,
        AutoComplete<string> typeFilter)
    {
        var response = await Context.Services.GetRequiredService<IGrandArchiveApi>().SearchAsync(name);
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

            var embed = new LocalEmbed()
                .WithTitle(CardPresentationBuilder.BuildTitle(card))
                .WithDescription(CardPresentationBuilder.BuildDescription(card))
                .WithFooter($"Page {pageNumber}/{cards.Count} - Data provided by the Grand Archive Index API");

            var imageUrl = CardPresentationBuilder.BuildImageUrl(edition);
            if (imageUrl is not null)
            {
                embed = embed.WithImageUrl(imageUrl);
            }

            pages[i] = new Page().WithEmbeds(embed);
        }

        return Pages(pages);
    }

    [AutoComplete("search")]
    public void AutoCompleteEditionTypeAsync(AutoComplete<string> typeFilter)
    {
        if (!typeFilter.IsFocused) return;
        var result = Enum.GetValues<EditionType>()
            .Select(e => e.ToString()).ToArray();
        for (int i = 0; i < result.Length; i++)
        {
            typeFilter.Choices.Add(result[i], result[i]);
        }
    }
}