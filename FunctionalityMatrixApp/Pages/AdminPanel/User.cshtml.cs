using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.AdminPanel
{
    public class UserModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [BindProperty]
        public IdentityUser SelectedUser { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
        [BindProperty]
        public bool IsEditor { get; set; }
        [BindProperty]
        public bool IsObserver { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            SelectedUser = await userManager.FindByIdAsync(userId);
            if(SelectedUser != null)
            {
                await GetRolesAsBools(SelectedUser);

                return Page();
            }
            else
            {
                return RedirectToPage("NotFound");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            SelectedUser = await userManager.FindByIdAsync(SelectedUser.Id);
            if (SelectedUser != null)
            {
                var selectedRoles = GetCurrentRolesAsList();
                await userManager.RemoveFromRolesAsync(SelectedUser, new List<string>()
                {
                    "Observer",
                    "Editor",
                    "Administrator"
                });
            }
            else
            {
                return RedirectToPage("NotFound");
            }
            return Page();
        }

        private async Task GetRolesAsBools(IdentityUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                switch (role)
                {
                    case "Observer":
                        IsObserver = true;
                        break;
                    case "Editor":
                        IsEditor = true;
                        break;
                    case "Administrator":
                        IsAdmin = true;
                        break;
                    default:
                        throw new Exception("Unknown role assigmnet");
                }
            }
        }

        private List<string> GetCurrentRolesAsList()
        {
            List<string> roles = new List<string>();
            if (IsObserver) roles.Add("Observer");
            if (IsEditor) roles.Add("Editor");
            if (IsAdmin) roles.Add("Administrator");
            return roles;
        }
    }
}