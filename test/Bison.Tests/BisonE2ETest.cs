using SimpleDB;


public class BisonE2ETests()
{
    /**
        * Test that the "read" command correctly outputs the expected author name "mivh" to the console.
    */
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

    /**
        * Test that the "comment" command correctly outputs the expected author name "mivh" to the console.
    */
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