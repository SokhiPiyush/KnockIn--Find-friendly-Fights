// we are using DBContext of entity frameworks in this class
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){

        }//constructor

        public DbSet<AppUser> Users {get; set;}//represents tables inside our databse//tables will have a name Users with cols defined in appUser file

    }
}