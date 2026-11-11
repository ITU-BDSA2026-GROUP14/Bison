using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bison.Razor.Initial.Pages;

public class PublicModel : PageModel
{

    public int CurrentPage { get; set; }

    [BindProperty] public string Text { get; set; } = null!;

    [BindProperty] public IFormFile? Picture { get; set; }

    private readonly IPostRepository _service;
    public List<ObservationDTO> Observations { get; set; } = null!;

    public PublicModel(IPostRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1)
    {
        Observations = await _service.GetObservations(page);
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated != true) return Challenge();
        if (Picture != null && (!Picture.ContentType.StartsWith("image/") || Picture.Length > 5 * 1024 * 1024))
        {
            return RedirectToPage("/Public", new { page = 1 });
        }

        if (string.IsNullOrWhiteSpace(Text)) return RedirectToPage("/Public", new { page = 1 });

    if (Picture != null)
    {
        using var memoryStream = new MemoryStream();
        await Picture.CopyToAsync(memoryStream);
        var pictureBytes = memoryStream.ToArray();
        var pictureContentType = Picture.ContentType;

        await _service.CreateObservation(User.Identity!.Name!, Text, pictureBytes, pictureContentType);
    }
    else
    {
        await _service.CreateObservation(User.Identity!.Name!, Text, null, null);
    }
    return RedirectToPage("/Public", new { page = 1 });
    }
}