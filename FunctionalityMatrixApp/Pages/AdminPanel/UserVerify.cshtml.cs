using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.AdminPanel
{
    public class UserVerifyModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserVerifyModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IdentityUser UserToVerify { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            UserToVerify = await userManager.FindByIdAsync(userId);
            if(UserToVerify == null)
            {
                return NotFound("Cant find user");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            UserToVerify = await userManager.FindByIdAsync(userId);
            if (UserToVerify == null)
            {
                return NotFound("Cant find user");
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(UserToVerify);
            var confirmationResult = await userManager.ConfirmEmailAsync(UserToVerify, token);

            if (!confirmationResult.Succeeded)
            {
                throw new Exception("Cant verify email");
            }

            return RedirectToPage("User", new { userId = UserToVerify.Id });
        }
    }
}