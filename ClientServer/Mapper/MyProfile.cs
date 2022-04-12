using Entities.Requests.Account;
using Entities.Requests.Products;
using Entities.Responses.Account;
using Entities.ViewModels.Account;
using Entities.ViewModels.Products;

namespace AutoMapper
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<LoginViewModel, LoginRequest>();
            CreateMap<RegisterViewModel, RegisterRequest>();
            CreateMap<ProductResponse, ProductViewModel>();
            CreateMap<CreateProductViewModel, CreateProductRequest>();
        }
    }
}
