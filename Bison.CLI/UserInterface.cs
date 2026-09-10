public static class UserInterface<T>
{
    public static void PrintObservations(IEnumerable<T> obs)
    {
        foreach (var o in obs)
        {
            Console.WriteLine(o?.ToString());
        }
    }

    // currently its own method, could be merged with PrintObservations if need be
    public static void PrintComments(IEnumerable<T> obs, int id)
    {
        var idProperty = typeof(Comment).GetProperty("Id");

        foreach (var o in obs)
        {
            if ((int?)idProperty?.GetValue(o) == id)
            {
                Console.WriteLine(o?.ToString());
                return;
            }

        }
    }
}