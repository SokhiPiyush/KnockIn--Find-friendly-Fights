

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context){
            if(await context.Users.AnyAsync()) return; //stop this method if there are any users in the DB


            //no user in DB then
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};//in case we did casing mistake in the seed-data//should be Pascal

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);//deserializer bcz we convert from json to c# object//also type in which we want to deserielize it into --> List of appuser//

            //generate passwords for each user
            foreach (var user in users){
              using var hmac=new HMACSHA512();

              user.UserName = user.UserName.ToLower();
              user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
              user.PasswordSalt = hmac.Key;

              context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}