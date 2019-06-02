using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.AdminPanel
{
    public class UserDeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserDeleteModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IdentityUser UserToDelete { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            UserToDelete = await userManager.FindByIdAsync(userId);
            if(UserToDelete == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            UserToDelete = await userManager.FindByIdAsync(userId);
            if(UserToDelete == null)
            {
                return NotFound("Cant find user");
            }
            var deleteResult = await userManager.DeleteAsync(UserToDelete);
            if (!deleteResult.Succeeded)
            {
                throw new Exception("Cant delete user account");
            }
            return RedirectToPage("UsersList");
        }
    }
}