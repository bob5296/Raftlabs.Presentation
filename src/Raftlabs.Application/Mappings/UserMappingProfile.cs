using AutoMapper;
using Application.Response;
using Raftlabs.Core;

namespace Raftlabs.Application.Mappings;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserResponse, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Data.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Data.Email))
            .ForMember(dest => dest.First_Name, opt => opt.MapFrom(src => src.Data.First_Name))
            .ForMember(dest => dest.Last_Name, opt => opt.MapFrom(src => src.Data.Last_Name))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Data.Avatar));

        CreateMap<UsersResponse, IEnumerable<User>>()
            .ConvertUsing(src => src.Data.Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Avatar = u.Avatar,
                First_Name = u.First_Name,
                Last_Name = u.Last_Name
            }));
    }
}
