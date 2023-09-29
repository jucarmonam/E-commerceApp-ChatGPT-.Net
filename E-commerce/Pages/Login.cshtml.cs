using E_commerce.Models;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_commerce.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LoginModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    LoginViewModel.Email, LoginViewModel.Password, LoginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index"); // Redirect to the home page or another page
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If ModelState is not valid or login fails, redisplay the login form with errors
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Clear the user's session data upon logout
            HttpContext.Session.Clear();

            // Sign the user out using the authentication middleware
            await _signInManager.SignOutAsync();

            // Redirect to a suitable page after logout
            return RedirectToPage("/Index");
        }
    }
}
