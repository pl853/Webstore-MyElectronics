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
    public class MyAccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly DatabaseContext _dbcontext;


        public  MyAccountController (UserManager<ApplicationUser> userManager,DatabaseContext dbContext,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _dbcontext = dbContext;
            _signInManager = signInManager;
        }

        private async Task<ApplicationUser> GetUserById(string id)=> await _userManager.FindByIdAsync(id);
        private async Task<ApplicationUser> GetUserByEmail(string id)=> await _userManager.FindByEmailAsync(id);

        public async Task<ActionResult> Index(int? page)
        {      
            var result = _dbcontext.UserWishLists.Where(u=>u.User.Email==User.Identity.Name).Include(i=>i.product);
            int pageSize = 4;
            return View(await PaginatedList<UserWishList>.CreateAsync(result, page ?? 1, pageSize));
        }
        

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await GetUserByEmail(id);
            var model = new EditUser
            {
                UserId = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                ZipCode = user.ZipCode,
                State = user.State,
                Country=user.Country,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUser editUser,string id)
        {
            var user = await GetUserByEmail(id);
            string currentemail = User.Identity.Name;
            if (user != null && ModelState.IsValid)
            {
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.AddressLine1 = editUser.AddressLine1;
                user.AddressLine2 = editUser.AddressLine2;
                user.Country = editUser.Country;
                user.State = editUser.State;
                user.ZipCode = editUser.ZipCode;
                user.PhoneNumber = editUser.PhoneNumber;
                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user,editUser.Password);

                await _userManager.UpdateAsync(user);

                if(editUser.Email != currentemail){
                  await _signInManager.SignOutAsync();
                }

                return RedirectToAction("Index");
            }
        
            return View(editUser);
        }


        public IActionResult OrderHistory(string id)
        {
            var orderList= _dbcontext.Orders.Where(o=> o.User.Email== id);

            return View(orderList);
        }

        public IActionResult OrderDetails(int id)
        {
            var Order = _dbcontext.Orders.FirstOrDefault(x=>x.OrderId == id);

            

            return View(Order);
        }




    }
}

