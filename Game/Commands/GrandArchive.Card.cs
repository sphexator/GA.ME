using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Game.GrandArchive;
using Qmmands;
using Refit;

namespace Game.Commands;

public partial class GrandArchive
{
    [SlashGroup("card")]
    public class CardCommands(IGrandArchiveApi api, ILogger<CardCommands> logger) : DiscordApplicationGuildModuleBase
    {
        [SlashCommand("slug")]
        [Description("Gets a card by its slug.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> SlugAsync(
            [Description("The card slug.")] string slug)
        {
            var response = await api.GetBySlugAsync(slug);
            return RespondWithCard(response, "No card found for that slug.");
        }

        [AutoComplete("slug")]
        public async Task AutoCompleteSlugAsync(AutoComplete<string> slug)
        {
            if (!slug.IsFocused) return;

            var input = slug.RawArgument as string;
            if (string.IsNullOrWhiteSpace(input)) return;

            var response = await api.AutocompleteAsync(input);
            if (!response.IsSuccessStatusCode || response.Content is null) return;

            foreach (var card in response.Content)
            {
                if (card.Name is not null && card.Slug is not null)
                {
                    slug.Choices.Add(card.Name, card.Slug);
                }
            }
        }

        [SlashCommand("edition")]
        [Description("Gets a card by an edition slug or edition UUID.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> EditionAsync(
            [Description("The edition slug or UUID.")]
            string editionSlugOrUuid)
        {
            var response = await api.GetByEditionAsync(editionSlugOrUuid);
            return RespondWithCard(response, "No card found for that edition.");
        }

        [SlashCommand("print")]
        [Description("Gets a card by its set prefix and collector number.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> PrintAsync(
            [Description("The set prefix (e.g. \"FTC\").")]
            string setPrefix,
            [Description("The collector number (e.g. \"001\").")]
            string collectorNumber)
        {
            var response = await api.GetBySetAndCollectorNumberAsync(setPrefix, collectorNumber);
            return RespondWithCard(response, "No card found for that set and collector number.");
        }

        private DiscordCommandResult<IDiscordCommandContext> RespondWithCard(IApiResponse<Card> response,
            string notFoundMessage)
        {
            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                return Response(notFoundMessage);
            }

            logger.LogInformation("Returned card {Name} ({Slug})", response.Content.Name, response.Content.Slug);
            return Response(GrandArchivePresentation.BuildCardEmbed(response.Content));
        }
    }
}