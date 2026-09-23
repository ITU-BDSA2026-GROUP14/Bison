using System.Globalization;

public record Proposal(int ObservationId, string Author, string TaxonId, long Timestamp)
{
    public override string ToString()
    {
        DateTimeOffset ts = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
        return $"{Author} @ {ts.ToString("MM/dd/yy HH:mm:ss", CultureInfo.InvariantCulture)}: proposes taxon {TaxonId}";
    }
}