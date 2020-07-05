using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers {
    public class HomeController : Controller {

        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        public HomeController (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index () {
            return View ();
        }

        [Authorize]
        public IActionResult Secret () {
            return View ();

        }
        public IActionResult Login () {
            return View ();
        }

        public IActionResult Register () {
            return View ();
        }

        [HttpPost]
        public async Task<IActionResult> Login (string userName, string password) {
            var user = await _userManager.FindByNameAsync (userName);

            if (user != null) {
                var signInResult = await _signInManager.PasswordSignInAsync (user, password, false, false);
                if (signInResult.Succeeded) {
                    return RedirectToAction ("Secret");
                }
            }
            return RedirectToAction ("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Register (string userName, string password) {
            var user = new IdentityUser {
                UserName = userName,
                Email = "",
            };
            var result = await _userManager.CreateAsync (user, password);

            if (result.Succeeded) {
                //sign user here
                var signInResult = await _signInManager.PasswordSignInAsync (user, password, false, false);
                if (signInResult.Succeeded) {
                    return RedirectToAction ("Index");
                }
            }
            return RedirectToAction ("Index");
        }

        public async Task<IActionResult> LogOut () {
            await _signInManager.SignOutAsync ();
            return RedirectToAction ("Index");
        }
    }
}