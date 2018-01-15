using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;
using Microsoft.EntityFrameworkCore;


namespace MyElectronics_Webstore.Controllers
{
    public class StoreController : Controller
    {
        private readonly DatabaseContext _context;

        public  StoreController (DatabaseContext context)
        {
            _context = context;
        }
        public ActionResult  Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public ActionResult Browse(string category)
        {
            var categoryModel = _context.Categories.Include("Products").Single(c=>c.CategoryName==category);

            return View(categoryModel);
        }
        
        public ActionResult Details(int id)
        {
            var product = _context.Products.Find(id); 
            return View(product);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search(string sortOrder, string searchString, int category)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.CurrentFilter = searchString;   
            ViewBag.Category = category;
            
            var products = from s in _context.Products select s;
            if(category != 0){
                products = products.Where(s => s.CategoryId == category);
            }

            if(category == 0)
            {
                if(!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
                    || s.ProductDescription.ToLower().Contains(searchString.ToLower())
                    // || s.spec1.ToLower().Contains(searchString.ToLower())
                    // || s.spec2.ToLower().Contains(searchString.ToLower())
                    // || s.spec3.ToLower().Contains(searchString.ToLower())
                    // || s.spec4.ToLower().Contains(searchString.ToLower())
                    // || s.spec5.ToLower().Contains(searchString.ToLower())
                    );
                }
            }else{
                if(!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
                    || s.ProductDescription.ToLower().Contains(searchString.ToLower())
                    && s.CategoryId == category
                    // || s.spec1.ToLower().Contains(searchString.ToLower())
                    // || s.spec2.ToLower().Contains(searchString.ToLower())
                    // || s.spec3.ToLower().Contains(searchString.ToLower())
                    // || s.spec4.ToLower().Contains(searchString.ToLower())
                    // || s.spec5.ToLower().Contains(searchString.ToLower())
                    );
                }
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
            return View(await products.AsNoTracking().ToListAsync());
        }
        
        // public async Task<IActionResult> Search(string sortOrder, string searchString)
        // {
        //     ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //     ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
        //     ViewBag.CurrentFilter = searchString;

        //     var products = from s in _context.Products select s;

        //     if(!String.IsNullOrEmpty(searchString))
        //     {
        //         products = products.Where(s => s.ProductName.ToLower().Contains(searchString.ToLower())
        //         || s.ProductDescription.ToLower().Contains(searchString.ToLower())
        //         // || s.spec1.ToLower().Contains(searchString.ToLower())
        //         // || s.spec2.ToLower().Contains(searchString.ToLower())
        //         // || s.spec3.ToLower().Contains(searchString.ToLower())
        //         // || s.spec4.ToLower().Contains(searchString.ToLower())
        //         // || s.spec5.ToLower().Contains(searchString.ToLower())
        //         );
        //     }

        //     switch(sortOrder)
        //     {
        //         case "name_desc":
        //             products = products.OrderByDescending(s => s.ProductName);
        //             break;
        //         case "Price":
        //             products = products.OrderBy(s => s.ProductPrice);
        //             break;
        //         case "price_desc":
        //             products = products.OrderByDescending(s => s.ProductPrice);
        //             break;
        //         default:
        //             products = products.OrderBy(s => s.ProductName);
        //             break;
        //     }
        //     return View(await products.AsNoTracking().ToListAsync());
        // }
    }
}