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
		public async Task<IActionResult> LoginEID(string eid)
		{
			try
			{
				var properties = new AuthenticationProperties
				{
					AllowRefresh = false,
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.Now.AddDays(7),
				};
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, eid),
					new Claim(ClaimTypes.Name, eid),
					new Claim(ClaimTypes.Role, "User")
				};
				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(principal, properties);
				
				return RedirectToAction("Index", "Home");
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		//[HttpPost]
  //      public async Task<IActionResult> LoginEID()
  //      {
  //          try
  //          {
  //              var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

  //              identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Test"));
  //              identity.AddClaim(new Claim(ClaimTypes.Name, "test2"));
  //              identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

  //              ClaimsPrincipal principal = new ClaimsPrincipal(identity);
  //              var authProp = new AuthenticationProperties
  //              {
  //                  AllowRefresh = true,
  //                  ExpiresUtc = DateTimeOffset.Now.AddDays(7),
  //                  IsPersistent = true
  //              };
  //              await HttpContext.SignInAsync(principal, authProp);

  //              return RedirectToAction("Index", "Home");
  //          }
  //          catch (Exception)
  //          {
  //              return BadRequest();
  //          }
  //      }

    }
}