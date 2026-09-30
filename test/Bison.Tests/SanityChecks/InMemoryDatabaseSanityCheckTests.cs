using Microsoft.Data.Sqlite;
using Xunit;

public class InMemoryDatabaseSanityCheckTests
{
    [Fact]
    public void SeparateConnectionsShareTheSameInMemoryData()
    {
        // 1. Build a connection string pointing at a *named*, shared-cache, in-memory database.
        var dbName = Guid.NewGuid().ToString("N");
        var dbUri = $"file:{dbName}?mode=memory&cache=shared";
        var connectionString = new SqliteConnectionStringBuilder { DataSource = dbUri }.ToString();

        // 2. Open connection "A" and keep it open - this is what keeps the in-memory
        //    database alive for the rest of this test.
        using var keepAliveConnection = new SqliteConnection(connectionString);
        keepAliveConnection.Open();

        // 3. Create a table and insert one row, using connection A.
        using (var create = keepAliveConnection.CreateCommand())
        {
            create.CommandText = "CREATE TABLE greeting (message TEXT NOT NULL);";
            create.ExecuteNonQuery();
        }
        using (var insert = keepAliveConnection.CreateCommand())
        {
            insert.CommandText = "INSERT INTO greeting (message) VALUES ('hello from connection A');";
            insert.ExecuteNonQuery();
        }

        // 4. Open a SECOND, completely separate connection "B" - same connection
        //    string, brand new SqliteConnection object. This is exactly what
        //    DBFacade does every time one of its methods runs a query.
        using var secondConnection = new SqliteConnection(connectionString);
        secondConnection.Open();

        string? result;
        using (var select = secondConnection.CreateCommand())
        {
            select.CommandText = "SELECT message FROM greeting;";
            result = (string?)select.ExecuteScalar();
        }

        // 5. If connection B can see what connection A inserted, this passes.
        Assert.Equal("hello from connection A", result);
    }
}