using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunctionalityMatrixApp.Pages.AdminPanel
{
    public class UsersListModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersListModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IQueryable<IdentityUser> Users { get; set; }

        public void OnGet()
        {
            Users = userManager.Users;
        }
    }
}