using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
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
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public  StoreManagerController (DatabaseContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public ActionResult  Index()
        {
                
            var montjan=CreateMonthlyRevenue("jan");
            var montfeb=CreateMonthlyRevenue("feb");
            var montmar=CreateMonthlyRevenue("mar");
            var montapr=CreateMonthlyRevenue("apr");
            var montmay=CreateMonthlyRevenue("may");
            var montjun=CreateMonthlyRevenue("jun");
            var montjul=CreateMonthlyRevenue("jul");
            var montaug=CreateMonthlyRevenue("aug");
            var montsep=CreateMonthlyRevenue("sep");
            var montokt=CreateMonthlyRevenue("okt");
            var montnov=CreateMonthlyRevenue("nov");
            var montdec=CreateMonthlyRevenue("dec");

            

            List<DataPoint> AmountProductsPerCat = new List<DataPoint>{
                new DataPoint("jan",montjan),
                new DataPoint("feb",montfeb),
                new DataPoint("mar",montmar),
                new DataPoint("apr",montapr),
                new DataPoint("may",montmay),
                new DataPoint("jun",montjun),
                new DataPoint("jul",montjul),
                new DataPoint("aug",montaug),
                new DataPoint("sep",montsep),
                new DataPoint("okt",montokt),
                new DataPoint("nov",montnov),
                new DataPoint("dec",montdec)
            };


 
			


            List<Product> top10=new List<Product>();

            var bestsold=_context.Products.OrderByDescending(x=>x.TimesBought).Take(10);

            foreach (var item in bestsold)
            {
                top10.Add(item);
            }

            List<DataPoint> bestsoldprod= new List<DataPoint>{
                new DataPoint(top10[0].ProductName,top10[0].TimesBought),
                new DataPoint(top10[1].ProductName,top10[1].TimesBought),
                new DataPoint(top10[2].ProductName,top10[2].TimesBought),
                new DataPoint(top10[3].ProductName,top10[3].TimesBought),
                new DataPoint(top10[4].ProductName,top10[4].TimesBought),
                new DataPoint(top10[5].ProductName,top10[5].TimesBought),
                new DataPoint(top10[6].ProductName,top10[6].TimesBought),
                new DataPoint(top10[7].ProductName,top10[7].TimesBought),
                new DataPoint(top10[8].ProductName,top10[8].TimesBought),
                new DataPoint(top10[9].ProductName,top10[9].TimesBought)
            };


            ViewBag.DataPoints= JsonConvert.SerializeObject(AmountProductsPerCat);
            ViewBag.DataPointsbest = JsonConvert.SerializeObject(bestsoldprod);




			return View();
        }



        public decimal CreateMonthlyRevenue(string month){
            var thisMonthRevenue = _context.Orders.Where(x=>x.OrderPlaced.ToString("MMM") == month);
            decimal thisMonthTotalRevenue = 0;

            foreach(var i in thisMonthRevenue)
            {
                thisMonthTotalRevenue += i.OrderTotal;
            }

            return(thisMonthTotalRevenue);
        }

        public async Task<ActionResult>  ManageProducts(string searchString, string sortOrder, int? category, string currentFilter, int? page)
        {
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            ViewBag.Category = category;

            if(searchString != null){
                page = 1;
            }else{
                searchString = currentFilter;
            }

            switch(category){
                case 1:
                    ViewBag.CategoryName ="Laptops";
                    break;
                case 2:
                    ViewBag.CategoryName ="Desktops";
                    break;
                case 3:
                    ViewBag.CategoryName ="Tablets";
                    break;
                case 4:
                    ViewBag.CategoryName ="Phones";
                    break;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from s in _context.Products select s;
            
            if(!String.IsNullOrEmpty(searchString)){
                    products = products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
                    || s.ProductDescription.ToLower().Contains(searchString.ToLower())
                    && s.CategoryId == category
                    );
            }


            int pageSize = 15;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
            

        }
        public ActionResult  ManageCategories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

    	[HttpGet]
        public ActionResult  ManageUsers()
        {
            var usermodel = new User
            {
                 Users = _context.Users.OrderBy(m => m.Email).ToList()
            };
            return View(usermodel);
        }

        //USER SECTION

        public async Task<IActionResult> AddUserRole(string id)
        {
            var user = await GetUserById(id);
            var model = new AddUserRole
            {
                Roles =GetAllRoles(),
                UserId = id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserRole(AddUserRole addUserRole)
        {
            var user = await GetUserById(addUserRole.UserId);
            if(ModelState.IsValid)
            {
                var result = await _userManager.AddToRoleAsync(user,addUserRole.NewRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("ManageUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
            }
            addUserRole.Email = user.Email;
            addUserRole.Roles = GetAllRoles();
            return View(addUserRole);
        }


        public async Task<IActionResult> DeleteUserRole(string id)
        {
            var user = await GetUserById(id);
            var model = new AddUserRole
            {
                Roles =GetAllRoles(),
                UserId = id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserRole(AddUserRole addUserRole)
        {
            var user = await GetUserById(addUserRole.UserId);
            if(ModelState.IsValid)
            {
                var result = await _userManager.RemoveFromRoleAsync(user,addUserRole.NewRole);
                if(result.Succeeded)
                {
                    return RedirectToAction("ManageUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
            }
            addUserRole.Email = user.Email;
            addUserRole.Roles = GetAllRoles();
            
            return View(addUserRole);
        }

        public IActionResult DeleteUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await GetUserById(id);
            var list = _context.UserWishLists.Where(l=>l.User.Id==id);

            foreach(var x in list)
            {
                _context.UserWishLists.Remove(x);
            }
            
            if(ModelState.IsValid)
            {
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ManageUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await GetUserById(id);
            var model = new EditUser
            {
                UserId = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ZipCode = user.ZipCode,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                State = user.State,
                Country = user.Country
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUser editUser,string id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                user.FirstName = editUser.FirstName;
                user.Email = editUser.Email;
                user.LastName = editUser.LastName;
                user.PhoneNumber = editUser.PhoneNumber;
                user.ZipCode = editUser.ZipCode;
                user.AddressLine1 = editUser.AddressLine1;
                user.AddressLine2 = editUser.AddressLine2;
                user.State = editUser.State;
                user.Country = editUser.Country;
            

                await _userManager.UpdateAsync(user);
                return RedirectToAction("ManageUsers");
            }
        
            return View(editUser);
        }




        private SelectList GetAllRoles()=>new SelectList(_roleManager.Roles.OrderBy(r=>r.Name));

        private async Task<ApplicationUser> GetUserById(string id)=> await _userManager.FindByIdAsync(id);



        // CATEGORY SECTION

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("ManageCategories");
            }
            return View(category);
        }

        public IActionResult EditCategory(int? id)
        {
            if(id == null)
            {
                return null;
            }

            Category category = _context.Categories.Single(model => model.CategoryId == id);

            if(category == null)
            {
                return null;
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();

                return RedirectToAction("ManageCategories");
            }

            return View(category);
        }


        public IActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Category category= _context.Categories.Single(model => model.CategoryId == id);
            if (category == null)
            {
                return null;
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            Category category= _context.Categories.Single(model => model.CategoryId == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("ManageCategories");
        }

        //PRODUCT SECTION

        public IActionResult CreateProduct()
        {
            ViewBag.CategoryId = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product product)
        {
            if(ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("ManageProducts");
            }
            ViewBag.CategoryId = _context.Categories.ToList();
            return View(product);
        }

        public IActionResult DetailsProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Product item = _context.Products.Find(id);
            if (item == null)
            {
                return null;
            }
            return View(item);
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Product product= _context.Products.Single(model => model.ProductId == id);
            if (product == null)
            {
                return null;
            }
            ViewBag.CategoryId = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product,int id)
        {
            var oldprod = _context.Products.FirstOrDefault(p=>p.ProductId == id);
            if (ModelState.IsValid)
            {
            
            _context.Products.Update(product);
            if(oldprod!= null)
            {
                DeleteProductsFromCart(id);
                DeleteProductsFromWish(id);
            }
            _context.Products.Remove(oldprod);
            _context.SaveChanges();
            return RedirectToAction("ManageProducts");
            }
            ViewBag.CategoryId = _context.Categories.ToList();
            return View(product); 
        }

        public IActionResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Product product= _context.Products.SingleOrDefault(model => model.ProductId == id);
            if (product == null)
            {
                return null;
            }
            ViewBag.CategoryId = _context.Categories.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            DeleteProductsFromCart(id);
            DeleteProductsFromWish(id);
            Product product = _context.Products.SingleOrDefault(model => model.ProductId == id);
                _context.Products.Remove(product);
                _context.SaveChanges();
            ViewBag.CategoryId = _context.Categories.ToList();
            return RedirectToAction("ManageProducts");
        }



        public void DeleteProductsFromWish(int id)
        {
            var list = _context.UserWishLists.Where(x=>x.product.ProductId == id);
            foreach(var x in list)
            {
                _context.UserWishLists.Remove(x);
            }
            _context.SaveChanges();
        }

        public void DeleteProductsFromCart(int id)
        {
            var list = _context.ShoppingCartItems.Where(x=>x.Product.ProductId == id);
            foreach(var x in list)
            {
                _context.ShoppingCartItems.Remove(x);
            }
            _context.SaveChanges();

        }


    }
}