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

            ViewBag.products = _context.Products.OrderByDescending(x=>x.TimesBought).Take(4);

            
            int pageSize = 3;
            return View(await PaginatedList<Product>.CreateAsync(products, page ?? 1, pageSize));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
