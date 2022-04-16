using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Requests.FridgeProducts;
using Entities.Responses.Account;
using Entities.Responses.FridgeProducts;
using Entities.ViewModels.FridgeProducts;
using Entities.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("Fridges/{fridgeId}/Products")]
    public class FridgeProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;

        public FridgeProductsController(IMapper mapper, ILoggerManager logger, IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }

        [HttpGet]
        public async Task<IActionResult> Products(Guid fridgeId, [FromQuery] int pageNumber = 1)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");

            string token = HttpContext.Request.Cookies["JWT"];
            string query = $"pageNumber={pageNumber}&pageSize={5}";

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products", token, query);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeProductsResponse = JsonConvert.DeserializeObject<IEnumerable<FridgeProductResponse>>(jsonResponse.Message);
                        var fridgeProducts = _mapper.Map<IEnumerable<FridgeProduct>>(fridgeProductsResponse);
                        var fridgeProductsViewModel = new FridgeProductsViewModel()
                        {
                            fridgeProducts = _mapper.Map<IEnumerable<FridgeProductViewModel>>(fridgeProducts),
                            MetaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"]),
                            FridgeId = fridgeId
                        };
                        return View(fridgeProductsViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                case 404:
                    {
                        return RedirectToAction("Fridges", "Fridges");
                    }
                default:
                    {
                        return RedirectToAction("Fridge", "Fridges", new { id = fridgeId });
                    }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid fridgeId, Guid id)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");
            if (id == Guid.Empty)
                return RedirectToAction("Products", new { fridgeId = fridgeId });

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.DeleteRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products/{id}", token);

            switch (jsonResponse.StatusCode)
            {
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                case 404:
                    {
                        return RedirectToAction("Fridge", "Fridges", new { id = fridgeId });
                    }
                default:
                    {
                        return RedirectToAction("Products", new { fridgeId = fridgeId });
                    }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Product(Guid fridgeId, Guid id)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");
            if (id == Guid.Empty)
                return RedirectToAction("Products", new { fridgeId = fridgeId });

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeProductResponse = JsonConvert.DeserializeObject<FridgeProductResponse>(jsonResponse.Message);
                        var fridgeProduct = _mapper.Map<FridgeProduct>(fridgeProductResponse);
                        var fridgeProductViewModel = _mapper.Map<FridgeProductViewModel>(fridgeProduct);
                        fridgeProductViewModel.FridgeId = fridgeId;
                        return View(fridgeProductViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                case 404:
                    {
                        return RedirectToAction("Fridge", "Fridges", new { id = fridgeId });
                    }
                default:
                    {
                        return RedirectToAction("Products", new { fridgeId = fridgeId });
                    }
            }
        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update(Guid fridgeId, [FromQuery] Guid id)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");
            if (id == Guid.Empty)
                return RedirectToAction("Products", new { fridgeId = fridgeId });

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeProductResponse = JsonConvert.DeserializeObject<FridgeProductResponse>(jsonResponse.Message);
                        var fridgeProduct = _mapper.Map<FridgeProduct>(fridgeProductResponse);
                        var updateFridgeProductViewModel = _mapper.Map<UpdateFridgeProductViewModel>(fridgeProduct);
                        updateFridgeProductViewModel.FridgeId = fridgeId;
                        return View(updateFridgeProductViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                case 404:
                    {
                        return RedirectToAction("Fridge", "Fridges", new { id = fridgeId });
                    }
                default:
                    {
                        return RedirectToAction("Products", new { fridgeId = fridgeId });
                    }
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(Guid fridgeId, UpdateFridgeProductViewModel model)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");

            if (ModelState.IsValid)
            {
                var fridgeProduct = _mapper.Map<FridgeProduct>(model);
                var fridgeProductRequest = _mapper.Map<UpdateFridgeProductRequest>(fridgeProduct);

                string token = HttpContext.Request.Cookies["JWT"];
                _logger.LogInfo(fridgeProduct.ProductId.ToString());
                string jsonRequest = JsonConvert.SerializeObject(fridgeProductRequest);
                var jsonResponse = await _messenger.PutRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products/{fridgeProduct.ProductId}", token, jsonRequest);
                _logger.LogInfo(jsonResponse.StatusCode.ToString());
                switch (jsonResponse.StatusCode)
                {
                    case 204:
                        {
                            return RedirectToAction("Product", new { fridgeId = fridgeId, id = fridgeProduct.ProductId });
                        }
                    case 404:
                        {
                            return RedirectToAction("Fridge", "Fridges", new { id = fridgeId });
                        }
                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeProductResponse = new UpdateFridgeProductResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeProductResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError("", message);
                        }
                        break;
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    default:
                        {
                            return RedirectToAction("Products", new { fridgeId = fridgeId });
                        }

                }
            }
            model.FridgeId = fridgeId;
            return View(model);
        }

        [HttpGet("Create/Products")]
        public async Task<IActionResult> AllProducts(Guid fridgeId, [FromQuery] int pageNumber = 1)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");
            string token = HttpContext.Request.Cookies["JWT"];
            string query = $"pageNumber={pageNumber}&pageSize={5}";

            var jsonResponse = await _messenger.GetRequestAsync("https://localhost:44381/api/products", token, query);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var productsResponse = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(jsonResponse.Message);
                        var products = _mapper.Map<IEnumerable<Product>>(productsResponse);
                        var productsViewModel = new ProductsViewModel()
                        {
                            Products = _mapper.Map<IEnumerable<ProductViewModel>>(products),
                            MetaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
                        };
                        ViewBag.fridgeId = fridgeId;
                        return View(productsViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Index", "Home");
                    }
            }
        }

        [HttpGet("Create/Products/{productId}")]
        public IActionResult Create(Guid fridgeId, Guid productId)
        {
            if (fridgeId == Guid.Empty)
                return RedirectToAction("Fridges", "Fridges");
            if (productId == Guid.Empty)
                return RedirectToAction("AllProducts", new { fridgeId = fridgeId });

            return View(
                new CreateFridgeProductViewModel()
                {
                    FridgeId = fridgeId,
                    ProductId = productId
                });
        }

        [HttpPost("Create/Products/{productId}")]
        public async Task<IActionResult> Create(Guid fridgeId, Guid productId, CreateFridgeProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Request.Cookies["JWT"];

                model.ProductId = productId;
                model.FridgeId = fridgeId;

                var fridgeProduct = _mapper.Map<FridgeProduct>(model);
                var createFridgeProductRequest = _mapper.Map<CreateFridgeProductRequest>(fridgeProduct);

                string jsonRequest = JsonConvert.SerializeObject(createFridgeProductRequest);
                var jsonResponse = await _messenger.PostRequestAsync($"https://localhost:44381/api/fridges/{fridgeId}/products", token, jsonRequest);
                _logger.LogInfo(jsonResponse.StatusCode.ToString());
                switch (jsonResponse.StatusCode)
                {
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeProductResponse = new CreateFridgeProductResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeProductResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError(error.Key, message);
                        }
                        break;
                    case 404:
                        {
                            return RedirectToAction("Fridges", "Fridges");
                        }
                    default:
                        {
                            return RedirectToAction("Products", new { fridgeId = fridgeId });
                        }
                }
            }
            return View(model);
        }
    }
}