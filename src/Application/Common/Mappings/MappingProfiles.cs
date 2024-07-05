using AutoMapper;
using Domain;

namespace Application;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterUserCommand, User>();
        CreateMap<User, UserDTO>();
        CreateMap<UpdateUserCommand, User>();
    }
}
