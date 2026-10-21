using Bison.Razor.Models;

public class FilterTest
{
    [Fact]
    public void FilterObservationsReturnsTaxonAndSubtaxa()
    {
        // Arrange
        var hejrer = new Taxon { TaxonId = 2, VernacularName = "Hejrer" };
        var ardea = new Taxon { TaxonId = 8, Parent = hejrer };
        var pelikaner = new Taxon { TaxonId = 4, VernacularName = "Pelikaner" };

        var heron = new Observation { Text = "En hejre", Taxon = ardea };
        var someHeron = new Observation { Text = "Endnu en hejre", Taxon = hejrer };
        var pelican = new Observation { Text = "En eller anden pelikan", Taxon = pelikaner };
        var observations = new[] { heron, someHeron, pelican };

        // Act
        var result = Bison.Razor.Filtering.__default.FilterObservations(hejrer, observations);

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Contains(heron, result);
        Assert.Contains(someHeron, result);
        Assert.DoesNotContain(pelican, result);
    }
}