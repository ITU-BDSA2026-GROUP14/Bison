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
    private readonly SignInManager<Author> _signInManager;
    private readonly IPostService _postService;

    public Author Me {get; set; } = null!;
    public IList<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>();
    public UserDataDTO Posts { get; set; } = null!;
    public bool AskForget { get; set; }

    public AboutMeModel(UserManager<Author> userManager, SignInManager<Author> signInManager, IPostService postService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _postService = postService;
    }

    public async Task<IActionResult> OnGetAsync(bool askForget = false)
    {
        AskForget = askForget;

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        Me = user;
        Logins = await _userManager.GetLoginsAsync(user);
        Posts = await _postService.GetUserData(user.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostForgetAsync(bool keepObservations)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        await _postService.ForgetAuthor(user.Id, keepObservations);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest("Could not delete your account.");

        await _signInManager.SignOutAsync();
        return RedirectToPage("/Public");
    }
}