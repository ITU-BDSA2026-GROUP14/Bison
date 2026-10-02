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

    public ActionResult OnGet([FromQuery] int? Id)
    {
        if (Id is null) { return Redirect("/obs"); }
        ObId = Id.Value;

        Observation = _service.GetObservationFromId(ObId)[0];
        Comments = _service.GetCommentsFromId(ObId);
        Proposals = _service.GetProposalsFromId(ObId);

        return Page();
    }
}