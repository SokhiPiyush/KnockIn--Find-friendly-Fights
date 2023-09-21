
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   
    // [ApiController]
    // [Route("api/[controller]")] // /api/users
    // no longer needed since we made our own BaseControllerClass
    //INHERITENCE

    [Authorize]

    public class UsersController : BaseApiController
    {
        
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;

        }

        [AllowAnonymous]

        //creating API endpoints
        [HttpGet] // GET   /api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){ // IEnumeranle bcz we want a list users and AppUSers is the type of data in that list i.e our users, Getusers is thne name of the method

        var users = await _context.Users.ToListAsync(); //Tolist gets the list of all users in the database

        return users;

        }
        //we used async and wrapped inside Task<> and changed ToList to ToListAsync to make this code asynchronous

        //method to get an indivisual user
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           var user = await  _context.Users.FindAsync(id);
           return user;

        }
    }
}