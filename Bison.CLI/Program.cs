using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;
using System.CommandLine;
using System.Data.Common;


public class Program
{
    // static CsvDatabase<Cheep> db = new CsvDatabase<Cheep>("bison_observe_cli_db.csv");

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
                db.Store(new Cheep(
                    Author: Environment.UserName,
                    Observation: args[1],
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                ));
                return;
            }

            Console.WriteLine("No commands we're given. Use --help for assistance.");
            return;
        });


        rootCommand.Parse(args).Invoke();


        // // Read from CSV
        // if (args.Length > 0 && args[0] == "read")
        // {
        //     UserInterface<Cheep>.PrintObservations(db.Read());
        // }

        // Write to CSV
        // if (args.Length > 0 && args[0] == "observe" && args.Length > 1)
        // {
        //     db.Store(new Cheep(
        //             Author: Environment.UserName,
        //             Observation: args[1],
        //             Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        //         ));
        // }
    }
}
