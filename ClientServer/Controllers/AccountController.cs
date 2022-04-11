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
    [Route("Account")]
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

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginRequest = _mapper.Map<LoginRequest>(model);
                string jsonRequest = JsonConvert.SerializeObject(loginRequest);
                var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/authentication/login",null, jsonRequest);
                var response = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse.Message);

                switch (jsonResponse.StatusCode)
                {
                    case 200:
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

                    case 401:
                        {
                            ModelState.AddModelError("", $"Wrong Login or Password");
                        }
                        break;

                    default:
                        {
                            return RedirectToAction("Error", "Home");
                        }
                        
                }
            }
            return View(model);
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerRequest = _mapper.Map<RegisterRequest>(model);
                string jsonRequest = JsonConvert.SerializeObject(registerRequest);
                var jsonResponse = await _messenger.PostRequestAsync("https://localhost:44381/api/authentication",null, jsonRequest);
                switch (jsonResponse.StatusCode)
                {
                    case 201:
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    case 400:
                        {
                            ModelState.AddModelError("", $"This user name or email not available");
                        }
                        break;

                    case 422:
                        {
                            ModelState.AddModelError("", $"Model is invalid");
                        }
                        break;

                    default:
                        {
                            return RedirectToAction("Error", "Home");
                        }
                }
            }
            return View(model);
        }
    }
}
