// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var url1 = new UrlBuilder(new Uri("https://google.com/api?pageId=hello+world"));
var url2 = new UrlBuilder("https://google.com/api?pageId=hello+world");

Console.WriteLine(url1.ToString());
Console.WriteLine(url2.ToString());

var url = new UrlBuilder("https://a.com")
    .WithPath("users")
    .WithPath("a b")          // encoded
    .WithPath("a b", false);

var result = url.ToString();


// var pageUrl = new UrlBuilder("https://a.com")
//     .WithPath("page")
//     .WithPath("my page")   // substituted page.PagePath
//     .ToString();

// // Build the page image URL for overlaying a QR code logo or custom image
// var pageImageUrl = new UrlBuilder("https://a.com")
//     .WithPath("assets", "images")
//     .WithPath("logo.png")  // substituted page.BgImage
//     .ToString();

// // Build the final API call URL cleanly
// var qrCodeApiUrl = new UrlBuilder("https://api.qrcode.com")  // substituted config["QRCodeAPIUrl"]
//     .WithPath("qrcodegenerator", "generate")
//     .WithQueryString("pageUrl", pageUrl)
//     .WithQueryString("pageImageUrl", pageImageUrl)
//     .WithQueryString("PixelsPerModule", "8")  // substituted PixelsPerModule
//     .ToString();

// // Output for testing
// Console.WriteLine("Page URL: " + pageUrl);
// Console.WriteLine("Page Image URL: " + pageImageUrl);
// Console.WriteLine("Final QR Code API URL: " + qrCodeApiUrl);
