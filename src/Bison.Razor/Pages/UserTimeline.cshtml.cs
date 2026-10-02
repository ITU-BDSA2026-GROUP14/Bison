using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class UserTimelineModel : PageModel
{
    public int CurrentPage {get; set;}
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, [FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        Observations = _service.GetObservationsFromAuthor(author, CurrentPage);
        return Page();
    }
}