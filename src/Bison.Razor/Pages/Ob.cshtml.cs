using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Infrastructure.Services;

namespace Bison.Razor.Initial.Pages;

public class ObModel : PageModel
{
    private readonly IPostService _service;
    public ObservationDTO Observation { get; set; } = null!;
    public List<CommentDTO> Comments { get; set; } = new();
    public List<ProposalDTO> Proposals { get; set; } = new();
    public List<TaxonDTO> Taxa { get; set; } = new();

    // Filled in from the comment box
    [BindProperty] public string? CommentText { get; set; }

    // Filled in from the proposal box
    [BindProperty] public string? ProposalTaxonId { get; set; }
    [BindProperty] public string? ProposalText { get; set; }

    public ObModel(IPostService service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int? id, int page = 1)
    {
        if (id is null) { return Redirect("/"); }
        if (!await LoadObservation(id.Value, page)) return NotFound();
        return Page();
    }

    // Runs when the comment form is posted (asp-page-handler="Comment")
    public async Task<ActionResult> OnPostCommentAsync(int id)
    {
        if (User.Identity?.IsAuthenticated != true) return Challenge();
        if (!await LoadObservation(id)) return NotFound();

        if (string.IsNullOrWhiteSpace(CommentText))
        {
            ModelState.AddModelError(nameof(CommentText), "Write a comment before posting.");
            return Page();
        }

        await _service.CreateComment(User.Identity.Name!, id, CommentText);
        return RedirectToPage(new { id });
    }

    // Runs when the proposal form is posted (asp-page-handler="Proposal")
    public async Task<ActionResult> OnPostProposalAsync(int id)
    {
        if (User.Identity?.IsAuthenticated != true) return Challenge();
        if (!await LoadObservation(id)) return NotFound();

        if (ProposalTaxonId is null || Taxa.All(t => t.TaxonId != ProposalTaxonId))
        {
            ModelState.AddModelError(nameof(ProposalTaxonId), "Choose what you think the animal is.");
            return Page();
        }

        await _service.CreateProposal(User.Identity.Name!, id, ProposalTaxonId, ProposalText);
        return RedirectToPage(new { id });
    }

    // The name to show for a taxon id, e.g. in the list of proposals
    public string TaxonName(string? taxonId) =>
        Taxa.FirstOrDefault(t => t.TaxonId == taxonId)?.TaxonName ?? taxonId ?? "";

    // Loads everything the page shows. Returns false if the observation doesn't exist.
    private async Task<bool> LoadObservation(int id, int page = 1)
    {
        var observation = await _service.GetObservation(id);
        if (observation is null) return false;

        Observation = observation;
        Comments = await _service.GetCommentsForObservation(id, page);
        Proposals = await _service.GetProposalsForObservation(id, page);
        Taxa = await _service.GetTaxa();
        return true;
    }

    public async Task<ActionResult> OnGetPictureAsync(int id)
    {
        var picture = await _service.GetImage(id);
        if (picture is null) return NotFound();
        return File(picture.Data, picture.ContentType);
    }
}