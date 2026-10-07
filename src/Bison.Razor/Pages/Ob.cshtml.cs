using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class ObModel : PageModel
{
    public int ObId { get; set; }
    private readonly IObservationService _service;
    public ObservationViewModel Observation { get; set; }
    public List<ObservationViewModel> Comments { get; set; } = new();
    public List<ObservationViewModel> Proposals { get; set; } = new();

    public ObModel(IObservationService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int? id, int page = 1)
    {
        if (id is null) { return Redirect("/"); }

        var observation = (await _service.GetObservationFromId(id.Value, page)).FirstOrDefault();
        if (observation is null)
        {
            return NotFound();
        }

        Observation = observation;
        Comments = await _service.GetCommentsFromObservationId(id.Value, page);
        Proposals = await _service.GetProposalsFromId(id.Value, page);

        return Page();
    }
}