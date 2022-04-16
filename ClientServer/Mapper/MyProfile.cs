using Entities.Models;
using Entities.Requests.Account;
using Entities.Requests.FridgeModels;
using Entities.Requests.FridgeProducts;
using Entities.Requests.Fridges;
using Entities.Requests.Products;
using Entities.Responses.Account;
using Entities.Responses.FridgeModels;
using Entities.Responses.FridgeProducts;
using Entities.Responses.Fridges;
using Entities.ViewModels.Account;
using Entities.ViewModels.FridgeModels;
using Entities.ViewModels.FridgeProducts;
using Entities.ViewModels.Fridges;
using Entities.ViewModels.Products;

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
            CreateMap<Fridge, UpdateFridgeViewModel>();
            CreateMap<UpdateFridgeViewModel, Fridge>();
            CreateMap<Fridge, UpdateFridgeRequest>();

            CreateMap<FridgeProductResponse, FridgeProduct>();
            CreateMap<FridgeProduct, FridgeProductViewModel>();
            CreateMap<CreateFridgeProductViewModel, FridgeProduct>();
            CreateMap<FridgeProduct, CreateFridgeProductViewModel>();
            CreateMap<FridgeProduct, UpdateFridgeProductViewModel>();
            CreateMap<UpdateFridgeProductViewModel, FridgeProduct>();
            CreateMap<FridgeProduct, UpdateFridgeProductRequest>();
            CreateMap<FridgeProduct, CreateFridgeProductRequest>();
        }
    }
}
