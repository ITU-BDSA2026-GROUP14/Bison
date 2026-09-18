using System.ComponentModel.Design;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
public record UniqueObservation(int Id, string Author, string Message, long Timestamp, [property: Index(4)] string Location)
: UniqueCheep(Id, Author, Message, Timestamp)
{
    public override string ToString() => base.ToString() + $" (Seen at: {Location})";
}

public record GenericObservation(string Author, string Message, long Timestamp, string Location) : Cheep(Author, Message, Timestamp)
{
    public override string ToString() => base.ToString() + $" (Seen at: {Location})";
}