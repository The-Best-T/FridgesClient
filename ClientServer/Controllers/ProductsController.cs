using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Requests.Products;
using Entities.Responses.Account;
using Entities.Responses.Products;
using Entities.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;

        public ProductsController(IMapper mapper, ILoggerManager logger, IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }

        [HttpGet]
        public async Task<IActionResult> Products([FromQuery] int pageNumber = 1)
        {
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
                            products = _mapper.Map<IEnumerable<ProductViewModel>>(products),
                            MetaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
                        };
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

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Products");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.DeleteRequestAsync($"https://localhost:44381/api/products/{id}", token);

            switch (jsonResponse.StatusCode)
            {
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Products");
                    }
            }
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Request.Cookies["JWT"];

                var product = _mapper.Map<Product>(model);
                var productRequest = _mapper.Map<CreateProductRequest>(product);

                string jsonRequest = JsonConvert.SerializeObject(productRequest);
                var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/products", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    case int code when (code == 400 || code == 422):
                        {
                            var productResponse = new CreateProductResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in productResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError(error.Key, message);
                        }
                        break;
                    default:
                        {
                            return RedirectToAction("Products");
                        }
                }
            }
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Product(Guid id)
        {
            if (id == Guid.Empty) 
                return RedirectToAction("Products");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/products/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var productResponse = JsonConvert.DeserializeObject<ProductResponse>(jsonResponse.Message);
                        var product = _mapper.Map<Product>(productResponse);
                        var productViewModel = _mapper.Map<ProductViewModel>(product);
                        return View(productViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Products");
                    }
            }
        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update([FromQuery] Guid id)
        {
            if (id == Guid.Empty) 
                return RedirectToAction("Products");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/products/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var productResponse = JsonConvert.DeserializeObject<ProductResponse>(jsonResponse.Message);
                        var product = _mapper.Map<Product>(productResponse);
                        var updateProductViewModel = _mapper.Map<UpdateProductViewModel>(product);
                        return View(updateProductViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Products");
                    }
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(model);
                var productRequest = _mapper.Map<UpdateProductRequest>(product);

                string token = HttpContext.Request.Cookies["JWT"];

                string jsonRequest = JsonConvert.SerializeObject(productRequest);
                var jsonResponse = await _messenger.PutRequestAsync($"https://localhost:44381/api/products/{product.Id}", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 204:
                        {
                            return RedirectToAction("Product", new { id = product.Id });
                        }

                    case int code when (code == 400 || code == 422):
                        {
                            var productResponse = new UpdateProductResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in productResponse.Errors)
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
                            return RedirectToAction("Products");
                        }

                }
            }
            return View(model);
        }
    }

}