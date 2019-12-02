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
        private IPasswordValidator<ApplicationUSer> passwordValidator;
        private IPasswordHasher<ApplicationUSer> passwordHasher;
        public AdminController(UserManager<ApplicationUSer> _userManager, IPasswordValidator<ApplicationUSer> _passwordValidator, IPasswordHasher<ApplicationUSer> _passwordHasher)
        {
            userManager = _userManager;
            passwordValidator = _passwordValidator;
            passwordHasher = _passwordHasher;

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

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
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
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string Id, string password, string email)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {

                user.Email = email;
                IdentityResult validPass = null;
                if (string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        foreach (var item in validPass.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                if (validPass.Succeeded)
                {
                    var result = await userManager.UpdateAsync(user);
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
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }

    }
}