using SimpleDB;


public class BisonE2ETests()
{

    [Fact]
    public void CommandReadTest()
    {
        // Arrange
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            Program.Main(new[] { "read" });

            // Assert
            Assert.Contains("mivh", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void CommandObservationTest()
    {
        // Arrange
        var obsDb = CsvDatabase<Observation>.GetInstance("bison_observe_cli_db.csv");
        var countBefore = obsDb.Read().ToList().Count;

        // Act
        Program.Main(new[] { "observe", "test message", "test location" });

        // Assert
        var observations = obsDb.Read().ToList();
        Assert.Equal(countBefore + 1, observations.Count);

        var stored = observations.Last();
        Assert.Equal("test message", stored.Message);
        Assert.Equal("test location", stored.Location);
    }
}