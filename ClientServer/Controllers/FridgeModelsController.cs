using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Requests.FridgeModels;
using Entities.Responses.FridgeModels;
using Entities.ViewModels.FridgeModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("Models")]
    public class FridgeModelsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;

        public FridgeModelsController(IMapper mapper, ILoggerManager logger, IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }

        [HttpGet]
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

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Models");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.DeleteRequestAsync($"https://localhost:44381/api/models/{id}", token);

            switch (jsonResponse.StatusCode)
            {
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Models");
                    }
            }

        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateFridgeModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = HttpContext.Request.Cookies["JWT"];

                var fridgeModel = _mapper.Map<FridgeModel>(model);
                var fridgeModelRequest = _mapper.Map<CreateFridgeModelRequest>(fridgeModel);

                string jsonRequest = JsonConvert.SerializeObject(fridgeModelRequest);
                var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/models", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeModelResponse = new CreateFridgeModelResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeModelResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError("", message);
                        }
                        break;
                    default:
                        {
                            return RedirectToAction("Models");
                        }
                }
            }
            return View(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Model(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Models");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/models/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeModelResponse = JsonConvert.DeserializeObject<FridgeModelResponse>(jsonResponse.Message);
                        var fridgeModel = _mapper.Map<FridgeModel>(fridgeModelResponse);
                        var fridgeModelViewModel = _mapper.Map<FridgeModelViewModel>(fridgeModel);
                        return View(fridgeModelViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Models");
                    }
            }

        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Models");

            string token = HttpContext.Request.Cookies["JWT"];

            var jsonResponse = await _messenger.GetRequestAsync($"https://localhost:44381/api/models/{id}", token, null);
            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var fridgeModelResponse = JsonConvert.DeserializeObject<FridgeModel>(jsonResponse.Message);
                        var fridgeModel = _mapper.Map<FridgeModel>(fridgeModelResponse);
                        var updateFridgeModelViewModel = _mapper.Map<UpdateFridgeModelViewModel>(fridgeModel);
                        return View(updateFridgeModelViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login", "Account");
                    }
                default:
                    {
                        return RedirectToAction("Models");
                    }
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(UpdateFridgeModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fridgeModel = _mapper.Map<FridgeModel>(model);
                var fridgeModelRequest = _mapper.Map<UpdateFridgeModelRequest>(fridgeModel);

                string token = HttpContext.Request.Cookies["JWT"];

                string jsonRequest = JsonConvert.SerializeObject(fridgeModelRequest);
                var jsonResponse = await _messenger.PutRequestAsync($"https://localhost:44381/api/models/{fridgeModel.Id}", token, jsonRequest);

                switch (jsonResponse.StatusCode)
                {
                    case 204:
                        {
                            return RedirectToAction("Model", new { id = fridgeModel.Id });
                        }

                    case int code when (code == 400 || code == 422):
                        {
                            var fridgeModelResponse = new UpdateFridgeModelResponse()
                            {
                                Errors = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(jsonResponse.Message)
                            };
                            foreach (var error in fridgeModelResponse.Errors)
                                foreach (var message in error.Value)
                                    ModelState.AddModelError(error.Key, message);
                        }
                        break;
                    case 401:
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    default:
                        {
                            return RedirectToAction("Models");
                        }

                }
            }
            return View(model);
        }
    }
}
