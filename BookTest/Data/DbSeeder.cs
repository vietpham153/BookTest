using BookTest.Constants;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookTest.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            // DI 2 dịch vụ UserManager và RoleManager:
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // thêm role vào trong db:
            // tìm xem role admin đã có chưa:
            var roleAdmin = await roleManager.FindByNameAsync(Roles.Admin.ToString());
            if (roleAdmin == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
            // tìm xem role user đã có chưa:
            var roleUser = await roleManager.FindByNameAsync(Roles.User.ToString());
            if (roleUser == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }
            // thêm admin user:
            var admin = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            // tìm xem admin đã có chưa:
            var userInDb = await userManager.FindByEmailAsync(admin.Email);
            if(userInDb == null)
            {
                // tạo mật khẩu admin từ biến môi trường trong launchSettings.json:
                var passwordAdmin = Environment.GetEnvironmentVariable("AdminPassword");
                // check xem biến môi trường có lỗi hay không:
                if (string.IsNullOrEmpty(passwordAdmin))
                {
                    throw new Exception("Mật khẩu admin có lỗi khi sử dụng biến môi trường");
                }
                // tạo admin với password đã lưu trong biến môi trường:
                await userManager.CreateAsync(admin, passwordAdmin);
                // thêm admin vào role admin:
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
