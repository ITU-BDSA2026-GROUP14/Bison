using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bison.Core.Models;
using Bison.Core.DTOs;
using Bison.Infrastructure.Services;

namespace Bison.Razor.Initial.Pages;

[Authorize]
public class AboutMeModel : PageModel
{
    private readonly UserManager<Author> _userManager;
    private readonly IPostService _postService;

    public Author Me {get; set; } = null!;
    public IList<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>();
    public PersonalDataDTO Posts { get; set; } = null!;

    public AboutMeModel(UserManager<Author> userManager, IPostService postService)
    {
        _userManager = userManager;
        _postService = postService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        Me = user;
        Logins = await _userManager.GetLoginsAsync(user);
        Posts = await _postService.GetPersonalData(user.Id);
        return Page();
    }

}