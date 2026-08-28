using System.Globalization;

if (args.Length > 0 && args[0] == "read")
{
foreach (var line in File.ReadLines("bison_observe_cli_db.csv").Skip(1))
{
    var first = line.IndexOf(',');
    var last = line.LastIndexOf(',');

    var author = line[..first];
    var message = line[(first + 1)..last].Trim('"');
    var time = DateTimeOffset.FromUnixTimeSeconds(long.Parse(line[(last + 1)..])).LocalDateTime;

    Console.WriteLine($"{author} @ {time.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {message}");
}
}
