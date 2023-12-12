
using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int> 
    {
        // public int Id {get; set; }//not optional//Identity handled

        // public string UserName { get; set; }//in c# strings have always been optional//Sicnce dotnet-6 they are not optional therefore "?" to make it optional//? will allow to keep it NULL as well (string?)//Identity handled

        // public byte[] PasswordHash {get; set;}//byte type array//Identity handled
        
        // public byte[] PasswordSalt {get; set;}//ASP.Net Identity is now taking care of this

        public DateOnly DateOfBirth {get; set;}
        public string KnownAs {get; set;}
        public DateTime Created {get; set;} = DateTime.UtcNow;//standard time zone (utc)

        public DateTime LastActive {get; set;} = DateTime.UtcNow;

        public string Gender {get; set;}
        public string Introduction {get; set;}
        public string LookingFor {get; set;}
        public string Interests {get; set;}
        public string City {get; set;}
        public string Country {get; set;}
        public List<Photo> Photos {get; set;} = new List<Photo>();

        public List<UserLike> LikedByUsers {get;set;}
        public List<UserLike> LikedUsers {get;set;}

        public List<Message> MessagesSent {get;set;}
        public List<Message> MessagesReceived {get;set;}


        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }//to make auto mapper a bit more efficient

        public ICollection<AppUserRole> UserRoles {get;set;}

    }


}