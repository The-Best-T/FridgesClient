using Entities.Requests;
using Entities.ViewModels;
using Entities.Responses;
namespace AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginViewModel, LoginRequest>();
            CreateMap<RegisterViewModel, RegisterRequest>();
            CreateMap<ProductResponse, ProductViewModel>();
        }
    }
}
