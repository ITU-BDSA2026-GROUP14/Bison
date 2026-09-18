using System.Globalization;
using CsvHelper;

namespace Bison.Taxonomy;

public class TaxonTree
{
    private readonly Dictionary<string, Taxon> byId = new(); //Look up a Taxon by its Id

    private readonly Dictionary<string, Taxon> byVernacularName = new(StringComparer.OrdinalIgnoreCase); 
    //Look up a Taxon by its Name (ignores upper/lower case)

    private readonly Dictionary<string, List<Taxon>> byParentId = new(); //Look up a list of Taxons by their ParentId

    public TaxonTree(IEnumerable<Taxon> tax)
    {
        foreach (var taxon in tax)
        {
            byId[taxon.TaxonID] = taxon;

            if (taxon.VernacularName != "")
            {
                byVernacularName[taxon.VernacularName] = taxon;
            }
            if (taxon.ParentNameUsageID != "")
            {
                if (!byParentId.ContainsKey(taxon.ParentNameUsageID))
                {
                    byParentId[taxon.ParentNameUsageID] = new List<Taxon>();
                }
                byParentId[taxon.ParentNameUsageID].Add(taxon);
            }
        }
    }
}