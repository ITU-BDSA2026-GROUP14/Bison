using Microsoft.Data.Sqlite;
public class DBFacade
{
    private readonly string _connectionString;

    public DBFacade(string dbFilePath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = dbFilePath
        }.ToString();
    }

    public List<ObservationViewModel> GetObservations()
    {
        const string sql = @"
        SELECT u.username, o.text, o.pub_date
        FROM observation o
        JOIN user u ON o.author_id = u.user_id
        ORDER BY o.pub_date DESC";

        return RunQuery(sql);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        const string sql = @"
        SELECT u.username, o.text, o.pub_date
        FROM observation o
        JOIN user u ON o.author_id = u.user_id
        WHERE u.username = @author
        ORDER BY o.pub_date DESC";

        return RunQuery(sql, ("@author", author));
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    private List<ObservationViewModel> RunQuery(string sql, params (string Name, object Value)[] parameters)
    {
        var result = new List<ObservationViewModel>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        foreach (var (name, value) in parameters)
        {
            command.Parameters.AddWithValue(name, value);
        }

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var author = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            result.Add(new ObservationViewModel(author, message, UnixTimeStampToDateTimeString(timestamp)));
        }

        return result;
    }


}