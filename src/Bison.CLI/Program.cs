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
    public static int Main(string[] args)
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
                GenericObservation observation = new
                (
                    Author: Environment.UserName,
                    Message: message,
                    Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Location: location
                );

                var response = await client.PostAsJsonAsync<GenericObservation>("/observation", observation);
                var created = await response.Content.ReadFromJsonAsync<UniqueObservation>();
                Console.WriteLine($"Observation created with id: {created?.Id}");
            }
            else
            {
                Console.WriteLine("need a message and location");
            }

            return 0;
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

            if (string.IsNullOrEmpty(message))
            {
                Console.Error.WriteLine("Error: You must provide a message.");
                return 1;
            }
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
                Console.Error.WriteLine($"Error: Observation with id {id} does not exist.");
                return 1;
            }

            Console.WriteLine($"Comment created for observation with id: {id}");
            return 0;
        });

        rootCommand.Subcommands.Add(commentCommand);

        // read 
        Command readCommand = new Command("read", "Reads observations from CSV file");
        readCommand.SetAction(async parseResult =>
        {
            var response = await client.GetFromJsonAsync<UniqueObservation[]>("/observations");
            // UserInterface<Observation>.PrintObservations(obs_db.Read())
            // TODO: Add check to see if response is healthy. Then print with headers and pretty formatting.
            UserInterface<UniqueObservation>.PrintObservations(response);
            // Console.Write(response);
            return 0;
        });
        rootCommand.Subcommands.Add(readCommand);

        //discuss <id>
        Command discussCommand = new Command("discuss", "Reads comments on bison observation");
        Argument<int> discussId = new Argument<int>("id");
        discussCommand.Arguments.Add(discussId);
        discussCommand.SetAction(async parseResult =>
        {
            var id = parseResult.GetValue(discussId);
            var response = await client.GetAsync($"/comments?id={id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.Error.WriteLine($"Error: Observation with id {id} does not exist.");
                return 1;
            }
            var comments = await response.Content.ReadFromJsonAsync<Comment[]>();
            if (comments == null || comments.Length == 0)
            {
                Console.WriteLine("No comments");
                return 0;
            }
            UserInterface<Comment>.PrintObservations(comments);
            return 0;
        });
        rootCommand.Subcommands.Add(discussCommand);

        //location <Location>
        Command locationCommand = new Command("location", "Reads all observations made at a given location");
        Argument<string> locationArg = new Argument<string>("location");
        locationCommand.Arguments.Add(locationArg);
        locationCommand.SetAction(async parseResult =>
        {
            var location = parseResult.GetValue(locationArg);
            var response = await client.GetFromJsonAsync<UniqueObservation[]>("/observations");
            
            var matches = response?.Where(o => string.Equals(o.Location, location, StringComparison.OrdinalIgnoreCase));

            if(matches != null && matches.Any()){
                UserInterface<UniqueObservation>.PrintObservations(matches);
            }else{
                Console.WriteLine("No observations on this location");
            }

            return 0;
        });
        rootCommand.Subcommands.Add(locationCommand);

        return rootCommand.Parse(args).Invoke();
    }
}