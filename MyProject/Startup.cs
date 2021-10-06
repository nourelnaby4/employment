using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MyProject.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyProject.Startup))]
namespace MyProject
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            //لادارة role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            //  users لادارة 
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //object from Role
            IdentityRole role = new IdentityRole();
            if(!roleManager.RoleExists("Admins"))
            {
                role.Name = "Admins";
                roleManager.Create(role);

                //object from user
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Ahmed";
                user.Email = "ahmed.nourelnaby.11@gmail.com";


                //useremail=ahmed.nourelnaby.11@gmail.com,password=ahmed.nourelnaby.11@gmail.com
                var Check = userManager.Create(user, "ahmed.nourelnaby.11@gmail.com");
                if(Check.Succeeded)
                {
                    //if process is done we will add the account in role admin
                    userManager.AddToRole(user.Id, "Admins");
                }
            }
        }
    }
}
