using AutoMapper;
using Entites.ViewModels;
using Entities.Requests;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private static readonly HttpClient client = new HttpClient();

        public AccountController(IMapper mapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _logger = logger;
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
                var loginModelDto = _mapper.Map<UserLoginRequest>(model);
                try
                {
                    string jsonRequest = JsonConvert.SerializeObject(loginModelDto);
                    var jsoneRespone = await PostRequestAsync("https://localhost:44381/api/authentication/login", jsonRequest);
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
                catch (Exception ex)
                {
                    _logger.LogError($"AccountController -> Login: {ex.Message}");
                }
            }
            return View(model);

        }
        private static async Task<JsoneRespone> PostRequestAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);

            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var statusCode = (int)response.StatusCode;

            return new JsoneRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
