using AutoMapper;
using Contracts;
using Entities.Requests;
using Entities.Responses;
using Entities.ViewModels;
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

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginRequest = _mapper.Map<LoginRequest>(model);
                string jsonRequest = JsonConvert.SerializeObject(loginRequest);
                var jsonRespone = await _messenger.PostRequestAsync("https://localhost:44381/api/authentication/login", jsonRequest);
                var response = JsonConvert.DeserializeObject<LoginResponse>(jsonRespone.Message);

                if (jsonRespone.StatusCode == 200)
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

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerRequest = _mapper.Map<RegisterRequest>(model);
                string jsonRequest = JsonConvert.SerializeObject(registerRequest);
                var jsonRespone = await _messenger.PostRequestAsync("https://localhost:44381/api/authentication", jsonRequest);
                _logger.LogError(jsonRespone.StatusCode.ToString());
                switch (jsonRespone.StatusCode)
                {
                    case 201:
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    case 400:
                        {
                            ModelState.AddModelError("", $"This user name not available");
                        } break;

                    default:
                        {
                            ModelState.AddModelError("", $"Model is invalid");
                        }
                        break;
                }
            }
            return View(model);
        }
    }
}
