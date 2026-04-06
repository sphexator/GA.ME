namespace Game.GrandArchive;

public static class CardPresentationBuilder
{
    public static string BuildTitle(Card card)
    {
        return string.IsNullOrWhiteSpace(card.Name) ? "Unknown Card" : card.Name;
    }

    public static string BuildDescription(Card card)
    {
        var descriptionParts = new[] { card.Flavor, card.EffectRaw }
            .Where(part => !string.IsNullOrWhiteSpace(part));

        var description = string.Join("\n\n", descriptionParts);
        return string.IsNullOrWhiteSpace(description)
            ? "No description available."
            : description;
    }

    public static string? BuildImageUrl(Edition? edition)
    {
        if (string.IsNullOrWhiteSpace(edition?.Image))
        {
            return null;
        }

        return $"https://api.gatcg.com{edition.Image}";
    }
}

