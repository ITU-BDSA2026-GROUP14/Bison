namespace Bison.Core.Models
{
    public class Proposal : Post
    {
        public Observation Observation { get; set; } = null!;
        public Taxon Taxon { get; set; } = null!;
    }
}