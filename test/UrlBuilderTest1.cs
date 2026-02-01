namespace UrlBuilderTest;

public sealed class UrlBuilderTest1
{
    [Fact]
    public void TestUriConstructorNullThrows()
    {
        Assert.Throws<ArgumentNullException>(() => new UrlBuilder((Uri)null!));
    }

    [Fact]
    public void TestStringConstructorNullThrows()
    {
        Assert.Throws<ArgumentNullException>(() => new UrlBuilder((string)null!));
    }

    [Fact]
    public void TestStringConstructorEmptyThrows()
    {
        Assert.Throws<ArgumentException>(() => new UrlBuilder(string.Empty));
    }

    [Fact]
    public void TestConstructorTrimsTrailingSlash()
    {
        var url = new UrlBuilder("https://a.com/");
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestConstructorDetectsExistingQuery()
    {
        var url = new UrlBuilder("https://a.com?p=1");
        Assert.Equal("https://a.com?p=1", url.ToString());
    }

    // WithFragment

    [Fact]
    public void TestWithFragmentAppendsEncodedFragment()
    {
        var url = new UrlBuilder("https://a.com").WithFragment("a b");
        Assert.Equal("https://a.com#a%20b", url.ToString());
    }

    [Fact]
    public void TestWithFragmentNullIgnored()
    {
        var url = new UrlBuilder("https://a.com").WithFragment(null!);
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithPath single

    [Fact]
    public void TestWithPathAppendsSegment()
    {
        var url = new UrlBuilder("https://a.com").WithPath("test");
        Assert.Equal("https://a.com/test", url.ToString());
    }

    [Fact]
    public void TestWithPathEncodesSegment()
    {
        var url = new UrlBuilder("https://a.com").WithPath("a b");
        Assert.Equal("https://a.com/a%20b", url.ToString());
    }

    [Fact]
    public void TestWithPathNoEncode()
    {
        var url = new UrlBuilder("https://a.com").WithPath("a b", false);
        Assert.Equal("https://a.com/a b", url.ToString());
    }

    [Fact]
    public void TestWithPathNullIgnored()
    {
        var url = new UrlBuilder("https://a.com").WithPath(null!);
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestWithPathOnlySlashesIgnored()
    {
        var url = new UrlBuilder("https://a.com").WithPath("///");
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithPathIf

    [Fact]
    public void TestWithPathIfTrueAddsPath()
    {
        var url = new UrlBuilder("https://a.com")
            .WithPathIf(() => true, "x");
        Assert.Equal("https://a.com/x", url.ToString());
    }

    [Fact]
    public void TestWithPathIfFalseDoesNothing()
    {
        var url = new UrlBuilder("https://a.com")
            .WithPathIf(() => false, "x");
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestWithPathIfNullConditionDoesNothing()
    {
        var url = new UrlBuilder("https://a.com")
            .WithPathIf(null!, "x");
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithPath multiple

    [Fact]
    public void TestWithPathMultipleSegments()
    {
        var url = new UrlBuilder("https://a.com")
            .WithPath("a", "b", "c");
        Assert.Equal("https://a.com/a/b/c", url.ToString());
    }

    [Fact]
    public void TestWithPathMultipleNullIgnored()
    {
        var url = new UrlBuilder("https://a.com")
            .WithPath(null!);
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithQueryString key value

    [Fact]
    public void TestWithQueryStringAddsQuery()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a", "1");
        Assert.Equal("https://a.com?a=1", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringAppendsMultiple()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a", "1")
            .WithQueryString("b", "2");
        Assert.Equal("https://a.com?a=1&b=2", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringEncodesKeyAndValue()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a b", "c d");
        Assert.Equal("https://a.com?a%20b=c%20d", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringNullKeyIgnored()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString(null!, "1");
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringNullValueProducesEmptyValue()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a", (string)null!);
        Assert.Equal("https://a.com?a=", url.ToString());
    }

    // WithQueryStringIf

    [Fact]
    public void TestWithQueryStringIfTrueAdds()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryStringIf(() => true, "a", "1");
        Assert.Equal("https://a.com?a=1", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringIfFalseDoesNothing()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryStringIf(() => false, "a", "1");
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithQueryString collection

    [Fact]
    public void TestWithQueryStringCollectionAddsAll()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString(
            [
                new KeyValuePair<string,string>("a","1"),
                new KeyValuePair<string,string>("b","2")
            ]);
        Assert.Equal("https://a.com?a=1&b=2", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringCollectionNullIgnored()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString((IEnumerable<KeyValuePair<string, string>>)null!);
        Assert.Equal("https://a.com", url.ToString());
    }

    // WithQueryString multiple values

    [Fact]
    public void TestWithQueryStringMultipleValues()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a", ["1", "2"]);
        Assert.Equal("https://a.com?a=1&a=2", url.ToString());
    }

    [Fact]
    public void TestWithQueryStringMultipleValuesNullIgnored()
    {
        var url = new UrlBuilder("https://a.com")
            .WithQueryString("a", (string)null!);
        Assert.Equal("https://a.com?a=", url.ToString());
    }

    // ClearQuery

    [Fact]
    public void TestClearQueryRemovesQuery()
    {
        var url = new UrlBuilder("https://a.com?p=1")
            .ClearQuery();
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestClearQueryNoQueryNoChange()
    {
        var url = new UrlBuilder("https://a.com")
            .ClearQuery();
        Assert.Equal("https://a.com", url.ToString());
    }

    // ClearPath

    [Fact]
    public void TestClearPathRemovesPathKeepsAuthority()
    {
        var url = new UrlBuilder("https://a.com/a/b")
            .ClearPath();
        Assert.Equal("https://a.com", url.ToString());
    }

    [Fact]
    public void TestClearPathPreservesQueryAfterToString()
    {
        var url = new UrlBuilder("https://a.com/a/b?p=1")
            .ClearPath();
        Assert.Equal("https://a.com?p=1", url.ToString());
    }

    [Fact]
    public void TestClearPathThenAddPathKeepsQueryOrder()
    {
        var url = new UrlBuilder("https://a.com/a?p=1")
            .ClearPath()
            .WithPath("b");
        Assert.Equal("https://a.com/b?p=1", url.ToString());
    }

    // ToUri

    [Fact]
    public void TestToUriReturnsValidUri()
    {
        var url = new UrlBuilder("https://a.com").WithPath("x");
        var uri = url.ToUri();
        Assert.Equal(new Uri("https://a.com/x"), uri);
    }

    // Mixed scenarios

    [Fact]
    public void TestFullBuildScenario()
    {
        var url = new UrlBuilder("https://a.com/")
            .WithPath("a")
            .WithQueryString("x", "1")
            .WithFragment("end");

        Assert.Equal("https://a.com/a?x=1#end", url.ToString());
    }
}