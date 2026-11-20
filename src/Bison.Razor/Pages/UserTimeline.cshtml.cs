using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Infrastructure.Services;

namespace Bison.Razor.Initial.Pages;

public class UserTimelineModel : PageModel
{
    public int CurrentPage { get; set; }
    private readonly IPostService _service;
    public List<ObservationDTO> Observations { get; set; } = new();
    public AuthorDTO? Author { get; set; }

    public UserTimelineModel(IPostService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(string userId, [FromQuery] int page = 1)
    {
        CurrentPage = page;
        Author = await _service.GetAuthor(userId);
        if (Author is null) return NotFound();   // optional, for an unknown user ID
        Observations = await _service.GetObservationsByAuthor(userId, page);
        return Page();
    }
}