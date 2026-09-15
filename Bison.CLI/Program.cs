using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;
using System.CommandLine;
using System.Data.Common;
using CsvHelper.Configuration.Attributes;


public class Program
{
    // static CsvDatabase<Cheep> db = new CsvDatabase<Cheep>("bison_observe_cli_db.csv");

    public static void Main(string[] args)
    {
        // Option<bool> readOption = new("--read", "--r") { Description = "Reads observations from CSV file" };
        // Option<string> observeOption = new("--observe", "--obs") { Description = "Store a new observation to CSV file" };

        // rootCommand.Options.Add(readOption);
        // rootCommand.Options.Add(storeOption);
        // rootCommand.Options.Add(fileOption);

        // Parsh dbs
        CsvDatabase<Observation> obs_db = CsvDatabase<Observation>.GetInstance("bison_observe_cli_db-csv");
        CsvDatabase<Comment> cmt_db = CsvDatabase<Comment>.GetInstance("bison_comment_cli_db-csv");

        int idCount = obs_db.Read().ToList().Count;

        RootCommand rootCommand = new("Animal observation portal");

        // observe <message> 
        Command observeCommand = new Command("observe", "Store a new bison observation to CSV file");
        var observerMessage = new Argument<string>("message");
        observeCommand.Arguments.Add(observerMessage);
        observeCommand.SetAction(parseResult =>
        {
            var message = parseResult.GetValue(observerMessage);

            if (message != null)
            {
                obs_db.Store(new Observation(
                    Id: idCount++,
                    Author: Environment.UserName,
                    Message: message,
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    ));
            }
            else
            {
                Console.WriteLine("need a message");
            }
        });
        rootCommand.Subcommands.Add(observeCommand);


        // comment <message> <id>
        Command commentCommand = new Command("comment", "Store a new comment linked to a bison observation");
        var commentMessage = new Argument<string>("message");
        var commentId = new Argument<int>("id");
        commentCommand.Arguments.Add(commentMessage);
        commentCommand.Arguments.Add(commentId);
        commentCommand.SetAction(parseResult =>
        {
            var id = parseResult.GetValue(commentId);
            var message = parseResult.GetValue(commentMessage);

            if (id <= idCount && message != null)
            {
                cmt_db.Store(new Comment(
                    Id: id,
                    Author: Environment.UserName,
                    Message: message,
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                ));
            }
            else
            {
                Console.WriteLine("comment id must match a bison observation id");
            }
        }
        );
        rootCommand.Subcommands.Add(commentCommand);

        // read 
        Command readCommand = new Command("read", "Reads observations from CSV file");
        readCommand.SetAction(parseResult => UserInterface<Observation>.PrintObservations(obs_db.Read()));
        rootCommand.Subcommands.Add(readCommand);

        //discuss <id>
        Command discussCommand = new Command("discuss", "Reads comments on bison observation");
        Argument<int> discussId = new Argument<int>("id");
        discussCommand.Arguments.Add(discussId);
        discussCommand.SetAction(parseResult =>
        {
            var id = parseResult.GetValue(discussId);
            if (id <= idCount)
            {
                if (cmt_db.Read().ToList().Count > 0)
                {
                    UserInterface<Comment>.PrintComments(cmt_db.Read(), id);
                }
                else
                {
                    Console.WriteLine("No comments");
                }
            }
            else
            {
                Console.WriteLine("Observation does not exist");
            }
        });
        rootCommand.Subcommands.Add(discussCommand);

        rootCommand.Parse(args).Invoke();

        // obsreve command
        // rootCommand.SetAction(parseResult =>
        // {

        //     // Initialize database
        //     string? filename = parseResult.GetValue(fileOption);
        //     if (filename is null)
        //     {
        //         Console.Error.WriteLine("A filename is required.");
        //         return;
        //     }
        //     bool isRead = parseResult.GetValue(readOption);
        //     string? isStore = parseResult.GetValue(storeOption);

        //     if (isRead && isStore is not null)
        //     {
        //         Console.Error.WriteLine("You can't read and store at the same time.");
        //         return;
        //     }

        //     if (isRead)
        //     {
        //         UserInterface<Cheep>.PrintObservations(.Read());
        //         return;
        //     }

        //     if (isStore is not null)
        //     {
        //         obsdb.Store(new Observation(
        //             Id: idCount++,
        //             Author: Environment.UserName,
        //             Message: args[1],
        //             Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        //         ));
        //         return;
        //     }

        //     Console.WriteLine("No commands we're given. Use --help for assistance.");
        //     return;
        // });





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

