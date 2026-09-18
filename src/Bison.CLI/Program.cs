using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;
using System.CommandLine;
using System.Data.Common;
using CsvHelper.Configuration.Attributes;
using System.Net.Http.Json;
using System.Net;


public class Program
{
    // static CsvDatabase<Cheep> db = new CsvDatabase<Cheep>("bison_observe_cli_db.csv");

    public static void Main(string[] args)
    {
        // Initialize DB connections
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        // Initiailize CLI command tool
        RootCommand rootCommand = new("Animal observation portal");

        // observe <message> <location> 
        Command observeCommand = new Command("observe", "Store a new bison observation to CSV file");
        var observerMessage = new Argument<string>("message");
        var observerLocation = new Argument<string>("location");
        observeCommand.Arguments.Add(observerMessage);
        observeCommand.Arguments.Add(observerLocation);
        observeCommand.SetAction(async parseResult =>
        {
            var message = parseResult.GetValue(observerMessage);
            var location = parseResult.GetValue(observerLocation);

            if (message != null && location != null)
            {
                Observation observation = new Observation(
                    // TODO: Should not provide id. Web API should determine id automatically.
                    Author: Environment.UserName,
                    Message: message,
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Location: location
                    );

                var response = await client.PostAsJsonAsync<Observation>("/observation", observation);
            }
            else
            {
                Console.WriteLine("need a message and location");
            }
        });
        rootCommand.Subcommands.Add(observeCommand);


        // comment <message> <id>
        Command commentCommand = new Command("comment", "Store a new comment linked to a bison observation");
        var commentMessage = new Argument<string>("message");
        var commentId = new Argument<int>("id");
        commentCommand.Arguments.Add(commentMessage);
        commentCommand.Arguments.Add(commentId);
        commentCommand.SetAction(async parseResult =>
        {
            var id = parseResult.GetValue(commentId);
            var message = parseResult.GetValue(commentMessage);

            if (message != null || message.Trim().Length == 0)
            {
                Comment comment = new Comment(
                    Id: id,
                    Author: Environment.UserName,
                    Message: message,
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                );

                var response = await client.PostAsJsonAsync<Comment>("/comment", comment);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // TODO: Web API must throw error if the id doesn't match any in the CsvDatabase. This must be implemented in database. Google which errorcode is best.
                    Console.WriteLine("Observation does not exist with that id.");
                }
            }
            else
            {
                Console.WriteLine("You must provide a message.");
            }
        }
        );
        rootCommand.Subcommands.Add(commentCommand);

        // read 
        Command readCommand = new Command("read", "Reads observations from CSV file");
        readCommand.SetAction(async parseResult =>
        {
            var response = await client.GetFromJsonAsync<Observation[]>("/observations");
            // UserInterface<Observation>.PrintObservations(obs_db.Read())
            // TODO: Add check to see if response is healthy. Then print with headers and pretty formatting.
            Console.Write(response);
        });
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

