using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Requests.Fridges;
using Entities.Responses.FridgeModels;
using Entities.Responses.Fridges;
using Entities.ViewModels.FridgeModels;
using Entities.ViewModels.Fridges;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("Fridges")]
    public class FridgesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;
        public FridgesController(IMapper mapper, ILoggerManager logger, IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }

        [HttpGet]
        public async Task<IActionResult> Fridges([FromQuery] int pageNumber = 1)
        {
            string token = HttpContext.Request.Cookies["JWT"];
            string query = $"pageNumber={pageNumber}&pageSize={3}";

            var jsonResponse = await _messenger.GetRequestAsync("https://localhost:44381/api/fridges", token, query);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgesResponse = JsonConvert.DeserializeObject<IEnumerable<FridgeResponse>>(jsonResponse.Message);
                        var fridges = _mapper.Map<IEnumerable<Fridge>>(fridgesResponse);
                        var fridgesViewModel = new FridgesViewModel()
                        {
                            fridges = _mapper.Map<IEnumerable<FridgeViewModel>>(fridges),
                            metaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
                        };
                        return View(fridgesViewModel);
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

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                string token = HttpContext.Request.Cookies["JWT"];

                var jsonResponse = await _messenger.DeleteRequestAsync($"https://localhost:44381/api/fridges/{id}", token);

                switch (jsonResponse.StatusCode)
                {
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    default:
                        {
                            return RedirectToAction("Fridges");
                        }
                }
            }
            return RedirectToAction("Fridges");
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create([FromQuery] int pageNumber = 1)
        {
            string token = HttpContext.Request.Cookies["JWT"];
            string query = $"pageNumber={pageNumber}&pageSize={3}";

            var jsonResponse = await _messenger.GetRequestAsync("https://localhost:44381/api/models", token, query);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeModelsResponse = JsonConvert.DeserializeObject<IEnumerable<FridgeModelResponse>>(jsonResponse.Message);
                        var fridgeModels = _mapper.Map<IEnumerable<FridgeModel>>(fridgeModelsResponse);
                        var fridgeModelsViewModel = new FridgeModelsViewModel()
                        {
                            fridgeModels = _mapper.Map<IEnumerable<FridgeModelViewModel>>(fridgeModels),
                            metaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
                        };
                        var createFridgeViewModel = new CreateFridgeViewModel()
                        {
                            Models = fridgeModelsViewModel
                        };
                        return View(createFridgeViewModel);
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

        //[HttpPost("Create")]
        //public async Task<IActionResult> Create(CreateFridgeViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string token = HttpContext.Request.Cookies["JWT"];

        //        var fridge = _mapper.Map<Fridge>(model);
        //        var fridgeRequest = _mapper.Map<CreateFridgeRequest>(fridge);

        //        string jsonRequest = JsonConvert.SerializeObject(fridgeRequest);
        //        var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/fridges", token, jsonRequest);

        //        switch (jsonResponse.StatusCode)
        //        {
        //            case 401:
        //                {
        //                    return RedirectToAction("Login", "Account");
        //                }
        //            case int code when (code == 400 || code == 422):
        //                {
        //                    var fridgeResponse = new CreateFridgeResponse()
        //                    {
        //                        Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
        //                    };
        //                    foreach (var error in fridgeResponse.Errors)
        //                        foreach (var message in error.Value)
        //                            ModelState.AddModelError(error.Key, message);
        //                }
        //                break;
        //            default:
        //                {
        //                    return RedirectToAction("Fridges");
        //                }
        //        }
        //    }
        //    return View(model);
        //}
    }
}
