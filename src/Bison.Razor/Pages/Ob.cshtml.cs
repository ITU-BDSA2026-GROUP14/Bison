using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class ObModel : PageModel
{
    public int ObId { get; set; }
    private readonly PostRepository _service;
    public ObservationDTO Observation { get; set; } = null!;
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();

    public ObModel(PostRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int? id, int page = 1)
    {
        if (id is null) { return Redirect("/"); }

        var observation = await _service.GetObservation(id.Value);
        if (observation is null)
        {
            return NotFound();
        }

        Observation = observation;
        Comments = await _service.GetCommentsForObservation(id.Value, page);
        Proposals = await _service.GetProposalsForObservation(id.Value, page);

        return Page();
    }
}