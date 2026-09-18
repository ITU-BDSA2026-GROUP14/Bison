// Prepare DB
using SimpleDB;

CsvDatabase<Observation> obsDb = CsvDatabase<Observation>.GetInstance("../Bison.CLI/bison_observe_cli_db.csv");
CsvDatabase<Observation> comDb = CsvDatabase<Observation>.GetInstance("../Bison.CLI/bison_comment_cli_db.csv");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// HTTP Get Requests
app.MapGet("/observations", () => obsDb.Read());

// TODO: App endpoint for comments

// HTTP Post Requests


app.Run();
