using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;
using System.CommandLine;
using System.Data.Common;


public class Program
{
    public static void Main(string[] args)
    {
        Option<bool> readOption = new("--read", "--r") { Description = "Reads observations from CSV file" };
        Option<string> storeOption = new("--observe", "--obs") { Description = "Store a new observation to CSV file" };
        Option<string> fileOption = new("--file", "--f") { Description = "Name of CSV file to read from or store to" };

        RootCommand rootCommand = new("Animal observation portal");
        rootCommand.Options.Add(readOption);
        rootCommand.Options.Add(storeOption);
        rootCommand.Options.Add(fileOption);


        rootCommand.SetAction(parseResult =>
        {
            // Initialize database
            string? filename = parseResult.GetValue(fileOption);
            if (filename is null)
            {
                Console.Error.WriteLine("A filename is required.");
                return;
            }
            var db = new CsvDatabase<Cheep>(filename);

            // Read options
            bool isRead = parseResult.GetValue(readOption);
            string? isStore = parseResult.GetValue(storeOption);

            if (isRead && isStore is not null)
            {
                Console.Error.WriteLine("You can't read and store at the same time.");
                return;
            }

            if (isRead)
            {
                UserInterface<Cheep>.PrintObservations(db.Read());
                return;
            }

            if (isStore is not null)
            {
                handleObserve(db, args[1]);
                return;
            }

            Console.WriteLine("No commands we're given. Use --help for assistance.");
            return;
        });


        rootCommand.Parse(args).Invoke();
    }

    private static void handleObserve(CsvDatabase<Cheep> db, string message)
    {
        db.Read().Last();
        db.Store(new Cheep(
            Author: Environment.UserName,
            Message: message,
            Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        ));
    }
}
