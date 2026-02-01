namespace UrlBuilderTest;

public class UrlBuilderTests
{
    private readonly Page page = new Page
    {
        PageIdentifier = "123",
        FriendlyName = "donate now",
        BgImage = "background.jpg",
        Tags = ["charity", "fundraiser"]
    };

    [Fact]
    public void BuildsSimplePathAndQuery()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithQueryString("page", "1")
            .ToString();

        Assert.Equal("https://example.com/campaigns?page=1", url);
    }

    [Fact]
    public void EncodesPathSegmentsCorrectly()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("folder name")
            .WithPath("sub/folder", false)
            .ToString();

        Assert.Equal("https://example.com/folder%20name/sub/folder", url);
    }

    [Fact]
    public void ConditionalPathAddsOnlyWhenTrue()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithPathIf(() => !string.IsNullOrWhiteSpace(page.FriendlyName), page.FriendlyName)
            .ToString();

        Assert.Equal("https://example.com/campaigns/donate%20now", url);
    }

    [Fact]
    public void ConditionalQueryAddsOnlyWhenTrue()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithQueryStringIf(() => !string.IsNullOrWhiteSpace(page.BgImage), "bg", page.BgImage)
            .ToString();

        Assert.Equal("https://example.com/campaigns?bg=background.jpg", url);
    }

    [Fact]
    public void QueryWithCollectionAddsAllValues()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithQueryString("tag", page.Tags)
            .ToString();

        Assert.Equal("https://example.com/campaigns?tag=charity&tag=fundraiser", url);
    }

    [Fact]
    public void FragmentIsAddedCorrectly()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithFragment("section 1")
            .ToString();

        Assert.Equal("https://example.com/campaigns#section%201", url);
    }

    [Fact]
    public void ClearQueryRemovesAllQueryParameters()
    {
        var builder = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithQueryString("page", "1");

        builder.ClearQuery()
               .WithQueryString("page", "2");

        Assert.Equal("https://example.com/campaigns?page=2", builder.ToString());
    }

    [Fact]
    public void ClearPathResetsPathCorrectly()
    {
        var builder = new UrlBuilder("https://example.com")
            .WithPath("campaigns")
            .WithQueryString("page", "1");

        builder.ClearPath()
            .WithPath("archive", "2026");
        
        Assert.Equal("https://example.com/archive/2026?page=1", builder.ToString());
    }

    [Fact]
    public void FullCombinedExampleWorksAsExpected()
    {
        var url = new UrlBuilder("https://example.com")
            .WithPath("campaigns", page.PageIdentifier)
            .WithPathIf(() => !string.IsNullOrWhiteSpace(page.FriendlyName), page.FriendlyName)
            .WithQueryString("ref", "newsletter")
            .WithQueryStringIf(() => !string.IsNullOrWhiteSpace(page.BgImage), "bg", page.BgImage)
            .WithQueryString("tag", page.Tags)
            .WithFragment("section1")
            .ToString();

        Assert.Equal("https://example.com/campaigns/123/donate%20now?ref=newsletter&bg=background.jpg&tag=charity&tag=fundraiser#section1", url);
    }

    [Fact]
    public void ReuseBuilderWithClearQueryAndClearPathWorksCorrectly()
    {
        var builder = new UrlBuilder("https://example.com")
            .WithPath("campaigns", "123")
            .WithQueryString("page", "1")
            .WithQueryString("sort", "asc");

        // Reuse: clear query and path
        builder.ClearQuery()
            .ClearPath()
            .WithPath("archive", "2026")
            .WithQueryString("page", "2")
            .WithQueryString("sort", "desc");

        var url = builder.ToString();

        Assert.Equal("https://example.com/archive/2026?page=2&sort=desc", url);
    }
    
    public class Page
    {
        public string PageIdentifier { get; set; } = null!;
        public string FriendlyName { get; set; } = null!;
        public string BgImage { get; set; } = null!;
        public string[] Tags { get; set; } = null!;
    }
}