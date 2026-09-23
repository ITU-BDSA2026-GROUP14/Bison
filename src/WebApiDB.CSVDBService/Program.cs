// Prepare DB
using SimpleDB;
using Bison.Taxonomy;

TaxonTree taxonomy = TaxonTree.LoadFromEmbeddedResource();
Console.WriteLine($"Loaded {taxonomy.Count} taxa");
CsvDatabase<Observation> obsDb = CsvDatabase<Observation>.GetInstance("../Bison.CLI/bison_observe_cli_db.csv");
CsvDatabase<Comment> comDb = CsvDatabase<Comment>.GetInstance("../Bison.CLI/bison_comment_cli_db.csv");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// HTTP Get Requests
app.MapGet("/observations", () => obsDb.Read());
app.MapGet("/Comments", (int id) => getComments(id));

// HTTP Post Requests
app.MapPost("/observation", (Observation obs) => getObservation(obs));
app.MapPost("/comment", (Comment obs) => getComment(obs));



// Helper methods
IEnumerable<Comment> getComments(int id)
{
    IEnumerable<Comment> comments = comDb.Read();
    var tmp = new List<Comment>(); ;

    var idProperty = typeof(Comment).GetProperty("Id");

    foreach (var o in comments)
    {
        if ((int?)idProperty?.GetValue(o) == id)
        {
            tmp.Add(o);
        }
    }
    return tmp;
}

Observation getObservation(Observation obs)
{
    obsDb.Store(obs);
    return obs;
}

Comment getComment(Comment obs)
{
    comDb.Store(obs);
    return obs;
}

app.Run();
