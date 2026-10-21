namespace Bison.Razor.Models
{
    public class Observation : Post
    {
        public Taxon? Taxon { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<Proposal> Proposals { get; set; } = new();
        public Taxon getTaxon() => Taxon!;
    }
}