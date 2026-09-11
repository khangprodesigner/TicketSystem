using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TicketSystem.Infrastructure.Data;

namespace TicketSystem.Infrastructure.Data.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            // Seed roles
            string[] roleNames = { "Manager", "Employee" };

            // 1. Tạo các Roles nếu chưa có
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            // 2. Tạo sẵn tài khoản Manager mẫu để test
            string managerEmail = "manager@cep.org.vn";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);

            if (managerUser == null)
            {
                var newManager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FullName = "Trưởng phòng",
                    Department = "Phòng IT",
                    EmailConfirmed = true,
                };

                var createResult = await userManager.CreateAsync(newManager, "Cep@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newManager, "Manager");
                }
            }
        }
    }
}