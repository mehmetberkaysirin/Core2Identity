using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core2Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUSer> userManager;
        public AdminController(UserManager<ApplicationUSer> _userManager)
        {
            userManager = _userManager;

        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel Model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUSer user = new ApplicationUSer();
                user.UserName = Model.UserName;
                user.Email = Model.Mail;
                var result = await userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(Model);
        }
    }
}