using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedDate(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
               var HasUsers= userManager.Users.Any();
               var HasRoles= roleManager.Roles.Any();
                if(HasUsers && HasRoles) return false;
                if(!HasRoles)
                {
                    var Roles = new List<IdentityRole>
                    {
                        new (){ Name = "SuperAdmin" },
                        new () { Name = "Admin"}
                    };
                    foreach (var Role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(Role.Name!).Result)
                        {
                                roleManager.CreateAsync(Role).Wait();
                        }
                      
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Yassen",
                        LastName = "AbdAlSabor",
                        UserName = "YassenAbdAlSabor",
                        Email = "YassenAbdAlSabor@gmail.com",
                        PhoneNumber = "01000000000"
                    };
                     userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                     userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Omer",
                        LastName = "Yassen",
                        UserName = "OmerYassen",
                        Email = "OmerYassen@gmail.com",
                        PhoneNumber = "01100000000"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                return false;
            }
        }
    }
}
