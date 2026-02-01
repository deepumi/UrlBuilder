using System.Text;

public sealed class UrlBuilder
{
    private readonly StringBuilder _builder;
    private bool _hasQuery;
    private string? _pendingQuery;

    public UrlBuilder(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);

        _builder = new StringBuilder(uri.ToString().TrimEnd('/'));
        _hasQuery = !string.IsNullOrEmpty(uri.Query);
    }

    public UrlBuilder(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        _builder = new StringBuilder(url.TrimEnd('/'));
        _hasQuery = url.Contains('?');
    }

    public UrlBuilder WithFragment(string fragment)
    {
        if (string.IsNullOrEmpty(fragment)) return this;

        _builder.Append('#').Append(Uri.EscapeDataString(fragment));
        return this;
    }
    
    public UrlBuilder WithPath(string pathSegment, bool encode = true)
    {
        if (string.IsNullOrEmpty(pathSegment)) return this;

        pathSegment = pathSegment.Trim('/');
        if (pathSegment.Length == 0) return this;

        _builder.Append('/');
        _builder.Append(encode ? Uri.EscapeDataString(pathSegment) : pathSegment);
        return this;
    }

    public UrlBuilder WithPathIf(Func<bool> condition, string pathSegment, bool encode = true)
    {
        if (condition != null && condition())
            return WithPath(pathSegment, encode);

        return this;
    }

    public UrlBuilder WithPath(params string[] pathSegments)
    {
        if (pathSegments == null || pathSegments.Length == 0) return this;

        foreach (var segment in pathSegments)
        {
            WithPath(segment);
        }

        return this;
    }

    public UrlBuilder WithQueryString(string key, string value)
    {
        if (string.IsNullOrEmpty(key)) { return this; }

        _builder.Append(_hasQuery ? '&' : '?');
        _builder.Append(Uri.EscapeDataString(key));
        _builder.Append('=');
        
        if (value is not null)
        {
            _builder.Append(Uri.EscapeDataString(value));
        }
        _hasQuery = true;
        return this;
    }

    public UrlBuilder WithQueryStringIf(Func<bool> condition, string key, string value)
    {
        if (condition != null && condition())
            return WithQueryString(key, value);

        return this;
    }

    public UrlBuilder WithQueryString(IEnumerable<KeyValuePair<string, string>> parameters)
    {
        if (parameters == null) { return this; }

        foreach (var kvp in parameters)
        {
            WithQueryString(kvp.Key, kvp.Value);
        }

        return this;
    }
 
    public UrlBuilder WithQueryString(string key, IEnumerable<string> values)
    {
        if (string.IsNullOrEmpty(key) || values == null) return this;

        foreach (var value in values)
        {
            WithQueryString(key, value);
        }

        return this;
    }

    public UrlBuilder ClearQuery()
    {
        var index = _builder.ToString().IndexOf('?');
        if (index >= 0)
        {
            _builder.Length = index; // remove everything after '?'
            _hasQuery = false;
        }
        return this;
    }

    public UrlBuilder ClearPath()
    {
        var uri = new Uri(_builder.ToString());

        // Store query for later (do NOT append now)
        _pendingQuery = uri.Query;

        _builder.Clear();
        _builder.Append(uri.GetLeftPart(UriPartial.Authority));

        // Reset internal flags
        _hasQuery = !string.IsNullOrEmpty(_pendingQuery);

        return this;
    }

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(_pendingQuery))
        {
            _builder.Append(_pendingQuery);
            _pendingQuery = null;
        }

        return _builder.ToString();
    }


    public Uri ToUri() => new(_builder.ToString());
}