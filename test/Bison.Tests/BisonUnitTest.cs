

public class BisonUnitTest
{
    [Fact]
    public void CommentIdErrorTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            Program.Main(new[] { "comment", "100000", "test" });

            // Assert
            Assert.Contains("comment id must match a bison observation id", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }

    }
}