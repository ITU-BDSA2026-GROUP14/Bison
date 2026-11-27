using System.Net.Http.Json;
using System.Text.Json;
using Bison.Core.DTOs;
using Microsoft.Extensions.Configuration;

namespace Bison.Infrastructure.Repositories;

public class SoundRepository(HttpClient http, IConfiguration config) : ISoundRepository
{
    public async Task<SoundDTO?> GetTaxonSound(string scienName)
    {
        var apiKey = config["XenoCanto:ApiKey"];
        var species = string.Join(' ' , scienName.Split(' ').Take(2));
        var query = Uri.EscapeDataString($"sp:\"{species}\"");
        var url = $"https://xeno-canto.org/api/3/recordings?query={query}&key={apiKey}";

        using var response = await http.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode || !body.TrimStart().StartsWith("{"))
        {
            return null;
        }


        using var doc = JsonDocument.Parse(body);
        var recordings = doc.RootElement.GetProperty("recordings");

        if (recordings.GetArrayLength() == 0)
        {
            return null;
        }

        return new SoundDTO
        (
            scienName,
            recordings[0].GetProperty("url").ToString()!,
            recordings[0].GetProperty("file").ToString()!
        );
    }
}