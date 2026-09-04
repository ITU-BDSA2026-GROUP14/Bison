using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

// Read from CSV
if (args.Length > 0 && args[0] == "read")
{
    using (var reader = new StreamReader("bison_observe_cli_db.csv"))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var records = csv.GetRecords<Cheep>();

        foreach (var line in records)
        {
            Console.WriteLine($"{line.Author} @ {line.Timestamp.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {line.Observation}");
        }
    }
}

// Write to CSV
if (args.Length > 0 && args[0] == "observe" && args.Length > 1)
{
    var message = args[1];
    var list = new List<Cheep>
    {
        new Cheep(
            Author: Environment.UserName,
            Observation: message,
            Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        )
    };

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };
    using (var stream = File.Open("bison_observe_cli_db.csv", FileMode.Append))
    using (var writer = new StreamWriter(stream))
    using (var csv = new CsvWriter(writer, config))
    {
        csv.WriteRecords(list);
        Console.WriteLine($"Observation recorded for '{message}'.");
    }
}