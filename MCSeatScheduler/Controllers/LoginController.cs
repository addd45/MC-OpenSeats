using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MCSeatScheduler.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginEID()
        {
            try
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Test"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "test2"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var authProp = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(7),
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(principal, authProp);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}