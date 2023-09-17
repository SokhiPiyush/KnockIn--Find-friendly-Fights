
namespace API.Entities
{
    public class AppUser
    {
        public int Id {get; set; }//not optional

        public string UserName { get; set; }//in c# strings have always been optional//Sicnce dotnet-6 they are not optional therefore "?" to make it optional//? will allow to keep it NULL as well (string?)
    }
}