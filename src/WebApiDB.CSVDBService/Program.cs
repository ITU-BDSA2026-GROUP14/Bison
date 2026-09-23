// Prepare DB
using SimpleDB;
using Bison.Taxonomy;

TaxonTree taxonomy = TaxonTree.LoadFromEmbeddedResource();
Console.WriteLine($"Loaded {taxonomy.Count} taxa");

CsvDatabase<Proposal> propDb = CsvDatabase<Proposal>.GetInstance("../Bison.CLI/bison_proposal_cli_db.csv");
CsvDatabase<UniqueObservation> obsDb = CsvDatabase<UniqueObservation>.GetInstance("../Bison.CLI/bison_observe_cli_db.csv");
CsvDatabase<Comment> comDb = CsvDatabase<Comment>.GetInstance("../Bison.CLI/bison_comment_cli_db.csv");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// HTTP Get Requests
app.MapGet("/observations", () => obsDb.Read());
app.MapGet("/comments", (int id) => getComments(id));
app.MapGet("/proposals", (int id) => getProposals(id));

// HTTP Post Requests
app.MapPost("/observation", (UniqueObservation obs) => postObservation(obs));
app.MapPost("/comment", (Comment obs) => postComment(obs));
app.MapPost("/proposal", (Proposal proposal) => postProposal(proposal));

bool observationExists(int id)
{
    return obsDb.Read().Any(o => o.Id == id);
}

IResult getComments(int id)
{
    if (!observationExists(id))
    {
        return Results.NotFound($"Observation with ID {id} not found.");
    }
    var comments = comDb.Read().Where(c => c.Id == id);
    return Results.Ok(comments);
}

// Helper methods
UniqueObservation postObservation(UniqueObservation obs)
{
    var observations = obsDb.Read().ToList();
    int nextId = observations.Count == 0 ? 0 : observations.Max(o => o.Id) + 1; //biggest id + 1

    var newObs = obs with { Id = nextId }; // copy of obs with the server-generated ID
    obsDb.Store(newObs);
    return newObs;
}

IResult postComment(Comment comment)
{
    if (!observationExists(comment.Id))
    {
        return Results.NotFound($"Observation with ID {comment.Id} not found.");
    }
    comDb.Store(comment);
    return Results.Ok(comment);
}

bool taxonExists(string taxonId)
{
    return taxonomy.GetById(taxonId) != null;
}

IResult getProposals(int id)
{
    if (!observationExists(id))
    {
        return Results.NotFound($"Observation with ID {id} not found.");
    }
    var proposals = propDb.Read().Where(p => p.ObservationId == id);
    return Results.Ok(proposals);
}

IResult postProposal(Proposal proposal)
{
    //An unknown taxonId is not allowed, because the server must be able to validate it.
    if (!observationExists(proposal.ObservationId) || !taxonExists(proposal.TaxonId))
    {
        return Results.NotFound($"Observation {proposal.ObservationId} or taxon {proposal.TaxonId} not found.");
    }
    propDb.Store(proposal);
    return Results.Ok(proposal);
}

app.Run();
