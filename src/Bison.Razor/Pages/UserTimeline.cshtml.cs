using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class UserTimelineModel : PageModel
{
    public int CurrentPage { get; set; }
    public int Author_id {get; set;}
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public UserTimelineModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int author, [FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        Author_id = author;
        Observations = _service.GetObservationsFromAuthorId(Author_id, CurrentPage);
        return Page();
    }
}