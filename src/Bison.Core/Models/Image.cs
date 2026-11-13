namespace Bison.Core.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public byte[] Data { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}