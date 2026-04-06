using Game.GrandArchive;

namespace Game.Tests;

public class CardPresentationBuilderTests
{
    [Fact]
    public void BuildTitle_UsesUnknownCard_WhenNameMissing()
    {
        var card = new Card { Name = "   " };

        var title = CardPresentationBuilder.BuildTitle(card);

        Assert.Equal("Unknown Card", title);
    }

    [Fact]
    public void BuildDescription_CombinesFlavorAndEffectRaw()
    {
        var card = new Card
        {
            Flavor = "Ancient power stirs.",
            EffectRaw = "On Enter: Draw a card."
        };

        var description = CardPresentationBuilder.BuildDescription(card);

        Assert.Equal("Ancient power stirs.\n\nOn Enter: Draw a card.", description);
    }

    [Fact]
    public void BuildDescription_UsesFallback_WhenNoFlavorOrEffect()
    {
        var card = new Card { Flavor = "", EffectRaw = null };

        var description = CardPresentationBuilder.BuildDescription(card);

        Assert.Equal("No description available.", description);
    }

    [Fact]
    public void BuildImageUrl_ReturnsAbsoluteApiUrl_WhenEditionHasImage()
    {
        var edition = new Edition { Image = "/images/cards/test.png" };

        var imageUrl = CardPresentationBuilder.BuildImageUrl(edition);

        Assert.Equal("https://api.gatcg.com/images/cards/test.png", imageUrl);
    }

    [Fact]
    public void BuildImageUrl_ReturnsNull_WhenImageMissing()
    {
        var edition = new Edition { Image = "  " };

        var imageUrl = CardPresentationBuilder.BuildImageUrl(edition);

        Assert.Null(imageUrl);
    }
}

