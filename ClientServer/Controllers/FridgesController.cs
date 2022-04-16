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
            string query = $"pageNumber={pageNumber}&pageSize={5}";

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
                            MetaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
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

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) 
                return RedirectToAction("Fridges");

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

        [HttpGet("Create/Models")]
        public async Task<IActionResult> Models([FromQuery] int pageNumber = 1)
        {
            string token = HttpContext.Request.Cookies["JWT"];
            string query = $"pageNumber={pageNumber}&pageSize={5}";

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
                            MetaData = JsonConvert.DeserializeObject<MetaData>(jsonResponse.Headres["X-Pagination"])
                        };
                        return View(fridgeModelsViewModel);
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

        [HttpGet("Create/Models/{modelId}")]
        public IActionResult Create(Guid modelId)
        {
            if (modelId == Guid.Empty)
                return RedirectToAction("Models");
            return View();
        }

        [HttpPost("Create/Models/{modelId}")]
        public async Task<IActionResult> Create(Guid modelId, CreateFridgeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Request.Cookies["JWT"];

                model.ModelId = modelId;
                var fridge = _mapper.Map<Fridge>(model);
                var createFridgeRequest = _mapper.Map<CreateFridgeRequest>(fridge);

                string jsonRequest = JsonConvert.SerializeObject(createFridgeRequest);
                var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/fridges", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeResponse = new CreateFridgeResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError(error.Key, message);
                        }
                        break;
                    case 404:
                        {
                            return RedirectToAction("Models");
                        }
                    default:
                        {
                            return RedirectToAction("Fridges");
                        }
                }
            }
            return RedirectToAction("Create");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Fridge(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Fridges");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/fridges/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeResponse = JsonConvert.DeserializeObject<FridgeResponse>(jsonResponse.Message);
                        var fridge = _mapper.Map<Fridge>(fridgeResponse);
                        var fridgeViewModel = _mapper.Map<FridgeViewModel>(fridge);
                        return View(fridgeViewModel);
                    }
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

        [HttpGet("Update")]
        public async Task<IActionResult> Update([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Fridges");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/fridges/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeResponse = JsonConvert.DeserializeObject<FridgeResponse>(jsonResponse.Message);
                        var fridge = _mapper.Map<Fridge>(fridgeResponse);
                        var updateFridgeViewModel = _mapper.Map<UpdateFridgeViewModel>(fridge);
                        return View(updateFridgeViewModel);
                    }
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

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateFridgeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fridge = _mapper.Map<Fridge>(model);
                var fridgeRequest = _mapper.Map<UpdateFridgeRequest>(fridge);

                string token = HttpContext.Request.Cookies["JWT"];

                string jsonRequest = JsonConvert.SerializeObject(fridgeRequest);
                var jsonResponse = await _messenger.PutRequestAsync($"https://localhost:44381/api/fridges/{fridge.Id}", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 204:
                        {
                            return RedirectToAction("Fridge", new { id = fridge.Id });
                        }

                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeResponse = new UpdateFridgeResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeResponse.Errors)
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
                            return RedirectToAction("Fridges");
                        }
                }
            }
            return View(model);
        }

        [HttpPost("Fill")]
        public async Task<IActionResult> FillFridges()
        {
            string token = HttpContext.Request.Cookies["JWT"];
            var jsonResponse = await _messenger.PostRequestAsync($"https://localhost:44381/api/fridges/fill", token, "");

            switch (jsonResponse.StatusCode)
            {
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
    }
}