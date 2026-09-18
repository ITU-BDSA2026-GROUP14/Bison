using System.Globalization;
using CsvHelper.Configuration.Attributes;

public abstract record Cheep(
    [property: Index(0)]
    int Id,
    [property: Index(1)]
    string Author,
    [property: Index(2)]
    string Message,
    [property: Index(3)]
    long Timestamp)
{
    public override string ToString()
    {
        DateTimeOffset ts = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
        return $"{Author} @ {ts.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {Message}";
    }
};