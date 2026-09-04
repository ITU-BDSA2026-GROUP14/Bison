using System.Globalization;

public record Cheep(string Author, string Observation, long Timestamp)
{
    public override string ToString()
    {
        DateTimeOffset ts = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
        return $"{Author} @ {ts.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: {Observation}";
    }
};