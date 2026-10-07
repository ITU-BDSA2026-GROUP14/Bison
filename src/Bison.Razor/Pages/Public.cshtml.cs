using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class PublicModel : PageModel
{

    public int CurrentPage { get; set; }
    private readonly IObservationService _service;
    public List<ObservationViewModel> Observations { get; set; }

    public PublicModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1)
    {
        Observations = await _service.GetObservations(page);
        return Page();
    }
}