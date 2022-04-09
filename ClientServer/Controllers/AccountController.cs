using AutoMapper;
using Contracts;
using Entites.ViewModels;
using Entities.Requests;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IMessenger _messenger;

        public AccountController(IMapper mapper, ILoggerManager logger, IMessenger messenger)
        {
            _mapper = mapper;
            _logger = logger;
            _messenger = messenger;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userLoginRequest = _mapper.Map<UserLoginRequest>(model);
                string jsonRequest = JsonConvert.SerializeObject(userLoginRequest);
                var jsoneRespone = await _messenger.PostRequestAsync("https://localhost:44381/api/authentication/login", jsonRequest);
                var response = JsonConvert.DeserializeObject<UserLoginResponse>(jsoneRespone.Message);

                if (jsoneRespone.StatusCode == 200)
                {
                    HttpContext.Response.Cookies.Append("JWT", response.Token);
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"Wrong Login or Password");
                }
            }

            return View(model);
        }
    }
}
