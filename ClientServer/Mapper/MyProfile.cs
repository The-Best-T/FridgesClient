using Entities.Requests.Account;
using Entities.Requests.Products;
using Entities.Responses.Account;
using Entities.ViewModels.Account;
using Entities.ViewModels.Products;
using Entities.Models.Account;
using Entities.Models.Products;
namespace AutoMapper
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<LoginViewModel, Login>();
            CreateMap<Login, LoginRequest>();
            CreateMap<RegisterViewModel, Register>();
            CreateMap<Register, RegisterRequest>();

            CreateMap<CreateProductViewModel, Product>();
            CreateMap<Product, CreateProductRequest>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductResponse, Product>();
            CreateMap<Product, UpdateProductViewModel>();
            CreateMap<UpdateProductViewModel,Product>();
            CreateMap<Product, UpdateProductRequest>();

        }
    }
}
