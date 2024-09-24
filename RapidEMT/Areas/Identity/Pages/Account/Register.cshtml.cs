using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                   await _signInManager.SignInAsync(identity,isPersistent : false);
                    return LocalRedirect("~/");
                }
            }
            return Page();
            
        }

    }
}
