
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
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
        
        // private readonly DataContext _context; Datacontext context was injected before//a
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            // _context = context;//a

        }

        // [AllowAnonymous]

        //creating API endpoints
        [HttpGet] // GET   /api/users
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){ // IEnumeranle bcz we want a list users and AppUSers is the type of data in that list i.e our users, Getusers is thne name of the method

        // var users = await _context.Users.ToListAsync(); //Tolist gets the list of all users in the database//b

        // var users = await _userRepository.GetUsersAsync();//returning users as a list//d

        // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);//removed after optimizing the mapper code//d
        //Map into IEnumerable of memberDto and we are gonna use users

        var users = await _userRepository.GetMembersAsync();

        return Ok(users);
        
        // return users;//b

        }
        //we used async and wrapped inside Task<> and changed ToList to ToListAsync to make this code asynchronous

        //method to get an indivisual user
        // [HttpGet("{id}")]//c
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
        //    var user = await  _context.Users.FindAsync(id);
        //    return user;//c

            return await _userRepository.GetMemberAsync(username);

            // return _mapper.Map<MemberDto>(user);


        }
    }
}