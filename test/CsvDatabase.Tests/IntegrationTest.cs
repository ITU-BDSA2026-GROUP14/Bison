using SimpleDB;

namespace CsvDatabase.Tests;

public class IntegrationTest
{
    [Fact]
    public void DatabaseReturnsRecords()
    {
        var db = CsvDatabase<UniqueObservation>.GetInstance("testData/o.csv");

        var records = db.Read().ToList();

        Assert.NotEmpty(records);
    }

    [Fact]
    public void DatabaseReturnStoredRecord()
    {
        CsvDatabase<UniqueObservation> db = CsvDatabase<UniqueObservation>.GetInstance("testData/o.csv");

        var ob = new UniqueObservation(
                Id: 67,
                Author: "test",
                Message: "message",
                Timestamp: 6767,
                Location: "Test"
             );

        db.Store(ob);

        List<UniqueObservation> records = db.Read().ToList();

        var ob2 = records.Last();

        Assert.Equal(ob, ob2);
    }

}