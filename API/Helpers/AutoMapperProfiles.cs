
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest=>dest.PhotoUrl, opt=> opt.MapFrom(src => src.Photos.FirstOrDefault(x=> x.IsMain).Url))//bcz automapper was giving null photoUrl field
            .ForMember(dest => dest.Age, opt=> opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            
            //createmap<from,to>//to configure an indivisual property we use Formember(destination member we are interesed in)
            CreateMap<Photo,PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
        }
    }
}