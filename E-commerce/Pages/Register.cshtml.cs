using E_commerce.Models;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_commerce.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = RegisterViewModel.Email, Email = RegisterViewModel.Email };

                var result = await _userManager.CreateAsync(user, RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    // Sign in the user after successful registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index"); // Redirect to the home page or another page
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is not valid, redisplay the registration form with errors
            return Page();
        }
    }
}
