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

    /// <summary>
    /// Tests that the "location" command returns observations for a known location.
    /// </summary>
    [Fact]
    public void CommandLocationReturnsMatchesTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            Program.Main(new[] { "location", "eee" });
            Assert.Contains("mivh", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    /// Tests that the "location" command returns a message indicating that there are no observations for an unknown location.
    /// </summary>
    [Fact]
    public void CommandLocationReturnsNoneForUnknownLocationTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            Program.Main(new[] { "location", "nowhere-that-exists" });
            Assert.Contains("No observations on this location", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}