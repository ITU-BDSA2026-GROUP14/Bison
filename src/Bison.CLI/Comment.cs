using System.Globalization;

public record Comment(int Id, string Author, string Message, long Timestamp) : Cheep(Author, Message, Timestamp)
{
    public override string ToString() => base.ToString();
}