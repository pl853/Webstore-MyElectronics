using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;

namespace Webstore_MyElectronics.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public  HomeController (DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  async Task<ActionResult> Index(int? page)
        {
            var products = from s in _context.Products select s;
            products = products.Where(s => s.OnSale == true);

           List<Product> toremove = new List<Product>();
            List<Product> toremove1 = new List<Product>();

            ViewBag.products = _context.Products.OrderByDescending(x=>x.TimesBought).Take(4);

            Product p2 = new Product{
                CategoryId = 4,
                ProductName ="Apple iMac 27 (2017) MNE92N/A 3,4 GHz 5K",
                ImgUrl = "https://image.coolblue.io/products/799313?width=500&height=500" ,
                ProductPrice = 3000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };


            _context.SaveChanges();
            


            var allprods = _context.Products;

            // foreach (var item in allprods)
            // {
            //     var to = _context.Products.First(x=>x.ProductName == item.ProductName);
            //     if(toremove.Contains(to))
            //     {
            //        System.Console.WriteLine("na");
            //     }
            //     else{
            //         toremove.Add(to);
            //     }


            // }

            // foreach(var i in allprods)
            // {
            //     _context.Remove(i);
            // }
            
            // _context.SaveChanges();

            // System.Console.WriteLine(allprods.Count());
            // System.Console.WriteLine(toremove.Count());
            
            // foreach (var item in toremove)
            // {
            //     _context.Products.Add(item);
            // }
    


            // _context.SaveChanges();
            int pageSize = 3;
            return View(await PaginatedList<Product>.CreateAsync(products, page ?? 1, pageSize));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
