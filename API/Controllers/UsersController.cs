
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
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
        // private readonly IUserRepository _uow.UserRepository;//UOW
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;

        public UsersController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _photoService = photoService;
            _mapper = mapper;
            // _uow.UserRepository = userRepository;//UOW
            // _context = context;//a

        }

        // [AllowAnonymous]

        //creating API endpoints
        // [Authorize(Roles ="Admin")]//which roles should be allowed to access this endpoint
        [HttpGet] // GET   /api/users
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        { // IEnumeranle bcz we want a list users and AppUSers is the type of data in that list i.e our users, Getusers is thne name of the method

            // var users = await _context.Users.ToListAsync(); //Tolist gets the list of all users in the database//b

            // var users = await _uow.UserRepository.GetUsersAsync();//returning users as a list//d

            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);//removed after optimizing the mapper code//d
            //Map into IEnumerable of memberDto and we are gonna use users

            var gender = await _uow.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUsername = User.GetUsername();

            if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender = gender == "male" ? "female" : "male";
                if(gender=="others"){
                    userParams.Gender="others";
                }
            }   

            var users = await _uow.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);

            // return users;//b

        }
        //we used async and wrapped inside Task<> and changed ToList to ToListAsync to make this code asynchronous

        //method to get an indivisual user
        // [HttpGet("{id}")]//c
        // [Authorize(Roles ="Member")]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //    var user = await  _context.Users.FindAsync(id);
            //    return user;//c

            return await _uow.UserRepository.GetMemberAsync(username);

            // return _mapper.Map<MemberDto>(user);


        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//made claimsprincipal extension method therefore using that now 

            var username = User.GetUsername();

            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);//we need to go from memberupdateDto to user//this line is updating all the properties in memberupdDto into the user

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Failed to update User");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            //if this is the first photo of the user, setting it to main photo

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _uow.Complete())
            {
                return CreatedAtAction(nameof(GetUser),
                new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));//now we get location also
            }

            return BadRequest("Problem adding photo");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _uow.Complete()) return Ok();

            return BadRequest("Problem deleting photo");
        }

    }
}