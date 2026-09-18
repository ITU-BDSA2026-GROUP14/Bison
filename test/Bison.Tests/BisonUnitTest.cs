

public class BisonUnitTest
{
    /**
        * This test checks that the comment command returns an error message when provided with a comment ID that does not match any existing bison observation ID.
    */
    [Fact]
    public void CommentIdErrorTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            Program.Main(new[] { "comment", "test", "100000" });

            // Assert
            Assert.Contains("comment id must match a bison observation id", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /**
        * This test creates a Comment instance with a Unix timestamp of 0 and checks that the output string matches the expected format.
    */
    [Fact]
    public void UnixTimestampTest()
    {
        // Arrange
        var comment = new Comment(
            Id: 1,
            Author: "unix test",
            Message: "unix timestamp test",
            Timestamp: 0
        );

        // Act
        var result = comment.ToString();

        // Assert
        Assert.Equal("unix test @ 01/01/70 00:00:00: unix timestamp test", result);
    }

    /**
        * Test that the PrintComments method correctly filters comments by the specified observation ID.
        * This test creates a list of comments with different IDs and checks that only the comment with the matching ID is printed.
    */
    [Fact]
    public void CommentsMatchingIdTest()
    {
        // Arrange
        var comments = new List<Comment>
    {
        new Comment(Id: 1, Author: "simon", Message: "first", Timestamp: 100),
        new Comment(Id: 2, Author: "daniel", Message: "second", Timestamp: 200),
        new Comment(Id: 3, Author: "filip", Message: "third", Timestamp: 300),
    };

        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            UserInterface<Comment>.PrintComments(comments, 2);

            // Assert
            var output = writer.ToString();
            Assert.Contains("daniel", output);
            Assert.DoesNotContain("simon", output);
            Assert.DoesNotContain("filip", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}