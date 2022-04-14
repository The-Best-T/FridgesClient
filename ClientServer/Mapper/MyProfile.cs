using Entities.Models;
using Entities.Requests.Account;
using Entities.Requests.Products;
using Entities.Responses.Account;
using Entities.ViewModels.Account;
using Entities.ViewModels.Products;
using Entities.ViewModels.FridgeModels;
using Entities.Requests.FridgeModels;
using Entities.Responses.FridgeModels;
using Entities.Responses.Fridges;
using Entities.ViewModels.Fridges;
using Entities.Requests.Fridges;

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
            CreateMap<UpdateProductViewModel, Product>();
            CreateMap<Product, UpdateProductRequest>();

            CreateMap<CreateFridgeModelViewModel, FridgeModel>();
            CreateMap<FridgeModel, CreateFridgeModelRequest>();
            CreateMap<FridgeModel, FridgeModelViewModel>();
            CreateMap<FridgeModelResponse, FridgeModel>();
            CreateMap<FridgeModel, UpdateFridgeModelViewModel>();
            CreateMap<UpdateFridgeModelViewModel, FridgeModel>();
            CreateMap<FridgeModel, UpdateFridgeModelRequest>();

            CreateMap<FridgeResponse, Fridge>();
            CreateMap<Fridge, FridgeViewModel>();
            CreateMap<CreateFridgeViewModel, Fridge>();
            CreateMap<Fridge, CreateFridgeRequest>();
        }
    }
}
