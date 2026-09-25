using System.Net.Http.Json;

namespace Bison.Taxonomy.Tests;

public class TaxonomyE2ETest
{

// E2E: "propose" with a real taxon stores the proposal on the server.
[Fact]

public async Task CommandProposeValidTaxonTest()
{

    //Setup
    var baseURL = "http://localhost:5189";
    using HttpClient client = new();
    client.BaseAddress = new Uri(baseURL);

    var fiskehejre = "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea";

    //Arrange: pick an observation that exists and count its proposals
    var observations = await client.GetFromJsonAsync<UniqueObservation[]>("/observations");
    int obsId = observations!.First().Id;
    var before = await client.GetFromJsonAsync<Proposal[]>($"/proposals?id={obsId}");

    var originalOut = Console.Out;
    using var writer = new StringWriter();
    Console.SetOut(writer);

    try
    {
        //Act: run the real CLI command
        int exitCode = Program.Main(new[] {"propose", fiskehejre, obsId.ToString()});

        //Assert: CLI says it worked, and the server has one more proposal with our taxon
        var after = await
        client.GetFromJsonAsync<Proposal[]>($"/proposals?id={obsId}");
        Assert.Equal(0, exitCode);
        Assert.Contains("Proposal created", writer.ToString());
        Assert.Equal(before!.Length + 1, after!.Length);
        Assert.Equal(fiskehejre, after.Last().TaxonId);
        }
            finally
    {
        Console.SetOut(originalOut);
    }
    }

//E2E: "propose" with an unknown taxon fails with exit code 1 and stores nothing.
[Fact]
public async Task CommandProposeInvalidTaxonTest()
{
    //Setup
    var baseURL = "http://localhost:5189";
    using HttpClient client = new();
    client.BaseAddress = new Uri(baseURL);

    //Arrange
    var observation = await client.GetFromJsonAsync<UniqueObservation[]>
    ("/observations");
    int obsId = observation!.First().Id;
    var before = await client.GetFromJsonAsync<Proposal[]>($"/proposals?id={obsId}");

    var originalError = Console.Error;
    using var writer = new StringWriter();
    Console.SetError(writer);

    try
    {
        //Act
        int exitCode = Program.Main(new []
        {
            "propose", "not-a-real-taxon", obsId.ToString() });

        //Assert: CLI reports the error, and  nothing new was stored
        var after = await client.GetFromJsonAsync<Proposal[]>($"/proposals?id={obsId}");
        Assert.Equal(1, exitCode);
        Assert.Contains("does not exist", writer.ToString());
        Assert.Equal(before!.Length, after!.Length);
        }
        finally
    {
        Console.SetError(originalError);
    }
    }
}
