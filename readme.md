# UrlBuilder API Documentation

Provides a fluent API for building URLs with paths, query strings, and fragments.

```
var url = new UrlBuilder("https://a.com")
    .WithPath("users", "42")
    .WithQueryString("active", "true")
    .ToString();

// https://a.com/users/42?active=true
```
---

## Constructors

### UrlBuilder(Uri uri)

Creates a new UrlBuilder from an existing Uri.

```
public UrlBuilder(Uri uri)
```

Throws `ArgumentNullException` if uri is null.

---

### UrlBuilder(string url)

Creates a new UrlBuilder from a URL string.

```
public UrlBuilder(string url)
```

Throws `ArgumentNullException` if url is null, or `ArgumentException` if url is empty.

---

## Path Methods

### WithPath(string pathSegment, bool encode = true)

Appends a single path segment to the URL.

```
public UrlBuilder WithPath(string pathSegment, bool encode = true)
```

Null, empty, or slash only segments are ignored.

---

### WithPath(params string[] pathSegments)

Appends multiple path segments in order.

```
public UrlBuilder WithPath(params string[] pathSegments)
```

---

### WithPathIf(Func&lt;bool&gt; condition, string pathSegment, bool encode = true)

Conditionally appends a path segment.

```
public UrlBuilder WithPathIf(Func<bool> condition, string pathSegment, bool encode = true)
```

If the condition is null or returns false, no path is added.

Example:
```
builder.WithPathIf(() => isAdmin, "admin");
```
---

## Query Methods

### WithQueryString(string key, string value)

Adds a query string parameter.

```
public UrlBuilder WithQueryString(string key, string value)
```

If key is null or empty, the call is ignored.
If value is null, the result is `key=`

---

### WithQueryStringIf(Func&lt;bool&gt; condition, string key, string value)

Conditionally adds a query parameter.

```
public UrlBuilder WithQueryStringIf(Func<bool> condition, string key, string value)
```

---

### WithQueryString(IEnumerable&lt;KeyValuePair&lt;string, string&gt;&gt; parameters)

Adds multiple query parameters.

```
public UrlBuilder WithQueryString(IEnumerable<KeyValuePair<string, string>> parameters)
```

If parameters is null, the call is ignored.

---

### WithQueryString(string key, IEnumerable&lt;string&gt; values)

Adds the same query key multiple times.

```
public UrlBuilder WithQueryString(string key, IEnumerable<string> values)
```

If values is null, the call is ignored.

---

## Fragment

### WithFragment(string fragment)

Appends a URL fragment.

```
public UrlBuilder WithFragment(string fragment)
```

Null or empty fragments are ignored.

---

## Clearing Methods

### ClearQuery()

Removes all query parameters from the URL.

```
public UrlBuilder ClearQuery()
```

---

### ClearPath()

Removes the path while preserving scheme, host, and query.

```
public UrlBuilder ClearPath()
```

---

## Conversion

### ToString()

Returns the final URL string.

```
public override string ToString()
```

---

### ToUri()

Returns the final URL as a Uri.

```
public Uri ToUri()
```
---