﻿using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Requests.FridgeProducts;
using Entities.Responses.FridgeProducts;
using Entities.ViewModels.FridgeProducts;
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

        [HttpPost("{id}")]
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
        public async Task<IActionResult> Update(Guid fridgeId,UpdateFridgeProductViewModel model)
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
                            return RedirectToAction("Product", new { fridgeId=fridgeId,id = fridgeProduct.ProductId });
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
                            return RedirectToAction("Products",new { fridgeId=fridgeId});
                        }

                }
            }
            model.FridgeId = fridgeId;
            return View(model);
        }
    }
}