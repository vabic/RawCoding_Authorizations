using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cokkies.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        [Authorize]
        public IActionResult Secret() {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy() {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole() {
            return View("Secret");
        }

        public async Task<IActionResult> Authenticate() {

            var grandmaClaims = new List<Claim>() {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/2000"),
                new Claim(ClaimTypes.Role, "AdminTwo"),
                new Claim("Grandma.Says", "Very Nice boi."),
            };

            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!1");

            var licenceClaims = new List<Claim>() {
                new Claim(ClaimTypes.Name, "Bob k Foo"),
                new Claim("DrivingLicence", "A+"),
            };

            var granmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenceIdentity = new ClaimsIdentity(licenceClaims, "Governement Identity");

            var userPrincipal = new ClaimsPrincipal(new [] { granmaIdentity, licenceIdentity });

            await HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
