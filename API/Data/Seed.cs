

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {

        public static async Task ClearConnections(DataContext context)
        {
          context.Connections.RemoveRange(context.Connections);
          await context.SaveChangesAsync();
        }

        // public static async Task SeedUsers(DataContext context){//replacing with UserManager
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager){
            if(await userManager.Users.AnyAsync()) return; //stop this method if there are any users in the DB


            //no user in DB then
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};//in case we did casing mistake in the seed-data//should be Pascal

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);//deserializer bcz we convert from json to c# object//also type in which we want to deserielize it into --> List of appuser//

            var roles = new List<AppRole>{
              new AppRole{Name="Member"},
              new AppRole{Name="Admin"},
              new AppRole{Name="Moderator"},
            };

            foreach (var role in roles){
              await roleManager.CreateAsync(role);
            }

            //generate passwords for each user
            foreach (var user in users){
            //   using var hmac=new HMACSHA512();//Identity Handled

              user.UserName = user.UserName.ToLower();
            //   user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            //   user.PasswordSalt = hmac.Key;//Identity Handled

              user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
              user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);

              // context.Users.Add(user);//replaced by userManager
              await userManager.CreateAsync(user, "Pa$$w0rd");//saves the changes automatically

              await userManager.AddToRoleAsync(user,"Member");//adding users into role (member)
            }

            var Admin = new AppUser
            {
              UserName = "admin"
            };

            await userManager.CreateAsync(Admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(Admin, new[] {"Admin","Moderator"});

            // await context.SaveChangesAsync();
        }
    }
}