using AutoMapper;
using UserAuthentication.Data.Entities;
using UserAuthentication.Models;

namespace UserAuthentication.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegistrationModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
