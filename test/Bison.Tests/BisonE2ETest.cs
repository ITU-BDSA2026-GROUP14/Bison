


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
    public void DoesProgramStoreObservations()
    {

    }

    [Fact]
    public void CommandObserveteste2e()
    {

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