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

    // ----Helper Methods----
    private async Task RegisterTestAccount(string email, string password) {
        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Register");
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        await Page.GetByLabel("Confirm password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "register" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register confirmation" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" }).ClickAsync();
    }

    private async Task LogInTestAccount(string email, string password) {
        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Login");

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
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
    public async Task EnteringLogInPageTest()
    {
        await Page.GotoAsync(_app.BaseUrl);

        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register", Exact = true })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CanUserRegisterAccountTest()
    {
        // creates a new test example everytime
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";

        await Page.GotoAsync(_app.BaseUrl);
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        await Page.GetByLabel("Confirm password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "register" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register confirmation" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Confirm email" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Thank you for confirming your email")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CanUserLogInTest() {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";

        await RegisterTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Login");

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Bison" })).ToBeVisibleAsync();
    }

    public async Task CanUserLogInOAuthTest() {
        
    }
}