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
        var url = $"https://xeno-canto.org/api/3/recordings?query=sp:\"{scienName}\"&key={apiKey}";

        using var response = await http.GetAsync(url);
        var contentType = response.Content.Headers.ContentType?.MediaType;
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Xeno-canto status: {(int)response.StatusCode}");
        Console.WriteLine($"Xeno-canto content type: {contentType}");
        Console.WriteLine($"Xeno-canto body starts: {body[..Math.Min(body.Length, 200)]}");

        response.EnsureSuccessStatusCode();
        var json = body;

        using var doc = JsonDocument.Parse(json);
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