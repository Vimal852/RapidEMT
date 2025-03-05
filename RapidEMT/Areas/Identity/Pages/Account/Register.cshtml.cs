using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog; // Add this to use Serilog

namespace RapidEMT.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public readonly SignInManager<IdentityUser> _signInManager;
        public readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public required InputModal Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var identity = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(identity, Input.Password);

                if (result.Succeeded)
                {
                    Log.Information("New user registered: {Email}", identity.Email);  // 🔥 Logs the registration event

                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return LocalRedirect("~/");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Warning("User registration failed for {Email}. Reason: {Error}", Input.Email, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}
