using System.Globalization;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Application;
using Game.GrandArchive;
using Qmmands;

namespace Game.Commands;

public partial class GrandArchive
{
    [SlashGroup("search")]
    public class SearchCommands(IGrandArchiveApi api, ILogger<SearchCommands> logger)
        : DiscordApplicationGuildModuleBase
    {
        [SlashCommand("name")]
        [Description("Searches cards by name.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> NameAsync(
            [Description("The card name (or part of it).")]
            string name,
            [Description("The field to sort by (default: collector_number).")]
            string? sort = null,
            [Description("The result order.")] ResultOrder order = ResultOrder.Asc,
            [Description("Results per page (1-50, default 50).")] [Range(1, 50)]
            int pageSize = 50,
            [Description("The result page (default 1).")] [Minimum(1)]
            int page = 1)
        {
            var query = SharedQuery(sort, order, pageSize, page);
            query.Name = name;
            return await SearchAndRespondAsync(query);
        }

        [AutoComplete("name")]
        public async Task AutoCompleteNameAsync(AutoComplete<string> name)
        {
            if (!name.IsFocused) return;

            var input = name.RawArgument as string;
            if (string.IsNullOrWhiteSpace(input)) return;

            var response = await api.AutocompleteAsync(input);
            if (!response.IsSuccessStatusCode || response.Content is null) return;

            foreach (var card in response.Content)
            {
                if (card.Name is not null)
                {
                    name.Choices.Add(card.Name, card.Name);
                }
            }
        }

        [SlashCommand("text")]
        [Description("Searches cards by text: effect, flavor, rules and illustrator.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> TextAsync(
            [Description("Card or edition effect text contains this.")]
            string? effect = null,
            [Description("Card flavor text contains this.")]
            string? flavor = null,
            [Description("Rule title or description contains this.")]
            string? rule = null,
            [Description("Rule title exactly matches this (\"null\" matches no title).")]
            string? ruleTitle = null,
            [Description("Edition illustrator exactly matches this (\"null\" matches no illustrator).")]
            string? illustrator = null,
            [Description("Edition effect text contains this.")]
            string? editionEffect = null,
            [Description("Edition flavor text contains this.")]
            string? editionFlavor = null,
            [Description("Only cards updated after this date (yyyy-MM-dd).")]
            string? updatedAfter = null,
            [Description("Only editions updated after this date (yyyy-MM-dd).")]
            string? editionUpdatedAfter = null,
            [Description("The field to sort by (default: collector_number).")]
            string? sort = null,
            [Description("The result order.")] ResultOrder order = ResultOrder.Asc,
            [Description("Results per page (1-50, default 50).")] [Range(1, 50)]
            int pageSize = 50,
            [Description("The result page (default 1).")] [Minimum(1)]
            int page = 1)
        {
            if (!TryParseDate(updatedAfter, out var lastUpdate) ||
                !TryParseDate(editionUpdatedAfter, out var editionLastUpdate))
            {
                return Response("Invalid date format. Please use yyyy-MM-dd.");
            }

            var query = SharedQuery(sort, order, pageSize, page);
            query.Effect = effect;
            query.Flavor = flavor;
            query.Rule = rule;
            query.RuleTitle = ruleTitle;
            query.Illustrator = illustrator;
            query.EditionEffect = editionEffect;
            query.EditionFlavor = editionFlavor;
            query.LastUpdate = lastUpdate;
            query.EditionLastUpdate = editionLastUpdate;
            return await SearchAndRespondAsync(query);
        }

        [SlashCommand("attributes")]
        [Description("Searches cards by attributes: class, element, type, subtype, speed, stats and rarity.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> AttributesAsync(
            [Description("Comma-separated classes (e.g. \"Mage,Warrior\").")]
            string? @class = null,
            [Description("How to combine the classes.")]
            FilterLogic? classLogic = null,
            [Description("Comma-separated elements (\"null\" matches no element).")]
            string? element = null,
            [Description("How to combine the elements.")]
            FilterLogic? elementLogic = null,
            [Description("Comma-separated types.")]
            string? type = null,
            [Description("How to combine the types.")]
            FilterLogic? typeLogic = null,
            [Description("Comma-separated subtypes.")]
            string? subtype = null,
            [Description("How to combine the subtypes.")]
            FilterLogic? subtypeLogic = null,
            [Description("The speed stat.")] CardSpeed? speed = null,
            [Description("Semicolon-delimited numeric stats (e.g. \"1;2\").")]
            string? stats = null,
            [Description("Edition rarity (exact number).")]
            int? rarity = null,
            [Description("The field to sort by (default: collector_number).")]
            string? sort = null,
            [Description("The result order.")] ResultOrder order = ResultOrder.Asc,
            [Description("Results per page (1-50, default 50).")] [Range(1, 50)]
            int pageSize = 50,
            [Description("The result page (default 1).")] [Minimum(1)]
            int page = 1)
        {
            var query = SharedQuery(sort, order, pageSize, page);
            query.Class = ParseCsv(@class);
            query.ClassLogic = classLogic?.ToApiValue();
            query.Element = ParseCsv(element);
            query.ElementLogic = elementLogic?.ToApiValue();
            query.Type = ParseCsv(type);
            query.TypeLogic = typeLogic?.ToApiValue();
            query.Subtype = ParseCsv(subtype);
            query.SubtypeLogic = subtypeLogic?.ToApiValue();
            query.Speed = speed is null ? null : [speed.Value.ToApiValue()];
            query.Stats = stats;
            query.Rarity = rarity;
            return await SearchAndRespondAsync(query);
        }

        [SlashCommand("edition")]
        [Description("Searches cards by edition: collector number, set, language and print details.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> EditionAsync(
            [Description("Edition collector number (exact match).")]
            string? collectorNumber = null,
            [Description("Edition slug (exact match).")]
            string? editionSlug = null,
            [Description("Comma-separated set prefixes.")]
            string? prefix = null,
            [Description("Comma-separated set languages (e.g. \"en,de\").")]
            string? language = null,
            [Description("The card configuration.")]
            CardConfiguration? configuration = null,
            [Description("Include or exclude editions with collaborators.")]
            bool? collab = null,
            [Description("Split every result edition into its own entry.")]
            bool? separateEditions = null,
            [Description("The field to sort by (default: collector_number).")]
            string? sort = null,
            [Description("The result order.")] ResultOrder order = ResultOrder.Asc,
            [Description("Results per page (1-50, default 50).")] [Range(1, 50)]
            int pageSize = 50,
            [Description("The result page (default 1).")] [Minimum(1)]
            int page = 1)
        {
            var query = SharedQuery(sort, order, pageSize, page);
            query.CollectorNumber = collectorNumber;
            query.EditionSlug = editionSlug;
            query.Prefix = ParseCsv(prefix);
            query.Language = ParseCsv(language);
            query.Configuration = configuration?.ToApiValue();
            query.Collab = collab;
            query.SeparateEditions = separateEditions;
            return await SearchAndRespondAsync(query);
        }

        [SlashCommand("legality")]
        [Description("Searches cards by legality in a game format.")]
        public async Task<DiscordCommandResult<IDiscordCommandContext>> LegalityAsync(
            [Description("The game format.")] LegalityFormat? format = null,
            [Description("Exact card limit in the format (overrides the state filter).")] [Minimum(0)]
            int? limit = null,
            [Description("Whether cards are legal or restricted.")]
            LegalityState? state = null,
            [Description("The field to sort by (default: collector_number).")]
            string? sort = null,
            [Description("The result order.")] ResultOrder order = ResultOrder.Asc,
            [Description("Results per page (1-50, default 50).")] [Range(1, 50)]
            int pageSize = 50,
            [Description("The result page (default 1).")] [Minimum(1)]
            int page = 1)
        {
            var query = SharedQuery(sort, order, pageSize, page);
            query.LegalityFormat = format?.ToApiValue();
            query.LegalityLimit = limit;
            query.LegalityState = state?.ToApiValue();
            return await SearchAndRespondAsync(query);
        }

        private async Task<DiscordCommandResult<IDiscordCommandContext>> SearchAndRespondAsync(CardsSearchQuery query)
        {
            var response = await api.SearchAsync(query);
            if (!response.IsSuccessStatusCode || response.Content is null || response.Content.Data.Count == 0)
            {
                return Response("No cards found.");
            }

            logger.LogInformation("Search returned {Count} cards (page {Page}/{TotalPages})",
                response.Content.Data.Count, response.Content.Page, response.Content.TotalPages);
            return Pages(GrandArchivePresentation.BuildCardPages(response.Content.Data));
        }

        private static CardsSearchQuery SharedQuery(string? sort, ResultOrder order, int pageSize, int page)
            => new()
            {
                Sort = sort,
                Order = order.ToApiValue(),
                PageSize = pageSize,
                Page = page
            };

        private static List<string>? ParseCsv(string? value)
            => string.IsNullOrWhiteSpace(value)
                ? null
                : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        private static bool TryParseDate(string? value, out DateTime? date)
        {
            date = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsed))
            {
                date = parsed;
                return true;
            }

            return false;
        }
    }
}