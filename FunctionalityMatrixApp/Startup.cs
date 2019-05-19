using FunctionalityMatrix.Services;
using FunctionalityMatrixApp.Data;
using FunctionalityMatrixApp.DataAccess;
using FunctionalityMatrixApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FunctionalityMatrixApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContextPool<ProductsDbContext>(options =>
                options.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductsData, DBProductsDataService>();

            services.AddDefaultIdentity<IdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(config =>
            {
                config.AddPolicy("RequireAdministratorRole",
                    policy => policy.RequireRole("Administrator"));
                config.AddPolicy("RequireEditorRole",
                    policy => policy.RequireRole("Editor"));
                config.AddPolicy("RequireObserverRole",
                    policy => policy.RequireRole("Observer"));
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddRazorPages()
                .AddNewtonsoftJson()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/Products/List", "RequireObserverRole");
                    options.Conventions.AuthorizePage("/Products/Details", "RequireObserverRole");

                    options.Conventions.AuthorizePage("/Products/Delete", "RequireEditorRole");
                    options.Conventions.AuthorizePage("/Products/Edit", "RequireEditorRole");

                    options.Conventions.AuthorizeFolder("/AdminPanel", "RequireAdministratorRole");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/node_modules"
            });

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            CreateDatabase(app);
            CreateRolesAsync(roleManager).Wait();
            CreateSuperUser(userManager).Wait();
        }

        private void CreateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
        }

        private async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //adding custom roles
            string[] roleNames = { "Administrator", "Editor", "Observer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task CreateSuperUser(UserManager<IdentityUser> userManager)
        {
            var superUser = new IdentityUser { UserName = "Yelinek8@Yelinek8.com", Email = "Yelinek8@Yelinek8.com" };
            await userManager.CreateAsync(superUser, "Dupasaca.123");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(superUser);
            await userManager.ConfirmEmailAsync(superUser, token);

            await userManager.AddToRolesAsync(superUser, new string[] {
                "Administrator",
                "Editor",
                "Observer"
            });
        }
    }
}
