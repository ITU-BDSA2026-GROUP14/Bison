// using System.Globalization;
// using System.Runtime.InteropServices;
// using CsvHelper;
// using CsvHelper.Configuration;

// Read
using System.Globalization;
using CsvHelper;

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

// Write
if (args.Length > 0 && args[0] == "observe" && args.Length > 1)
{
    // Handle the observe command for the specific location
    using (StreamWriter stream = File.AppendText("bison_observe_cli_db.csv"))
    {

        string author = Environment.UserName;
        string message = args[1];
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();


        stream.WriteLine($"{author},\"{message}\",{timestamp}");
        Console.WriteLine($"Observation recorded for '{message}'.");
    }

}