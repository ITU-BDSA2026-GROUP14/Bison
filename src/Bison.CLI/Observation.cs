using System.ComponentModel.Design;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
public record Observation(int Id, string Author, string Message, long Timestamp,[property: Index(4)] string Location) 
: Cheep(Id, Author, Message, Timestamp)
{
    public override string ToString() => base.ToString();
}