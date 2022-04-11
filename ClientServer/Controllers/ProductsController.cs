using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Entities.Responses;
using Entities.ViewModels;

namespace ClientServer.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;

        public ProductsController(IMapper mapper,ILoggerManager logger,IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }
        [HttpGet]
        public async Task<IActionResult> Products()
        {
            string token = HttpContext.Request.Cookies["JWT"];
            var jsonResponse = await _messenger.GetRequestAsync("https://localhost:44381/api/products", token);
            var productsResponse = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(jsonResponse.Message);

            switch (jsonResponse.StatusCode)
            {
                case 200:
                    {
                        var productsViewModel = _mapper.Map<IEnumerable<ProductViewModel>>(productsResponse);
                        return View(productsViewModel);
                    }
                case 401:
                    {
                        return RedirectToAction("Login","Account");
                    }
                default:
                    {
                        return RedirectToAction("Error", "Home");
                    }
            }
        }
    }
}
