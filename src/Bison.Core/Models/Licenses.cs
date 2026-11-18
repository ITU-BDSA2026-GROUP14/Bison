namespace Bison.Core.Models
{
    public static class Licenses
    {
        public static readonly Dictionary<string, string> CreativeCommons = new()
        {
            ["CC BY"] = "https://creativecommons.org/licenses/by/4.0/",
            ["CC BY-SA"] = "https://creativecommons.org/licenses/by-sa/4.0/",
            ["CC BY-ND"] = "https://creativecommons.org/licenses/by-nd/4.0/",
            ["CC BY-NC"] = "https://creativecommons.org/licenses/by-nc/4.0/",
            ["CC BY-NC-SA"] = "https://creativecommons.org/licenses/by-nc-sa/4.0/",
            ["CC BY-NC-ND"] = "https://creativecommons.org/licenses/by-nc-nd/4.0/"
        };
    }
}