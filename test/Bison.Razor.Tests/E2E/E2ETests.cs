using System.Data.Common;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace Bison.Razor.Tests;

// documentation on how 
[CollectionDefinition("E2E")]
public class UICollection : ICollectionFixture<BisonFixture> { }


[Collection("E2E")]
[Trait("Category", "E2E")]
public class FrontPageTests : PageTest
{
    private readonly BisonFixture _app;

    public FrontPageTests(BisonFixture app)
    {
        _app = app;
    }

    // ----Helper Methods----
    private async Task RegisterTestAccount(string email, string password)
    {
        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Register");
        await Page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync(password);
        await Page.GetByLabel("Confirm password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "register" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register confirmation" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" }).ClickAsync();
    }

    private async Task LogInTestAccount(string email, string password)
    {
        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Login");

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }

    [Fact]
    public async Task FrontPage()
    {
        await Page.GotoAsync(_app.BaseUrl);

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "register" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Public Observations" })).ToBeVisibleAsync();
    }

    [Theory]
    [InlineData("Petra")]
    [InlineData("Anton")]
    public async Task PublicObservationsUsersTest(string name)
    {
        await Page.GotoAsync(_app.BaseUrl);
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync(name);
    }

    [Theory]
    [InlineData("A heron")]
    [InlineData("A bear")]
    [InlineData("A pelican")]
    public async Task PublicObservationsTest(string ObservationText)
    {
        await Page.GotoAsync(_app.BaseUrl);
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync(ObservationText);
    }

    [Fact]
    public async Task PublicObservationsCommentsTest()
    {
        await Page.GotoAsync(_app.BaseUrl);

        await Page.Locator("#messagelist")
                .GetByRole(AriaRole.Listitem)
                .Filter(new() { HasText = "A heron" })
                .GetByRole(AriaRole.Link, new() { Name = "Comments & proposals" })
                .ClickAsync();

        await Expect(Page.GetByText("Great spot!")).ToBeVisibleAsync();
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
    public async Task RegisterButtonTest()
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
    public async Task CanUserLogInTest()
    {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";

        await RegisterTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl + "/Identity/Account/Login");

        await Page.GetByLabel("Email").FillAsync(email);
        await Page.GetByLabel("Password").FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Bison" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = " Logout [" + email + "]" })).ToBeVisibleAsync();
    }

    //TODO: lets wait with this one untill we find a secure way to test it 
    [Fact]
    public async Task CanUserLogInOAuthTest()
    {

    }

    [Fact]
    public async Task CanUserLogOut()
    {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";

        await RegisterTestAccount(email, password);
        await LogInTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl);
        await Page.GetByRole(AriaRole.Link, new() { Name = "Logout [" + email + "]" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log Out" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout", Exact = true }).ClickAsync();

        await Expect(Page.GetByText("You have successfully logged out of the application.")).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Observations" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Bison" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Public Observations" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "My Observations" })).Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task CanUserMakeObservation()
    {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";
        var observationText = "A heron by the lake";

        await RegisterTestAccount(email, password);
        await LogInTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl);
        await Page.GetByText("Create post", new() { Exact = true }).ClickAsync();

        var observationInput = Page.Locator("textarea[name='Text']");
        await observationInput.FillAsync(observationText);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Post" }).ClickAsync();

        var postedObservation = Page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = observationText });
        await Expect(postedObservation).ToBeVisibleAsync();

        await Page.ReloadAsync();
        await Expect(postedObservation).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CanUserMakeComment()
    {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";
        var commentText = $"Test comment {Guid.NewGuid()}";

        await RegisterTestAccount(email, password);
        await LogInTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl);
        await Page.Locator("#messagelist")
            .GetByRole(AriaRole.Listitem)
            .Filter(new() { HasText = "A heron" })
            .GetByRole(AriaRole.Link, new() { Name = "Comments & proposals" })
            .ClickAsync();

        var commentInput = Page.Locator("#CommentText");
        await commentInput.FillAsync(commentText);
        await commentInput.PressAsync("Enter");

        var postedComment = Page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = commentText });
        await Expect(postedComment).ToBeVisibleAsync();

        await Page.ReloadAsync();
        await Expect(postedComment).ToBeVisibleAsync();
    }

    [Fact]
    public async Task CanUserMakeProposal()
    // TODO: This test is not working. Proposals can't be posted without selecting a species. Must select a species.
    {
        var email = $"test{Guid.NewGuid():N}@example.com";
        const string password = "Test123!";
        var proposalText = $"Test comment {Guid.NewGuid()}";

        await RegisterTestAccount(email, password);
        await LogInTestAccount(email, password);

        await Page.GotoAsync(_app.BaseUrl);
        await Page.Locator("#messagelist")
            .GetByRole(AriaRole.Listitem)
            .Filter(new() { HasText = "A heron" })
            .GetByRole(AriaRole.Link, new() { Name = "Comments & proposals" })
            .ClickAsync();

        var proposalInput = Page.Locator("#ProposalText");
        await proposalInput.FillAsync(proposalText);
        await proposalInput.PressAsync("Enter");

        var postedComment = Page.GetByRole(AriaRole.Listitem).Filter(new() { HasText = proposalText });
        await Expect(postedComment).ToBeVisibleAsync();

        await Page.ReloadAsync();
        await Expect(postedComment).ToBeVisibleAsync();
    }

    // TODO: Implement test to make sure user can't post a proposal without first selecting a species.
    // TODO: Implement test to confirm audio player appears for species where audio is available
    // TODO: Implement test to confirm audio player doesn't appear for species that represent a group of subspecies, or species that are quiet


}