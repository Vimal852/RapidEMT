using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RapidEMT.Areas.Identity.Pages.Account
{
    
    public class LoginModel : PageModel
    {


        public readonly SignInManager<IdentityUser> _signInManager;
        
        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
          

        }

        [BindProperty]
        public required InputModal Input {  get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync (Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect("~/");
                }
            }
            return Page();
        }



    }

    public class InputModal
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set;}
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
