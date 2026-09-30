using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class PublicModel : PageModel
{

    public int CurrentPage {get; set;}
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        Observations = _service.GetObservations(CurrentPage);
        return Page();
    }
}