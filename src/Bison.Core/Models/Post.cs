namespace Bison.Core.Models
{
    public abstract class Post
    {
        public int PostId { get; set; }
        public string Text { get; set; } = "";
        public DateTime TimeStamp { get; set; }
        public Author? Author { get; set; }
        public string? AuthorId { get; set; }
        public bool IsDeleted { get; set; }
    }
}