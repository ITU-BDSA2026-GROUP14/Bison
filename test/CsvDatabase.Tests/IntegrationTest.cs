using SimpleDB;

namespace CsvDatabase.Tests;

public class IntegrationTest
{
    [Fact]
    public void DatabaseReturnsRecords()
    {
        var db = CsvDatabase<Observation>.GetInstance("testData/o.csv");

        var records = db.Read().ToList();

        Assert.NotEmpty(records);
    }

    [Fact]
    public void DatabaseReturnStoredRecord()
    {
        CsvDatabase<Observation> db = CsvDatabase<Observation>.GetInstance("testData/o.csv");

        var ob = new Observation(
                Id: 67,
                Author: "test",
                Message: "message",
                Timestamp: 6767,
                Location: "location"
             );

        db.Store(ob);

        List<Observation> records = db.Read().ToList();

        var ob2 = records.Last();

        Assert.Equal(ob, ob2);
    }

}