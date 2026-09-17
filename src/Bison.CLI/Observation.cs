using System.ComponentModel.Design;
using System.Globalization;
public record Observation(int Id, string Author, string Message, long Timestamp, string Location) : Cheep(Id, Author, Message, Timestamp)
{
    public override string ToString() => base.ToString();
}