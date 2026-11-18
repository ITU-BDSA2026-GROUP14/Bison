using Bison.Core.Models;
using Bison.Infrastructure.Repositories;
using Xunit;

namespace Bison.Infrastructure.Tests;

public class RepositoryUnitTest : TestDatabase
{
    [Theory]
    [InlineData(1, "A heron")]
    [InlineData(2, "A bear")]
    [InlineData(3, "A pelican")]
    public async Task GetObservationTest(int postId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var obs = await repository.GetObservation(postId);

        // assert
        Assert.NotNull(obs);
        Assert.Equal(expectedText, obs.Text);
    }

    [Fact]
    public async Task GetObservationsTest()
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetObservations(1);

        // assert
        Assert.NotNull(actual);

        Assert.Contains(actual, o => o.Id == 1);
        Assert.Contains(actual, o => o.Id == 2);
        Assert.Contains(actual, o => o.Id == 3);

        Assert.Contains(actual, o => o.Text == "A heron");
        Assert.Contains(actual, o => o.Text == "A bear");
        Assert.Contains(actual, o => o.Text == "A pelican");
    }

    [Theory]
    [InlineData(1, 1, "A heron")]
    [InlineData(2, 2, "A bear")]
    [InlineData(2, 3, "A pelican")]
    public async Task GetObservationsByAuthorTest(int authorId, int expectedPostId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetObservationsByAuthor(authorId, 1);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, o => o.Id == expectedPostId && o.Text == expectedText);
    }

    [Theory]
    [InlineData(1, "Great spot!")]
    [InlineData(1, "YOOOOOO SHUT YOUR BITCH ASS UP")]
    public async Task GetCommentsForObservationTest(int observationId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetCommentsForObservation(observationId, 1);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, c => c.Text == expectedText);
    }

    [Theory]
    [InlineData(1, 5, "YOOOOOO SHUT YOUR BITCH ASS UP")]
    [InlineData(2, 4, "Great spot!")]
    public async Task GetCommentsByAuthorTest(int authorId, int expectedPostId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetCommentsByAuthor(authorId, 1);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, c => c.Id == expectedPostId && c.Text == expectedText);
    }

    [Theory]
    [InlineData(6, "Maybe a pelican?")]
    [InlineData(7, "WTFFFF IS THAT HOLY SHITTTTT")]
    public async Task GetProposalsTest(int expectedPostId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetProposals(1);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, p => p.Id == expectedPostId && p.Text == expectedText);
    }

    [Theory]
    [InlineData(2, 6, "Maybe a pelican?")]
    [InlineData(3, 7, "WTFFFF IS THAT HOLY SHITTTTT")]
    public async Task GetProposalsForObservationTest(int observationId, int expectedPostId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetProposalsForObservation(observationId, 1);

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, p => p.Id == expectedPostId && p.Text == expectedText);
    }

    [Theory]
    [InlineData(1, "Petra")]
    [InlineData(2, "Anton")]
    public async Task GetAuthorTest(int authorId, string expectedName)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetAuthor(authorId);

        // assert
        Assert.NotNull(actual);
        Assert.Equal(expectedName, actual.Name);
    }

    [Theory]
    [InlineData("test-hejrer", "Hejrer")]
    [InlineData("test-fiskehejre", "Fiskehejre")]
    [InlineData("test-pelikaner", "Pelikaner")]
    public async Task GetTaxonTest(string dwcTaxonId, string expectedVernacularName)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetTaxon(dwcTaxonId);

        // assert
        Assert.NotNull(actual);
        Assert.Equal(dwcTaxonId, actual.TaxonId);
        Assert.Equal(expectedVernacularName, actual.TaxonName);
    }
}