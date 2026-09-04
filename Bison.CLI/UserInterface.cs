public static class UserInterface<T>
{
    public static void PrintObservations(IEnumerable<T> obs)
    {
        foreach (var o in obs)
        {
            Console.WriteLine(o?.ToString());
        }
    }
}