using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class UserTimelineModel : PageModel
{
    public int CurrentPage { get; set; }
    public int Author_id { get; set; }
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int authorId, [FromQuery] int page = 1)
    {
        Observations = await _service.GetObservationsFromAuthorId(authorId, page);
        return Page();
    }
}