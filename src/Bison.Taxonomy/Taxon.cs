

using CsvHelper.Configuration.Attributes;

public record Taxon
{
    [Name("dwc:taxonID")]
    public string TaxonID { get; init; } = "";

    [Name("dwc:parenNameUsageID")]
    public string? ParentNameUsageID { get; init; }

    [Name("dwc:taxonRank")]
    public string TaxonRank { get; init; } = "";

    [Name("dwc:scientificName")]
    public string ScientificName { get; init; } = "";

    [Name("dwc:vernacularName")]
    public string? VernacularName { get; init; }
}
    
