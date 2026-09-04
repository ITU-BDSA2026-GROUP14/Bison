public record Comment(string Author, string Message, long Timestamp, int id) : Cheep(Author, Message, Timestamp)
{
    public override string ToString()
    {
        return $"(id = ${id}) {base.ToString()}";
    }
}