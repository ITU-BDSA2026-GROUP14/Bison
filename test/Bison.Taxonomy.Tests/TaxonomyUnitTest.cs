
namespace Bison.Taxonomy.Tests;

public class TaxonomyUnitTest

{
    private readonly TaxonTree tree = TaxonTree.LoadFromEmbeddedResource();
    [Fact]
    public void ProposalIdMatches()
    {
      var proposal = new Proposal(
        ObservationId: 5,
        Author: "tester",
        TaxonId: "MSTSNM:Arter:c28811f4-f785-ea11-aa77-501ac539d1ea", // Fiskehejre
        Timestamp: 0);

        var taxon = tree.GetById(proposal.TaxonId);

        Assert.NotNull(taxon);
        Assert.Equal("Ardea cinerea", taxon.ScientificName);
    }
      [Fact]
      public void ProposalWithInvalidTaxonId_IsNotFoundInTaxonomy()
    {
        var proposal = new Proposal(
            ObservationId: 5,
            Author: "tester",
            TaxonId: "not-a-real-taxon",
            Timestamp: 0);

            Assert.Null(tree.GetById(proposal.TaxonId));
    } 
    }

