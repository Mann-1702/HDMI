using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

public class MatchesService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MatchesService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public IEnumerable<Match> GetMatches()
    {
        var jsonFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "data", "matches.json");
        using (var jsonFileReader = File.OpenText(jsonFilePath))
        {
            return JsonSerializer.Deserialize<Match[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
