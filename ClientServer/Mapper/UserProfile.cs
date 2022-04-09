using Entites.ViewModels;
using Entities.Requests;

namespace AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginViewModel,UserLoginRequest>();
        }
    }
}
