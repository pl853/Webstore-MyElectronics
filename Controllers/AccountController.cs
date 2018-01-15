using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace Webstore_MyElectronics.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public  AccountController (UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }  

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email,login.Password,login.RememberMe,false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("","Email and password do not match.");
            }
            return View(login);
        }  

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if(ModelState.IsValid)
            {            
                var user = new ApplicationUser{
                    UserName = register.Email,
                    Email = register.Email,
                    FirstName=register.FirstName,
                    LastName=register.LastName,
                    Country=register.Country,
                    PhoneNumber = register.PhoneNumber,
                    State=register.State,
                    AddressLine1=register.AddressLine1,
                    AddressLine2=register.AddressLine2,
                    ZipCode=register.ZipCode};
                    
                var result = await _userManager.CreateAsync(user,register.Password);

                if(result.Succeeded)
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Test Project", "freddydacruz90@gmail.com"));
                    message.To.Add(new MailboxAddress("Freddy da Cruz", user.Email));
                    message.Subject = "Test email";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Registration Confirmed"
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);  
                        client.Authenticate("freddydacruz90@gmail.com", password: "c374d1bb");
        
                        client.Send(message);
        
                        client.Disconnect(true);
                    }

                    await _signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(register);

        }


        [HttpPost] 
        public async Task<IActionResult> Logout() 
        { 
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Index", "Home"); 
        } 


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
