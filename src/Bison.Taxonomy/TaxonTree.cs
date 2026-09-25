using System.Globalization;
using CsvHelper;

namespace Bison.Taxonomy;

// All taxa from taxon.csv, linked as a tree (each taxon knows its parent's id).
public class TaxonTree
{
    //Look up a Taxon by its Id
    private readonly Dictionary<string, Taxon> byId = new();


    //Look up a Taxon by its Name (ignores upper/lower case)
    private readonly Dictionary<string, Taxon> byVernacularName = new(StringComparer.OrdinalIgnoreCase);

    //Look up a list of Taxons by their ParentId
    private readonly Dictionary<string, List<Taxon>> childrenByParentId = new();

    public TaxonTree(IEnumerable<Taxon> taxa)
    {
        foreach (var taxon in taxa)
        {
            byId[taxon.TaxonId] = taxon;

            // Not every taxon has a Danish name
            if (taxon.VernacularName != "")
            {
                byVernacularName[taxon.VernacularName] = taxon;
            }
            if (taxon.ParentId != "")
            {
                if (!childrenByParentId.ContainsKey(taxon.ParentId))
                {
                    childrenByParentId[taxon.ParentId] = new List<Taxon>();
                }
                childrenByParentId[taxon.ParentId].Add(taxon);
            }
        }
    }
    //Reads taxon.csv from inside the .dll and builds the tree
    public static TaxonTree LoadFromEmbeddedResource()
    {
        var assembly = typeof(TaxonTree).Assembly;
        using var stream = assembly.GetManifestResourceStream("taxon.csv")
            ?? throw new InvalidOperationException("Embedded resource 'taxon.csv' not found.");
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return new TaxonTree(csv.GetRecords<Taxon>().ToList());
    }
    public int Count => byId.Count;

    // Return null if not found
    public Taxon? GetById(string taxonId)
    {
        return byId.GetValueOrDefault(taxonId);
    }

    // Returns null if not found
    public Taxon? GetByVernacularName(string vernacularName)
    {
        return byVernacularName.GetValueOrDefault(vernacularName);
    }

    // One step up, e.g. Hejrer -> Årefodede. Null if there is no parent in the data.   
    public Taxon? GetSupertaxon(Taxon taxon)
    {
        return byId.GetValueOrDefault(taxon.ParentId);
    }

    // One step down, e.g. Hejrer -> Ardea, Ardeola, ... Empty list if none.
    public IReadOnlyList<Taxon> GetSubtaxa(Taxon taxon)
    {
        return childrenByParentId.GetValueOrDefault(taxon.TaxonId) ?? new List<Taxon>();
    }
}