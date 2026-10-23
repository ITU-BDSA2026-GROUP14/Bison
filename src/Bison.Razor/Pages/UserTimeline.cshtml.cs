using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class UserTimelineModel : PageModel
{
    public int CurrentPage { get; set; }
    private readonly IPostRepository _service;
    public List<ObservationDTO> Observations { get; set; } = new();

    public UserTimelineModel(IPostRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int authorId, [FromQuery] int page = 1)
    {
        Observations = await _service.GetObservationsByAuthor(authorId, page);
        return Page();
    }
}