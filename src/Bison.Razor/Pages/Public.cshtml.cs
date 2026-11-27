using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.DTOs;
using Bison.Infrastructure.Services;
using Bison.Core.Models;

namespace Bison.Razor.Initial.Pages;

public class PublicModel : PageModel
{

    public int CurrentPage { get; set; }

    [BindProperty] public string Text { get; set; } = null!;

    [BindProperty] public IFormFile? Picture { get; set; }

    [BindProperty] public string? License { get; set; }

    private readonly IPostService _service;
    private readonly IFilterService _filterService;
    public List<ObservationDTO> Observations { get; set; } = null!;

    // All taxa, for choosing which taxon to filter the timeline by
    public List<TaxonDTO> Taxa { get; set; } = new();

    // The dwc_TaxonID of the taxon the timeline is filtered by, or null when not filtering
    public string? SelectedTaxon { get; set; }

    public PublicModel(IPostService service, IFilterService filterService)
    {
        _service = service;
        _filterService = filterService;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1, [FromQuery] string? taxon = null)
    {
        if (page < 1) page = 1;
        CurrentPage = page;
        Taxa = await _service.GetTaxa();

        if (string.IsNullOrWhiteSpace(taxon))
        {
            Observations = await _service.GetObservations(page);
        }
        else
        {
            // The taxon itself and all its subtaxa, filtered by the verified Dafny code
            SelectedTaxon = taxon;
            Observations = await _filterService.GetObservationsByTaxon(taxon, page);
        }
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated != true) return Challenge();
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (Picture != null && (!allowedTypes.Contains(Picture.ContentType) || Picture.Length > 5 * 1024 * 1024))


        {
            TempData["ErrorMessage"] = "Invalid picture. Please upload a valid image file (JPEG, PNG, GIF, or WebP) under 5MB.";
            return RedirectToPage("/Public", new { page = 1 });
        }
        if (Picture != null && (License is null || !Licenses.CreativeCommons.ContainsKey(License)))
        {
            TempData["ErrorMessage"] = "Choose a license for your picture";
            return RedirectToPage("/Public", new { page = 1 });
        }

        if (string.IsNullOrWhiteSpace(Text))
        {
            TempData["ErrorMessage"] = "Write what you spotted";
            return RedirectToPage("/Public", new { page = 1 });
        }

        if (Picture != null)
        {
            using var memoryStream = new MemoryStream();
            await Picture.CopyToAsync(memoryStream);
            var pictureBytes = memoryStream.ToArray();
            var pictureContentType = Picture.ContentType;

            await _service.CreateObservation(User.Identity!.Name!, Text, pictureBytes, pictureContentType, License);
        }
        else
        {
            await _service.CreateObservation(User.Identity!.Name!, Text, null, null, null);
        }
        return RedirectToPage("/Public", new { page = 1 });
    }
}