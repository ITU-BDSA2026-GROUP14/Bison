using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.Models;

namespace Bison.Razor.Initial.Pages;

[Authorize]
public class AboutMeModel : PageModel
{
    private readonly UserManager<Author> _userManager;

    public Author Me {get; set; } = null!;
    public IList<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>();

    public AboutMeModel(UserManager<Author> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        Me = user;
        Logins = await _userManager.GetLoginsAsync(user);
        return Page();
    }

}