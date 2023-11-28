using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);//these two properties make up the primary key
        Task<AppUser> GetUserWithLikes(int userId);

        // Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);//predicate-> dont knwo if the user liked or liked by,therefore method to function accordingly we use predicate//removed after pagination was done in lists

        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);



    }
}