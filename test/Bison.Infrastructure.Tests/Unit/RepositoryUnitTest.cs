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
        var actual = await repository.GetObservations(1); // use your method name

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
    [InlineData(1, "A heron")]
    [InlineData(2, "A pelican")]
    public async Task GetObservationsByAuthorTest(int postId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetObservationsByAuthorTest(postId,1); 

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, o => o.Id == postId);
        Assert.Contains(actual, o => o.Text == expectedText);

    }

    [Theory]
    [InlineData(1, "Great spot")]
    [InlineData(2, "YOOOOOO SHUT YOUR BITCH ASS UP")]
    public async Task GetCommentsForObservationTest(int postId, string expectedText)
    {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetCommentsForObservation(postId, 1); 

        // assert
        Assert.NotNull(actual);
        Assert.Equal(expectedText, actual.Text);
    }

    [Theory]
    [InlineData(1, "Great spot")]
    [InlineData(2, "YOOOOOO SHUT YOUR BITCH ASS UP")]
    public async Task GetCommentsByAuthorTest(int authorId, string expectedText) {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetCommentsByAuthor(author, 1); 

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, o => o.Id == postId);
        Assert.Contains(actual, o => o.Text == expectedText);
    }

    [Theory]
    [InlineData(1,"Maybe a pelican?")]
    [InlineData(2,"WTFFFF IS THAT HOLY SHITTTTT")]
    public async Task GetProposalsTest(int postId, string expectedText) {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetProposals(1); 

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, o => o.Id == postId);
        Assert.Contains(actual, o => o.Text == expectedText);
    }
    [Theory]
    [InlineData(1,"Maybe a pelican?")]
    [InlineData(2,"WTFFFF IS THAT HOLY SHITTTTT")]
    public async Task GetProposalsForObservationTest(int observationId, int page) {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetProposalsForObservation(postId, 1); 

        // assert
        Assert.NotNull(actual);
        Assert.Contains(actual, o => o.Id == postId);
        Assert.Contains(actual, o => o.Text == expectedText);
    }

    [Theory]
    [InlineData(1,"Petra")]
    [InlineData(2,"Anton")]
    public async Task GetAuthorTest(int authorId, string expectedName) {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetAuthor(postId, 1);

        // assert
        Assert.NotNull(obs);
        Assert.Equal(expectedName, actual.Name);
    }

    [Theory]
    [InlineData(1,"test-hejrer", "Hejrer")]
    [InlineData(2,"test-fiskehejre", "Fiskehejre")]
    [InlineData(2,"test-pelikaner", "Pelikane")]
    public async Task GetTaxon(string taxonId, string expectedDwc_TaxonId, string expectedVernacularName) {
        // arrange
        var repository = new PostRepository(Context);

        // act
        var actual = await repository.GetTaxon(taxonId);

        // assert
        Assert.NotNull(obs);
        Assert.Equal(expectedDwc_TaxonId, actual.dwc_TaxonID);
        Assert.Equal(expectedVernacularName, actual.VernacularName);
    }
}