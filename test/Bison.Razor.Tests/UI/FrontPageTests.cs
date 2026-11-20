using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace Bison.Razor.Tests;

// documentation on how 
[CollectionDefinition("UI")]
public class UICollection : ICollectionFixture<BisonFixture> { }


[Collection("UI")]
[Trait("Category", "UI")]
public class FrontPageTests : PageTest
{
    private readonly BisonFixture _app;

    public FrontPageTests(BisonFixture app) {
        _app = app;
    }

    [Fact]
    public async Task HasTitle()
    {
        await Page.GotoAsync(_app.BaseUrl);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));
    }

    [Fact]
    public async Task GetStartedLink()
    {
        await Page.GotoAsync("https://playwright.dev");

        // Click the get started link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        // Expects page to have a heading with the name of Installation.
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task LogIn() {

        await Page.GotoAsync(app.BaseUrl);

        await Page.GetByRole(AriaRole.Link, new() {Name = "register"})

        await 
    }
}