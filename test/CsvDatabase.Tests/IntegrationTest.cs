using SimpleDB;

namespace CsvDatabase.Tests;

public class IntegrationTest
{
    /**
        * Test that the CsvDatabase correctly returns records from the CSV file.
    */
    [Fact]
    public void DatabaseReturnsRecords()
    {
        var db = CsvDatabase<UniqueObservation>.GetInstance("testData/o.csv");

        var records = db.Read().ToList();

        Assert.NotEmpty(records);
    }

    /**
        * Test that the CsvDatabase correctly stores and retrieves an Observation record.
    */
    [Fact]
    public void DatabaseReturnStoredRecord()
    {
        CsvDatabase<UniqueObservation> db = CsvDatabase<UniqueObservation>.GetInstance("testData/o.csv");

        var ob = new UniqueObservation(
                Id: 67,
                Author: "test",
                Message: "message",
                Timestamp: 6767,
                Location: "location"
             );

        db.Store(ob);

        List<UniqueObservation> records = db.Read().ToList();

        var ob2 = records.Last();

        Assert.Equal(ob, ob2);
    }

}