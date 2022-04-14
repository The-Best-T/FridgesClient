using AutoMapper;
using Contracts;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.Responses.Fridges;
using Entities.ViewModels.Fridges;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
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
    }
}
