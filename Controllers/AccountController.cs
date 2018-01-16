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
        private readonly DatabaseContext _dbcontext;

        public  AccountController (UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager,DatabaseContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }  

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if(ModelState.IsValid && user ==null)
            {
                ModelState.AddModelError("","Email and password do not match.");
            }
            else{
                if(ModelState.IsValid && user.IsAuthenticated)
                {
                    var result = await _signInManager.PasswordSignInAsync(login.Email,login.Password,login.RememberMe,false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    ModelState.AddModelError("","Email and password do not match.");
                }
                if(ModelState.IsValid && !user.IsAuthenticated)
                {
                    return RedirectToAction("authenticate");
                }

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
                        Text = "Registration Confirmed your authentication code is abc"
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);  
                        client.Authenticate("freddydacruz90@gmail.com", password: "c374d1bb");
        
                        client.Send(message);
        
                        client.Disconnect(true);
                    }

                    //await _signInManager.SignInAsync(user,false);
                    return RedirectToAction("Account","Authenticate");
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


        public async Task<IActionResult> Authenticate(Authenticate authenticate,string id)
        {
            var user = await _userManager.FindByEmailAsync("freddy@hotmail.com");
            if(authenticate.code == "abc"){
                user.IsAuthenticated = true;
                _dbcontext.SaveChanges();
                await _signInManager.SignInAsync(user,false);
                return RedirectToAction("Index","Home");
            }
            if(authenticate.code !="abc")
            {
                ModelState.AddModelError("","The authentication code is incorrect");
            }

            return View();
        }
    }
}
