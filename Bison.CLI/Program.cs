using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;

var db = new CsvDatabase<Cheep>("bison_observe_cli_db.csv");

// Read from CSV
if (args.Length > 0 && args[0] == "read")
{
    foreach (var line in db.Read())
    {
        DateTimeOffset timestamp = DateTimeOffset.FromUnixTimeSeconds(line.Timestamp);
        Console.WriteLine($"{line.Author} @ {timestamp.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {line.Observation}");
    }
}

// Write to CSV
if (args.Length > 0 && args[0] == "observe" && args.Length > 1)
{
    db.Store(new Cheep(
            Author: Environment.UserName,
            Observation: args[1],
            Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        ));
}