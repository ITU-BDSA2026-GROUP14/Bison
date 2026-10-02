using Microsoft.Data.Sqlite;
public class DBFacade
{
    private readonly int pageSize = 32;
    private readonly string _connectionString;

    public DBFacade(string dbFilePath)
    {
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = dbFilePath
        }.ToString();
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        const string sql = @"
        SELECT u.username, o.text, o.pub_date
        FROM observation o
        JOIN user u ON o.author_id = u.user_id
        ORDER BY o.pub_date DESC
        LIMIT $limit OFFSET $offset"; 

        return RunQuery(sql,
                ("$limit", pageSize),
                ("$offset", (page - 1) * pageSize));
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        const string sql = @"
        SELECT u.username, o.text, o.pub_date
        FROM observation o
        JOIN user u ON o.author_id = u.user_id
        WHERE u.username = @author
        ORDER BY o.pub_date DESC
        LIMIT $limit OFFSET $offset";

        return RunQuery(sql, 
                    ("@author", author),
                    ("$limit", pageSize),
                    ("$offset", (page - 1) * pageSize));
    }

    public List<ObservationViewModel> GetObservationFromId(int id)
    {
        const string sql = @"
        SELECT u.username, o.text, o.pub_date
        FROM observation o
        JOIN user u ON o.author_id = u.user_id
        WHERE o.observation_id = @id
        ORDER BY o.pub_date DESC";

        return RunQuery(sql, ("@id", id));
    }


    // Comment quries
    // NOTE: observationViewModel is used for comments as well,
    // ether change the name of ObservationViewModel or make a new model
    // new model is harder since we have to map every runQuery call so it knows wha tit is
    public List<ObservationViewModel> GetCommentsFromId(int id) {
        const string sql = @"
        SELECT u.username, c.text, c.pub_date
        FROM comment c
        JOIN user u ON c.author_id = u.user_id
        WHERE c.observation_id = @id
        ORDER BY c.pub_date DESC";

        return RunQuery(sql,("@id", id));
    }

    public List<ObservationViewModel> GetCommentsFromAuthor(string author) {
        const string sql = @"
        SELECT u.username, c.text, c.pub_date
        FROM comment c
        JOIN user u ON c.author_id = u.user_id
        WHERE u.username = @author
        ORDER BY c.pub_date DESC";   

        return RunQuery(sql,("@author", author));
    }

    //Proposals 
    //NOTE: uses observationViewModel also
    public List<ObservationViewModel> GetProposals() {
        const string sql = @"
        SELECT u.username, p.text, p.pub_date
        FROM proposal p
        JOIN user u ON p.author_id = u.user_id
        ORDER BY p.pub_date DESC";   

        return RunQuery(sql);
    }

    public List<ObservationViewModel> GetProposalsFromId(int id) {
        const string sql = @"
        SELECT u.username, p.text, p.pub_date
        FROM proposal p
        JOIN user u ON p.author_id = u.user_id
        WHERE p.observation_id = @id
        ORDER BY p.pub_date DESC";   

        return RunQuery(sql,("@id", id));
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