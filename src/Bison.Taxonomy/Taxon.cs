using CsvHelper.Configuration.Attributes;

namespace Bison.Taxonomy;

public record Taxon
{
    [Name("dwc:taxonID")]
    public string TaxonId { get; init; } = "";

    [Name("dwc:parentNameUsageID")]
    public string? ParentId { get; init; } = "";

    [Name("dwc:taxonRank")]
    public string Rank { get; init; } = "";

    [Name("dwc:scientificName")]
    public string ScientificName { get; init; } = "";

    [Name("dwc:vernacularName")]
    public string? VernacularName { get; init; } = "";
}
    
