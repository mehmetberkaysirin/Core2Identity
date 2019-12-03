using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core2Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUSer> userManager;
        private SignInManager<ApplicationUSer> signInManager;

        public AccountController(UserManager<ApplicationUSer> _userManager, SignInManager<ApplicationUSer> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user!=null)
                {
                    await signInManager.SignOutAsync();
                    var result = await signInManager.PasswordSignInAsync(user, model.Password,false,false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ??"/");
                    }
                }
                ModelState.AddModelError("Email", "Invalid Email or Password");
                
            }
           

           
            return View(model);
        }
    }
}