using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;
using Microsoft.EntityFrameworkCore;
using Webstore_MyElectronics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace MyElectronics_Webstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController (DatabaseContext context,UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [Route("Search/Index")]
        public async Task<ActionResult> Index(string searchString, string sortOrder, int? category, string currentFilter, int? page, int? max, int? min)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Min = min;
            ViewBag.Max = max;

            ViewBag.Category = category;

            if(searchString != null){
                page = 1;
            }else{
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int? Max = max;
            int? Min = min;

            var products = from s in _context.Products select s;

            if(Max == null && Min == null)
            {
                products = from s in _context.Products select s;
            }else{
                if(Min == null && Max != null)
                {
                    products = products.Where(p => p.ProductPrice <= Max);
                }
                else if(Max == null && Min != null)
                {
                    products = products.Where(p => p.ProductPrice >= Min);
                }else{
                    products = products.Where(p => p.ProductPrice >= Min && p.ProductPrice <= Max);
                }
            }    

            if(!String.IsNullOrEmpty(searchString)){
                    products = _context.Products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
                    || s.ProductDescription.ToLower().Contains(searchString.ToLower())
                    );
            }

            switch(sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
            // return View(await products.AsNoTracking().ToListAsync());
        }

        [Route("Search/Category/{category}")]
        public async Task<ActionResult> Category(string searchString, int? category, string currentFilter, int? page, string sortOrder, int? min, int? max)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Min = min;
            ViewBag.Max = max;

            ViewBag.Category = category;

            if(searchString != null){
                page = 1;
            }else{
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

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

            var products = from s in _context.Products select s;
            products = products.Where(s => s.CategoryId == category);

            int? Min = min;
            int? Max = max;

            if(Max == null && Min == null)
            {
                products = products.Where(s => s.CategoryId == category);
            }else{
                if(Min == null && Max != null)
                {
                    products = products.Where(p => p.ProductPrice <= Max);
                }
                else if(Max == null && Min != null)
                {
                    products = products.Where(p => p.ProductPrice >= Min);
                }else{
                    products = products.Where(p => p.ProductPrice >= Min && p.ProductPrice <= Max);
                }
            }

            if(!String.IsNullOrEmpty(searchString)){
                products = products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
                || s.ProductDescription.ToLower().Contains(searchString.ToLower())
                && s.CategoryId == category
                );
            }

            switch(sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
            // return View(await products.AsNoTracking().ToListAsync());
        }
        
        public ActionResult Details(int id)
        {
            var product = _context.Products.Find(id); 
            return View(product);
        }


         private async Task<ApplicationUser> GetUserById(string id)=> await _userManager.FindByIdAsync(id);
         private async Task<ApplicationUser> GetUserByEmail(string id)=> await _userManager.FindByEmailAsync(id);
    }

}