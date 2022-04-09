using Entities.Requests;
using Entities.ViewModels;

namespace AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginViewModel, LoginRequest>();
            CreateMap<RegisterViewModel, RegisterRequest>();
        }
    }
}
