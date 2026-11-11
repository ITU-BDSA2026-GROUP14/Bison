using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Infrastructure.Services;

namespace Bison.Razor.Initial.Pages;

public class ObModel : PageModel
{
    public int ObId { get; set; }
    private readonly IPostService _service;
    public ObservationDTO Observation { get; set; } = null!;
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();

    public ObModel(IPostService service)
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

    public async Task<ActionResult> OnGetPictureAsync(int id)
    {
       var picture = await _service.GetPicture(id);
       if (picture is null) return NotFound();
       return File(picture.Data, picture.ContentType);
    }
}