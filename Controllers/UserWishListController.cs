using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Webstore_MyElectronics.Controllers
{
    [Authorize]
    public class UserWishListController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly DatabaseContext _dbcontext;


        public  UserWishListController (UserManager<ApplicationUser> userManager,DatabaseContext dbContext)
        {
            _userManager = userManager;
            _dbcontext = dbContext;
        }

        private async Task<ApplicationUser> GetUserById(string id)=> await _userManager.FindByIdAsync(id);
        private async Task<ApplicationUser> GetUserByEmail(string id)=> await _userManager.FindByEmailAsync(id);

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        } 


        public async Task<IActionResult> AddToSavedProducts(string id,int pid,UserWishList userWishList)
        {
            var user= await GetUserByEmail(id);
            var product = _dbcontext.Products.SingleOrDefault(p=>p.ProductId == pid);
   

            if(user !=null && product != null)
            {
                userWishList.product = product;
                userWishList.User = user;

                _dbcontext.UserWishLists.Add(userWishList);
                _dbcontext.SaveChanges();
            }

            return RedirectToAction("Index","MyAccount");

        }



        public ActionResult RemoveFromSavedProducts(int id)
        {
            var list = _dbcontext.UserWishLists.FirstOrDefault(l=>l.ListId == id);
   
             _dbcontext.UserWishLists.Remove(list);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index","MyAccount");
        }

       
        
    }
}
