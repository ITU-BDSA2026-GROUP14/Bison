namespace Bison.Tests;

public class BisonUnitTest
{
    [Fact]
    public void DoesProgramOutputObservations()
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
}