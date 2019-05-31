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

        public IdentityUser SelectedUser { get; set; }

        [BindProperty]
        public List<string> SelectedRoles { get; set; }

        public List<string> CurrentRoles { get; set; }

        public List<IdentityRole> AvailableRoles { get; set; }

        [BindProperty]
        public bool IsEmailConfirmed { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            SelectedUser = await userManager.FindByIdAsync(userId);
            if (SelectedUser == null)
            {
                return Redirect("NotFound");
            }

            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(SelectedUser);

            AvailableRoles = roleManager.Roles.ToList();
            CurrentRoles = await userManager.GetRolesAsync(SelectedUser) as List<string>;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            SelectedUser = await userManager.FindByIdAsync(userId);

            if (SelectedUser == null)
            {
                return NotFound("User not found");
            }

            AvailableRoles = roleManager.Roles.ToList();
            CurrentRoles = await userManager.GetRolesAsync(SelectedUser) as List<string>;

            var rolesToDelete = GetRolesToDelete(CurrentRoles, SelectedRoles);
            var rolesToAdd = GetRolesToAdd(CurrentRoles, SelectedRoles);

            var removeRolesResult = await userManager.RemoveFromRolesAsync(SelectedUser, rolesToDelete);
            if (!removeRolesResult.Succeeded)
            {
                throw new Exception("Cannot remove roles");
            }

            var addRolesResult = await userManager.AddToRolesAsync(SelectedUser, rolesToAdd);
            if (!addRolesResult.Succeeded)
            {
                throw new Exception("Cannot add roles");
            }

            var updateUserResult = await userManager.UpdateAsync(SelectedUser);
            if(!updateUserResult.Succeeded)
            {
                throw new Exception("Cannot update user");
            }

            // Thx to RedirectToPage() (not Page()) the form is properly refreshed
            return RedirectToPage();
        }

        private List<string> GetRolesToAdd(List<string> currentRoles, List<string> selectedRoles)
        {
            return selectedRoles.Except(currentRoles).ToList();
        }

        private List<string> GetRolesToDelete(List<string> currentRoles, List<string> selectedRoles)
        {
            return currentRoles.Except(selectedRoles).ToList(); 
        }
    }
}