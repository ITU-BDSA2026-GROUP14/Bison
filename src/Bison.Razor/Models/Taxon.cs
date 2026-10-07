namespace Bison.Razor.Models
{
    public class Taxon
    {
      public string dwc_TaxonID { get; set; } = "";
      public string? vernacularName { get; set; }
      public Taxon? Parent { get; set; }
      public List<Taxon> Children { get; set; } = new();
    }
}