// using System.Security.Cryptography;
// using System.Text;
// using API.Data;
// using API.DTOs;
// using API.Entities;
// using API.interfaces;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace API.Controllers;



//     public class AccountController : BaseApiController
//     {
//         private readonly DataContext _context;
//         private readonly ITokenService _tokenService;

//         public AccountController(DataContext context, ITokenService tokenService)
//         {
//             _tokenService = tokenService;
//             _context = context;
//         }

//         [HttpPost("register")] //POST: api/account/register
//         public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
//         {

//             if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

//             using var hmac = new HMACSHA512();//when we create a new instance in a class, it will consume memory//to dispose off that memory after the use.. automatically we use "using"
//             //garbage collection does that but we cant controll that.

//             var user = new AppUser
//             {
//                 UserName = registerDto.UserName.ToLower(),
//                 PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
//                 PasswordSalt = hmac.Key

//             };

//             _context.Users.Add(user);
//             await _context.SaveChangesAsync();

//             return new UserDto
//             {
//                 Username = user.UserName,
//                 Token = _tokenService.CreateToken(user);
//             }

//         }

//         [HttpPost("login")]

//         public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
//         {   
//             var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

//             if (user == null) return Unauthorized("invalid username");

//             using var hmac = new HMACSHA512(user.PasswordSalt);//returns a byte array

//             var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));//byte array

//             //comapring both byte arrays
//             for (int i=0; i < computedHash.Length; i++)
//             {
//                 if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
//             }

//             return new UserDto
//             {
//                 Username = user.UserName,
//                 Token = _tokenService.CreateToken(user)
//             };



//         }

//         private async Task<bool> UserExists(string username)
//         {
//             return await _context.Users.AnyAsync(x => x.UserName ==username.ToLower());// returns true or false if same user exists
//         }
    
// }

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    // private readonly DataContext _context;//usermanager replacement
    private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
    {
            _userManager = userManager;
            _mapper = mapper;
        // _context = context;//user manager replacement
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);  

        // using var hmac = new HMACSHA512();//identity handled

        // var user = new AppUser
        // {
            user.UserName = registerDto.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            // user.PasswordSalt = hmac.Key;//identity handled
        // };

        // _context.Users.Add(user);
        // await _context.SaveChangesAsync();//usermanager

        var result = await _userManager.CreateAsync(user,registerDto.Password);//saves this user in the database 

        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if(!roleResult.Succeeded) return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Weight = user.Weight
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);//checks the pass

        if (!result) return Unauthorized("Invalid Password");

        // using var hmac = new HMACSHA512(user.PasswordSalt);

        // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // for (int i = 0; i < computedHash.Length; i++)
        // {
        //     if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        // }//identity handled

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender

        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}