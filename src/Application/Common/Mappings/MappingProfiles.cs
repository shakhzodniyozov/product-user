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
        CreateMap<CreateProductCommand, Product>();
        CreateMap<Product, ProductDTO>();
        CreateMap<UpdateProductCommand, Product>();
    }
}
